#pragma once

#include <stdint.h>

#if defined(_WIN32)
#  if defined(CTP_MD_BRIDGE_EXPORTS) || defined(CTP_TRADER_BRIDGE_EXPORTS)
#    define CTP_BRIDGE_API __declspec(dllexport)
#  else
#    define CTP_BRIDGE_API __declspec(dllimport)
#  endif
#else
#  define CTP_BRIDGE_API
#endif

#ifdef __cplusplus
extern "C" {
#endif

typedef struct ctp_rsp_info {
  int32_t error_id;
  char error_msg[81];
} ctp_rsp_info;

typedef struct ctp_specific_instrument {
  char instrument_id[81];
} ctp_specific_instrument;

typedef struct ctp_req_user_login {
  char trading_day[9];
  char broker_id[11];
  char user_id[16];
  char password[41];
  char user_product_info[11];
  char interface_product_info[11];
  char protocol_info[11];
  char mac_address[21];
  char one_time_password[41];
  char login_remark[36];
  int32_t client_ip_port;
  char client_ip_address[33];
  char sms_code[41];
} ctp_req_user_login;

typedef struct ctp_rsp_user_login {
  char trading_day[9];
  char login_time[9];
  char broker_id[11];
  char user_id[16];
  char system_name[41];
  int32_t front_id;
  int32_t session_id;
  char max_order_ref[13];
  char shfe_time[9];
  char dce_time[9];
  char czce_time[9];
  char ffex_time[9];
  char ine_time[9];
  char sys_version[41];
  char gfex_time[9];
  int32_t login_dr_identity_id;
  int32_t user_dr_identity_id;
  char last_login_time[17];
  char reserve_info[65];
} ctp_rsp_user_login;

typedef struct ctp_user_logout {
  char broker_id[11];
  char user_id[16];
} ctp_user_logout;

typedef struct ctp_rsp_authenticate {
  char broker_id[11];
  char user_id[16];
  char user_product_info[11];
  char app_id[33];
  char app_type;
} ctp_rsp_authenticate;

typedef struct ctp_req_authenticate {
  char broker_id[11];
  char user_id[16];
  char user_product_info[11];
  char auth_code[17];
  char app_id[33];
} ctp_req_authenticate;

typedef struct ctp_settlement_info_confirm {
  char broker_id[11];
  char investor_id[13];
  char confirm_date[9];
  char confirm_time[9];
  int32_t settlement_id;
  char account_id[13];
  char currency_id[4];
} ctp_settlement_info_confirm;

typedef struct ctp_depth_market_data {
  char trading_day[9];
  char exchange_id[9];
  double last_price;
  double pre_settlement_price;
  double pre_close_price;
  double pre_open_interest;
  double open_price;
  double highest_price;
  double lowest_price;
  int32_t volume;
  double turnover;
  double open_interest;
  double close_price;
  double settlement_price;
  double upper_limit_price;
  double lower_limit_price;
  double pre_delta;
  double curr_delta;
  char update_time[9];
  int32_t update_millisec;
  double bid_price1;
  int32_t bid_volume1;
  double ask_price1;
  int32_t ask_volume1;
  double bid_price2;
  int32_t bid_volume2;
  double ask_price2;
  int32_t ask_volume2;
  double bid_price3;
  int32_t bid_volume3;
  double ask_price3;
  int32_t ask_volume3;
  double bid_price4;
  int32_t bid_volume4;
  double ask_price4;
  int32_t ask_volume4;
  double bid_price5;
  int32_t bid_volume5;
  double ask_price5;
  int32_t ask_volume5;
  double average_price;
  char action_day[9];
  char instrument_id[81];
  char exchange_inst_id[81];
  double banding_upper_price;
  double banding_lower_price;
} ctp_depth_market_data;

typedef struct ctp_qry_trading_account {
  char broker_id[11];
  char investor_id[13];
  char currency_id[4];
  char biz_type;
  char account_id[13];
} ctp_qry_trading_account;

typedef struct ctp_trading_account {
  char broker_id[11];
  char account_id[13];
  char currency_id[4];
  char trading_day[9];
  double deposit;
  double withdraw;
  double balance;
  double available;
  double curr_margin;
  double frozen_margin;
  double frozen_cash;
  double frozen_commission;
  double commission;
  double close_profit;
  double position_profit;
  double withdraw_quota;
  double reserve;
} ctp_trading_account;

typedef struct ctp_qry_investor_position {
  char broker_id[11];
  char investor_id[13];
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_investor_position;

typedef struct ctp_investor_position {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char exchange_id[9];
  char posi_direction;
  char hedge_flag;
  char position_date;
  int32_t yd_position;
  int32_t position;
  int32_t today_position;
  int32_t long_frozen;
  int32_t short_frozen;
  int32_t open_volume;
  int32_t close_volume;
  double position_profit;
  double close_profit;
  double use_margin;
  double position_cost;
  double open_cost;
} ctp_investor_position;

typedef struct ctp_input_order {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char order_ref[13];
  char user_id[16];
  char order_price_type;
  char direction;
  char comb_offset_flag[5];
  char comb_hedge_flag[5];
  double limit_price;
  int32_t volume_total_original;
  char time_condition;
  char gtd_date[9];
  char volume_condition;
  int32_t min_volume;
  char contingent_condition;
  double stop_price;
  char force_close_reason;
  int32_t is_auto_suspend;
  char business_unit[21];
  int32_t request_id;
  int32_t user_force_close;
  int32_t is_swap_order;
  char exchange_id[9];
  char invest_unit_id[17];
  char account_id[13];
  char currency_id[4];
  char client_id[11];
  char mac_address[21];
  char ip_address[33];
  char order_memo[13];
} ctp_input_order;

typedef struct ctp_input_order_action {
  char broker_id[11];
  char investor_id[13];
  int32_t order_action_ref;
  char order_ref[13];
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char exchange_id[9];
  char order_sys_id[21];
  char action_flag;
  double limit_price;
  int32_t volume_change;
  char user_id[16];
  char invest_unit_id[17];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
  char order_memo[13];
} ctp_input_order_action;

typedef struct ctp_order {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char exchange_id[9];
  char order_ref[13];
  char order_sys_id[21];
  char user_id[16];
  char order_price_type;
  char direction;
  char comb_offset_flag[5];
  char comb_hedge_flag[5];
  double limit_price;
  int32_t volume_total_original;
  int32_t volume_traded;
  int32_t volume_total;
  int32_t front_id;
  int32_t session_id;
  char order_status;
  char order_submit_status;
  char status_msg[81];
  char insert_date[9];
  char insert_time[9];
  char active_time[9];
  char suspend_time[9];
  char update_time[9];
  char cancel_time[9];
} ctp_order;

typedef struct ctp_trade {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char exchange_id[9];
  char order_ref[13];
  char order_sys_id[21];
  char trade_id[21];
  char user_id[16];
  char direction;
  char offset_flag;
  char hedge_flag;
  double price;
  int32_t volume;
  char trade_date[9];
  char trade_time[9];
  char trading_day[9];
} ctp_trade;

typedef struct ctp_md_spi {
  void (*on_front_connected)(void* user_data);
  void (*on_front_disconnected)(int32_t reason, void* user_data);
  void (*on_heartbeat_warning)(int32_t time_lapse, void* user_data);
  void (*on_rsp_user_login)(const ctp_rsp_user_login* login, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_user_logout)(const ctp_user_logout* logout, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_error)(const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_sub_market_data)(const ctp_specific_instrument* instrument, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_unsub_market_data)(const ctp_specific_instrument* instrument, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rtn_depth_market_data)(const ctp_depth_market_data* market_data, void* user_data);
} ctp_md_spi;

typedef struct ctp_trader_spi {
  void (*on_front_connected)(void* user_data);
  void (*on_front_disconnected)(int32_t reason, void* user_data);
  void (*on_heartbeat_warning)(int32_t time_lapse, void* user_data);
  void (*on_rsp_authenticate)(const ctp_rsp_authenticate* auth, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_settlement_info_confirm)(const ctp_settlement_info_confirm* confirm, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_user_login)(const ctp_rsp_user_login* login, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_user_logout)(const ctp_user_logout* logout, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_error)(const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_qry_trading_account)(const ctp_trading_account* account, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_qry_investor_position)(const ctp_investor_position* position, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_order_insert)(const ctp_input_order* input_order, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rsp_order_action)(const ctp_input_order_action* action, const ctp_rsp_info* rsp_info, int32_t request_id, int32_t is_last, void* user_data);
  void (*on_rtn_order)(const ctp_order* order, void* user_data);
  void (*on_rtn_trade)(const ctp_trade* trade, void* user_data);
} ctp_trader_spi;

typedef struct ctp_md_handle ctp_md_handle;
typedef struct ctp_trader_handle ctp_trader_handle;

CTP_BRIDGE_API const char* ctp_md_get_api_version(void);
CTP_BRIDGE_API ctp_md_handle* ctp_md_create(const char* flow_path, int32_t using_udp, int32_t multicast, int32_t production_mode);
CTP_BRIDGE_API void ctp_md_destroy(ctp_md_handle* handle);
CTP_BRIDGE_API int32_t ctp_md_set_spi(ctp_md_handle* handle, const ctp_md_spi* spi, void* user_data);
CTP_BRIDGE_API int32_t ctp_md_register_front(ctp_md_handle* handle, const char* front_address);
CTP_BRIDGE_API void ctp_md_init(ctp_md_handle* handle);
CTP_BRIDGE_API int32_t ctp_md_join(ctp_md_handle* handle);
CTP_BRIDGE_API int32_t ctp_md_req_user_login(ctp_md_handle* handle, const ctp_req_user_login* request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_md_req_user_logout(ctp_md_handle* handle, const ctp_user_logout* request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_md_subscribe_market_data(ctp_md_handle* handle, const char* const* instruments, int32_t count);
CTP_BRIDGE_API int32_t ctp_md_unsubscribe_market_data(ctp_md_handle* handle, const char* const* instruments, int32_t count);

CTP_BRIDGE_API const char* ctp_trader_get_api_version(void);
CTP_BRIDGE_API ctp_trader_handle* ctp_trader_create(const char* flow_path, int32_t production_mode);
CTP_BRIDGE_API void ctp_trader_destroy(ctp_trader_handle* handle);
CTP_BRIDGE_API int32_t ctp_trader_set_spi(ctp_trader_handle* handle, const ctp_trader_spi* spi, void* user_data);
CTP_BRIDGE_API int32_t ctp_trader_register_front(ctp_trader_handle* handle, const char* front_address);
CTP_BRIDGE_API int32_t ctp_trader_subscribe_private_topic(ctp_trader_handle* handle, int32_t resume_type, int32_t seq_no);
CTP_BRIDGE_API int32_t ctp_trader_subscribe_public_topic(ctp_trader_handle* handle, int32_t resume_type);
CTP_BRIDGE_API void ctp_trader_init(ctp_trader_handle* handle);
CTP_BRIDGE_API int32_t ctp_trader_join(ctp_trader_handle* handle);
CTP_BRIDGE_API int32_t ctp_trader_req_authenticate(ctp_trader_handle* handle, const ctp_req_authenticate* request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_settlement_info_confirm(ctp_trader_handle* handle, const ctp_settlement_info_confirm* request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_user_login(ctp_trader_handle* handle, const ctp_req_user_login* request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_user_logout(ctp_trader_handle* handle, const ctp_user_logout* request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_trading_account(ctp_trader_handle* handle, const ctp_qry_trading_account* request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_position(ctp_trader_handle* handle, const ctp_qry_investor_position* request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_order_insert(ctp_trader_handle* handle, const ctp_input_order* request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_order_action(ctp_trader_handle* handle, const ctp_input_order_action* request, int32_t request_id);

#ifdef __cplusplus
}
#endif
