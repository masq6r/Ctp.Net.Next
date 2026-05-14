#include "ctp_bridge.h"

#include <cstring>
#include <memory>
#include <new>
#include <string>

#include "ThostFtdcTraderApi.h"

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

void fill_rsp_authenticate(ctp_rsp_authenticate& dest, const CThostFtdcRspAuthenticateField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.user_product_info, src->UserProductInfo);
  copy_field(dest.app_id, src->AppID);
  dest.app_type = src->AppType;
}

void fill_settlement_info_confirm(ctp_settlement_info_confirm& dest, const CThostFtdcSettlementInfoConfirmField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.confirm_date, src->ConfirmDate);
  copy_field(dest.confirm_time, src->ConfirmTime);
  dest.settlement_id = src->SettlementID;
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
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

void fill_trading_account(ctp_trading_account& dest, const CThostFtdcTradingAccountField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.trading_day, src->TradingDay);
  dest.deposit = src->Deposit;
  dest.withdraw = src->Withdraw;
  dest.balance = src->Balance;
  dest.available = src->Available;
  dest.curr_margin = src->CurrMargin;
  dest.frozen_margin = src->FrozenMargin;
  dest.frozen_cash = src->FrozenCash;
  dest.frozen_commission = src->FrozenCommission;
  dest.commission = src->Commission;
  dest.close_profit = src->CloseProfit;
  dest.position_profit = src->PositionProfit;
  dest.withdraw_quota = src->WithdrawQuota;
  dest.reserve = src->Reserve;
}

void fill_investor_position(ctp_investor_position& dest, const CThostFtdcInvestorPositionField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.posi_direction = src->PosiDirection;
  dest.hedge_flag = src->HedgeFlag;
  dest.position_date = src->PositionDate;
  dest.yd_position = src->YdPosition;
  dest.position = src->Position;
  dest.today_position = src->TodayPosition;
  dest.long_frozen = src->LongFrozen;
  dest.short_frozen = src->ShortFrozen;
  dest.open_volume = src->OpenVolume;
  dest.close_volume = src->CloseVolume;
  dest.position_profit = src->PositionProfit;
  dest.close_profit = src->CloseProfit;
  dest.use_margin = src->UseMargin;
  dest.position_cost = src->PositionCost;
  dest.open_cost = src->OpenCost;
}

void fill_instrument_margin_rate(ctp_instrument_margin_rate& dest, const CThostFtdcInstrumentMarginRateField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  dest.investor_range = src->InvestorRange;
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.hedge_flag = src->HedgeFlag;
  dest.long_margin_ratio_by_money = src->LongMarginRatioByMoney;
  dest.long_margin_ratio_by_volume = src->LongMarginRatioByVolume;
  dest.short_margin_ratio_by_money = src->ShortMarginRatioByMoney;
  dest.short_margin_ratio_by_volume = src->ShortMarginRatioByVolume;
  dest.is_relative = src->IsRelative;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_exchange_margin_rate(ctp_exchange_margin_rate& dest, const CThostFtdcExchangeMarginRateField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.reserve1, src->reserve1);
  dest.hedge_flag = src->HedgeFlag;
  dest.long_margin_ratio_by_money = src->LongMarginRatioByMoney;
  dest.long_margin_ratio_by_volume = src->LongMarginRatioByVolume;
  dest.short_margin_ratio_by_money = src->ShortMarginRatioByMoney;
  dest.short_margin_ratio_by_volume = src->ShortMarginRatioByVolume;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_instrument_commission_rate(ctp_instrument_commission_rate& dest, const CThostFtdcInstrumentCommissionRateField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  dest.investor_range = src->InvestorRange;
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.open_ratio_by_money = src->OpenRatioByMoney;
  dest.open_ratio_by_volume = src->OpenRatioByVolume;
  dest.close_ratio_by_money = src->CloseRatioByMoney;
  dest.close_ratio_by_volume = src->CloseRatioByVolume;
  dest.close_today_ratio_by_money = src->CloseTodayRatioByMoney;
  dest.close_today_ratio_by_volume = src->CloseTodayRatioByVolume;
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.biz_type = src->BizType;
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_input_order(ctp_input_order& dest, const CThostFtdcInputOrderField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.order_ref, src->OrderRef);
  copy_field(dest.user_id, src->UserID);
  dest.order_price_type = src->OrderPriceType;
  dest.direction = src->Direction;
  copy_field(dest.comb_offset_flag, src->CombOffsetFlag);
  copy_field(dest.comb_hedge_flag, src->CombHedgeFlag);
  dest.limit_price = src->LimitPrice;
  dest.volume_total_original = src->VolumeTotalOriginal;
  dest.time_condition = src->TimeCondition;
  copy_field(dest.gtd_date, src->GTDDate);
  dest.volume_condition = src->VolumeCondition;
  dest.min_volume = src->MinVolume;
  dest.contingent_condition = src->ContingentCondition;
  dest.stop_price = src->StopPrice;
  dest.force_close_reason = src->ForceCloseReason;
  dest.is_auto_suspend = src->IsAutoSuspend;
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.request_id = src->RequestID;
  dest.user_force_close = src->UserForceClose;
  dest.is_swap_order = src->IsSwapOrder;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.order_memo, src->OrderMemo);
}

