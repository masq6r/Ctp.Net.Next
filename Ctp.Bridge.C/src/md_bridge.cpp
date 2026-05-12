#include "ctp_bridge.h"

#include <cstring>
#include <memory>
#include <new>
#include <string>
#include <vector>

#include "ThostFtdcMdApi.h"

namespace {

template <size_t N>
void copy_field(char (&dest)[N], const char* src) {
  std::memset(dest, 0, N);
  if (src != nullptr) {
    std::strncpy(dest, src, N - 1);
  }
}

void fill_rsp_info(ctp_rsp_info& dest, const CThostFtdcRspInfoField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  dest.error_id = src->ErrorID;
  copy_field(dest.error_msg, src->ErrorMsg);
}

void fill_specific_instrument(ctp_specific_instrument& dest, const CThostFtdcSpecificInstrumentField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_rsp_user_login(ctp_rsp_user_login& dest, const CThostFtdcRspUserLoginField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.login_time, src->LoginTime);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.system_name, src->SystemName);
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.max_order_ref, src->MaxOrderRef);
  copy_field(dest.shfe_time, src->SHFETime);
  copy_field(dest.dce_time, src->DCETime);
  copy_field(dest.czce_time, src->CZCETime);
  copy_field(dest.ffex_time, src->FFEXTime);
  copy_field(dest.ine_time, src->INETime);
  copy_field(dest.sys_version, src->SysVersion);
  copy_field(dest.gfex_time, src->GFEXTime);
  dest.login_dr_identity_id = src->LoginDRIdentityID;
  dest.user_dr_identity_id = src->UserDRIdentityID;
  copy_field(dest.last_login_time, src->LastLoginTime);
  copy_field(dest.reserve_info, src->ReserveInfo);
}

void fill_user_logout(ctp_user_logout& dest, const CThostFtdcUserLogoutField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
}

void fill_depth_market_data(ctp_depth_market_data& dest, const CThostFtdcDepthMarketDataField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.last_price = src->LastPrice;
  dest.pre_settlement_price = src->PreSettlementPrice;
  dest.pre_close_price = src->PreClosePrice;
  dest.pre_open_interest = src->PreOpenInterest;
  dest.open_price = src->OpenPrice;
  dest.highest_price = src->HighestPrice;
  dest.lowest_price = src->LowestPrice;
  dest.volume = src->Volume;
  dest.turnover = src->Turnover;
  dest.open_interest = src->OpenInterest;
  dest.close_price = src->ClosePrice;
  dest.settlement_price = src->SettlementPrice;
  dest.upper_limit_price = src->UpperLimitPrice;
  dest.lower_limit_price = src->LowerLimitPrice;
  dest.pre_delta = src->PreDelta;
  dest.curr_delta = src->CurrDelta;
  copy_field(dest.update_time, src->UpdateTime);
  dest.update_millisec = src->UpdateMillisec;
  dest.bid_price1 = src->BidPrice1;
  dest.bid_volume1 = src->BidVolume1;
  dest.ask_price1 = src->AskPrice1;
  dest.ask_volume1 = src->AskVolume1;
  dest.bid_price2 = src->BidPrice2;
  dest.bid_volume2 = src->BidVolume2;
  dest.ask_price2 = src->AskPrice2;
  dest.ask_volume2 = src->AskVolume2;
  dest.bid_price3 = src->BidPrice3;
  dest.bid_volume3 = src->BidVolume3;
  dest.ask_price3 = src->AskPrice3;
  dest.ask_volume3 = src->AskVolume3;
  dest.bid_price4 = src->BidPrice4;
  dest.bid_volume4 = src->BidVolume4;
  dest.ask_price4 = src->AskPrice4;
  dest.ask_volume4 = src->AskVolume4;
  dest.bid_price5 = src->BidPrice5;
  dest.bid_volume5 = src->BidVolume5;
  dest.ask_price5 = src->AskPrice5;
  dest.ask_volume5 = src->AskVolume5;
  dest.average_price = src->AveragePrice;
  copy_field(dest.action_day, src->ActionDay);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  dest.banding_upper_price = src->BandingUpperPrice;
  dest.banding_lower_price = src->BandingLowerPrice;
}

class MdSpiAdapter final : public CThostFtdcMdSpi {
public:
  MdSpiAdapter(const ctp_md_spi& callbacks, void* user_data)
      : callbacks_(callbacks), user_data_(user_data) {}

  void OnFrontConnected() override {
    if (callbacks_.on_front_connected != nullptr) {
      callbacks_.on_front_connected(user_data_);
    }
  }

  void OnFrontDisconnected(int nReason) override {
    if (callbacks_.on_front_disconnected != nullptr) {
      callbacks_.on_front_disconnected(nReason, user_data_);
    }
  }

  void OnHeartBeatWarning(int nTimeLapse) override {
    if (callbacks_.on_heartbeat_warning != nullptr) {
      callbacks_.on_heartbeat_warning(nTimeLapse, user_data_);
    }
  }

