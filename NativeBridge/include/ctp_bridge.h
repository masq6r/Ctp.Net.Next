#pragma once

#include <stdint.h>

#if defined(_WIN32)
#if defined(CTP_MD_BRIDGE_EXPORTS) || defined(CTP_TRADER_BRIDGE_EXPORTS)
#define CTP_BRIDGE_API __declspec(dllexport)
#else
#define CTP_BRIDGE_API __declspec(dllimport)
#endif
#else
#define CTP_BRIDGE_API
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

typedef struct ctp_qry_instrument_margin_rate {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char hedge_flag;
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_instrument_margin_rate;

typedef struct ctp_instrument_margin_rate {
  char reserve1[31];
  char investor_range;
  char broker_id[11];
  char investor_id[13];
  char hedge_flag;
  double long_margin_ratio_by_money;
  double long_margin_ratio_by_volume;
  double short_margin_ratio_by_money;
  double short_margin_ratio_by_volume;
  int32_t is_relative;
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_instrument_margin_rate;

typedef struct ctp_qry_exchange_margin_rate {
  char broker_id[11];
  char reserve1[31];
  char hedge_flag;
  char exchange_id[9];
  char instrument_id[81];
} ctp_qry_exchange_margin_rate;

typedef struct ctp_exchange_margin_rate {
  char broker_id[11];
  char reserve1[31];
  char hedge_flag;
  double long_margin_ratio_by_money;
  double long_margin_ratio_by_volume;
  double short_margin_ratio_by_money;
  double short_margin_ratio_by_volume;
  char exchange_id[9];
  char instrument_id[81];
} ctp_exchange_margin_rate;

typedef struct ctp_qry_instrument_commission_rate {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_instrument_commission_rate;

typedef struct ctp_instrument_commission_rate {
  char reserve1[31];
  char investor_range;
  char broker_id[11];
  char investor_id[13];
  double open_ratio_by_money;
  double open_ratio_by_volume;
  double close_ratio_by_money;
  double close_ratio_by_volume;
  double close_today_ratio_by_money;
  double close_today_ratio_by_volume;
  char exchange_id[9];
  char biz_type;
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_instrument_commission_rate;

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
  void (*on_front_connected)(void *user_data);
  void (*on_front_disconnected)(int32_t reason, void *user_data);
  void (*on_heartbeat_warning)(int32_t time_lapse, void *user_data);
  void (*on_rsp_user_login)(const ctp_rsp_user_login *login,
                            const ctp_rsp_info *rsp_info, int32_t request_id,
                            int32_t is_last, void *user_data);
  void (*on_rsp_user_logout)(const ctp_user_logout *logout,
                             const ctp_rsp_info *rsp_info, int32_t request_id,
                             int32_t is_last, void *user_data);
  void (*on_rsp_error)(const ctp_rsp_info *rsp_info, int32_t request_id,
                       int32_t is_last, void *user_data);
  void (*on_rsp_sub_market_data)(const ctp_specific_instrument *instrument,
                                 const ctp_rsp_info *rsp_info,
                                 int32_t request_id, int32_t is_last,
                                 void *user_data);
  void (*on_rsp_unsub_market_data)(const ctp_specific_instrument *instrument,
                                   const ctp_rsp_info *rsp_info,
                                   int32_t request_id, int32_t is_last,
                                   void *user_data);
  void (*on_rtn_depth_market_data)(const ctp_depth_market_data *market_data,
                                   void *user_data);
} ctp_md_spi;

typedef struct ctp_accountregister {
  char trade_day[9];
  char bank_id[4];
  char bank_branch_id[5];
  char bank_account[41];
  char broker_id[11];
  char broker_branch_id[31];
  char account_id[13];
  char id_card_type;
  char identified_card_no[51];
  char customer_name[51];
  char currency_id[4];
  char open_or_destroy;
  char reg_date[9];
  char out_date[9];
  int32_t t_id;
  char cust_type;
  char bank_acc_type;
  char long_customer_name[161];
} ctp_accountregister;

typedef struct ctp_batch_order_action {
  char broker_id[11];
  char investor_id[13];
  int32_t order_action_ref;
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char exchange_id[9];
  char action_date[9];
  char action_time[9];
  char trader_id[21];
  int32_t install_id;
  char action_local_id[13];
  char participant_id[11];
  char client_id[11];
  char business_unit[21];
  char order_action_status;
  char user_id[16];
  char status_msg[81];
  char invest_unit_id[17];
  char reserve1[16];
  char mac_address[21];
  char ip_address[33];
} ctp_batch_order_action;

typedef struct ctp_broker_trading_algos {
  char broker_id[11];
  char exchange_id[9];
  char reserve1[31];
  char handle_position_algo_id;
  char find_margin_rate_algo_id;
  char handle_trading_account_algo_id;
  char instrument_id[81];
} ctp_broker_trading_algos;

typedef struct ctp_broker_trading_params {
  char broker_id[11];
  char investor_id[13];
  char margin_price_type;
  char algorithm;
  char avail_include_close_profit;
  char currency_id[4];
  char option_royalty_price_type;
  char account_id[13];
} ctp_broker_trading_params;

typedef struct ctp_cfmmc_trading_account_key {
  char broker_id[11];
  char participant_id[11];
  char account_id[13];
  int32_t key_id;
  char current_key[21];
} ctp_cfmmc_trading_account_key;

typedef struct ctp_cancel_offset_setting {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char underlying_instr_id[81];
  char product_id[41];
  char offset_type;
  int32_t volume;
  int32_t is_offset;
  int32_t request_id;
  char user_id[16];
  char exchange_id[9];
  char ip_address[33];
  char mac_address[21];
  char exchange_inst_id[81];
  char exchange_serial_no[81];
  char exchange_product_id[41];
  char trader_id[21];
  int32_t install_id;
  char participant_id[11];
  char client_id[11];
  char order_action_status;
  char status_msg[81];
  char action_local_id[13];
  char action_date[9];
  char action_time[9];
} ctp_cancel_offset_setting;

typedef struct ctp_comb_action {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char comb_action_ref[13];
  char user_id[16];
  char direction;
  int32_t volume;
  char comb_direction;
  char hedge_flag;
  char action_local_id[13];
  char exchange_id[9];
  char participant_id[11];
  char client_id[11];
  char reserve2[31];
  char trader_id[21];
  int32_t install_id;
  char action_status;
  int32_t notify_sequence;
  char trading_day[9];
  int32_t settlement_id;
  int32_t sequence_no;
  int32_t front_id;
  int32_t session_id;
  char user_product_info[11];
  char status_msg[81];
  char reserve3[16];
  char mac_address[21];
  char com_trade_id[21];
  char branch_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
  char exchange_inst_id[81];
  char ip_address[33];
} ctp_comb_action;

typedef struct ctp_comb_instrument_guard {
  char broker_id[11];
  char reserve1[31];
  double guarant_ratio;
  char exchange_id[9];
  char instrument_id[81];
} ctp_comb_instrument_guard;

typedef struct ctp_comb_leg {
  char comb_instrument_id[81];
  int32_t leg_id;
  char leg_instrument_id[81];
  char direction;
  int32_t leg_multiple;
  int32_t imply_level;
} ctp_comb_leg;

typedef struct ctp_comb_promotion_param {
  char exchange_id[9];
  char instrument_id[81];
  char comb_hedge_flag[5];
  double xparameter;
} ctp_comb_promotion_param;

typedef struct ctp_contract_bank {
  char broker_id[11];
  char bank_id[4];
  char bank_brch_id[5];
  char bank_name[101];
  char csrc_bank_id[4];
} ctp_contract_bank;

typedef struct ctp_e_warrant_offset {
  char trading_day[9];
  char broker_id[11];
  char investor_id[13];
  char exchange_id[9];
  char reserve1[31];
  char direction;
  char hedge_flag;
  int32_t volume;
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_e_warrant_offset;

typedef struct ctp_exchange {
  char exchange_id[9];
  char exchange_name[61];
  char exchange_property;
} ctp_exchange;

typedef struct ctp_exchange_margin_rate_adjust {
  char broker_id[11];
  char reserve1[31];
  char hedge_flag;
  double long_margin_ratio_by_money;
  double long_margin_ratio_by_volume;
  double short_margin_ratio_by_money;
  double short_margin_ratio_by_volume;
  double exch_long_margin_ratio_by_money;
  double exch_long_margin_ratio_by_volume;
  double exch_short_margin_ratio_by_money;
  double exch_short_margin_ratio_by_volume;
  double no_long_margin_ratio_by_money;
  double no_long_margin_ratio_by_volume;
  double no_short_margin_ratio_by_money;
  double no_short_margin_ratio_by_volume;
  char instrument_id[81];
} ctp_exchange_margin_rate_adjust;

typedef struct ctp_exchange_rate {
  char broker_id[11];
  char from_currency_id[4];
  double from_currency_unit;
  char to_currency_id[4];
  double exchange_rate;
} ctp_exchange_rate;

typedef struct ctp_exec_order_action {
  char broker_id[11];
  char investor_id[13];
  int32_t exec_order_action_ref;
  char exec_order_ref[13];
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char exchange_id[9];
  char exec_order_sys_id[21];
  char action_flag;
  char action_date[9];
  char action_time[9];
  char trader_id[21];
  int32_t install_id;
  char exec_order_local_id[13];
  char action_local_id[13];
  char participant_id[11];
  char client_id[11];
  char business_unit[21];
  char order_action_status;
  char user_id[16];
  char action_type;
  char status_msg[81];
  char reserve1[31];
  char branch_id[9];
  char invest_unit_id[17];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
} ctp_exec_order_action;

typedef struct ctp_exec_order {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exec_order_ref[13];
  char user_id[16];
  int32_t volume;
  int32_t request_id;
  char business_unit[21];
  char offset_flag;
  char hedge_flag;
  char action_type;
  char posi_direction;
  char reserve_position_flag;
  char close_flag;
  char exec_order_local_id[13];
  char exchange_id[9];
  char participant_id[11];
  char client_id[11];
  char reserve2[31];
  char trader_id[21];
  int32_t install_id;
  char order_submit_status;
  int32_t notify_sequence;
  char trading_day[9];
  int32_t settlement_id;
  char exec_order_sys_id[21];
  char insert_date[9];
  char insert_time[9];
  char cancel_time[9];
  char exec_result;
  char clearing_part_id[11];
  int32_t sequence_no;
  int32_t front_id;
  int32_t session_id;
  char user_product_info[11];
  char status_msg[81];
  char active_user_id[16];
  int32_t broker_exec_order_seq;
  char branch_id[9];
  char invest_unit_id[17];
  char account_id[13];
  char currency_id[4];
  char reserve3[16];
  char mac_address[21];
  char instrument_id[81];
  char exchange_inst_id[81];
  char ip_address[33];
} ctp_exec_order;

typedef struct ctp_fens_user_info {
  char broker_id[11];
  char user_id[16];
  char login_mode;
} ctp_fens_user_info;

typedef struct ctp_for_quote {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char for_quote_ref[13];
  char user_id[16];
  char for_quote_local_id[13];
  char exchange_id[9];
  char participant_id[11];
  char client_id[11];
  char reserve2[31];
  char trader_id[21];
  int32_t install_id;
  char insert_date[9];
  char insert_time[9];
  char for_quote_status;
  int32_t front_id;
  int32_t session_id;
  char status_msg[81];
  char active_user_id[16];
  int32_t broker_for_quto_seq;
  char invest_unit_id[17];
  char reserve3[16];
  char mac_address[21];
  char instrument_id[81];
  char exchange_inst_id[81];
  char ip_address[33];
} ctp_for_quote;

typedef struct ctp_for_quote_rsp {
  char trading_day[9];
  char reserve1[31];
  char for_quote_sys_id[21];
  char for_quote_time[9];
  char action_day[9];
  char exchange_id[9];
  char instrument_id[81];
} ctp_for_quote_rsp;

typedef struct ctp_front_info {
  char front_addr[101];
  int32_t qry_freq;
  int32_t ftd_pkg_freq;
} ctp_front_info;

typedef struct ctp_hedge_cfm_action {
  char broker_id[11];
  char investor_id[13];
  char action_date[9];
  char action_time[9];
  char trader_id[21];
  int32_t install_id;
  char order_local_id[13];
  char action_local_id[13];
  char participant_id[11];
  char client_id[11];
  char order_action_status;
  char user_id[16];
  char exchange_id[9];
  char order_sys_id[21];
  int32_t request_id;
  char status_msg[81];
  char order_ref[13];
  int32_t front_id;
  int32_t session_id;
  char ip_address[33];
  char mac_address[21];
} ctp_hedge_cfm_action;

typedef struct ctp_hedge_cfm {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char user_id[16];
  int32_t volume;
  char direction;
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char order_ref[13];
  char active_user_id[16];
  int32_t broker_order_seq;
  char order_sys_id[21];
  char apply_status;
  int32_t sequence_no;
  int32_t deal_volume;
  char insert_date[9];
  char insert_time[9];
  char cancel_time[9];
  char req_date[9];
  char order_local_id[13];
  char exchange_id[9];
  char participant_id[11];
  char client_id[11];
  char exchange_inst_id[81];
  char trader_id[21];
  int32_t install_id;
  char order_submit_status;
  int32_t notify_sequence;
  char trading_day[9];
  int32_t settlement_id;
  char status_msg[81];
  char ip_address[33];
  char mac_address[21];
} ctp_hedge_cfm;

typedef struct ctp_input_batch_order_action {
  char broker_id[11];
  char investor_id[13];
  int32_t order_action_ref;
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char exchange_id[9];
  char user_id[16];
  char invest_unit_id[17];
  char reserve1[16];
  char mac_address[21];
  char ip_address[33];
} ctp_input_batch_order_action;

typedef struct ctp_input_comb_action {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char comb_action_ref[13];
  char user_id[16];
  char direction;
  int32_t volume;
  char comb_direction;
  char hedge_flag;
  char exchange_id[9];
  char reserve2[16];
  char mac_address[21];
  char invest_unit_id[17];
  int32_t front_id;
  int32_t session_id;
  char instrument_id[81];
  char ip_address[33];
} ctp_input_comb_action;

typedef struct ctp_input_exec_order_action {
  char broker_id[11];
  char investor_id[13];
  int32_t exec_order_action_ref;
  char exec_order_ref[13];
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char exchange_id[9];
  char exec_order_sys_id[21];
  char action_flag;
  char user_id[16];
  char reserve1[31];
  char invest_unit_id[17];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
} ctp_input_exec_order_action;

typedef struct ctp_input_exec_order {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exec_order_ref[13];
  char user_id[16];
  int32_t volume;
  int32_t request_id;
  char business_unit[21];
  char offset_flag;
  char hedge_flag;
  char action_type;
  char posi_direction;
  char reserve_position_flag;
  char close_flag;
  char exchange_id[9];
  char invest_unit_id[17];
  char account_id[13];
  char currency_id[4];
  char client_id[11];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
} ctp_input_exec_order;

typedef struct ctp_input_for_quote {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char for_quote_ref[13];
  char user_id[16];
  char exchange_id[9];
  char invest_unit_id[17];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
} ctp_input_for_quote;

typedef struct ctp_input_hedge_cfm_action {
  char broker_id[11];
  char user_id[16];
  char investor_id[13];
  char exchange_id[9];
  char order_sys_id[21];
  char order_ref[13];
  int32_t front_id;
  int32_t session_id;
  int32_t request_id;
  char ip_address[33];
  char mac_address[21];
} ctp_input_hedge_cfm_action;

typedef struct ctp_input_hedge_cfm {
  char broker_id[11];
  char user_id[16];
  char investor_id[13];
  char exchange_id[9];
  char instrument_id[81];
  int32_t volume;
  char direction;
  int32_t request_id;
  char order_ref[13];
  char ip_address[33];
  char mac_address[21];
} ctp_input_hedge_cfm;

typedef struct ctp_input_offset_setting {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char underlying_instr_id[81];
  char product_id[41];
  char offset_type;
  int32_t volume;
  int32_t is_offset;
  int32_t request_id;
  char user_id[16];
  char exchange_id[9];
  char ip_address[33];
  char mac_address[21];
} ctp_input_offset_setting;

typedef struct ctp_input_option_self_close_action {
  char broker_id[11];
  char investor_id[13];
  int32_t option_self_close_action_ref;
  char option_self_close_ref[13];
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char exchange_id[9];
  char option_self_close_sys_id[21];
  char action_flag;
  char user_id[16];
  char reserve1[31];
  char invest_unit_id[17];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
} ctp_input_option_self_close_action;

typedef struct ctp_input_option_self_close {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char option_self_close_ref[13];
  char user_id[16];
  int32_t volume;
  int32_t request_id;
  char business_unit[21];
  char hedge_flag;
  char opt_self_close_flag;
  char exchange_id[9];
  char invest_unit_id[17];
  char account_id[13];
  char currency_id[4];
  char client_id[11];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
} ctp_input_option_self_close;

typedef struct ctp_input_quote_action {
  char broker_id[11];
  char investor_id[13];
  int32_t quote_action_ref;
  char quote_ref[13];
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char exchange_id[9];
  char quote_sys_id[21];
  char action_flag;
  char user_id[16];
  char reserve1[31];
  char invest_unit_id[17];
  char client_id[11];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
  char order_memo[13];
  int32_t session_req_seq;
} ctp_input_quote_action;

typedef struct ctp_input_quote {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char quote_ref[13];
  char user_id[16];
  double ask_price;
  double bid_price;
  int32_t ask_volume;
  int32_t bid_volume;
  int32_t request_id;
  char business_unit[21];
  char ask_offset_flag;
  char bid_offset_flag;
  char ask_hedge_flag;
  char bid_hedge_flag;
  char ask_order_ref[13];
  char bid_order_ref[13];
  char for_quote_sys_id[21];
  char exchange_id[9];
  char invest_unit_id[17];
  char client_id[11];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
  char replace_sys_id[21];
  char time_condition;
  char order_memo[13];
  int32_t session_req_seq;
} ctp_input_quote;

typedef struct ctp_input_spd_apply_action {
  char broker_id[11];
  char user_id[16];
  char investor_id[13];
  char exchange_id[9];
  char order_sys_id[21];
  char order_ref[13];
  int32_t front_id;
  int32_t session_id;
  int32_t request_id;
  char ip_address[33];
  char mac_address[21];
} ctp_input_spd_apply_action;

typedef struct ctp_input_spd_apply {
  char broker_id[11];
  char user_id[16];
  char investor_id[13];
  char exchange_id[9];
  char first_leg_instrument_id[81];
  char second_leg_instrument_id[81];
  int32_t volume;
  char direction;
  char cmb_type;
  int32_t request_id;
  char order_ref[13];
  char ip_address[33];
  char mac_address[21];
} ctp_input_spd_apply;

typedef struct ctp_instrument {
  char reserve1[31];
  char exchange_id[9];
  char instrument_name[21];
  char reserve2[31];
  char reserve3[31];
  char product_class;
  int32_t delivery_year;
  int32_t delivery_month;
  int32_t max_market_order_volume;
  int32_t min_market_order_volume;
  int32_t max_limit_order_volume;
  int32_t min_limit_order_volume;
  int32_t volume_multiple;
  double price_tick;
  char create_date[9];
  char open_date[9];
  char expire_date[9];
  char start_deliv_date[9];
  char end_deliv_date[9];
  char inst_life_phase;
  int32_t is_trading;
  char position_type;
  char position_date_type;
  double long_margin_ratio;
  double short_margin_ratio;
  char max_margin_side_algorithm;
  char reserve4[31];
  double strike_price;
  char options_type;
  double underlying_multiple;
  char combination_type;
  char instrument_id[81];
  char exchange_inst_id[81];
  char product_id[81];
  char underlying_instr_id[81];
} ctp_instrument;

typedef struct ctp_instrument_order_comm_rate {
  char reserve1[31];
  char investor_range;
  char broker_id[11];
  char investor_id[13];
  char hedge_flag;
  double order_comm_by_volume;
  double order_action_comm_by_volume;
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
  double order_comm_by_trade;
  double order_action_comm_by_trade;
} ctp_instrument_order_comm_rate;

typedef struct ctp_invest_unit {
  char broker_id[11];
  char investor_id[13];
  char invest_unit_id[17];
  char investor_unit_name[81];
  char investor_group_id[13];
  char comm_model_id[13];
  char margin_model_id[13];
  char account_id[13];
  char currency_id[4];
} ctp_invest_unit;

typedef struct ctp_investor_commodity_group_spmm_margin {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char commodity_group_id[41];
  double margin_before_discount;
  double margin_no_discount;
  double long_risk;
  double short_risk;
  double close_frozen_margin;
  double inter_commodity_rate;
  double mini_margin_ratio;
  double adjust_ratio;
  double intra_commodity_discount;
  double inter_commodity_discount;
  double exch_margin;
  double investor_margin;
  double frozen_commission;
  double commission;
  double frozen_cash;
  double cash_in;
  double strike_frozen_margin;
} ctp_investor_commodity_group_spmm_margin;

typedef struct ctp_investor_commodity_spmm_margin {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char commodity_id[41];
  double margin_before_discount;
  double margin_no_discount;
  double long_pos_risk;
  double long_open_frozen_risk;
  double long_close_frozen_risk;
  double short_pos_risk;
  double short_open_frozen_risk;
  double short_close_frozen_risk;
  double intra_commodity_rate;
  double option_discount_rate;
  double pos_discount;
  double open_frozen_discount;
  double net_risk;
  double close_frozen_margin;
  double frozen_commission;
  double commission;
  double frozen_cash;
  double cash_in;
  double strike_frozen_margin;
} ctp_investor_commodity_spmm_margin;

typedef struct ctp_investor {
  char investor_id[13];
  char broker_id[11];
  char investor_group_id[13];
  char investor_name[81];
  char identified_card_type;
  char identified_card_no[51];
  int32_t is_active;
  char telephone[41];
  char address[101];
  char open_date[9];
  char mobile[41];
  char comm_model_id[13];
  char margin_model_id[13];
  char is_order_freq;
  char is_open_vol_limit;
} ctp_investor;

typedef struct ctp_investor_info_comm_rec {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  int32_t order_count;
  int32_t order_action_count;
  int32_t for_quote_cnt;
  double info_comm;
  int32_t is_opt_series;
  char product_id[41];
  int32_t info_cnt;
} ctp_investor_info_comm_rec;

typedef struct ctp_investor_portf_margin_ratio {
  char investor_range;
  char broker_id[11];
  char investor_id[13];
  char exchange_id[9];
  double margin_ratio;
  char product_group_id[41];
} ctp_investor_portf_margin_ratio;

typedef struct ctp_investor_portf_setting {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char hedge_flag;
  int32_t use_portf;
} ctp_investor_portf_setting;

typedef struct ctp_investor_position_combine_detail {
  char trading_day[9];
  char open_date[9];
  char exchange_id[9];
  int32_t settlement_id;
  char broker_id[11];
  char investor_id[13];
  char com_trade_id[21];
  char trade_id[21];
  char reserve1[31];
  char hedge_flag;
  char direction;
  int32_t total_amt;
  double margin;
  double exch_margin;
  double margin_rate_by_money;
  double margin_rate_by_volume;
  int32_t leg_id;
  int32_t leg_multiple;
  char reserve2[31];
  int32_t trade_group_id;
  char invest_unit_id[17];
  char instrument_id[81];
  char comb_instrument_id[81];
} ctp_investor_position_combine_detail;

typedef struct ctp_investor_position_detail {
  char reserve1[31];
  char broker_id[11];
  char investor_id[13];
  char hedge_flag;
  char direction;
  char open_date[9];
  char trade_id[21];
  int32_t volume;
  double open_price;
  char trading_day[9];
  int32_t settlement_id;
  char trade_type;
  char reserve2[31];
  char exchange_id[9];
  double close_profit_by_date;
  double close_profit_by_trade;
  double position_profit_by_date;
  double position_profit_by_trade;
  double margin;
  double exch_margin;
  double margin_rate_by_money;
  double margin_rate_by_volume;
  double last_settlement_price;
  double settlement_price;
  int32_t close_volume;
  double close_amount;
  int32_t time_first_volume;
  char invest_unit_id[17];
  char spec_posi_type;
  char instrument_id[81];
  char comb_instrument_id[81];
} ctp_investor_position_detail;

typedef struct ctp_investor_prod_rcams_margin {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char comb_product_id[41];
  char hedge_flag;
  char product_group_id[41];
  double risk_before_discount;
  double intra_instr_risk;
  double b_pos_risk;
  double s_pos_risk;
  double intra_prod_risk;
  double net_risk;
  double inter_prod_risk;
  double short_opt_risk_adj;
  double option_royalty;
  double mmsa_close_frozen_margin;
  double close_comb_frozen_margin;
  double close_frozen_margin;
  double mmsa_open_frozen_margin;
  double delivery_open_frozen_margin;
  double open_frozen_margin;
  double use_frozen_margin;
  double mmsa_exch_margin;
  double delivery_exch_margin;
  double comb_exch_margin;
  double exch_margin;
  double use_margin;
} ctp_investor_prod_rcams_margin;

typedef struct ctp_investor_prod_rule_margin {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char prod_family_code[81];
  char instrument_class;
  int32_t commodity_group_id;
  double b_std_position;
  double s_std_position;
  double b_std_open_frozen;
  double s_std_open_frozen;
  double b_std_close_frozen;
  double s_std_close_frozen;
  double intra_prod_std_position;
  double net_std_position;
  double inter_prod_std_position;
  double single_std_position;
  double intra_prod_margin;
  double inter_prod_margin;
  double single_margin;
  double non_comb_margin;
  double add_on_margin;
  double exch_margin;
  double add_on_frozen_margin;
  double open_frozen_margin;
  double close_frozen_margin;
  double margin;
  double frozen_margin;
} ctp_investor_prod_rule_margin;

typedef struct ctp_investor_prod_spbm_detail {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char prod_family_code[81];
  double intra_instr_margin;
  double b_collecting_margin;
  double s_collecting_margin;
  double intra_prod_margin;
  double net_margin;
  double inter_prod_margin;
  double single_margin;
  double add_on_margin;
  double delivery_margin;
  double call_option_min_risk;
  double put_option_min_risk;
  double option_min_risk;
  double option_value_offset;
  double option_royalty;
  double real_option_value_offset;
  double margin;
  double exch_margin;
} ctp_investor_prod_spbm_detail;

typedef struct ctp_investor_product_group_margin {
  char reserve1[31];
  char broker_id[11];
  char investor_id[13];
  char trading_day[9];
  int32_t settlement_id;
  double frozen_margin;
  double long_frozen_margin;
  double short_frozen_margin;
  double use_margin;
  double long_use_margin;
  double short_use_margin;
  double exch_margin;
  double long_exch_margin;
  double short_exch_margin;
  double close_profit;
  double frozen_commission;
  double commission;
  double frozen_cash;
  double cash_in;
  double position_profit;
  double offset_amount;
  double long_offset_amount;
  double short_offset_amount;
  double exch_offset_amount;
  double long_exch_offset_amount;
  double short_exch_offset_amount;
  char hedge_flag;
  char exchange_id[9];
  char invest_unit_id[17];
  char product_group_id[81];
} ctp_investor_product_group_margin;

typedef struct ctp_mm_instrument_commission_rate {
  char reserve1[31];
  char investor_range;
  char broker_id[11];
  char investor_id[13];
  double open_ratio_by_money;
  double open_ratio_by_volume;
  double close_ratio_by_money;
  double close_ratio_by_volume;
  double close_today_ratio_by_money;
  double close_today_ratio_by_volume;
  char instrument_id[81];
} ctp_mm_instrument_commission_rate;

typedef struct ctp_mm_option_instr_comm_rate {
  char reserve1[31];
  char investor_range;
  char broker_id[11];
  char investor_id[13];
  double open_ratio_by_money;
  double open_ratio_by_volume;
  double close_ratio_by_money;
  double close_ratio_by_volume;
  double close_today_ratio_by_money;
  double close_today_ratio_by_volume;
  double strike_ratio_by_money;
  double strike_ratio_by_volume;
  char instrument_id[81];
} ctp_mm_option_instr_comm_rate;

typedef struct ctp_notice {
  char broker_id[11];
  char content[501];
  char sequence_label[2];
} ctp_notice;

typedef struct ctp_notify_query_account {
  char trade_code[7];
  char bank_id[4];
  char bank_branch_id[5];
  char broker_id[11];
  char broker_branch_id[31];
  char trade_date[9];
  char trade_time[9];
  char bank_serial[13];
  char trading_day[9];
  int32_t plate_serial;
  char last_fragment;
  int32_t session_id;
  char customer_name[51];
  char id_card_type;
  char identified_card_no[51];
  char cust_type;
  char bank_account[41];
  char bank_pass_word[41];
  char account_id[13];
  char password[41];
  int32_t future_serial;
  int32_t install_id;
  char user_id[16];
  char verify_cert_no_flag;
  char currency_id[4];
  char digest[36];
  char bank_acc_type;
  char device_id[3];
  char bank_secu_acc_type;
  char broker_id_by_bank[33];
  char bank_secu_acc[41];
  char bank_pwd_flag;
  char secu_pwd_flag;
  char oper_no[17];
  int32_t request_id;
  int32_t t_id;
  double bank_use_amount;
  double bank_fetch_amount;
  int32_t error_id;
  char error_msg[81];
  char long_customer_name[161];
} ctp_notify_query_account;

typedef struct ctp_offset_setting {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char underlying_instr_id[81];
  char product_id[41];
  char offset_type;
  int32_t volume;
  int32_t is_offset;
  int32_t request_id;
  char user_id[16];
  char exchange_id[9];
  char ip_address[33];
  char mac_address[21];
  char exchange_inst_id[81];
  char exchange_serial_no[81];
  char exchange_product_id[41];
  char participant_id[11];
  char client_id[11];
  char trader_id[21];
  int32_t install_id;
  char order_submit_status;
  char trading_day[9];
  int32_t settlement_id;
  char insert_date[9];
  char insert_time[9];
  char cancel_time[9];
  char exec_result;
  int32_t sequence_no;
  int32_t front_id;
  int32_t session_id;
  char status_msg[81];
  char active_user_id[16];
  int32_t broker_offset_setting_seq;
  char apply_src;
} ctp_offset_setting;

typedef struct ctp_option_instr_comm_rate {
  char reserve1[31];
  char investor_range;
  char broker_id[11];
  char investor_id[13];
  double open_ratio_by_money;
  double open_ratio_by_volume;
  double close_ratio_by_money;
  double close_ratio_by_volume;
  double close_today_ratio_by_money;
  double close_today_ratio_by_volume;
  double strike_ratio_by_money;
  double strike_ratio_by_volume;
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_option_instr_comm_rate;

typedef struct ctp_option_instr_trade_cost {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char hedge_flag;
  double fixed_margin;
  double mini_margin;
  double royalty;
  double exch_fixed_margin;
  double exch_mini_margin;
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_option_instr_trade_cost;

typedef struct ctp_option_self_close_action {
  char broker_id[11];
  char investor_id[13];
  int32_t option_self_close_action_ref;
  char option_self_close_ref[13];
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char exchange_id[9];
  char option_self_close_sys_id[21];
  char action_flag;
  char action_date[9];
  char action_time[9];
  char trader_id[21];
  int32_t install_id;
  char option_self_close_local_id[13];
  char action_local_id[13];
  char participant_id[11];
  char client_id[11];
  char business_unit[21];
  char order_action_status;
  char user_id[16];
  char status_msg[81];
  char reserve1[31];
  char branch_id[9];
  char invest_unit_id[17];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
} ctp_option_self_close_action;

typedef struct ctp_option_self_close {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char option_self_close_ref[13];
  char user_id[16];
  int32_t volume;
  int32_t request_id;
  char business_unit[21];
  char hedge_flag;
  char opt_self_close_flag;
  char option_self_close_local_id[13];
  char exchange_id[9];
  char participant_id[11];
  char client_id[11];
  char reserve2[31];
  char trader_id[21];
  int32_t install_id;
  char order_submit_status;
  int32_t notify_sequence;
  char trading_day[9];
  int32_t settlement_id;
  char option_self_close_sys_id[21];
  char insert_date[9];
  char insert_time[9];
  char cancel_time[9];
  char exec_result;
  char clearing_part_id[11];
  int32_t sequence_no;
  int32_t front_id;
  int32_t session_id;
  char user_product_info[11];
  char status_msg[81];
  char active_user_id[16];
  int32_t broker_option_self_close_seq;
  char branch_id[9];
  char invest_unit_id[17];
  char account_id[13];
  char currency_id[4];
  char reserve3[16];
  char mac_address[21];
  char instrument_id[81];
  char exchange_inst_id[81];
  char ip_address[33];
} ctp_option_self_close;

typedef struct ctp_order_action {
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
  char action_date[9];
  char action_time[9];
  char trader_id[21];
  int32_t install_id;
  char order_local_id[13];
  char action_local_id[13];
  char participant_id[11];
  char client_id[11];
  char business_unit[21];
  char order_action_status;
  char user_id[16];
  char status_msg[81];
  char reserve1[31];
  char branch_id[9];
  char invest_unit_id[17];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
  char order_memo[13];
  int32_t session_req_seq;
} ctp_order_action;

typedef struct ctp_parked_order_action {
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
  char reserve1[31];
  char parked_order_action_id[13];
  char user_type;
  char status;
  int32_t error_id;
  char error_msg[81];
  char invest_unit_id[17];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
} ctp_parked_order_action;

typedef struct ctp_parked_order {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
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
  char exchange_id[9];
  char parked_order_id[13];
  char user_type;
  char status;
  int32_t error_id;
  char error_msg[81];
  int32_t is_swap_order;
  char account_id[13];
  char currency_id[4];
  char client_id[11];
  char invest_unit_id[17];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
} ctp_parked_order;

typedef struct ctp_product_exch_rate {
  char reserve1[31];
  char quote_currency_id[4];
  double exchange_rate;
  char exchange_id[9];
  char product_id[81];
} ctp_product_exch_rate;

typedef struct ctp_product {
  char reserve1[31];
  char product_name[21];
  char exchange_id[9];
  char product_class;
  int32_t volume_multiple;
  double price_tick;
  int32_t max_market_order_volume;
  int32_t min_market_order_volume;
  int32_t max_limit_order_volume;
  int32_t min_limit_order_volume;
  char position_type;
  char position_date_type;
  char close_deal_type;
  char trade_currency_id[4];
  char mortgage_fund_use_range;
  char reserve2[31];
  double underlying_multiple;
  char product_id[81];
  char exchange_product_id[81];
  char open_limit_control_level;
  char order_freq_control_level;
} ctp_product;

typedef struct ctp_product_group {
  char reserve1[31];
  char exchange_id[9];
  char reserve2[31];
  char product_id[81];
  char product_group_id[81];
} ctp_product_group;

typedef struct ctp_qry_accountregister {
  char broker_id[11];
  char account_id[13];
  char bank_id[4];
  char bank_branch_id[5];
  char currency_id[4];
} ctp_qry_accountregister;

typedef struct ctp_qry_broker_trading_algos {
  char broker_id[11];
  char exchange_id[9];
  char reserve1[31];
  char instrument_id[81];
} ctp_qry_broker_trading_algos;

typedef struct ctp_qry_broker_trading_params {
  char broker_id[11];
  char investor_id[13];
  char currency_id[4];
  char account_id[13];
} ctp_qry_broker_trading_params;

typedef struct ctp_qry_cfmmc_trading_account_key {
  char broker_id[11];
  char investor_id[13];
} ctp_qry_cfmmc_trading_account_key;

typedef struct ctp_qry_classified_instrument {
  char instrument_id[81];
  char exchange_id[9];
  char exchange_inst_id[81];
  char product_id[81];
  char trading_type;
  char class_type;
} ctp_qry_classified_instrument;

typedef struct ctp_qry_comb_action {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_comb_action;

typedef struct ctp_qry_comb_instrument_guard {
  char broker_id[11];
  char reserve1[31];
  char exchange_id[9];
  char instrument_id[81];
} ctp_qry_comb_instrument_guard;

typedef struct ctp_qry_comb_leg {
  char leg_instrument_id[81];
} ctp_qry_comb_leg;

typedef struct ctp_qry_comb_promotion_param {
  char exchange_id[9];
  char instrument_id[81];
} ctp_qry_comb_promotion_param;

typedef struct ctp_qry_contract_bank {
  char broker_id[11];
  char bank_id[4];
  char bank_brch_id[5];
} ctp_qry_contract_bank;

typedef struct ctp_qry_depth_market_data {
  char reserve1[31];
  char exchange_id[9];
  char instrument_id[81];
  char product_class;
} ctp_qry_depth_market_data;

typedef struct ctp_qry_e_warrant_offset {
  char broker_id[11];
  char investor_id[13];
  char exchange_id[9];
  char reserve1[31];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_e_warrant_offset;

typedef struct ctp_qry_exchange {
  char exchange_id[9];
} ctp_qry_exchange;

typedef struct ctp_qry_exchange_margin_rate_adjust {
  char broker_id[11];
  char reserve1[31];
  char hedge_flag;
  char instrument_id[81];
} ctp_qry_exchange_margin_rate_adjust;

typedef struct ctp_qry_exchange_rate {
  char broker_id[11];
  char from_currency_id[4];
  char to_currency_id[4];
} ctp_qry_exchange_rate;

typedef struct ctp_qry_exec_order {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char exec_order_sys_id[21];
  char insert_time_start[9];
  char insert_time_end[9];
  char instrument_id[81];
} ctp_qry_exec_order;

typedef struct ctp_qry_for_quote {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char insert_time_start[9];
  char insert_time_end[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_for_quote;

typedef struct ctp_qry_hedge_cfm {
  char broker_id[11];
  char investor_id[13];
  char exchange_id[9];
  char order_sys_id[21];
  char instrument_id[81];
} ctp_qry_hedge_cfm;

typedef struct ctp_qry_instrument {
  char reserve1[31];
  char exchange_id[9];
  char reserve2[31];
  char reserve3[31];
  char instrument_id[81];
  char exchange_inst_id[81];
  char product_id[81];
} ctp_qry_instrument;

typedef struct ctp_qry_instrument_order_comm_rate {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char instrument_id[81];
} ctp_qry_instrument_order_comm_rate;

typedef struct ctp_qry_invest_unit {
  char broker_id[11];
  char investor_id[13];
  char invest_unit_id[17];
} ctp_qry_invest_unit;

typedef struct ctp_qry_investor_commodity_group_spmm_margin {
  char broker_id[11];
  char investor_id[13];
  char commodity_group_id[41];
} ctp_qry_investor_commodity_group_spmm_margin;

typedef struct ctp_qry_investor_commodity_spmm_margin {
  char broker_id[11];
  char investor_id[13];
  char commodity_id[41];
} ctp_qry_investor_commodity_spmm_margin;

typedef struct ctp_qry_investor {
  char broker_id[11];
  char investor_id[13];
} ctp_qry_investor;

typedef struct ctp_qry_investor_info_comm_rec {
  char investor_id[13];
  char instrument_id[81];
  char broker_id[11];
} ctp_qry_investor_info_comm_rec;

typedef struct ctp_qry_investor_portf_margin_ratio {
  char broker_id[11];
  char investor_id[13];
  char exchange_id[9];
  char product_group_id[41];
} ctp_qry_investor_portf_margin_ratio;

typedef struct ctp_qry_investor_portf_setting {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
} ctp_qry_investor_portf_setting;

typedef struct ctp_qry_investor_position_combine_detail {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char invest_unit_id[17];
  char comb_instrument_id[81];
} ctp_qry_investor_position_combine_detail;

typedef struct ctp_qry_investor_position_detail {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_investor_position_detail;

typedef struct ctp_qry_investor_prod_rcams_margin {
  char broker_id[11];
  char investor_id[13];
  char comb_product_id[41];
  char product_group_id[41];
} ctp_qry_investor_prod_rcams_margin;

typedef struct ctp_qry_investor_prod_rule_margin {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char prod_family_code[81];
  int32_t commodity_group_id;
} ctp_qry_investor_prod_rule_margin;

typedef struct ctp_qry_investor_prod_spbm_detail {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char prod_family_code[81];
} ctp_qry_investor_prod_spbm_detail;

typedef struct ctp_qry_investor_product_group_margin {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char hedge_flag;
  char exchange_id[9];
  char invest_unit_id[17];
  char product_group_id[81];
} ctp_qry_investor_product_group_margin;

typedef struct ctp_qry_mm_instrument_commission_rate {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char instrument_id[81];
} ctp_qry_mm_instrument_commission_rate;

typedef struct ctp_qry_mm_option_instr_comm_rate {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char instrument_id[81];
} ctp_qry_mm_option_instr_comm_rate;

typedef struct ctp_qry_max_order_volume {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char direction;
  char offset_flag;
  char hedge_flag;
  int32_t max_volume;
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_max_order_volume;

typedef struct ctp_qry_notice {
  char broker_id[11];
} ctp_qry_notice;

typedef struct ctp_qry_offset_setting {
  char broker_id[11];
  char investor_id[13];
  char product_id[41];
  char offset_type;
} ctp_qry_offset_setting;

typedef struct ctp_qry_option_instr_comm_rate {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_option_instr_comm_rate;

typedef struct ctp_qry_option_instr_trade_cost {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char hedge_flag;
  double input_price;
  double underlying_price;
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_option_instr_trade_cost;

typedef struct ctp_qry_option_self_close {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char option_self_close_sys_id[21];
  char insert_time_start[9];
  char insert_time_end[9];
  char instrument_id[81];
} ctp_qry_option_self_close;

typedef struct ctp_qry_order {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char order_sys_id[21];
  char insert_time_start[9];
  char insert_time_end[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_order;

typedef struct ctp_qry_parked_order_action {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_parked_order_action;

typedef struct ctp_qry_parked_order {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_parked_order;

typedef struct ctp_qry_product_exch_rate {
  char reserve1[31];
  char exchange_id[9];
  char product_id[81];
} ctp_qry_product_exch_rate;

typedef struct ctp_qry_product {
  char reserve1[31];
  char product_class;
  char exchange_id[9];
  char product_id[81];
} ctp_qry_product;

typedef struct ctp_qry_product_group {
  char reserve1[31];
  char exchange_id[9];
  char product_id[81];
} ctp_qry_product_group;

typedef struct ctp_qry_quote {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char quote_sys_id[21];
  char insert_time_start[9];
  char insert_time_end[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_quote;

typedef struct ctp_qry_rcams_comb_product_info {
  char product_id[41];
  char comb_product_id[41];
  char product_group_id[41];
} ctp_qry_rcams_comb_product_info;

typedef struct ctp_qry_rcams_instr_parameter {
  char product_id[41];
} ctp_qry_rcams_instr_parameter;

typedef struct ctp_qry_rcams_inter_parameter {
  char product_group_id[41];
  char comb_product1[41];
  char comb_product2[41];
} ctp_qry_rcams_inter_parameter;

typedef struct ctp_qry_rcams_intra_parameter {
  char comb_product_id[41];
} ctp_qry_rcams_intra_parameter;

typedef struct ctp_qry_rcams_investor_comb_position {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char comb_instrument_id[81];
} ctp_qry_rcams_investor_comb_position;

typedef struct ctp_qry_rcams_short_opt_adjust_param {
  char comb_product_id[41];
} ctp_qry_rcams_short_opt_adjust_param;

typedef struct ctp_qry_rule_instr_parameter {
  char exchange_id[9];
  char instrument_id[81];
} ctp_qry_rule_instr_parameter;

typedef struct ctp_qry_rule_inter_parameter {
  char exchange_id[9];
  char leg1_prod_family_code[81];
  char leg2_prod_family_code[81];
  int32_t commodity_group_id;
} ctp_qry_rule_inter_parameter;

typedef struct ctp_qry_rule_intra_parameter {
  char exchange_id[9];
  char prod_family_code[81];
} ctp_qry_rule_intra_parameter;

typedef struct ctp_qry_risk_settle_invst_position {
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
} ctp_qry_risk_settle_invst_position;

typedef struct ctp_qry_risk_settle_product_status {
  char product_id[81];
} ctp_qry_risk_settle_product_status;

typedef struct ctp_qry_spbm_add_on_inter_parameter {
  char exchange_id[9];
  char leg1_prod_family_code[81];
  char leg2_prod_family_code[81];
} ctp_qry_spbm_add_on_inter_parameter;

typedef struct ctp_qry_spbm_future_parameter {
  char exchange_id[9];
  char instrument_id[81];
  char prod_family_code[81];
} ctp_qry_spbm_future_parameter;

typedef struct ctp_qry_spbm_inter_parameter {
  char exchange_id[9];
  char leg1_prod_family_code[81];
  char leg2_prod_family_code[81];
} ctp_qry_spbm_inter_parameter;

typedef struct ctp_qry_spbm_intra_parameter {
  char exchange_id[9];
  char prod_family_code[81];
} ctp_qry_spbm_intra_parameter;

typedef struct ctp_qry_spbm_investor_portf_def {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
} ctp_qry_spbm_investor_portf_def;

typedef struct ctp_qry_spbm_option_parameter {
  char exchange_id[9];
  char instrument_id[81];
  char prod_family_code[81];
} ctp_qry_spbm_option_parameter;

typedef struct ctp_qry_spbm_portf_definition {
  char exchange_id[9];
  int32_t portfolio_def_id;
  char prod_family_code[81];
} ctp_qry_spbm_portf_definition;

typedef struct ctp_qry_spmm_inst_param {
  char instrument_id[81];
} ctp_qry_spmm_inst_param;

typedef struct ctp_qry_spmm_product_param {
  char product_id[41];
} ctp_qry_spmm_product_param;

typedef struct ctp_qry_sec_agent_ac_id_map {
  char broker_id[11];
  char user_id[16];
  char account_id[13];
  char currency_id[4];
} ctp_qry_sec_agent_ac_id_map;

typedef struct ctp_qry_sec_agent_check_mode {
  char broker_id[11];
  char investor_id[13];
} ctp_qry_sec_agent_check_mode;

typedef struct ctp_qry_sec_agent_trade_info {
  char broker_id[11];
  char broker_sec_agent_id[13];
} ctp_qry_sec_agent_trade_info;

typedef struct ctp_qry_settlement_info_confirm {
  char broker_id[11];
  char investor_id[13];
  char account_id[13];
  char currency_id[4];
} ctp_qry_settlement_info_confirm;

typedef struct ctp_qry_settlement_info {
  char broker_id[11];
  char investor_id[13];
  char trading_day[9];
  char account_id[13];
  char currency_id[4];
} ctp_qry_settlement_info;

typedef struct ctp_qry_spd_apply {
  char broker_id[11];
  char investor_id[13];
  char exchange_id[9];
  char order_sys_id[21];
  char first_leg_instrument_id[81];
  char second_leg_instrument_id[81];
} ctp_qry_spd_apply;

typedef struct ctp_qry_trade {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char exchange_id[9];
  char trade_id[21];
  char trade_time_start[9];
  char trade_time_end[9];
  char invest_unit_id[17];
  char instrument_id[81];
} ctp_qry_trade;

typedef struct ctp_qry_trader_offer {
  char exchange_id[9];
  char participant_id[11];
  char trader_id[21];
} ctp_qry_trader_offer;

typedef struct ctp_qry_trading_code {
  char broker_id[11];
  char investor_id[13];
  char exchange_id[9];
  char client_id[11];
  char client_id_type;
  char invest_unit_id[17];
} ctp_qry_trading_code;

typedef struct ctp_qry_trading_notice {
  char broker_id[11];
  char investor_id[13];
  char invest_unit_id[17];
} ctp_qry_trading_notice;

typedef struct ctp_qry_transfer_bank {
  char bank_id[4];
  char bank_brch_id[5];
} ctp_qry_transfer_bank;

typedef struct ctp_qry_transfer_serial {
  char broker_id[11];
  char account_id[13];
  char bank_id[4];
  char currency_id[4];
} ctp_qry_transfer_serial;

typedef struct ctp_qry_user_session {
  int32_t front_id;
  int32_t session_id;
  char broker_id[11];
  char user_id[16];
} ctp_qry_user_session;

typedef struct ctp_query_cfmmc_trading_account_token {
  char broker_id[11];
  char investor_id[13];
  char invest_unit_id[17];
} ctp_query_cfmmc_trading_account_token;

typedef struct ctp_quote_action {
  char broker_id[11];
  char investor_id[13];
  int32_t quote_action_ref;
  char quote_ref[13];
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char exchange_id[9];
  char quote_sys_id[21];
  char action_flag;
  char action_date[9];
  char action_time[9];
  char trader_id[21];
  int32_t install_id;
  char quote_local_id[13];
  char action_local_id[13];
  char participant_id[11];
  char client_id[11];
  char business_unit[21];
  char order_action_status;
  char user_id[16];
  char status_msg[81];
  char reserve1[31];
  char branch_id[9];
  char invest_unit_id[17];
  char reserve2[16];
  char mac_address[21];
  char instrument_id[81];
  char ip_address[33];
  char order_memo[13];
  int32_t session_req_seq;
} ctp_quote_action;

typedef struct ctp_quote {
  char broker_id[11];
  char investor_id[13];
  char reserve1[31];
  char quote_ref[13];
  char user_id[16];
  double ask_price;
  double bid_price;
  int32_t ask_volume;
  int32_t bid_volume;
  int32_t request_id;
  char business_unit[21];
  char ask_offset_flag;
  char bid_offset_flag;
  char ask_hedge_flag;
  char bid_hedge_flag;
  char quote_local_id[13];
  char exchange_id[9];
  char participant_id[11];
  char client_id[11];
  char reserve2[31];
  char trader_id[21];
  int32_t install_id;
  int32_t notify_sequence;
  char order_submit_status;
  char trading_day[9];
  int32_t settlement_id;
  char quote_sys_id[21];
  char insert_date[9];
  char insert_time[9];
  char cancel_time[9];
  char quote_status;
  char clearing_part_id[11];
  int32_t sequence_no;
  char ask_order_sys_id[21];
  char bid_order_sys_id[21];
  int32_t front_id;
  int32_t session_id;
  char user_product_info[11];
  char status_msg[81];
  char active_user_id[16];
  int32_t broker_quote_seq;
  char ask_order_ref[13];
  char bid_order_ref[13];
  char for_quote_sys_id[21];
  char branch_id[9];
  char invest_unit_id[17];
  char account_id[13];
  char currency_id[4];
  char reserve3[16];
  char mac_address[21];
  char instrument_id[81];
  char exchange_inst_id[81];
  char ip_address[33];
  char replace_sys_id[21];
  char time_condition;
  char order_memo[13];
  int32_t session_req_seq;
} ctp_quote;

typedef struct ctp_rcams_comb_product_info {
  char trading_day[9];
  char exchange_id[9];
  char product_id[41];
  char comb_product_id[41];
  char product_group_id[41];
} ctp_rcams_comb_product_info;

typedef struct ctp_rcams_instr_parameter {
  char trading_day[9];
  char exchange_id[9];
  char product_id[41];
  double hedge_rate;
} ctp_rcams_instr_parameter;

typedef struct ctp_rcams_inter_parameter {
  char trading_day[9];
  char exchange_id[9];
  char product_group_id[41];
  int32_t priority;
  double credit_rate;
  char comb_product1[41];
  char comb_product2[41];
} ctp_rcams_inter_parameter;

typedef struct ctp_rcams_intra_parameter {
  char trading_day[9];
  char exchange_id[9];
  char comb_product_id[41];
  double hedge_rate;
} ctp_rcams_intra_parameter;

typedef struct ctp_rcams_investor_comb_position {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  char instrument_id[81];
  char hedge_flag;
  char posi_direction;
  char comb_instrument_id[81];
  int32_t leg_id;
  char exchange_inst_id[81];
  int32_t total_amt;
  double exch_margin;
  double margin;
} ctp_rcams_investor_comb_position;

typedef struct ctp_rcams_short_opt_adjust_param {
  char trading_day[9];
  char exchange_id[9];
  char comb_product_id[41];
  char hedge_flag;
  double adjust_value;
} ctp_rcams_short_opt_adjust_param;

typedef struct ctp_rule_instr_parameter {
  char trading_day[9];
  char exchange_id[9];
  char instrument_id[81];
  char instrument_class;
  char std_instrument_id[81];
  double b_spec_ratio;
  double s_spec_ratio;
  double b_hedge_ratio;
  double s_hedge_ratio;
  double b_add_on_margin;
  double s_add_on_margin;
  int32_t commodity_group_id;
} ctp_rule_instr_parameter;

typedef struct ctp_rule_inter_parameter {
  char trading_day[9];
  char exchange_id[9];
  int32_t spread_id;
  double inter_rate;
  char leg1_prod_family_code[81];
  char leg2_prod_family_code[81];
  int32_t leg1_prop_factor;
  int32_t leg2_prop_factor;
  int32_t commodity_group_id;
  char commodity_group_name[21];
} ctp_rule_inter_parameter;

typedef struct ctp_rule_intra_parameter {
  char trading_day[9];
  char exchange_id[9];
  char prod_family_code[81];
  char std_instrument_id[81];
  double std_instr_margin;
  double usual_intra_rate;
  double delivery_intra_rate;
} ctp_rule_intra_parameter;

typedef struct ctp_remove_parked_order_action {
  char broker_id[11];
  char investor_id[13];
  char parked_order_action_id[13];
  char invest_unit_id[17];
} ctp_remove_parked_order_action;

typedef struct ctp_remove_parked_order {
  char broker_id[11];
  char investor_id[13];
  char parked_order_id[13];
  char invest_unit_id[17];
} ctp_remove_parked_order;

typedef struct ctp_req_gen_sms_code {
  char broker_id[11];
  char user_id[16];
  char mobile[17];
} ctp_req_gen_sms_code;

typedef struct ctp_req_gen_user_captcha {
  char trading_day[9];
  char broker_id[11];
  char user_id[16];
} ctp_req_gen_user_captcha;

typedef struct ctp_req_gen_user_text {
  char trading_day[9];
  char broker_id[11];
  char user_id[16];
} ctp_req_gen_user_text;

typedef struct ctp_req_query_account {
  char trade_code[7];
  char bank_id[4];
  char bank_branch_id[5];
  char broker_id[11];
  char broker_branch_id[31];
  char trade_date[9];
  char trade_time[9];
  char bank_serial[13];
  char trading_day[9];
  int32_t plate_serial;
  char last_fragment;
  int32_t session_id;
  char customer_name[51];
  char id_card_type;
  char identified_card_no[51];
  char cust_type;
  char bank_account[41];
  char bank_pass_word[41];
  char account_id[13];
  char password[41];
  int32_t future_serial;
  int32_t install_id;
  char user_id[16];
  char verify_cert_no_flag;
  char currency_id[4];
  char digest[36];
  char bank_acc_type;
  char device_id[3];
  char bank_secu_acc_type;
  char broker_id_by_bank[33];
  char bank_secu_acc[41];
  char bank_pwd_flag;
  char secu_pwd_flag;
  char oper_no[17];
  int32_t request_id;
  int32_t t_id;
  char long_customer_name[161];
} ctp_req_query_account;

typedef struct ctp_req_transfer {
  char trade_code[7];
  char bank_id[4];
  char bank_branch_id[5];
  char broker_id[11];
  char broker_branch_id[31];
  char trade_date[9];
  char trade_time[9];
  char bank_serial[13];
  char trading_day[9];
  int32_t plate_serial;
  char last_fragment;
  int32_t session_id;
  char customer_name[51];
  char id_card_type;
  char identified_card_no[51];
  char cust_type;
  char bank_account[41];
  char bank_pass_word[41];
  char account_id[13];
  char password[41];
  int32_t install_id;
  int32_t future_serial;
  char user_id[16];
  char verify_cert_no_flag;
  char currency_id[4];
  double trade_amount;
  double future_fetch_amount;
  char fee_pay_flag;
  double cust_fee;
  double broker_fee;
  char message[129];
  char digest[36];
  char bank_acc_type;
  char device_id[3];
  char bank_secu_acc_type;
  char broker_id_by_bank[33];
  char bank_secu_acc[41];
  char bank_pwd_flag;
  char secu_pwd_flag;
  char oper_no[17];
  int32_t request_id;
  int32_t t_id;
  char transfer_status;
  char long_customer_name[161];
} ctp_req_transfer;

typedef struct ctp_req_user_auth_method {
  char trading_day[9];
  char broker_id[11];
  char user_id[16];
} ctp_req_user_auth_method;

typedef struct ctp_req_user_login_with_captcha {
  char trading_day[9];
  char broker_id[11];
  char user_id[16];
  char password[41];
  char user_product_info[11];
  char interface_product_info[11];
  char protocol_info[11];
  char mac_address[21];
  char reserve1[16];
  char login_remark[36];
  char captcha[41];
  int32_t client_ip_port;
  char client_ip_address[33];
} ctp_req_user_login_with_captcha;

typedef struct ctp_req_user_login_with_otp {
  char trading_day[9];
  char broker_id[11];
  char user_id[16];
  char password[41];
  char user_product_info[11];
  char interface_product_info[11];
  char protocol_info[11];
  char mac_address[21];
  char reserve1[16];
  char login_remark[36];
  char otp_password[41];
  int32_t client_ip_port;
  char client_ip_address[33];
} ctp_req_user_login_with_otp;

typedef struct ctp_req_user_login_with_text {
  char trading_day[9];
  char broker_id[11];
  char user_id[16];
  char password[41];
  char user_product_info[11];
  char interface_product_info[11];
  char protocol_info[11];
  char mac_address[21];
  char reserve1[16];
  char login_remark[36];
  char text[41];
  int32_t client_ip_port;
  char client_ip_address[33];
} ctp_req_user_login_with_text;

typedef struct ctp_risk_settle_invst_position {
  char instrument_id[81];
  char broker_id[11];
  char investor_id[13];
  char posi_direction;
  char hedge_flag;
  char position_date;
  int32_t yd_position;
  int32_t position;
  int32_t long_frozen;
  int32_t short_frozen;
  double long_frozen_amount;
  double short_frozen_amount;
  int32_t open_volume;
  int32_t close_volume;
  double open_amount;
  double close_amount;
  double position_cost;
  double pre_margin;
  double use_margin;
  double frozen_margin;
  double frozen_cash;
  double frozen_commission;
  double cash_in;
  double commission;
  double close_profit;
  double position_profit;
  double pre_settlement_price;
  double settlement_price;
  char trading_day[9];
  int32_t settlement_id;
  double open_cost;
  double exchange_margin;
  int32_t comb_position;
  int32_t comb_long_frozen;
  int32_t comb_short_frozen;
  double close_profit_by_date;
  double close_profit_by_trade;
  int32_t today_position;
  double margin_rate_by_money;
  double margin_rate_by_volume;
  int32_t strike_frozen;
  double strike_frozen_amount;
  int32_t abandon_frozen;
  char exchange_id[9];
  int32_t yd_strike_frozen;
  char invest_unit_id[17];
  double position_cost_offset;
  int32_t tas_position;
  double tas_position_cost;
} ctp_risk_settle_invst_position;

typedef struct ctp_risk_settle_product_status {
  char exchange_id[9];
  char product_id[81];
  char product_status;
} ctp_risk_settle_product_status;

typedef struct ctp_rsp_gen_sms_code {
  char broker_id[11];
  char user_id[16];
  char gen_time[9];
} ctp_rsp_gen_sms_code;

typedef struct ctp_rsp_gen_user_captcha {
  char broker_id[11];
  char user_id[16];
  int32_t captcha_info_len;
  char captcha_info[2561];
} ctp_rsp_gen_user_captcha;

typedef struct ctp_rsp_gen_user_text {
  int32_t user_text_seq;
} ctp_rsp_gen_user_text;

typedef struct ctp_rsp_transfer {
  char trade_code[7];
  char bank_id[4];
  char bank_branch_id[5];
  char broker_id[11];
  char broker_branch_id[31];
  char trade_date[9];
  char trade_time[9];
  char bank_serial[13];
  char trading_day[9];
  int32_t plate_serial;
  char last_fragment;
  int32_t session_id;
  char customer_name[51];
  char id_card_type;
  char identified_card_no[51];
  char cust_type;
  char bank_account[41];
  char bank_pass_word[41];
  char account_id[13];
  char password[41];
  int32_t install_id;
  int32_t future_serial;
  char user_id[16];
  char verify_cert_no_flag;
  char currency_id[4];
  double trade_amount;
  double future_fetch_amount;
  char fee_pay_flag;
  double cust_fee;
  double broker_fee;
  char message[129];
  char digest[36];
  char bank_acc_type;
  char device_id[3];
  char bank_secu_acc_type;
  char broker_id_by_bank[33];
  char bank_secu_acc[41];
  char bank_pwd_flag;
  char secu_pwd_flag;
  char oper_no[17];
  int32_t request_id;
  int32_t t_id;
  char transfer_status;
  int32_t error_id;
  char error_msg[81];
  char long_customer_name[161];
} ctp_rsp_transfer;

typedef struct ctp_rsp_user_auth_method {
  int32_t usable_auth_method;
} ctp_rsp_user_auth_method;

typedef struct ctp_spbm_add_on_inter_parameter {
  char trading_day[9];
  char exchange_id[9];
  int32_t spread_id;
  double add_on_inter_rate_z2;
  char leg1_prod_family_code[81];
  char leg2_prod_family_code[81];
} ctp_spbm_add_on_inter_parameter;

typedef struct ctp_spbm_future_parameter {
  char trading_day[9];
  char exchange_id[9];
  char instrument_id[81];
  char prod_family_code[81];
  int32_t cvf;
  char time_range;
  double margin_rate;
  double lock_rate_x;
  double add_on_rate;
  double pre_settlement_price;
  double add_on_lock_rate_x2;
} ctp_spbm_future_parameter;

typedef struct ctp_spbm_inter_parameter {
  char trading_day[9];
  char exchange_id[9];
  int32_t spread_id;
  double inter_rate_z;
  char leg1_prod_family_code[81];
  char leg2_prod_family_code[81];
} ctp_spbm_inter_parameter;

typedef struct ctp_spbm_intra_parameter {
  char trading_day[9];
  char exchange_id[9];
  char prod_family_code[81];
  double intra_rate_y;
  double add_on_intra_rate_y2;
} ctp_spbm_intra_parameter;

typedef struct ctp_spbm_investor_portf_def {
  char exchange_id[9];
  char broker_id[11];
  char investor_id[13];
  int32_t portfolio_def_id;
} ctp_spbm_investor_portf_def;

typedef struct ctp_spbm_option_parameter {
  char trading_day[9];
  char exchange_id[9];
  char instrument_id[81];
  char prod_family_code[81];
  int32_t cvf;
  double down_price;
  double delta;
  double slimi_delta;
  double pre_settlement_price;
} ctp_spbm_option_parameter;

typedef struct ctp_spbm_portf_definition {
  char exchange_id[9];
  int32_t portfolio_def_id;
  char prod_family_code[81];
  int32_t is_spbm;
} ctp_spbm_portf_definition;

typedef struct ctp_spmm_inst_param {
  char exchange_id[9];
  char instrument_id[81];
  char inst_margin_cal_id;
  char commodity_id[41];
  char commodity_group_id[41];
} ctp_spmm_inst_param;

typedef struct ctp_spmm_product_param {
  char exchange_id[9];
  char product_id[41];
  char commodity_id[41];
  char commodity_group_id[41];
} ctp_spmm_product_param;

typedef struct ctp_sec_agent_ac_id_map {
  char broker_id[11];
  char user_id[16];
  char account_id[13];
  char currency_id[4];
  char broker_sec_agent_id[13];
} ctp_sec_agent_ac_id_map;

typedef struct ctp_sec_agent_check_mode {
  char investor_id[13];
  char broker_id[11];
  char currency_id[4];
  char broker_sec_agent_id[13];
  int32_t check_self_account;
} ctp_sec_agent_check_mode;

typedef struct ctp_sec_agent_trade_info {
  char broker_id[11];
  char broker_sec_agent_id[13];
  char investor_id[13];
  char long_customer_name[161];
} ctp_sec_agent_trade_info;

typedef struct ctp_settlement_info {
  char trading_day[9];
  int32_t settlement_id;
  char broker_id[11];
  char investor_id[13];
  int32_t sequence_no;
  char content[501];
  char account_id[13];
  char currency_id[4];
} ctp_settlement_info;

typedef struct ctp_spd_apply_action {
  char broker_id[11];
  char investor_id[13];
  char action_date[9];
  char action_time[9];
  char trader_id[21];
  int32_t install_id;
  char order_local_id[13];
  char action_local_id[13];
  char participant_id[11];
  char client_id[11];
  char order_action_status;
  char user_id[16];
  char exchange_id[9];
  char order_sys_id[21];
  int32_t request_id;
  char status_msg[81];
  char order_ref[13];
  int32_t front_id;
  int32_t session_id;
  char ip_address[33];
  char mac_address[21];
} ctp_spd_apply_action;

typedef struct ctp_spd_apply {
  char broker_id[11];
  char investor_id[13];
  char first_leg_instrument_id[81];
  char second_leg_instrument_id[81];
  char user_id[16];
  int32_t volume;
  char direction;
  int32_t request_id;
  int32_t front_id;
  int32_t session_id;
  char order_ref[13];
  char active_user_id[16];
  int32_t broker_order_seq;
  char order_sys_id[21];
  char apply_status;
  int32_t sequence_no;
  char insert_date[9];
  char insert_time[9];
  char cancel_time[9];
  char order_local_id[13];
  char exchange_id[9];
  char participant_id[11];
  char client_id[11];
  char exchange_inst_id[81];
  char trader_id[21];
  int32_t install_id;
  char order_submit_status;
  int32_t notify_sequence;
  char trading_day[9];
  int32_t settlement_id;
  char ip_address[33];
  char mac_address[21];
  char cmb_type;
  char status_msg[81];
} ctp_spd_apply;

typedef struct ctp_trader_offer {
  char exchange_id[9];
  char trader_id[21];
  char participant_id[11];
  char password[41];
  int32_t install_id;
  char order_local_id[13];
  char trader_connect_status;
  char connect_request_date[9];
  char connect_request_time[9];
  char last_report_date[9];
  char last_report_time[9];
  char connect_date[9];
  char connect_time[9];
  char start_date[9];
  char start_time[9];
  char trading_day[9];
  char broker_id[11];
  char max_trade_id[21];
  char max_order_message_reference[7];
  char order_cancel_alg;
} ctp_trader_offer;

typedef struct ctp_trading_account_password_update {
  char broker_id[11];
  char account_id[13];
  char old_password[41];
  char new_password[41];
  char currency_id[4];
} ctp_trading_account_password_update;

typedef struct ctp_trading_code {
  char investor_id[13];
  char broker_id[11];
  char exchange_id[9];
  char client_id[11];
  int32_t is_active;
  char client_id_type;
  char branch_id[9];
  char biz_type;
  char invest_unit_id[17];
} ctp_trading_code;

typedef struct ctp_trading_notice {
  char broker_id[11];
  char investor_range;
  char investor_id[13];
  int16_t sequence_series;
  char user_id[16];
  char send_time[9];
  int32_t sequence_no;
  char field_content[501];
  char invest_unit_id[17];
} ctp_trading_notice;

typedef struct ctp_transfer_bank {
  char bank_id[4];
  char bank_brch_id[5];
  char bank_name[101];
  int32_t is_active;
} ctp_transfer_bank;

typedef struct ctp_transfer_serial {
  int32_t plate_serial;
  char trade_date[9];
  char trading_day[9];
  char trade_time[9];
  char trade_code[7];
  int32_t session_id;
  char bank_id[4];
  char bank_branch_id[5];
  char bank_acc_type;
  char bank_account[41];
  char bank_serial[13];
  char broker_id[11];
  char broker_branch_id[31];
  char future_acc_type;
  char account_id[13];
  char investor_id[13];
  int32_t future_serial;
  char id_card_type;
  char identified_card_no[51];
  char currency_id[4];
  double trade_amount;
  double cust_fee;
  double broker_fee;
  char availability_flag;
  char operator_code[17];
  char bank_new_account[41];
  int32_t error_id;
  char error_msg[81];
} ctp_transfer_serial;

typedef struct ctp_user_password_update {
  char broker_id[11];
  char user_id[16];
  char old_password[41];
  char new_password[41];
} ctp_user_password_update;

typedef struct ctp_user_session {
  int32_t front_id;
  int32_t session_id;
  char broker_id[11];
  char user_id[16];
  char login_date[9];
  char login_time[9];
  char reserve1[16];
  char user_product_info[11];
  char interface_product_info[11];
  char protocol_info[11];
  char mac_address[21];
  char login_remark[36];
  char ip_address[33];
} ctp_user_session;

typedef struct ctp_user_system_info {
  char broker_id[11];
  char user_id[16];
  int32_t client_system_info_len;
  char client_system_info[273];
  char reserve1[16];
  int32_t client_ip_port;
  char client_login_time[9];
  char client_app_id[33];
  char client_public_ip[33];
  char client_login_remark[151];
  char mac[41];
} ctp_user_system_info;

typedef struct ctp_wechat_user_system_info {
  char broker_id[11];
  char user_id[16];
  int32_t wechat_clt_sys_info_len;
  char wechat_clt_sys_info[273];
  int32_t client_ip_port;
  char client_login_time[9];
  char client_app_id[33];
  char client_public_ip[33];
  char client_login_remark[151];
} ctp_wechat_user_system_info;

typedef struct ctp_trader_spi {
  void (*on_front_connected)(void *user_data);
  void (*on_front_disconnected)(int32_t reason, void *user_data);
  void (*on_heartbeat_warning)(int32_t time_lapse, void *user_data);
  void (*on_rtn_private_seq_no)(int32_t seq_no, void *user_data);
  void (*on_rsp_authenticate)(const ctp_rsp_authenticate *auth,
                              const ctp_rsp_info *rsp_info, int32_t request_id,
                              int32_t is_last, void *user_data);
  void (*on_rsp_settlement_info_confirm)(
      const ctp_settlement_info_confirm *confirm, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_user_login)(const ctp_rsp_user_login *login,
                            const ctp_rsp_info *rsp_info, int32_t request_id,
                            int32_t is_last, void *user_data);
  void (*on_rsp_user_logout)(const ctp_user_logout *logout,
                             const ctp_rsp_info *rsp_info, int32_t request_id,
                             int32_t is_last, void *user_data);
  void (*on_rsp_error)(const ctp_rsp_info *rsp_info, int32_t request_id,
                       int32_t is_last, void *user_data);
  void (*on_rsp_qry_trading_account)(const ctp_trading_account *account,
                                     const ctp_rsp_info *rsp_info,
                                     int32_t request_id, int32_t is_last,
                                     void *user_data);
  void (*on_rsp_qry_investor_position)(const ctp_investor_position *position,
                                       const ctp_rsp_info *rsp_info,
                                       int32_t request_id, int32_t is_last,
                                       void *user_data);
  void (*on_rsp_qry_instrument_margin_rate)(
      const ctp_instrument_margin_rate *margin_rate,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_qry_exchange_margin_rate)(
      const ctp_exchange_margin_rate *margin_rate, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_instrument_commission_rate)(
      const ctp_instrument_commission_rate *commission_rate,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_order_insert)(const ctp_input_order *input_order,
                              const ctp_rsp_info *rsp_info, int32_t request_id,
                              int32_t is_last, void *user_data);
  void (*on_rsp_order_action)(const ctp_input_order_action *action,
                              const ctp_rsp_info *rsp_info, int32_t request_id,
                              int32_t is_last, void *user_data);
  void (*on_rtn_order)(const ctp_order *order, void *user_data);
  void (*on_rtn_trade)(const ctp_trade *trade, void *user_data);
  void (*on_err_rtn_bank_to_future_by_future)(const ctp_req_transfer *item,
                                              const ctp_rsp_info *rsp_info,
                                              void *user_data);
  void (*on_err_rtn_batch_order_action)(const ctp_batch_order_action *item,
                                        const ctp_rsp_info *rsp_info,
                                        void *user_data);
  void (*on_err_rtn_cancel_offset_setting)(
      const ctp_cancel_offset_setting *item, const ctp_rsp_info *rsp_info,
      void *user_data);
  void (*on_err_rtn_comb_action_insert)(const ctp_input_comb_action *item,
                                        const ctp_rsp_info *rsp_info,
                                        void *user_data);
  void (*on_err_rtn_exec_order_action)(const ctp_exec_order_action *item,
                                       const ctp_rsp_info *rsp_info,
                                       void *user_data);
  void (*on_err_rtn_exec_order_insert)(const ctp_input_exec_order *item,
                                       const ctp_rsp_info *rsp_info,
                                       void *user_data);
  void (*on_err_rtn_for_quote_insert)(const ctp_input_for_quote *item,
                                      const ctp_rsp_info *rsp_info,
                                      void *user_data);
  void (*on_err_rtn_future_to_bank_by_future)(const ctp_req_transfer *item,
                                              const ctp_rsp_info *rsp_info,
                                              void *user_data);
  void (*on_err_rtn_hedge_cfm)(const ctp_input_hedge_cfm *item,
                               const ctp_rsp_info *rsp_info, void *user_data);
  void (*on_err_rtn_hedge_cfm_action)(const ctp_hedge_cfm_action *item,
                                      const ctp_rsp_info *rsp_info,
                                      void *user_data);
  void (*on_err_rtn_offset_setting)(const ctp_input_offset_setting *item,
                                    const ctp_rsp_info *rsp_info,
                                    void *user_data);
  void (*on_err_rtn_option_self_close_action)(
      const ctp_option_self_close_action *item, const ctp_rsp_info *rsp_info,
      void *user_data);
  void (*on_err_rtn_option_self_close_insert)(
      const ctp_input_option_self_close *item, const ctp_rsp_info *rsp_info,
      void *user_data);
  void (*on_err_rtn_order_action)(const ctp_order_action *item,
                                  const ctp_rsp_info *rsp_info,
                                  void *user_data);
  void (*on_err_rtn_order_insert)(const ctp_input_order *item,
                                  const ctp_rsp_info *rsp_info,
                                  void *user_data);
  void (*on_err_rtn_query_bank_balance_by_future)(
      const ctp_req_query_account *item, const ctp_rsp_info *rsp_info,
      void *user_data);
  void (*on_err_rtn_quote_action)(const ctp_quote_action *item,
                                  const ctp_rsp_info *rsp_info,
                                  void *user_data);
  void (*on_err_rtn_quote_insert)(const ctp_input_quote *item,
                                  const ctp_rsp_info *rsp_info,
                                  void *user_data);
  void (*on_err_rtn_spd_apply)(const ctp_input_spd_apply *item,
                               const ctp_rsp_info *rsp_info, void *user_data);
  void (*on_err_rtn_spd_apply_action)(const ctp_spd_apply_action *item,
                                      const ctp_rsp_info *rsp_info,
                                      void *user_data);
  void (*on_rsp_batch_order_action)(const ctp_input_batch_order_action *item,
                                    const ctp_rsp_info *rsp_info,
                                    int32_t request_id, int32_t is_last,
                                    void *user_data);
  void (*on_rsp_cancel_offset_setting)(const ctp_input_offset_setting *item,
                                       const ctp_rsp_info *rsp_info,
                                       int32_t request_id, int32_t is_last,
                                       void *user_data);
  void (*on_rsp_comb_action_insert)(const ctp_input_comb_action *item,
                                    const ctp_rsp_info *rsp_info,
                                    int32_t request_id, int32_t is_last,
                                    void *user_data);
  void (*on_rsp_exec_order_action)(const ctp_input_exec_order_action *item,
                                   const ctp_rsp_info *rsp_info,
                                   int32_t request_id, int32_t is_last,
                                   void *user_data);
  void (*on_rsp_exec_order_insert)(const ctp_input_exec_order *item,
                                   const ctp_rsp_info *rsp_info,
                                   int32_t request_id, int32_t is_last,
                                   void *user_data);
  void (*on_rsp_for_quote_insert)(const ctp_input_for_quote *item,
                                  const ctp_rsp_info *rsp_info,
                                  int32_t request_id, int32_t is_last,
                                  void *user_data);
  void (*on_rsp_from_bank_to_future_by_future)(const ctp_req_transfer *item,
                                               const ctp_rsp_info *rsp_info,
                                               int32_t request_id,
                                               int32_t is_last,
                                               void *user_data);
  void (*on_rsp_from_future_to_bank_by_future)(const ctp_req_transfer *item,
                                               const ctp_rsp_info *rsp_info,
                                               int32_t request_id,
                                               int32_t is_last,
                                               void *user_data);
  void (*on_rsp_gen_sms_code)(const ctp_rsp_gen_sms_code *item,
                              const ctp_rsp_info *rsp_info, int32_t request_id,
                              int32_t is_last, void *user_data);
  void (*on_rsp_gen_user_captcha)(const ctp_rsp_gen_user_captcha *item,
                                  const ctp_rsp_info *rsp_info,
                                  int32_t request_id, int32_t is_last,
                                  void *user_data);
  void (*on_rsp_gen_user_text)(const ctp_rsp_gen_user_text *item,
                               const ctp_rsp_info *rsp_info, int32_t request_id,
                               int32_t is_last, void *user_data);
  void (*on_rsp_hedge_cfm)(const ctp_input_hedge_cfm *item,
                           const ctp_rsp_info *rsp_info, int32_t request_id,
                           int32_t is_last, void *user_data);
  void (*on_rsp_hedge_cfm_action)(const ctp_input_hedge_cfm_action *item,
                                  const ctp_rsp_info *rsp_info,
                                  int32_t request_id, int32_t is_last,
                                  void *user_data);
  void (*on_rsp_offset_setting)(const ctp_input_offset_setting *item,
                                const ctp_rsp_info *rsp_info,
                                int32_t request_id, int32_t is_last,
                                void *user_data);
  void (*on_rsp_option_self_close_action)(
      const ctp_input_option_self_close_action *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_option_self_close_insert)(
      const ctp_input_option_self_close *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_parked_order_action)(const ctp_parked_order_action *item,
                                     const ctp_rsp_info *rsp_info,
                                     int32_t request_id, int32_t is_last,
                                     void *user_data);
  void (*on_rsp_parked_order_insert)(const ctp_parked_order *item,
                                     const ctp_rsp_info *rsp_info,
                                     int32_t request_id, int32_t is_last,
                                     void *user_data);
  void (*on_rsp_qry_accountregister)(const ctp_accountregister *item,
                                     const ctp_rsp_info *rsp_info,
                                     int32_t request_id, int32_t is_last,
                                     void *user_data);
  void (*on_rsp_qry_broker_trading_algos)(const ctp_broker_trading_algos *item,
                                          const ctp_rsp_info *rsp_info,
                                          int32_t request_id, int32_t is_last,
                                          void *user_data);
  void (*on_rsp_qry_broker_trading_params)(
      const ctp_broker_trading_params *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_cfmmc_trading_account_key)(
      const ctp_cfmmc_trading_account_key *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_classified_instrument)(const ctp_instrument *item,
                                           const ctp_rsp_info *rsp_info,
                                           int32_t request_id, int32_t is_last,
                                           void *user_data);
  void (*on_rsp_qry_comb_action)(const ctp_comb_action *item,
                                 const ctp_rsp_info *rsp_info,
                                 int32_t request_id, int32_t is_last,
                                 void *user_data);
  void (*on_rsp_qry_comb_instrument_guard)(
      const ctp_comb_instrument_guard *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_comb_leg)(const ctp_comb_leg *item,
                              const ctp_rsp_info *rsp_info, int32_t request_id,
                              int32_t is_last, void *user_data);
  void (*on_rsp_qry_comb_promotion_param)(const ctp_comb_promotion_param *item,
                                          const ctp_rsp_info *rsp_info,
                                          int32_t request_id, int32_t is_last,
                                          void *user_data);
  void (*on_rsp_qry_contract_bank)(const ctp_contract_bank *item,
                                   const ctp_rsp_info *rsp_info,
                                   int32_t request_id, int32_t is_last,
                                   void *user_data);
  void (*on_rsp_qry_depth_market_data)(const ctp_depth_market_data *item,
                                       const ctp_rsp_info *rsp_info,
                                       int32_t request_id, int32_t is_last,
                                       void *user_data);
  void (*on_rsp_qry_e_warrant_offset)(const ctp_e_warrant_offset *item,
                                      const ctp_rsp_info *rsp_info,
                                      int32_t request_id, int32_t is_last,
                                      void *user_data);
  void (*on_rsp_qry_exchange)(const ctp_exchange *item,
                              const ctp_rsp_info *rsp_info, int32_t request_id,
                              int32_t is_last, void *user_data);
  void (*on_rsp_qry_exchange_margin_rate_adjust)(
      const ctp_exchange_margin_rate_adjust *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_exchange_rate)(const ctp_exchange_rate *item,
                                   const ctp_rsp_info *rsp_info,
                                   int32_t request_id, int32_t is_last,
                                   void *user_data);
  void (*on_rsp_qry_exec_order)(const ctp_exec_order *item,
                                const ctp_rsp_info *rsp_info,
                                int32_t request_id, int32_t is_last,
                                void *user_data);
  void (*on_rsp_qry_for_quote)(const ctp_for_quote *item,
                               const ctp_rsp_info *rsp_info, int32_t request_id,
                               int32_t is_last, void *user_data);
  void (*on_rsp_qry_hedge_cfm)(const ctp_hedge_cfm *item,
                               const ctp_rsp_info *rsp_info, int32_t request_id,
                               int32_t is_last, void *user_data);
  void (*on_rsp_qry_instrument)(const ctp_instrument *item,
                                const ctp_rsp_info *rsp_info,
                                int32_t request_id, int32_t is_last,
                                void *user_data);
  void (*on_rsp_qry_instrument_order_comm_rate)(
      const ctp_instrument_order_comm_rate *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_invest_unit)(const ctp_invest_unit *item,
                                 const ctp_rsp_info *rsp_info,
                                 int32_t request_id, int32_t is_last,
                                 void *user_data);
  void (*on_rsp_qry_investor)(const ctp_investor *item,
                              const ctp_rsp_info *rsp_info, int32_t request_id,
                              int32_t is_last, void *user_data);
  void (*on_rsp_qry_investor_commodity_group_spmm_margin)(
      const ctp_investor_commodity_group_spmm_margin *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_qry_investor_commodity_spmm_margin)(
      const ctp_investor_commodity_spmm_margin *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_qry_investor_info_comm_rec)(
      const ctp_investor_info_comm_rec *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_investor_portf_margin_ratio)(
      const ctp_investor_portf_margin_ratio *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_investor_portf_setting)(
      const ctp_investor_portf_setting *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_investor_position_combine_detail)(
      const ctp_investor_position_combine_detail *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_qry_investor_position_detail)(
      const ctp_investor_position_detail *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_investor_prod_rcams_margin)(
      const ctp_investor_prod_rcams_margin *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_investor_prod_rule_margin)(
      const ctp_investor_prod_rule_margin *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_investor_prod_spbm_detail)(
      const ctp_investor_prod_spbm_detail *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_investor_product_group_margin)(
      const ctp_investor_product_group_margin *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_qry_mm_instrument_commission_rate)(
      const ctp_mm_instrument_commission_rate *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_qry_mm_option_instr_comm_rate)(
      const ctp_mm_option_instr_comm_rate *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_max_order_volume)(const ctp_qry_max_order_volume *item,
                                      const ctp_rsp_info *rsp_info,
                                      int32_t request_id, int32_t is_last,
                                      void *user_data);
  void (*on_rsp_qry_notice)(const ctp_notice *item,
                            const ctp_rsp_info *rsp_info, int32_t request_id,
                            int32_t is_last, void *user_data);
  void (*on_rsp_qry_offset_setting)(const ctp_offset_setting *item,
                                    const ctp_rsp_info *rsp_info,
                                    int32_t request_id, int32_t is_last,
                                    void *user_data);
  void (*on_rsp_qry_option_instr_comm_rate)(
      const ctp_option_instr_comm_rate *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_option_instr_trade_cost)(
      const ctp_option_instr_trade_cost *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_option_self_close)(const ctp_option_self_close *item,
                                       const ctp_rsp_info *rsp_info,
                                       int32_t request_id, int32_t is_last,
                                       void *user_data);
  void (*on_rsp_qry_order)(const ctp_order *item, const ctp_rsp_info *rsp_info,
                           int32_t request_id, int32_t is_last,
                           void *user_data);
  void (*on_rsp_qry_parked_order)(const ctp_parked_order *item,
                                  const ctp_rsp_info *rsp_info,
                                  int32_t request_id, int32_t is_last,
                                  void *user_data);
  void (*on_rsp_qry_parked_order_action)(const ctp_parked_order_action *item,
                                         const ctp_rsp_info *rsp_info,
                                         int32_t request_id, int32_t is_last,
                                         void *user_data);
  void (*on_rsp_qry_product)(const ctp_product *item,
                             const ctp_rsp_info *rsp_info, int32_t request_id,
                             int32_t is_last, void *user_data);
  void (*on_rsp_qry_product_exch_rate)(const ctp_product_exch_rate *item,
                                       const ctp_rsp_info *rsp_info,
                                       int32_t request_id, int32_t is_last,
                                       void *user_data);
  void (*on_rsp_qry_product_group)(const ctp_product_group *item,
                                   const ctp_rsp_info *rsp_info,
                                   int32_t request_id, int32_t is_last,
                                   void *user_data);
  void (*on_rsp_qry_quote)(const ctp_quote *item, const ctp_rsp_info *rsp_info,
                           int32_t request_id, int32_t is_last,
                           void *user_data);
  void (*on_rsp_qry_rcams_comb_product_info)(
      const ctp_rcams_comb_product_info *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_rcams_instr_parameter)(
      const ctp_rcams_instr_parameter *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_rcams_inter_parameter)(
      const ctp_rcams_inter_parameter *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_rcams_intra_parameter)(
      const ctp_rcams_intra_parameter *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_rcams_investor_comb_position)(
      const ctp_rcams_investor_comb_position *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_qry_rcams_short_opt_adjust_param)(
      const ctp_rcams_short_opt_adjust_param *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_qry_rule_instr_parameter)(const ctp_rule_instr_parameter *item,
                                          const ctp_rsp_info *rsp_info,
                                          int32_t request_id, int32_t is_last,
                                          void *user_data);
  void (*on_rsp_qry_rule_inter_parameter)(const ctp_rule_inter_parameter *item,
                                          const ctp_rsp_info *rsp_info,
                                          int32_t request_id, int32_t is_last,
                                          void *user_data);
  void (*on_rsp_qry_rule_intra_parameter)(const ctp_rule_intra_parameter *item,
                                          const ctp_rsp_info *rsp_info,
                                          int32_t request_id, int32_t is_last,
                                          void *user_data);
  void (*on_rsp_qry_risk_settle_invst_position)(
      const ctp_risk_settle_invst_position *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_risk_settle_product_status)(
      const ctp_risk_settle_product_status *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_spbm_add_on_inter_parameter)(
      const ctp_spbm_add_on_inter_parameter *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_spbm_future_parameter)(
      const ctp_spbm_future_parameter *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_spbm_inter_parameter)(const ctp_spbm_inter_parameter *item,
                                          const ctp_rsp_info *rsp_info,
                                          int32_t request_id, int32_t is_last,
                                          void *user_data);
  void (*on_rsp_qry_spbm_intra_parameter)(const ctp_spbm_intra_parameter *item,
                                          const ctp_rsp_info *rsp_info,
                                          int32_t request_id, int32_t is_last,
                                          void *user_data);
  void (*on_rsp_qry_spbm_investor_portf_def)(
      const ctp_spbm_investor_portf_def *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_spbm_option_parameter)(
      const ctp_spbm_option_parameter *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_spbm_portf_definition)(
      const ctp_spbm_portf_definition *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_spmm_inst_param)(const ctp_spmm_inst_param *item,
                                     const ctp_rsp_info *rsp_info,
                                     int32_t request_id, int32_t is_last,
                                     void *user_data);
  void (*on_rsp_qry_spmm_product_param)(const ctp_spmm_product_param *item,
                                        const ctp_rsp_info *rsp_info,
                                        int32_t request_id, int32_t is_last,
                                        void *user_data);
  void (*on_rsp_qry_sec_agent_ac_id_map)(const ctp_sec_agent_ac_id_map *item,
                                         const ctp_rsp_info *rsp_info,
                                         int32_t request_id, int32_t is_last,
                                         void *user_data);
  void (*on_rsp_qry_sec_agent_check_mode)(const ctp_sec_agent_check_mode *item,
                                          const ctp_rsp_info *rsp_info,
                                          int32_t request_id, int32_t is_last,
                                          void *user_data);
  void (*on_rsp_qry_sec_agent_trade_info)(const ctp_sec_agent_trade_info *item,
                                          const ctp_rsp_info *rsp_info,
                                          int32_t request_id, int32_t is_last,
                                          void *user_data);
  void (*on_rsp_qry_sec_agent_trading_account)(const ctp_trading_account *item,
                                               const ctp_rsp_info *rsp_info,
                                               int32_t request_id,
                                               int32_t is_last,
                                               void *user_data);
  void (*on_rsp_qry_settlement_info)(const ctp_settlement_info *item,
                                     const ctp_rsp_info *rsp_info,
                                     int32_t request_id, int32_t is_last,
                                     void *user_data);
  void (*on_rsp_qry_settlement_info_confirm)(
      const ctp_settlement_info_confirm *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_qry_spd_apply)(const ctp_spd_apply *item,
                               const ctp_rsp_info *rsp_info, int32_t request_id,
                               int32_t is_last, void *user_data);
  void (*on_rsp_qry_trade)(const ctp_trade *item, const ctp_rsp_info *rsp_info,
                           int32_t request_id, int32_t is_last,
                           void *user_data);
  void (*on_rsp_qry_trader_offer)(const ctp_trader_offer *item,
                                  const ctp_rsp_info *rsp_info,
                                  int32_t request_id, int32_t is_last,
                                  void *user_data);
  void (*on_rsp_qry_trading_code)(const ctp_trading_code *item,
                                  const ctp_rsp_info *rsp_info,
                                  int32_t request_id, int32_t is_last,
                                  void *user_data);
  void (*on_rsp_qry_trading_notice)(const ctp_trading_notice *item,
                                    const ctp_rsp_info *rsp_info,
                                    int32_t request_id, int32_t is_last,
                                    void *user_data);
  void (*on_rsp_qry_transfer_bank)(const ctp_transfer_bank *item,
                                   const ctp_rsp_info *rsp_info,
                                   int32_t request_id, int32_t is_last,
                                   void *user_data);
  void (*on_rsp_qry_transfer_serial)(const ctp_transfer_serial *item,
                                     const ctp_rsp_info *rsp_info,
                                     int32_t request_id, int32_t is_last,
                                     void *user_data);
  void (*on_rsp_qry_user_session)(const ctp_user_session *item,
                                  const ctp_rsp_info *rsp_info,
                                  int32_t request_id, int32_t is_last,
                                  void *user_data);
  void (*on_rsp_query_bank_account_money_by_future)(
      const ctp_req_query_account *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_query_cfmmc_trading_account_token)(
      const ctp_query_cfmmc_trading_account_token *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_quote_action)(const ctp_input_quote_action *item,
                              const ctp_rsp_info *rsp_info, int32_t request_id,
                              int32_t is_last, void *user_data);
  void (*on_rsp_quote_insert)(const ctp_input_quote *item,
                              const ctp_rsp_info *rsp_info, int32_t request_id,
                              int32_t is_last, void *user_data);
  void (*on_rsp_remove_parked_order)(const ctp_remove_parked_order *item,
                                     const ctp_rsp_info *rsp_info,
                                     int32_t request_id, int32_t is_last,
                                     void *user_data);
  void (*on_rsp_remove_parked_order_action)(
      const ctp_remove_parked_order_action *item, const ctp_rsp_info *rsp_info,
      int32_t request_id, int32_t is_last, void *user_data);
  void (*on_rsp_spd_apply)(const ctp_input_spd_apply *item,
                           const ctp_rsp_info *rsp_info, int32_t request_id,
                           int32_t is_last, void *user_data);
  void (*on_rsp_spd_apply_action)(const ctp_input_spd_apply_action *item,
                                  const ctp_rsp_info *rsp_info,
                                  int32_t request_id, int32_t is_last,
                                  void *user_data);
  void (*on_rsp_trading_account_password_update)(
      const ctp_trading_account_password_update *item,
      const ctp_rsp_info *rsp_info, int32_t request_id, int32_t is_last,
      void *user_data);
  void (*on_rsp_user_auth_method)(const ctp_rsp_user_auth_method *item,
                                  const ctp_rsp_info *rsp_info,
                                  int32_t request_id, int32_t is_last,
                                  void *user_data);
  void (*on_rsp_user_password_update)(const ctp_user_password_update *item,
                                      const ctp_rsp_info *rsp_info,
                                      int32_t request_id, int32_t is_last,
                                      void *user_data);
  void (*on_rtn_comb_action)(const ctp_comb_action *item, void *user_data);
  void (*on_rtn_exec_order)(const ctp_exec_order *item, void *user_data);
  void (*on_rtn_for_quote_rsp)(const ctp_for_quote_rsp *item, void *user_data);
  void (*on_rtn_from_bank_to_future_by_future)(const ctp_rsp_transfer *item,
                                               void *user_data);
  void (*on_rtn_from_future_to_bank_by_future)(const ctp_rsp_transfer *item,
                                               void *user_data);
  void (*on_rtn_hedge_cfm)(const ctp_hedge_cfm *item, void *user_data);
  void (*on_rtn_offset_setting)(const ctp_offset_setting *item,
                                void *user_data);
  void (*on_rtn_option_self_close)(const ctp_option_self_close *item,
                                   void *user_data);
  void (*on_rtn_query_bank_balance_by_future)(
      const ctp_notify_query_account *item, void *user_data);
  void (*on_rtn_quote)(const ctp_quote *item, void *user_data);
  void (*on_rtn_spd_apply)(const ctp_spd_apply *item, void *user_data);
} ctp_trader_spi;

typedef struct ctp_md_handle ctp_md_handle;
typedef struct ctp_trader_handle ctp_trader_handle;

CTP_BRIDGE_API const char *ctp_md_get_api_version(void);
CTP_BRIDGE_API ctp_md_handle *ctp_md_create(const char *flow_path,
                                            int32_t using_udp,
                                            int32_t multicast,
                                            int32_t production_mode);
CTP_BRIDGE_API void ctp_md_destroy(ctp_md_handle *handle);
CTP_BRIDGE_API int32_t ctp_md_set_spi(ctp_md_handle *handle,
                                      const ctp_md_spi *spi, void *user_data);
CTP_BRIDGE_API int32_t ctp_md_register_front(ctp_md_handle *handle,
                                             const char *front_address);
CTP_BRIDGE_API void ctp_md_init(ctp_md_handle *handle);
CTP_BRIDGE_API int32_t ctp_md_join(ctp_md_handle *handle);
CTP_BRIDGE_API int32_t ctp_md_req_user_login(ctp_md_handle *handle,
                                             const ctp_req_user_login *request,
                                             int32_t request_id);
CTP_BRIDGE_API int32_t ctp_md_req_user_logout(ctp_md_handle *handle,
                                              const ctp_user_logout *request,
                                              int32_t request_id);
CTP_BRIDGE_API int32_t ctp_md_subscribe_market_data(
    ctp_md_handle *handle, const char *const *instruments, int32_t count);
CTP_BRIDGE_API int32_t ctp_md_unsubscribe_market_data(
    ctp_md_handle *handle, const char *const *instruments, int32_t count);

CTP_BRIDGE_API const char *ctp_trader_get_api_version(void);
CTP_BRIDGE_API ctp_trader_handle *ctp_trader_create(const char *flow_path,
                                                    int32_t production_mode);
CTP_BRIDGE_API void ctp_trader_destroy(ctp_trader_handle *handle);
CTP_BRIDGE_API int32_t ctp_trader_set_spi(ctp_trader_handle *handle,
                                          const ctp_trader_spi *spi,
                                          void *user_data);
CTP_BRIDGE_API int32_t ctp_trader_register_front(ctp_trader_handle *handle,
                                                 const char *front_address);
CTP_BRIDGE_API int32_t ctp_trader_subscribe_private_topic(
    ctp_trader_handle *handle, int32_t resume_type, int32_t seq_no);
CTP_BRIDGE_API int32_t ctp_trader_subscribe_public_topic(
    ctp_trader_handle *handle, int32_t resume_type);
CTP_BRIDGE_API void ctp_trader_init(ctp_trader_handle *handle);
CTP_BRIDGE_API int32_t ctp_trader_join(ctp_trader_handle *handle);
CTP_BRIDGE_API int32_t ctp_trader_req_authenticate(
    ctp_trader_handle *handle, const ctp_req_authenticate *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_settlement_info_confirm(
    ctp_trader_handle *handle, const ctp_settlement_info_confirm *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_user_login(
    ctp_trader_handle *handle, const ctp_req_user_login *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t
ctp_trader_req_user_logout(ctp_trader_handle *handle,
                           const ctp_user_logout *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_trading_account(
    ctp_trader_handle *handle, const ctp_qry_trading_account *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_position(
    ctp_trader_handle *handle, const ctp_qry_investor_position *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_instrument_margin_rate(
    ctp_trader_handle *handle, const ctp_qry_instrument_margin_rate *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_exchange_margin_rate(
    ctp_trader_handle *handle, const ctp_qry_exchange_margin_rate *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_instrument_commission_rate(
    ctp_trader_handle *handle,
    const ctp_qry_instrument_commission_rate *request, int32_t request_id);
CTP_BRIDGE_API int32_t
ctp_trader_req_order_insert(ctp_trader_handle *handle,
                            const ctp_input_order *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_order_action(
    ctp_trader_handle *handle, const ctp_input_order_action *request,
    int32_t request_id);
CTP_BRIDGE_API const char *
ctp_trader_get_trading_day(ctp_trader_handle *handle);
CTP_BRIDGE_API int32_t ctp_trader_get_front_info(ctp_trader_handle *handle,
                                                 ctp_front_info *front_info);
CTP_BRIDGE_API int32_t ctp_trader_register_name_server(
    ctp_trader_handle *handle, const char *ns_address);
CTP_BRIDGE_API int32_t ctp_trader_register_fens_user_info(
    ctp_trader_handle *handle, const ctp_fens_user_info *request);
CTP_BRIDGE_API int32_t ctp_trader_register_user_system_info(
    ctp_trader_handle *handle, const ctp_user_system_info *request);
CTP_BRIDGE_API int32_t ctp_trader_submit_user_system_info(
    ctp_trader_handle *handle, const ctp_user_system_info *request);
CTP_BRIDGE_API int32_t ctp_trader_register_wechat_user_system_info(
    ctp_trader_handle *handle, const ctp_wechat_user_system_info *request);
CTP_BRIDGE_API int32_t ctp_trader_submit_wechat_user_system_info(
    ctp_trader_handle *handle, const ctp_wechat_user_system_info *request);
CTP_BRIDGE_API int32_t ctp_trader_req_user_password_update(
    ctp_trader_handle *handle, const ctp_user_password_update *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_trading_account_password_update(
    ctp_trader_handle *handle,
    const ctp_trading_account_password_update *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_user_auth_method(
    ctp_trader_handle *handle, const ctp_req_user_auth_method *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_gen_user_captcha(
    ctp_trader_handle *handle, const ctp_req_gen_user_captcha *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_gen_user_text(
    ctp_trader_handle *handle, const ctp_req_gen_user_text *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_user_login_with_captcha(
    ctp_trader_handle *handle, const ctp_req_user_login_with_captcha *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_user_login_with_text(
    ctp_trader_handle *handle, const ctp_req_user_login_with_text *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_user_login_with_otp(
    ctp_trader_handle *handle, const ctp_req_user_login_with_otp *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_gen_sms_code(
    ctp_trader_handle *handle, const ctp_req_gen_sms_code *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_parked_order_insert(
    ctp_trader_handle *handle, const ctp_parked_order *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_parked_order_action(
    ctp_trader_handle *handle, const ctp_parked_order_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_max_order_volume(
    ctp_trader_handle *handle, const ctp_qry_max_order_volume *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_remove_parked_order(
    ctp_trader_handle *handle, const ctp_remove_parked_order *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_remove_parked_order_action(
    ctp_trader_handle *handle, const ctp_remove_parked_order_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_exec_order_insert(
    ctp_trader_handle *handle, const ctp_input_exec_order *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_exec_order_action(
    ctp_trader_handle *handle, const ctp_input_exec_order_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_for_quote_insert(
    ctp_trader_handle *handle, const ctp_input_for_quote *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t
ctp_trader_req_quote_insert(ctp_trader_handle *handle,
                            const ctp_input_quote *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_quote_action(
    ctp_trader_handle *handle, const ctp_input_quote_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_batch_order_action(
    ctp_trader_handle *handle, const ctp_input_batch_order_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_option_self_close_insert(
    ctp_trader_handle *handle, const ctp_input_option_self_close *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_option_self_close_action(
    ctp_trader_handle *handle,
    const ctp_input_option_self_close_action *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_comb_action_insert(
    ctp_trader_handle *handle, const ctp_input_comb_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_offset_setting(
    ctp_trader_handle *handle, const ctp_input_offset_setting *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_cancel_offset_setting(
    ctp_trader_handle *handle, const ctp_input_offset_setting *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_spd_apply(
    ctp_trader_handle *handle, const ctp_input_spd_apply *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_spd_apply_action(
    ctp_trader_handle *handle, const ctp_input_spd_apply_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_hedge_cfm(
    ctp_trader_handle *handle, const ctp_input_hedge_cfm *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_hedge_cfm_action(
    ctp_trader_handle *handle, const ctp_input_hedge_cfm_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_order(ctp_trader_handle *handle,
                                                const ctp_qry_order *request,
                                                int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_trade(ctp_trader_handle *handle,
                                                const ctp_qry_trade *request,
                                                int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor(
    ctp_trader_handle *handle, const ctp_qry_investor *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_trading_code(
    ctp_trader_handle *handle, const ctp_qry_trading_code *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_user_session(
    ctp_trader_handle *handle, const ctp_qry_user_session *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_exchange(
    ctp_trader_handle *handle, const ctp_qry_exchange *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t
ctp_trader_req_qry_product(ctp_trader_handle *handle,
                           const ctp_qry_product *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_instrument(
    ctp_trader_handle *handle, const ctp_qry_instrument *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_depth_market_data(
    ctp_trader_handle *handle, const ctp_qry_depth_market_data *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_trader_offer(
    ctp_trader_handle *handle, const ctp_qry_trader_offer *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_settlement_info(
    ctp_trader_handle *handle, const ctp_qry_settlement_info *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_transfer_bank(
    ctp_trader_handle *handle, const ctp_qry_transfer_bank *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_position_detail(
    ctp_trader_handle *handle, const ctp_qry_investor_position_detail *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_notice(ctp_trader_handle *handle,
                                                 const ctp_qry_notice *request,
                                                 int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_settlement_info_confirm(
    ctp_trader_handle *handle, const ctp_qry_settlement_info_confirm *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_position_combine_detail(
    ctp_trader_handle *handle,
    const ctp_qry_investor_position_combine_detail *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_cfmmc_trading_account_key(
    ctp_trader_handle *handle, const ctp_qry_cfmmc_trading_account_key *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_e_warrant_offset(
    ctp_trader_handle *handle, const ctp_qry_e_warrant_offset *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_product_group_margin(
    ctp_trader_handle *handle,
    const ctp_qry_investor_product_group_margin *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_exchange_margin_rate_adjust(
    ctp_trader_handle *handle,
    const ctp_qry_exchange_margin_rate_adjust *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_exchange_rate(
    ctp_trader_handle *handle, const ctp_qry_exchange_rate *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_sec_agent_ac_id_map(
    ctp_trader_handle *handle, const ctp_qry_sec_agent_ac_id_map *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_product_exch_rate(
    ctp_trader_handle *handle, const ctp_qry_product_exch_rate *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_product_group(
    ctp_trader_handle *handle, const ctp_qry_product_group *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_mm_instrument_commission_rate(
    ctp_trader_handle *handle,
    const ctp_qry_mm_instrument_commission_rate *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_mm_option_instr_comm_rate(
    ctp_trader_handle *handle, const ctp_qry_mm_option_instr_comm_rate *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_instrument_order_comm_rate(
    ctp_trader_handle *handle,
    const ctp_qry_instrument_order_comm_rate *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_sec_agent_trading_account(
    ctp_trader_handle *handle, const ctp_qry_trading_account *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_sec_agent_check_mode(
    ctp_trader_handle *handle, const ctp_qry_sec_agent_check_mode *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_sec_agent_trade_info(
    ctp_trader_handle *handle, const ctp_qry_sec_agent_trade_info *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_option_instr_trade_cost(
    ctp_trader_handle *handle, const ctp_qry_option_instr_trade_cost *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_option_instr_comm_rate(
    ctp_trader_handle *handle, const ctp_qry_option_instr_comm_rate *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_exec_order(
    ctp_trader_handle *handle, const ctp_qry_exec_order *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_for_quote(
    ctp_trader_handle *handle, const ctp_qry_for_quote *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_quote(ctp_trader_handle *handle,
                                                const ctp_qry_quote *request,
                                                int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_option_self_close(
    ctp_trader_handle *handle, const ctp_qry_option_self_close *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_invest_unit(
    ctp_trader_handle *handle, const ctp_qry_invest_unit *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_comb_instrument_guard(
    ctp_trader_handle *handle, const ctp_qry_comb_instrument_guard *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_comb_action(
    ctp_trader_handle *handle, const ctp_qry_comb_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_transfer_serial(
    ctp_trader_handle *handle, const ctp_qry_transfer_serial *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_accountregister(
    ctp_trader_handle *handle, const ctp_qry_accountregister *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_contract_bank(
    ctp_trader_handle *handle, const ctp_qry_contract_bank *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_parked_order(
    ctp_trader_handle *handle, const ctp_qry_parked_order *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_parked_order_action(
    ctp_trader_handle *handle, const ctp_qry_parked_order_action *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_trading_notice(
    ctp_trader_handle *handle, const ctp_qry_trading_notice *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_broker_trading_params(
    ctp_trader_handle *handle, const ctp_qry_broker_trading_params *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_broker_trading_algos(
    ctp_trader_handle *handle, const ctp_qry_broker_trading_algos *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_query_cfmmc_trading_account_token(
    ctp_trader_handle *handle,
    const ctp_query_cfmmc_trading_account_token *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_classified_instrument(
    ctp_trader_handle *handle, const ctp_qry_classified_instrument *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_comb_promotion_param(
    ctp_trader_handle *handle, const ctp_qry_comb_promotion_param *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_risk_settle_invst_position(
    ctp_trader_handle *handle,
    const ctp_qry_risk_settle_invst_position *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_risk_settle_product_status(
    ctp_trader_handle *handle,
    const ctp_qry_risk_settle_product_status *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spbm_future_parameter(
    ctp_trader_handle *handle, const ctp_qry_spbm_future_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spbm_option_parameter(
    ctp_trader_handle *handle, const ctp_qry_spbm_option_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spbm_intra_parameter(
    ctp_trader_handle *handle, const ctp_qry_spbm_intra_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spbm_inter_parameter(
    ctp_trader_handle *handle, const ctp_qry_spbm_inter_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spbm_portf_definition(
    ctp_trader_handle *handle, const ctp_qry_spbm_portf_definition *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spbm_investor_portf_def(
    ctp_trader_handle *handle, const ctp_qry_spbm_investor_portf_def *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_portf_margin_ratio(
    ctp_trader_handle *handle,
    const ctp_qry_investor_portf_margin_ratio *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_prod_spbm_detail(
    ctp_trader_handle *handle, const ctp_qry_investor_prod_spbm_detail *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_commodity_spmm_margin(
    ctp_trader_handle *handle,
    const ctp_qry_investor_commodity_spmm_margin *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_commodity_group_spmm_margin(
    ctp_trader_handle *handle,
    const ctp_qry_investor_commodity_group_spmm_margin *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spmm_inst_param(
    ctp_trader_handle *handle, const ctp_qry_spmm_inst_param *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spmm_product_param(
    ctp_trader_handle *handle, const ctp_qry_spmm_product_param *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spbm_add_on_inter_parameter(
    ctp_trader_handle *handle,
    const ctp_qry_spbm_add_on_inter_parameter *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_rcams_comb_product_info(
    ctp_trader_handle *handle, const ctp_qry_rcams_comb_product_info *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_rcams_instr_parameter(
    ctp_trader_handle *handle, const ctp_qry_rcams_instr_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_rcams_intra_parameter(
    ctp_trader_handle *handle, const ctp_qry_rcams_intra_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_rcams_inter_parameter(
    ctp_trader_handle *handle, const ctp_qry_rcams_inter_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_rcams_short_opt_adjust_param(
    ctp_trader_handle *handle,
    const ctp_qry_rcams_short_opt_adjust_param *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_rcams_investor_comb_position(
    ctp_trader_handle *handle,
    const ctp_qry_rcams_investor_comb_position *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_prod_rcams_margin(
    ctp_trader_handle *handle,
    const ctp_qry_investor_prod_rcams_margin *request, int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_rule_instr_parameter(
    ctp_trader_handle *handle, const ctp_qry_rule_instr_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_rule_intra_parameter(
    ctp_trader_handle *handle, const ctp_qry_rule_intra_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_rule_inter_parameter(
    ctp_trader_handle *handle, const ctp_qry_rule_inter_parameter *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_prod_rule_margin(
    ctp_trader_handle *handle, const ctp_qry_investor_prod_rule_margin *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_portf_setting(
    ctp_trader_handle *handle, const ctp_qry_investor_portf_setting *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_investor_info_comm_rec(
    ctp_trader_handle *handle, const ctp_qry_investor_info_comm_rec *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_comb_leg(
    ctp_trader_handle *handle, const ctp_qry_comb_leg *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_offset_setting(
    ctp_trader_handle *handle, const ctp_qry_offset_setting *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_spd_apply(
    ctp_trader_handle *handle, const ctp_qry_spd_apply *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_qry_hedge_cfm(
    ctp_trader_handle *handle, const ctp_qry_hedge_cfm *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_from_bank_to_future_by_future(
    ctp_trader_handle *handle, const ctp_req_transfer *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_from_future_to_bank_by_future(
    ctp_trader_handle *handle, const ctp_req_transfer *request,
    int32_t request_id);
CTP_BRIDGE_API int32_t ctp_trader_req_query_bank_account_money_by_future(
    ctp_trader_handle *handle, const ctp_req_query_account *request,
    int32_t request_id);

#ifdef __cplusplus
}
#endif