void fill_input_order_action(ctp_input_order_action& dest, const CThostFtdcInputOrderActionField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.order_action_ref = src->OrderActionRef;
  copy_field(dest.order_ref, src->OrderRef);
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_sys_id, src->OrderSysID);
  dest.action_flag = src->ActionFlag;
  dest.limit_price = src->LimitPrice;
  dest.volume_change = src->VolumeChange;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.order_memo, src->OrderMemo);
}

void fill_order(ctp_order& dest, const CThostFtdcOrderField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_ref, src->OrderRef);
  copy_field(dest.order_sys_id, src->OrderSysID);
  copy_field(dest.user_id, src->UserID);
  dest.order_price_type = src->OrderPriceType;
  dest.direction = src->Direction;
  copy_field(dest.comb_offset_flag, src->CombOffsetFlag);
  copy_field(dest.comb_hedge_flag, src->CombHedgeFlag);
  dest.limit_price = src->LimitPrice;
  dest.volume_total_original = src->VolumeTotalOriginal;
  dest.volume_traded = src->VolumeTraded;
  dest.volume_total = src->VolumeTotal;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  dest.order_status = src->OrderStatus;
  dest.order_submit_status = src->OrderSubmitStatus;
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.insert_date, src->InsertDate);
  copy_field(dest.insert_time, src->InsertTime);
  copy_field(dest.active_time, src->ActiveTime);
  copy_field(dest.suspend_time, src->SuspendTime);
  copy_field(dest.update_time, src->UpdateTime);
  copy_field(dest.cancel_time, src->CancelTime);
}

void fill_trade(ctp_trade& dest, const CThostFtdcTradeField* src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_ref, src->OrderRef);
  copy_field(dest.order_sys_id, src->OrderSysID);
  copy_field(dest.trade_id, src->TradeID);
  copy_field(dest.user_id, src->UserID);
  dest.direction = src->Direction;
  dest.offset_flag = src->OffsetFlag;
  dest.hedge_flag = src->HedgeFlag;
  dest.price = src->Price;
  dest.volume = src->Volume;
  copy_field(dest.trade_date, src->TradeDate);
  copy_field(dest.trade_time, src->TradeTime);
  copy_field(dest.trading_day, src->TradingDay);
}

class TraderSpiAdapter final : public CThostFtdcTraderSpi {
public:
  TraderSpiAdapter(const ctp_trader_spi& callbacks, void* user_data)
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

  void OnRtnPrivateSeqNo(int nSeqNo) override {
    if (callbacks_.on_rtn_private_seq_no != nullptr) {
      callbacks_.on_rtn_private_seq_no(nSeqNo, user_data_);
    }
  }