  void OnRspUserLogin(CThostFtdcRspUserLoginField* login, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_user_login == nullptr) {
      return;
    }
    ctp_rsp_user_login login_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_rsp_user_login(login_bridge, login);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_user_login(login != nullptr ? &login_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspUserLogout(CThostFtdcUserLogoutField* logout, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_user_logout == nullptr) {
      return;
    }
    ctp_user_logout logout_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_user_logout(logout_bridge, logout);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_user_logout(logout != nullptr ? &logout_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspError(CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_error == nullptr) {
      return;
    }
    ctp_rsp_info rsp_bridge {};
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_error(rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspSubMarketData(CThostFtdcSpecificInstrumentField* instrument, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_sub_market_data == nullptr) {
      return;
    }
    ctp_specific_instrument instrument_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_specific_instrument(instrument_bridge, instrument);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_sub_market_data(instrument != nullptr ? &instrument_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspUnSubMarketData(CThostFtdcSpecificInstrumentField* instrument, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_unsub_market_data == nullptr) {
      return;
    }
    ctp_specific_instrument instrument_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_specific_instrument(instrument_bridge, instrument);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_unsub_market_data(instrument != nullptr ? &instrument_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRtnDepthMarketData(CThostFtdcDepthMarketDataField* market_data) override {
    if (callbacks_.on_rtn_depth_market_data == nullptr || market_data == nullptr) {
      return;
    }
    ctp_depth_market_data bridge {};
    fill_depth_market_data(bridge, market_data);
    callbacks_.on_rtn_depth_market_data(&bridge, user_data_);
  }

private:
  ctp_md_spi callbacks_ {};
  void* user_data_ {};
};

std::vector<char*> to_mutable_instruments(const char* const* instruments, int32_t count, std::vector<std::string>& storage) {
  storage.clear();
  std::vector<char*> result;
  if (instruments == nullptr || count <= 0) {
    return result;
  }
  storage.reserve(static_cast<size_t>(count));
  result.reserve(static_cast<size_t>(count));
  for (int32_t i = 0; i < count; ++i) {
    storage.emplace_back(instruments[i] == nullptr ? "" : instruments[i]);
    result.push_back(storage.back().data());
  }
  return result;
}

void fill_req_user_login(CThostFtdcReqUserLoginField& dest, const ctp_req_user_login& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.Password, src.password);
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.InterfaceProductInfo, src.interface_product_info);
  copy_field(dest.ProtocolInfo, src.protocol_info);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.OneTimePassword, src.one_time_password);
  copy_field(dest.LoginRemark, src.login_remark);
  dest.ClientIPPort = src.client_ip_port;
  copy_field(dest.ClientIPAddress, src.client_ip_address);
  copy_field(dest.SMSCode, src.sms_code);
}

void fill_req_user_logout(CThostFtdcUserLogoutField& dest, const ctp_user_logout& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
}

}  // namespace

struct ctp_md_handle {
  CThostFtdcMdApi* api {};
  std::unique_ptr<MdSpiAdapter> spi {};
};

extern "C" {

const char* ctp_md_get_api_version(void) {
  return CThostFtdcMdApi::GetApiVersion();
}

ctp_md_handle* ctp_md_create(const char* flow_path, int32_t using_udp, int32_t multicast, int32_t production_mode) {
  auto* handle = new (std::nothrow) ctp_md_handle();
  if (handle == nullptr) {
    return nullptr;
  }
  handle->api = CThostFtdcMdApi::CreateFtdcMdApi(flow_path != nullptr ? flow_path : "", using_udp != 0, multicast != 0, production_mode != 0);
  if (handle->api == nullptr) {
    delete handle;
    return nullptr;
  }
  return handle;
}

void ctp_md_destroy(ctp_md_handle* handle) {
  if (handle == nullptr) {
    return;
  }
  if (handle->api != nullptr) {
    handle->api->RegisterSpi(nullptr);
    handle->api->Release();
    handle->api = nullptr;
  }
  delete handle;
}

int32_t ctp_md_set_spi(ctp_md_handle* handle, const ctp_md_spi* spi, void* user_data) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  handle->spi.reset();
  if (spi == nullptr) {
    handle->api->RegisterSpi(nullptr);
    return 0;
  }
  handle->spi = std::make_unique<MdSpiAdapter>(*spi, user_data);
  handle->api->RegisterSpi(handle->spi.get());
  return 0;
}

int32_t ctp_md_register_front(ctp_md_handle* handle, const char* front_address) {
  if (handle == nullptr || handle->api == nullptr || front_address == nullptr) {
    return -1;
  }
  std::string front(front_address);
  handle->api->RegisterFront(front.data());
  return 0;
}

void ctp_md_init(ctp_md_handle* handle) {
  if (handle != nullptr && handle->api != nullptr) {
    handle->api->Init();
  }
}

int32_t ctp_md_join(ctp_md_handle* handle) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  return handle->api->Join();
}

int32_t ctp_md_req_user_login(ctp_md_handle* handle, const ctp_req_user_login* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqUserLoginField native_request {};
  fill_req_user_login(native_request, *request);
  return handle->api->ReqUserLogin(&native_request, request_id);
}

int32_t ctp_md_req_user_logout(ctp_md_handle* handle, const ctp_user_logout* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcUserLogoutField native_request {};
  fill_req_user_logout(native_request, *request);
  return handle->api->ReqUserLogout(&native_request, request_id);
}

int32_t ctp_md_subscribe_market_data(ctp_md_handle* handle, const char* const* instruments, int32_t count) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  std::vector<std::string> storage;
  auto mutable_ptrs = to_mutable_instruments(instruments, count, storage);
  return handle->api->SubscribeMarketData(mutable_ptrs.data(), count);
}

int32_t ctp_md_unsubscribe_market_data(ctp_md_handle* handle, const char* const* instruments, int32_t count) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  std::vector<std::string> storage;
  auto mutable_ptrs = to_mutable_instruments(instruments, count, storage);
  return handle->api->UnSubscribeMarketData(mutable_ptrs.data(), count);
}

}  // extern "C"
