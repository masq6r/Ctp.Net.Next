cmake_minimum_required(VERSION 3.20)
project(ctp_net_bridge LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

set(LOCAL_CTP_SDK_ROOT "${CMAKE_CURRENT_SOURCE_DIR}/ctp-sdk")
if(DEFINED ENV{CTP_SDK_ROOT} AND NOT "$ENV{CTP_SDK_ROOT}" STREQUAL "")
  set(DEFAULT_CTP_SDK_ROOT "$ENV{CTP_SDK_ROOT}")
elseif(EXISTS "${LOCAL_CTP_SDK_ROOT}")
  set(DEFAULT_CTP_SDK_ROOT "${LOCAL_CTP_SDK_ROOT}")
else()
  set(DEFAULT_CTP_SDK_ROOT "")
endif()

if(DEFINED ENV{CTP_SDK_VERSION} AND NOT "$ENV{CTP_SDK_VERSION}" STREQUAL "")
  set(DEFAULT_CTP_SDK_VERSION "$ENV{CTP_SDK_VERSION}")
else()
  set(DEFAULT_CTP_SDK_VERSION "")
endif()

set(CTP_SDK_ROOT "${DEFAULT_CTP_SDK_ROOT}" CACHE PATH "Path to the ctp-sdk root directory or a specific version directory")
set(CTP_SDK_VERSION "${DEFAULT_CTP_SDK_VERSION}" CACHE STRING "CTP SDK version directory name under CTP_SDK_ROOT")

if(NOT CTP_SDK_ROOT)
  message(FATAL_ERROR "CTP_SDK_ROOT is not set. Point it at the ctp-sdk root directory or a specific version directory.")
endif()

function(resolve_ctp_sdk_version_dir out_var)
  if(EXISTS "${CTP_SDK_ROOT}/mduserapi" AND EXISTS "${CTP_SDK_ROOT}/traderapi")
    get_filename_component(_resolved_version "${CTP_SDK_ROOT}" NAME)
    set(CTP_SDK_VERSION "${_resolved_version}" CACHE STRING "CTP SDK version directory name under CTP_SDK_ROOT" FORCE)
    set(${out_var} "${CTP_SDK_ROOT}" PARENT_SCOPE)
    return()
  endif()

  if(NOT IS_DIRECTORY "${CTP_SDK_ROOT}")
    message(FATAL_ERROR "CTP_SDK_ROOT does not exist or is not a directory: ${CTP_SDK_ROOT}")
  endif()

  if(CTP_SDK_VERSION)
    set(_candidate_dir "${CTP_SDK_ROOT}/${CTP_SDK_VERSION}")
    if(NOT EXISTS "${_candidate_dir}/mduserapi" OR NOT EXISTS "${_candidate_dir}/traderapi")
      message(FATAL_ERROR "CTP_SDK_VERSION '${CTP_SDK_VERSION}' does not contain mduserapi/ and traderapi/ under ${CTP_SDK_ROOT}")
    endif()
    set(${out_var} "${_candidate_dir}" PARENT_SCOPE)
    return()
  endif()

  file(GLOB _sdk_children RELATIVE "${CTP_SDK_ROOT}" "${CTP_SDK_ROOT}/*")
  set(_sdk_versions "")
  foreach(_child IN LISTS _sdk_children)
    if(IS_DIRECTORY "${CTP_SDK_ROOT}/${_child}" AND EXISTS "${CTP_SDK_ROOT}/${_child}/mduserapi" AND EXISTS "${CTP_SDK_ROOT}/${_child}/traderapi")
      list(APPEND _sdk_versions "${_child}")
    endif()
  endforeach()

  if(NOT _sdk_versions)
    message(FATAL_ERROR "No SDK version directories were found under ${CTP_SDK_ROOT}")
  endif()

  list(SORT _sdk_versions COMPARE NATURAL ORDER DESCENDING)
  list(GET _sdk_versions 0 _resolved_version)
  set(CTP_SDK_VERSION "${_resolved_version}" CACHE STRING "CTP SDK version directory name under CTP_SDK_ROOT" FORCE)
  set(${out_var} "${CTP_SDK_ROOT}/${_resolved_version}" PARENT_SCOPE)
endfunction()

resolve_ctp_sdk_version_dir(CTP_SDK_VERSION_DIR)
message(STATUS "Using CTP SDK version directory: ${CTP_SDK_VERSION_DIR}")

if(WIN32)
  set(CTP_PLATFORM_DIR "win-x64")
  set(MD_INCLUDE_DIR "${CTP_SDK_VERSION_DIR}/mduserapi/${CTP_PLATFORM_DIR}")
  set(TRADER_INCLUDE_DIR "${CTP_SDK_VERSION_DIR}/traderapi/${CTP_PLATFORM_DIR}")
  set(MD_IMPORT_LIB "${MD_INCLUDE_DIR}/thostmduserapi_se.lib")
  set(TRADER_IMPORT_LIB "${TRADER_INCLUDE_DIR}/thosttraderapi_se.lib")
  set(MD_RUNTIME_LIB "${MD_INCLUDE_DIR}/thostmduserapi_se.dll")
  set(TRADER_RUNTIME_LIB "${TRADER_INCLUDE_DIR}/thosttraderapi_se.dll")
else()
  set(CTP_PLATFORM_DIR "linux-x64")
  set(MD_INCLUDE_DIR "${CTP_SDK_VERSION_DIR}/mduserapi/${CTP_PLATFORM_DIR}")
  set(TRADER_INCLUDE_DIR "${CTP_SDK_VERSION_DIR}/traderapi/${CTP_PLATFORM_DIR}")
  set(MD_IMPORT_LIB "${MD_INCLUDE_DIR}/thostmduserapi_se.so")
  set(TRADER_IMPORT_LIB "${TRADER_INCLUDE_DIR}/thosttraderapi_se.so")
  set(MD_RUNTIME_LIB "${MD_IMPORT_LIB}")
  set(TRADER_RUNTIME_LIB "${TRADER_IMPORT_LIB}")
endif()

foreach(required_path IN ITEMS
  "${MD_INCLUDE_DIR}"
  "${TRADER_INCLUDE_DIR}"
  "${MD_IMPORT_LIB}"
  "${TRADER_IMPORT_LIB}"
  "${MD_RUNTIME_LIB}"
  "${TRADER_RUNTIME_LIB}"
)
  if(NOT EXISTS "${required_path}")
    message(FATAL_ERROR "Required CTP SDK path does not exist: ${required_path}")
  endif()
endforeach()

add_library(ctpmd_bridge SHARED
  src/md_bridge.cpp
)
target_include_directories(ctpmd_bridge PRIVATE include "${MD_INCLUDE_DIR}")
add_library(ctp_sdk_md SHARED IMPORTED GLOBAL)
set_target_properties(ctp_sdk_md PROPERTIES IMPORTED_LOCATION "${MD_IMPORT_LIB}")
target_link_libraries(ctpmd_bridge PRIVATE ctp_sdk_md)
target_compile_definitions(ctpmd_bridge PRIVATE CTP_MD_BRIDGE_EXPORTS=1)
set_target_properties(ctpmd_bridge PROPERTIES
  OUTPUT_NAME "ctpmd_bridge"
)
add_custom_command(TARGET ctpmd_bridge POST_BUILD
  COMMAND ${CMAKE_COMMAND} -E copy_if_different "${MD_RUNTIME_LIB}" "$<TARGET_FILE_DIR:ctpmd_bridge>"
)

add_library(ctptrader_bridge SHARED
  src/trader_bridge.cpp
)
target_include_directories(ctptrader_bridge PRIVATE include "${TRADER_INCLUDE_DIR}")
add_library(ctp_sdk_trader SHARED IMPORTED GLOBAL)
set_target_properties(ctp_sdk_trader PROPERTIES IMPORTED_LOCATION "${TRADER_IMPORT_LIB}")
target_link_libraries(ctptrader_bridge PRIVATE ctp_sdk_trader)
target_compile_definitions(ctptrader_bridge PRIVATE CTP_TRADER_BRIDGE_EXPORTS=1)
set_target_properties(ctptrader_bridge PROPERTIES
  OUTPUT_NAME "ctptrader_bridge"
)
add_custom_command(TARGET ctptrader_bridge POST_BUILD
  COMMAND ${CMAKE_COMMAND} -E copy_if_different "${TRADER_RUNTIME_LIB}" "$<TARGET_FILE_DIR:ctptrader_bridge>"
)

if(UNIX AND NOT APPLE)
  set_target_properties(ctpmd_bridge PROPERTIES
    BUILD_RPATH "${MD_INCLUDE_DIR}"
    INSTALL_RPATH "$ORIGIN"
  )
  set_target_properties(ctptrader_bridge PROPERTIES
    BUILD_RPATH "${TRADER_INCLUDE_DIR}"
    INSTALL_RPATH "$ORIGIN"
  )
endif()