  void OnRspAuthenticate(CThostFtdcRspAuthenticateField* auth, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_authenticate == nullptr) {
      return;
    }
    ctp_rsp_authenticate auth_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_rsp_authenticate(auth_bridge, auth);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_authenticate(auth != nullptr ? &auth_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspSettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField* confirm, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_settlement_info_confirm == nullptr) {
      return;
    }
    ctp_settlement_info_confirm confirm_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_settlement_info_confirm(confirm_bridge, confirm);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_settlement_info_confirm(confirm != nullptr ? &confirm_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
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

  void OnRspQryTradingAccount(CThostFtdcTradingAccountField* account, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_trading_account == nullptr) {
      return;
    }
    ctp_trading_account account_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_trading_account(account_bridge, account);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_trading_account(account != nullptr ? &account_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorPosition(CThostFtdcInvestorPositionField* position, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_position == nullptr) {
      return;
    }
    ctp_investor_position position_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_investor_position(position_bridge, position);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_position(position != nullptr ? &position_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInstrumentMarginRate(CThostFtdcInstrumentMarginRateField* margin_rate, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_instrument_margin_rate == nullptr) {
      return;
    }
    ctp_instrument_margin_rate margin_rate_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_instrument_margin_rate(margin_rate_bridge, margin_rate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_instrument_margin_rate(margin_rate != nullptr ? &margin_rate_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryExchangeMarginRate(CThostFtdcExchangeMarginRateField* margin_rate, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_exchange_margin_rate == nullptr) {
      return;
    }
    ctp_exchange_margin_rate margin_rate_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_exchange_margin_rate(margin_rate_bridge, margin_rate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_exchange_margin_rate(margin_rate != nullptr ? &margin_rate_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInstrumentCommissionRate(CThostFtdcInstrumentCommissionRateField* commission_rate, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_instrument_commission_rate == nullptr) {
      return;
    }
    ctp_instrument_commission_rate commission_rate_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_instrument_commission_rate(commission_rate_bridge, commission_rate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_instrument_commission_rate(commission_rate != nullptr ? &commission_rate_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspOrderInsert(CThostFtdcInputOrderField* input_order, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_order_insert == nullptr) {
      return;
    }
    ctp_input_order order_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_input_order(order_bridge, input_order);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_order_insert(input_order != nullptr ? &order_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspOrderAction(CThostFtdcInputOrderActionField* action, CThostFtdcRspInfoField* rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_order_action == nullptr) {
      return;
    }
    ctp_input_order_action action_bridge {};
    ctp_rsp_info rsp_bridge {};
    fill_input_order_action(action_bridge, action);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_order_action(action != nullptr ? &action_bridge : nullptr, rsp_info != nullptr ? &rsp_bridge : nullptr, request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRtnOrder(CThostFtdcOrderField* order) override {
    if (callbacks_.on_rtn_order == nullptr || order == nullptr) {
      return;
    }
    ctp_order bridge {};
    fill_order(bridge, order);
    callbacks_.on_rtn_order(&bridge, user_data_);
  }

  void OnRtnTrade(CThostFtdcTradeField* trade) override {
    if (callbacks_.on_rtn_trade == nullptr || trade == nullptr) {
      return;
    }
    ctp_trade bridge {};
    fill_trade(bridge, trade);
    callbacks_.on_rtn_trade(&bridge, user_data_);
  }

private:
  ctp_trader_spi callbacks_ {};
  void* user_data_ {};
};

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

void fill_req_authenticate(CThostFtdcReqAuthenticateField& dest, const ctp_req_authenticate& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.AuthCode, src.auth_code);
  copy_field(dest.AppID, src.app_id);
}

void fill_settlement_info_confirm(CThostFtdcSettlementInfoConfirmField& dest, const ctp_settlement_info_confirm& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ConfirmDate, src.confirm_date);
  copy_field(dest.ConfirmTime, src.confirm_time);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_qry_trading_account(CThostFtdcQryTradingAccountField& dest, const ctp_qry_trading_account& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.CurrencyID, src.currency_id);
  dest.BizType = src.biz_type;
  copy_field(dest.AccountID, src.account_id);
}

void fill_qry_investor_position(CThostFtdcQryInvestorPositionField& dest, const ctp_qry_investor_position& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_instrument_margin_rate(CThostFtdcQryInstrumentMarginRateField& dest, const ctp_qry_instrument_margin_rate& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_exchange_margin_rate(CThostFtdcQryExchangeMarginRateField& dest, const ctp_qry_exchange_margin_rate& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_instrument_commission_rate(CThostFtdcQryInstrumentCommissionRateField& dest, const ctp_qry_instrument_commission_rate& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_input_order(CThostFtdcInputOrderField& dest, const ctp_input_order& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.OrderRef, src.order_ref);
  copy_field(dest.UserID, src.user_id);
  dest.OrderPriceType = src.order_price_type;
  dest.Direction = src.direction;
  copy_field(dest.CombOffsetFlag, src.comb_offset_flag);
  copy_field(dest.CombHedgeFlag, src.comb_hedge_flag);
  dest.LimitPrice = src.limit_price;
  dest.VolumeTotalOriginal = src.volume_total_original;
  dest.TimeCondition = src.time_condition;
  copy_field(dest.GTDDate, src.gtd_date);
  dest.VolumeCondition = src.volume_condition;
  dest.MinVolume = src.min_volume;
  dest.ContingentCondition = src.contingent_condition;
  dest.StopPrice = src.stop_price;
  dest.ForceCloseReason = src.force_close_reason;
  dest.IsAutoSuspend = src.is_auto_suspend;
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.RequestID = src.request_id;
  dest.UserForceClose = src.user_force_close;
  dest.IsSwapOrder = src.is_swap_order;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.OrderMemo, src.order_memo);
}

void fill_input_order_action(CThostFtdcInputOrderActionField& dest, const ctp_input_order_action& src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.OrderActionRef = src.order_action_ref;
  copy_field(dest.OrderRef, src.order_ref);
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OrderSysID, src.order_sys_id);
  dest.ActionFlag = src.action_flag;
  dest.LimitPrice = src.limit_price;
  dest.VolumeChange = src.volume_change;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.OrderMemo, src.order_memo);
}

}  // namespace

struct ctp_trader_handle {
  CThostFtdcTraderApi* api {};
  std::unique_ptr<TraderSpiAdapter> spi {};
};

extern "C" {

const char* ctp_trader_get_api_version(void) {
  return CThostFtdcTraderApi::GetApiVersion();
}

ctp_trader_handle* ctp_trader_create(const char* flow_path, int32_t production_mode) {
  auto* handle = new (std::nothrow) ctp_trader_handle();
  if (handle == nullptr) {
    return nullptr;
  }
  handle->api = CThostFtdcTraderApi::CreateFtdcTraderApi(flow_path != nullptr ? flow_path : "", production_mode != 0);
  if (handle->api == nullptr) {
    delete handle;
    return nullptr;
  }
  return handle;
}

void ctp_trader_destroy(ctp_trader_handle* handle) {
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

int32_t ctp_trader_set_spi(ctp_trader_handle* handle, const ctp_trader_spi* spi, void* user_data) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  handle->spi.reset();
  if (spi == nullptr) {
    handle->api->RegisterSpi(nullptr);
    return 0;
  }
  handle->spi = std::make_unique<TraderSpiAdapter>(*spi, user_data);
  handle->api->RegisterSpi(handle->spi.get());
  return 0;
}

int32_t ctp_trader_register_front(ctp_trader_handle* handle, const char* front_address) {
  if (handle == nullptr || handle->api == nullptr || front_address == nullptr) {
    return -1;
  }
  std::string front(front_address);
  handle->api->RegisterFront(front.data());
  return 0;
}

int32_t ctp_trader_subscribe_private_topic(ctp_trader_handle* handle, int32_t resume_type, int32_t seq_no) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  handle->api->SubscribePrivateTopic(static_cast<THOST_TE_RESUME_TYPE>(resume_type), seq_no);
  return 0;
}

int32_t ctp_trader_subscribe_public_topic(ctp_trader_handle* handle, int32_t resume_type) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  handle->api->SubscribePublicTopic(static_cast<THOST_TE_RESUME_TYPE>(resume_type));
  return 0;
}

void ctp_trader_init(ctp_trader_handle* handle) {
  if (handle != nullptr && handle->api != nullptr) {
    handle->api->Init();
  }
}

int32_t ctp_trader_join(ctp_trader_handle* handle) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  return handle->api->Join();
}

int32_t ctp_trader_req_authenticate(ctp_trader_handle* handle, const ctp_req_authenticate* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqAuthenticateField native_request {};
  fill_req_authenticate(native_request, *request);
  return handle->api->ReqAuthenticate(&native_request, request_id);
}

int32_t ctp_trader_req_settlement_info_confirm(ctp_trader_handle* handle, const ctp_settlement_info_confirm* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcSettlementInfoConfirmField native_request {};
  fill_settlement_info_confirm(native_request, *request);
  return handle->api->ReqSettlementInfoConfirm(&native_request, request_id);
}

int32_t ctp_trader_req_user_login(ctp_trader_handle* handle, const ctp_req_user_login* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqUserLoginField native_request {};
  fill_req_user_login(native_request, *request);
  return handle->api->ReqUserLogin(&native_request, request_id);
}

int32_t ctp_trader_req_user_logout(ctp_trader_handle* handle, const ctp_user_logout* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcUserLogoutField native_request {};
  fill_req_user_logout(native_request, *request);
  return handle->api->ReqUserLogout(&native_request, request_id);
}

int32_t ctp_trader_req_qry_trading_account(ctp_trader_handle* handle, const ctp_qry_trading_account* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryTradingAccountField native_request {};
  fill_qry_trading_account(native_request, *request);
  return handle->api->ReqQryTradingAccount(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor_position(ctp_trader_handle* handle, const ctp_qry_investor_position* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorPositionField native_request {};
  fill_qry_investor_position(native_request, *request);
  return handle->api->ReqQryInvestorPosition(&native_request, request_id);
}

int32_t ctp_trader_req_qry_instrument_margin_rate(ctp_trader_handle* handle, const ctp_qry_instrument_margin_rate* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInstrumentMarginRateField native_request {};
  fill_qry_instrument_margin_rate(native_request, *request);
  return handle->api->ReqQryInstrumentMarginRate(&native_request, request_id);
}

int32_t ctp_trader_req_qry_exchange_margin_rate(ctp_trader_handle* handle, const ctp_qry_exchange_margin_rate* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryExchangeMarginRateField native_request {};
  fill_qry_exchange_margin_rate(native_request, *request);
  return handle->api->ReqQryExchangeMarginRate(&native_request, request_id);
}

int32_t ctp_trader_req_qry_instrument_commission_rate(ctp_trader_handle* handle, const ctp_qry_instrument_commission_rate* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInstrumentCommissionRateField native_request {};
  fill_qry_instrument_commission_rate(native_request, *request);
  return handle->api->ReqQryInstrumentCommissionRate(&native_request, request_id);
}

int32_t ctp_trader_req_order_insert(ctp_trader_handle* handle, const ctp_input_order* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputOrderField native_request {};
  fill_input_order(native_request, *request);
  return handle->api->ReqOrderInsert(&native_request, request_id);
}

int32_t ctp_trader_req_order_action(ctp_trader_handle* handle, const ctp_input_order_action* request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputOrderActionField native_request {};
  fill_input_order_action(native_request, *request);
  return handle->api->ReqOrderAction(&native_request, request_id);
}

}  // extern "C"
