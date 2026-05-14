#include "ctp_bridge.h"

#include <cstring>
#include <memory>
#include <new>
#include <string>

#include "ThostFtdcTraderApi.h"

namespace {

template <size_t N> void copy_field(char (&dest)[N], const char *src) {
  std::memset(dest, 0, N);
  if (src != nullptr) {
    std::strncpy(dest, src, N - 1);
  }
}

void fill_rsp_info(ctp_rsp_info &dest, const CThostFtdcRspInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  dest.error_id = src->ErrorID;
  copy_field(dest.error_msg, src->ErrorMsg);
}

void fill_rsp_authenticate(ctp_rsp_authenticate &dest,
                           const CThostFtdcRspAuthenticateField *src) {
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

void fill_settlement_info_confirm(
    ctp_settlement_info_confirm &dest,
    const CThostFtdcSettlementInfoConfirmField *src) {
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

void fill_rsp_user_login(ctp_rsp_user_login &dest,
                         const CThostFtdcRspUserLoginField *src) {
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

void fill_user_logout(ctp_user_logout &dest,
                      const CThostFtdcUserLogoutField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
}

void fill_trading_account(ctp_trading_account &dest,
                          const CThostFtdcTradingAccountField *src) {
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

void fill_investor_position(ctp_investor_position &dest,
                            const CThostFtdcInvestorPositionField *src) {
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

void fill_instrument_margin_rate(
    ctp_instrument_margin_rate &dest,
    const CThostFtdcInstrumentMarginRateField *src) {
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

void fill_exchange_margin_rate(ctp_exchange_margin_rate &dest,
                               const CThostFtdcExchangeMarginRateField *src) {
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

void fill_instrument_commission_rate(
    ctp_instrument_commission_rate &dest,
    const CThostFtdcInstrumentCommissionRateField *src) {
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

void fill_input_order(ctp_input_order &dest,
                      const CThostFtdcInputOrderField *src) {
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

void fill_input_order_action(ctp_input_order_action &dest,
                             const CThostFtdcInputOrderActionField *src) {
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

void fill_order(ctp_order &dest, const CThostFtdcOrderField *src) {
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

void fill_trade(ctp_trade &dest, const CThostFtdcTradeField *src) {
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

void fill_depth_market_data(ctp_depth_market_data &dest,
                            const CThostFtdcDepthMarketDataField *src) {
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
  copy_field(dest.instrument_id, src->reserve1);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  dest.banding_upper_price = src->BandingUpperPrice;
  dest.banding_lower_price = src->BandingLowerPrice;
}

void fill_accountregister(ctp_accountregister &dest,
                          const CThostFtdcAccountregisterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trade_day, src->TradeDay);
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_branch_id, src->BankBranchID);
  copy_field(dest.bank_account, src->BankAccount);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.broker_branch_id, src->BrokerBranchID);
  copy_field(dest.account_id, src->AccountID);
  dest.id_card_type = src->IdCardType;
  copy_field(dest.identified_card_no, src->IdentifiedCardNo);
  copy_field(dest.customer_name, src->CustomerName);
  copy_field(dest.currency_id, src->CurrencyID);
  dest.open_or_destroy = src->OpenOrDestroy;
  copy_field(dest.reg_date, src->RegDate);
  copy_field(dest.out_date, src->OutDate);
  dest.t_id = src->TID;
  dest.cust_type = src->CustType;
  dest.bank_acc_type = src->BankAccType;
  copy_field(dest.long_customer_name, src->LongCustomerName);
}

void fill_batch_order_action(ctp_batch_order_action &dest,
                             const CThostFtdcBatchOrderActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.order_action_ref = src->OrderActionRef;
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.action_date, src->ActionDate);
  copy_field(dest.action_time, src->ActionTime);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  copy_field(dest.action_local_id, src->ActionLocalID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.order_action_status = src->OrderActionStatus;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_broker_trading_algos(ctp_broker_trading_algos &dest,
                               const CThostFtdcBrokerTradingAlgosField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.reserve1, src->reserve1);
  dest.handle_position_algo_id = src->HandlePositionAlgoID;
  dest.find_margin_rate_algo_id = src->FindMarginRateAlgoID;
  dest.handle_trading_account_algo_id = src->HandleTradingAccountAlgoID;
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_broker_trading_params(ctp_broker_trading_params &dest,
                                const CThostFtdcBrokerTradingParamsField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.margin_price_type = src->MarginPriceType;
  dest.algorithm = src->Algorithm;
  dest.avail_include_close_profit = src->AvailIncludeCloseProfit;
  copy_field(dest.currency_id, src->CurrencyID);
  dest.option_royalty_price_type = src->OptionRoyaltyPriceType;
  copy_field(dest.account_id, src->AccountID);
}

void fill_cfmmc_trading_account_key(
    ctp_cfmmc_trading_account_key &dest,
    const CThostFtdcCFMMCTradingAccountKeyField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.account_id, src->AccountID);
  dest.key_id = src->KeyID;
  copy_field(dest.current_key, src->CurrentKey);
}

void fill_cancel_offset_setting(ctp_cancel_offset_setting &dest,
                                const CThostFtdcCancelOffsetSettingField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.underlying_instr_id, src->UnderlyingInstrID);
  copy_field(dest.product_id, src->ProductID);
  dest.offset_type = src->OffsetType;
  dest.volume = src->Volume;
  dest.is_offset = src->IsOffset;
  dest.request_id = src->RequestID;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.exchange_serial_no, src->ExchangeSerialNo);
  copy_field(dest.exchange_product_id, src->ExchangeProductID);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  dest.order_action_status = src->OrderActionStatus;
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.action_local_id, src->ActionLocalID);
  copy_field(dest.action_date, src->ActionDate);
  copy_field(dest.action_time, src->ActionTime);
}

void fill_comb_action(ctp_comb_action &dest,
                      const CThostFtdcCombActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.comb_action_ref, src->CombActionRef);
  copy_field(dest.user_id, src->UserID);
  dest.direction = src->Direction;
  dest.volume = src->Volume;
  dest.comb_direction = src->CombDirection;
  dest.hedge_flag = src->HedgeFlag;
  copy_field(dest.action_local_id, src->ActionLocalID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  dest.action_status = src->ActionStatus;
  dest.notify_sequence = src->NotifySequence;
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  dest.sequence_no = src->SequenceNo;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.user_product_info, src->UserProductInfo);
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.reserve3, src->reserve3);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.com_trade_id, src->ComTradeID);
  copy_field(dest.branch_id, src->BranchID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_comb_instrument_guard(ctp_comb_instrument_guard &dest,
                                const CThostFtdcCombInstrumentGuardField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.reserve1, src->reserve1);
  dest.guarant_ratio = src->GuarantRatio;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_comb_leg(ctp_comb_leg &dest, const CThostFtdcCombLegField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.comb_instrument_id, src->CombInstrumentID);
  dest.leg_id = src->LegID;
  copy_field(dest.leg_instrument_id, src->LegInstrumentID);
  dest.direction = src->Direction;
  dest.leg_multiple = src->LegMultiple;
  dest.imply_level = src->ImplyLevel;
}

void fill_comb_promotion_param(ctp_comb_promotion_param &dest,
                               const CThostFtdcCombPromotionParamField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.comb_hedge_flag, src->CombHedgeFlag);
  dest.xparameter = src->Xparameter;
}

void fill_contract_bank(ctp_contract_bank &dest,
                        const CThostFtdcContractBankField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_brch_id, src->BankBrchID);
  copy_field(dest.bank_name, src->BankName);
  copy_field(dest.csrc_bank_id, src->csrcBankID);
}

void fill_e_warrant_offset(ctp_e_warrant_offset &dest,
                           const CThostFtdcEWarrantOffsetField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.reserve1, src->reserve1);
  dest.direction = src->Direction;
  dest.hedge_flag = src->HedgeFlag;
  dest.volume = src->Volume;
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_exchange(ctp_exchange &dest, const CThostFtdcExchangeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.exchange_name, src->ExchangeName);
  dest.exchange_property = src->ExchangeProperty;
}

void fill_exchange_margin_rate_adjust(
    ctp_exchange_margin_rate_adjust &dest,
    const CThostFtdcExchangeMarginRateAdjustField *src) {
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
  dest.exch_long_margin_ratio_by_money = src->ExchLongMarginRatioByMoney;
  dest.exch_long_margin_ratio_by_volume = src->ExchLongMarginRatioByVolume;
  dest.exch_short_margin_ratio_by_money = src->ExchShortMarginRatioByMoney;
  dest.exch_short_margin_ratio_by_volume = src->ExchShortMarginRatioByVolume;
  dest.no_long_margin_ratio_by_money = src->NoLongMarginRatioByMoney;
  dest.no_long_margin_ratio_by_volume = src->NoLongMarginRatioByVolume;
  dest.no_short_margin_ratio_by_money = src->NoShortMarginRatioByMoney;
  dest.no_short_margin_ratio_by_volume = src->NoShortMarginRatioByVolume;
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_exchange_rate(ctp_exchange_rate &dest,
                        const CThostFtdcExchangeRateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.from_currency_id, src->FromCurrencyID);
  dest.from_currency_unit = src->FromCurrencyUnit;
  copy_field(dest.to_currency_id, src->ToCurrencyID);
  dest.exchange_rate = src->ExchangeRate;
}

void fill_exec_order_action(ctp_exec_order_action &dest,
                            const CThostFtdcExecOrderActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.exec_order_action_ref = src->ExecOrderActionRef;
  copy_field(dest.exec_order_ref, src->ExecOrderRef);
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.exec_order_sys_id, src->ExecOrderSysID);
  dest.action_flag = src->ActionFlag;
  copy_field(dest.action_date, src->ActionDate);
  copy_field(dest.action_time, src->ActionTime);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  copy_field(dest.exec_order_local_id, src->ExecOrderLocalID);
  copy_field(dest.action_local_id, src->ActionLocalID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.order_action_status = src->OrderActionStatus;
  copy_field(dest.user_id, src->UserID);
  dest.action_type = src->ActionType;
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.branch_id, src->BranchID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_exec_order(ctp_exec_order &dest,
                     const CThostFtdcExecOrderField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exec_order_ref, src->ExecOrderRef);
  copy_field(dest.user_id, src->UserID);
  dest.volume = src->Volume;
  dest.request_id = src->RequestID;
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.offset_flag = src->OffsetFlag;
  dest.hedge_flag = src->HedgeFlag;
  dest.action_type = src->ActionType;
  dest.posi_direction = src->PosiDirection;
  dest.reserve_position_flag = src->ReservePositionFlag;
  dest.close_flag = src->CloseFlag;
  copy_field(dest.exec_order_local_id, src->ExecOrderLocalID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  dest.order_submit_status = src->OrderSubmitStatus;
  dest.notify_sequence = src->NotifySequence;
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  copy_field(dest.exec_order_sys_id, src->ExecOrderSysID);
  copy_field(dest.insert_date, src->InsertDate);
  copy_field(dest.insert_time, src->InsertTime);
  copy_field(dest.cancel_time, src->CancelTime);
  dest.exec_result = src->ExecResult;
  copy_field(dest.clearing_part_id, src->ClearingPartID);
  dest.sequence_no = src->SequenceNo;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.user_product_info, src->UserProductInfo);
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.active_user_id, src->ActiveUserID);
  dest.broker_exec_order_seq = src->BrokerExecOrderSeq;
  copy_field(dest.branch_id, src->BranchID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.reserve3, src->reserve3);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_fens_user_info(ctp_fens_user_info &dest,
                         const CThostFtdcFensUserInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  dest.login_mode = src->LoginMode;
}

void fill_for_quote(ctp_for_quote &dest, const CThostFtdcForQuoteField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.for_quote_ref, src->ForQuoteRef);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.for_quote_local_id, src->ForQuoteLocalID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  copy_field(dest.insert_date, src->InsertDate);
  copy_field(dest.insert_time, src->InsertTime);
  dest.for_quote_status = src->ForQuoteStatus;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.active_user_id, src->ActiveUserID);
  dest.broker_for_quto_seq = src->BrokerForQutoSeq;
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve3, src->reserve3);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_for_quote_rsp(ctp_for_quote_rsp &dest,
                        const CThostFtdcForQuoteRspField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.for_quote_sys_id, src->ForQuoteSysID);
  copy_field(dest.for_quote_time, src->ForQuoteTime);
  copy_field(dest.action_day, src->ActionDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_front_info(ctp_front_info &dest,
                     const CThostFtdcFrontInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.front_addr, src->FrontAddr);
  dest.qry_freq = src->QryFreq;
  dest.ftd_pkg_freq = src->FTDPkgFreq;
}

void fill_hedge_cfm_action(ctp_hedge_cfm_action &dest,
                           const CThostFtdcHedgeCfmActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.action_date, src->ActionDate);
  copy_field(dest.action_time, src->ActionTime);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  copy_field(dest.order_local_id, src->OrderLocalID);
  copy_field(dest.action_local_id, src->ActionLocalID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  dest.order_action_status = src->OrderActionStatus;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_sys_id, src->OrderSysID);
  dest.request_id = src->RequestID;
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.order_ref, src->OrderRef);
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
}

void fill_hedge_cfm(ctp_hedge_cfm &dest, const CThostFtdcHedgeCfmField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.user_id, src->UserID);
  dest.volume = src->Volume;
  dest.direction = src->Direction;
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.order_ref, src->OrderRef);
  copy_field(dest.active_user_id, src->ActiveUserID);
  dest.broker_order_seq = src->BrokerOrderSeq;
  copy_field(dest.order_sys_id, src->OrderSysID);
  dest.apply_status = src->ApplyStatus;
  dest.sequence_no = src->SequenceNo;
  dest.deal_volume = src->DealVolume;
  copy_field(dest.insert_date, src->InsertDate);
  copy_field(dest.insert_time, src->InsertTime);
  copy_field(dest.cancel_time, src->CancelTime);
  copy_field(dest.req_date, src->ReqDate);
  copy_field(dest.order_local_id, src->OrderLocalID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  dest.order_submit_status = src->OrderSubmitStatus;
  dest.notify_sequence = src->NotifySequence;
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
}

void fill_input_batch_order_action(
    ctp_input_batch_order_action &dest,
    const CThostFtdcInputBatchOrderActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.order_action_ref = src->OrderActionRef;
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_input_comb_action(ctp_input_comb_action &dest,
                            const CThostFtdcInputCombActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.comb_action_ref, src->CombActionRef);
  copy_field(dest.user_id, src->UserID);
  dest.direction = src->Direction;
  dest.volume = src->Volume;
  dest.comb_direction = src->CombDirection;
  dest.hedge_flag = src->HedgeFlag;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_input_exec_order_action(
    ctp_input_exec_order_action &dest,
    const CThostFtdcInputExecOrderActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.exec_order_action_ref = src->ExecOrderActionRef;
  copy_field(dest.exec_order_ref, src->ExecOrderRef);
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.exec_order_sys_id, src->ExecOrderSysID);
  dest.action_flag = src->ActionFlag;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_input_exec_order(ctp_input_exec_order &dest,
                           const CThostFtdcInputExecOrderField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exec_order_ref, src->ExecOrderRef);
  copy_field(dest.user_id, src->UserID);
  dest.volume = src->Volume;
  dest.request_id = src->RequestID;
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.offset_flag = src->OffsetFlag;
  dest.hedge_flag = src->HedgeFlag;
  dest.action_type = src->ActionType;
  dest.posi_direction = src->PosiDirection;
  dest.reserve_position_flag = src->ReservePositionFlag;
  dest.close_flag = src->CloseFlag;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_input_for_quote(ctp_input_for_quote &dest,
                          const CThostFtdcInputForQuoteField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.for_quote_ref, src->ForQuoteRef);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_input_hedge_cfm_action(
    ctp_input_hedge_cfm_action &dest,
    const CThostFtdcInputHedgeCfmActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_sys_id, src->OrderSysID);
  copy_field(dest.order_ref, src->OrderRef);
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  dest.request_id = src->RequestID;
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
}

void fill_input_hedge_cfm(ctp_input_hedge_cfm &dest,
                          const CThostFtdcInputHedgeCfmField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
  dest.volume = src->Volume;
  dest.direction = src->Direction;
  dest.request_id = src->RequestID;
  copy_field(dest.order_ref, src->OrderRef);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
}

void fill_input_offset_setting(ctp_input_offset_setting &dest,
                               const CThostFtdcInputOffsetSettingField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.underlying_instr_id, src->UnderlyingInstrID);
  copy_field(dest.product_id, src->ProductID);
  dest.offset_type = src->OffsetType;
  dest.volume = src->Volume;
  dest.is_offset = src->IsOffset;
  dest.request_id = src->RequestID;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
}

void fill_input_option_self_close_action(
    ctp_input_option_self_close_action &dest,
    const CThostFtdcInputOptionSelfCloseActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.option_self_close_action_ref = src->OptionSelfCloseActionRef;
  copy_field(dest.option_self_close_ref, src->OptionSelfCloseRef);
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.option_self_close_sys_id, src->OptionSelfCloseSysID);
  dest.action_flag = src->ActionFlag;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_input_option_self_close(
    ctp_input_option_self_close &dest,
    const CThostFtdcInputOptionSelfCloseField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.option_self_close_ref, src->OptionSelfCloseRef);
  copy_field(dest.user_id, src->UserID);
  dest.volume = src->Volume;
  dest.request_id = src->RequestID;
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.hedge_flag = src->HedgeFlag;
  dest.opt_self_close_flag = src->OptSelfCloseFlag;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_input_quote_action(ctp_input_quote_action &dest,
                             const CThostFtdcInputQuoteActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.quote_action_ref = src->QuoteActionRef;
  copy_field(dest.quote_ref, src->QuoteRef);
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.quote_sys_id, src->QuoteSysID);
  dest.action_flag = src->ActionFlag;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.order_memo, src->OrderMemo);
  dest.session_req_seq = src->SessionReqSeq;
}

void fill_input_quote(ctp_input_quote &dest,
                      const CThostFtdcInputQuoteField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.quote_ref, src->QuoteRef);
  copy_field(dest.user_id, src->UserID);
  dest.ask_price = src->AskPrice;
  dest.bid_price = src->BidPrice;
  dest.ask_volume = src->AskVolume;
  dest.bid_volume = src->BidVolume;
  dest.request_id = src->RequestID;
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.ask_offset_flag = src->AskOffsetFlag;
  dest.bid_offset_flag = src->BidOffsetFlag;
  dest.ask_hedge_flag = src->AskHedgeFlag;
  dest.bid_hedge_flag = src->BidHedgeFlag;
  copy_field(dest.ask_order_ref, src->AskOrderRef);
  copy_field(dest.bid_order_ref, src->BidOrderRef);
  copy_field(dest.for_quote_sys_id, src->ForQuoteSysID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.replace_sys_id, src->ReplaceSysID);
  dest.time_condition = src->TimeCondition;
  copy_field(dest.order_memo, src->OrderMemo);
  dest.session_req_seq = src->SessionReqSeq;
}

void fill_input_spd_apply_action(
    ctp_input_spd_apply_action &dest,
    const CThostFtdcInputSpdApplyActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_sys_id, src->OrderSysID);
  copy_field(dest.order_ref, src->OrderRef);
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  dest.request_id = src->RequestID;
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
}

void fill_input_spd_apply(ctp_input_spd_apply &dest,
                          const CThostFtdcInputSpdApplyField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.first_leg_instrument_id, src->FirstLegInstrumentID);
  copy_field(dest.second_leg_instrument_id, src->SecondLegInstrumentID);
  dest.volume = src->Volume;
  dest.direction = src->Direction;
  dest.cmb_type = src->CmbType;
  dest.request_id = src->RequestID;
  copy_field(dest.order_ref, src->OrderRef);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
}

void fill_instrument(ctp_instrument &dest,
                     const CThostFtdcInstrumentField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_name, src->InstrumentName);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.reserve3, src->reserve3);
  dest.product_class = src->ProductClass;
  dest.delivery_year = src->DeliveryYear;
  dest.delivery_month = src->DeliveryMonth;
  dest.max_market_order_volume = src->MaxMarketOrderVolume;
  dest.min_market_order_volume = src->MinMarketOrderVolume;
  dest.max_limit_order_volume = src->MaxLimitOrderVolume;
  dest.min_limit_order_volume = src->MinLimitOrderVolume;
  dest.volume_multiple = src->VolumeMultiple;
  dest.price_tick = src->PriceTick;
  copy_field(dest.create_date, src->CreateDate);
  copy_field(dest.open_date, src->OpenDate);
  copy_field(dest.expire_date, src->ExpireDate);
  copy_field(dest.start_deliv_date, src->StartDelivDate);
  copy_field(dest.end_deliv_date, src->EndDelivDate);
  dest.inst_life_phase = src->InstLifePhase;
  dest.is_trading = src->IsTrading;
  dest.position_type = src->PositionType;
  dest.position_date_type = src->PositionDateType;
  dest.long_margin_ratio = src->LongMarginRatio;
  dest.short_margin_ratio = src->ShortMarginRatio;
  dest.max_margin_side_algorithm = src->MaxMarginSideAlgorithm;
  copy_field(dest.reserve4, src->reserve4);
  dest.strike_price = src->StrikePrice;
  dest.options_type = src->OptionsType;
  dest.underlying_multiple = src->UnderlyingMultiple;
  dest.combination_type = src->CombinationType;
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.product_id, src->ProductID);
  copy_field(dest.underlying_instr_id, src->UnderlyingInstrID);
}

void fill_instrument_order_comm_rate(
    ctp_instrument_order_comm_rate &dest,
    const CThostFtdcInstrumentOrderCommRateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  dest.investor_range = src->InvestorRange;
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.hedge_flag = src->HedgeFlag;
  dest.order_comm_by_volume = src->OrderCommByVolume;
  dest.order_action_comm_by_volume = src->OrderActionCommByVolume;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
  dest.order_comm_by_trade = src->OrderCommByTrade;
  dest.order_action_comm_by_trade = src->OrderActionCommByTrade;
}

void fill_invest_unit(ctp_invest_unit &dest,
                      const CThostFtdcInvestUnitField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.investor_unit_name, src->InvestorUnitName);
  copy_field(dest.investor_group_id, src->InvestorGroupID);
  copy_field(dest.comm_model_id, src->CommModelID);
  copy_field(dest.margin_model_id, src->MarginModelID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
}

void fill_investor_commodity_group_spmm_margin(
    ctp_investor_commodity_group_spmm_margin &dest,
    const CThostFtdcInvestorCommodityGroupSPMMMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.commodity_group_id, src->CommodityGroupID);
  dest.margin_before_discount = src->MarginBeforeDiscount;
  dest.margin_no_discount = src->MarginNoDiscount;
  dest.long_risk = src->LongRisk;
  dest.short_risk = src->ShortRisk;
  dest.close_frozen_margin = src->CloseFrozenMargin;
  dest.inter_commodity_rate = src->InterCommodityRate;
  dest.mini_margin_ratio = src->MiniMarginRatio;
  dest.adjust_ratio = src->AdjustRatio;
  dest.intra_commodity_discount = src->IntraCommodityDiscount;
  dest.inter_commodity_discount = src->InterCommodityDiscount;
  dest.exch_margin = src->ExchMargin;
  dest.investor_margin = src->InvestorMargin;
  dest.frozen_commission = src->FrozenCommission;
  dest.commission = src->Commission;
  dest.frozen_cash = src->FrozenCash;
  dest.cash_in = src->CashIn;
  dest.strike_frozen_margin = src->StrikeFrozenMargin;
}

void fill_investor_commodity_spmm_margin(
    ctp_investor_commodity_spmm_margin &dest,
    const CThostFtdcInvestorCommoditySPMMMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.commodity_id, src->CommodityID);
  dest.margin_before_discount = src->MarginBeforeDiscount;
  dest.margin_no_discount = src->MarginNoDiscount;
  dest.long_pos_risk = src->LongPosRisk;
  dest.long_open_frozen_risk = src->LongOpenFrozenRisk;
  dest.long_close_frozen_risk = src->LongCloseFrozenRisk;
  dest.short_pos_risk = src->ShortPosRisk;
  dest.short_open_frozen_risk = src->ShortOpenFrozenRisk;
  dest.short_close_frozen_risk = src->ShortCloseFrozenRisk;
  dest.intra_commodity_rate = src->IntraCommodityRate;
  dest.option_discount_rate = src->OptionDiscountRate;
  dest.pos_discount = src->PosDiscount;
  dest.open_frozen_discount = src->OpenFrozenDiscount;
  dest.net_risk = src->NetRisk;
  dest.close_frozen_margin = src->CloseFrozenMargin;
  dest.frozen_commission = src->FrozenCommission;
  dest.commission = src->Commission;
  dest.frozen_cash = src->FrozenCash;
  dest.cash_in = src->CashIn;
  dest.strike_frozen_margin = src->StrikeFrozenMargin;
}

void fill_investor(ctp_investor &dest, const CThostFtdcInvestorField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_group_id, src->InvestorGroupID);
  copy_field(dest.investor_name, src->InvestorName);
  dest.identified_card_type = src->IdentifiedCardType;
  copy_field(dest.identified_card_no, src->IdentifiedCardNo);
  dest.is_active = src->IsActive;
  copy_field(dest.telephone, src->Telephone);
  copy_field(dest.address, src->Address);
  copy_field(dest.open_date, src->OpenDate);
  copy_field(dest.mobile, src->Mobile);
  copy_field(dest.comm_model_id, src->CommModelID);
  copy_field(dest.margin_model_id, src->MarginModelID);
  dest.is_order_freq = src->IsOrderFreq;
  dest.is_open_vol_limit = src->IsOpenVolLimit;
}

void fill_investor_info_comm_rec(
    ctp_investor_info_comm_rec &dest,
    const CThostFtdcInvestorInfoCommRecField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  dest.order_count = src->OrderCount;
  dest.order_action_count = src->OrderActionCount;
  dest.for_quote_cnt = src->ForQuoteCnt;
  dest.info_comm = src->InfoComm;
  dest.is_opt_series = src->IsOptSeries;
  copy_field(dest.product_id, src->ProductID);
  dest.info_cnt = src->InfoCnt;
}

void fill_investor_portf_margin_ratio(
    ctp_investor_portf_margin_ratio &dest,
    const CThostFtdcInvestorPortfMarginRatioField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  dest.investor_range = src->InvestorRange;
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.margin_ratio = src->MarginRatio;
  copy_field(dest.product_group_id, src->ProductGroupID);
}

void fill_investor_portf_setting(
    ctp_investor_portf_setting &dest,
    const CThostFtdcInvestorPortfSettingField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.hedge_flag = src->HedgeFlag;
  dest.use_portf = src->UsePortf;
}

void fill_investor_position_combine_detail(
    ctp_investor_position_combine_detail &dest,
    const CThostFtdcInvestorPositionCombineDetailField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.open_date, src->OpenDate);
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.settlement_id = src->SettlementID;
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.com_trade_id, src->ComTradeID);
  copy_field(dest.trade_id, src->TradeID);
  copy_field(dest.reserve1, src->reserve1);
  dest.hedge_flag = src->HedgeFlag;
  dest.direction = src->Direction;
  dest.total_amt = src->TotalAmt;
  dest.margin = src->Margin;
  dest.exch_margin = src->ExchMargin;
  dest.margin_rate_by_money = src->MarginRateByMoney;
  dest.margin_rate_by_volume = src->MarginRateByVolume;
  dest.leg_id = src->LegID;
  dest.leg_multiple = src->LegMultiple;
  copy_field(dest.reserve2, src->reserve2);
  dest.trade_group_id = src->TradeGroupID;
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.comb_instrument_id, src->CombInstrumentID);
}

void fill_investor_position_detail(
    ctp_investor_position_detail &dest,
    const CThostFtdcInvestorPositionDetailField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.hedge_flag = src->HedgeFlag;
  dest.direction = src->Direction;
  copy_field(dest.open_date, src->OpenDate);
  copy_field(dest.trade_id, src->TradeID);
  dest.volume = src->Volume;
  dest.open_price = src->OpenPrice;
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  dest.trade_type = src->TradeType;
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.close_profit_by_date = src->CloseProfitByDate;
  dest.close_profit_by_trade = src->CloseProfitByTrade;
  dest.position_profit_by_date = src->PositionProfitByDate;
  dest.position_profit_by_trade = src->PositionProfitByTrade;
  dest.margin = src->Margin;
  dest.exch_margin = src->ExchMargin;
  dest.margin_rate_by_money = src->MarginRateByMoney;
  dest.margin_rate_by_volume = src->MarginRateByVolume;
  dest.last_settlement_price = src->LastSettlementPrice;
  dest.settlement_price = src->SettlementPrice;
  dest.close_volume = src->CloseVolume;
  dest.close_amount = src->CloseAmount;
  dest.time_first_volume = src->TimeFirstVolume;
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  dest.spec_posi_type = src->SpecPosiType;
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.comb_instrument_id, src->CombInstrumentID);
}

void fill_investor_prod_rcams_margin(
    ctp_investor_prod_rcams_margin &dest,
    const CThostFtdcInvestorProdRCAMSMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.comb_product_id, src->CombProductID);
  dest.hedge_flag = src->HedgeFlag;
  copy_field(dest.product_group_id, src->ProductGroupID);
  dest.risk_before_discount = src->RiskBeforeDiscount;
  dest.intra_instr_risk = src->IntraInstrRisk;
  dest.b_pos_risk = src->BPosRisk;
  dest.s_pos_risk = src->SPosRisk;
  dest.intra_prod_risk = src->IntraProdRisk;
  dest.net_risk = src->NetRisk;
  dest.inter_prod_risk = src->InterProdRisk;
  dest.short_opt_risk_adj = src->ShortOptRiskAdj;
  dest.option_royalty = src->OptionRoyalty;
  dest.mmsa_close_frozen_margin = src->MMSACloseFrozenMargin;
  dest.close_comb_frozen_margin = src->CloseCombFrozenMargin;
  dest.close_frozen_margin = src->CloseFrozenMargin;
  dest.mmsa_open_frozen_margin = src->MMSAOpenFrozenMargin;
  dest.delivery_open_frozen_margin = src->DeliveryOpenFrozenMargin;
  dest.open_frozen_margin = src->OpenFrozenMargin;
  dest.use_frozen_margin = src->UseFrozenMargin;
  dest.mmsa_exch_margin = src->MMSAExchMargin;
  dest.delivery_exch_margin = src->DeliveryExchMargin;
  dest.comb_exch_margin = src->CombExchMargin;
  dest.exch_margin = src->ExchMargin;
  dest.use_margin = src->UseMargin;
}

void fill_investor_prod_rule_margin(
    ctp_investor_prod_rule_margin &dest,
    const CThostFtdcInvestorProdRULEMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
  dest.instrument_class = src->InstrumentClass;
  dest.commodity_group_id = src->CommodityGroupID;
  dest.b_std_position = src->BStdPosition;
  dest.s_std_position = src->SStdPosition;
  dest.b_std_open_frozen = src->BStdOpenFrozen;
  dest.s_std_open_frozen = src->SStdOpenFrozen;
  dest.b_std_close_frozen = src->BStdCloseFrozen;
  dest.s_std_close_frozen = src->SStdCloseFrozen;
  dest.intra_prod_std_position = src->IntraProdStdPosition;
  dest.net_std_position = src->NetStdPosition;
  dest.inter_prod_std_position = src->InterProdStdPosition;
  dest.single_std_position = src->SingleStdPosition;
  dest.intra_prod_margin = src->IntraProdMargin;
  dest.inter_prod_margin = src->InterProdMargin;
  dest.single_margin = src->SingleMargin;
  dest.non_comb_margin = src->NonCombMargin;
  dest.add_on_margin = src->AddOnMargin;
  dest.exch_margin = src->ExchMargin;
  dest.add_on_frozen_margin = src->AddOnFrozenMargin;
  dest.open_frozen_margin = src->OpenFrozenMargin;
  dest.close_frozen_margin = src->CloseFrozenMargin;
  dest.margin = src->Margin;
  dest.frozen_margin = src->FrozenMargin;
}

void fill_investor_prod_spbm_detail(
    ctp_investor_prod_spbm_detail &dest,
    const CThostFtdcInvestorProdSPBMDetailField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
  dest.intra_instr_margin = src->IntraInstrMargin;
  dest.b_collecting_margin = src->BCollectingMargin;
  dest.s_collecting_margin = src->SCollectingMargin;
  dest.intra_prod_margin = src->IntraProdMargin;
  dest.net_margin = src->NetMargin;
  dest.inter_prod_margin = src->InterProdMargin;
  dest.single_margin = src->SingleMargin;
  dest.add_on_margin = src->AddOnMargin;
  dest.delivery_margin = src->DeliveryMargin;
  dest.call_option_min_risk = src->CallOptionMinRisk;
  dest.put_option_min_risk = src->PutOptionMinRisk;
  dest.option_min_risk = src->OptionMinRisk;
  dest.option_value_offset = src->OptionValueOffset;
  dest.option_royalty = src->OptionRoyalty;
  dest.real_option_value_offset = src->RealOptionValueOffset;
  dest.margin = src->Margin;
  dest.exch_margin = src->ExchMargin;
}

void fill_investor_product_group_margin(
    ctp_investor_product_group_margin &dest,
    const CThostFtdcInvestorProductGroupMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  dest.frozen_margin = src->FrozenMargin;
  dest.long_frozen_margin = src->LongFrozenMargin;
  dest.short_frozen_margin = src->ShortFrozenMargin;
  dest.use_margin = src->UseMargin;
  dest.long_use_margin = src->LongUseMargin;
  dest.short_use_margin = src->ShortUseMargin;
  dest.exch_margin = src->ExchMargin;
  dest.long_exch_margin = src->LongExchMargin;
  dest.short_exch_margin = src->ShortExchMargin;
  dest.close_profit = src->CloseProfit;
  dest.frozen_commission = src->FrozenCommission;
  dest.commission = src->Commission;
  dest.frozen_cash = src->FrozenCash;
  dest.cash_in = src->CashIn;
  dest.position_profit = src->PositionProfit;
  dest.offset_amount = src->OffsetAmount;
  dest.long_offset_amount = src->LongOffsetAmount;
  dest.short_offset_amount = src->ShortOffsetAmount;
  dest.exch_offset_amount = src->ExchOffsetAmount;
  dest.long_exch_offset_amount = src->LongExchOffsetAmount;
  dest.short_exch_offset_amount = src->ShortExchOffsetAmount;
  dest.hedge_flag = src->HedgeFlag;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.product_group_id, src->ProductGroupID);
}

void fill_mm_instrument_commission_rate(
    ctp_mm_instrument_commission_rate &dest,
    const CThostFtdcMMInstrumentCommissionRateField *src) {
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
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_mm_option_instr_comm_rate(
    ctp_mm_option_instr_comm_rate &dest,
    const CThostFtdcMMOptionInstrCommRateField *src) {
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
  dest.strike_ratio_by_money = src->StrikeRatioByMoney;
  dest.strike_ratio_by_volume = src->StrikeRatioByVolume;
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_notice(ctp_notice &dest, const CThostFtdcNoticeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.content, src->Content);
  copy_field(dest.sequence_label, src->SequenceLabel);
}

void fill_notify_query_account(ctp_notify_query_account &dest,
                               const CThostFtdcNotifyQueryAccountField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trade_code, src->TradeCode);
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_branch_id, src->BankBranchID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.broker_branch_id, src->BrokerBranchID);
  copy_field(dest.trade_date, src->TradeDate);
  copy_field(dest.trade_time, src->TradeTime);
  copy_field(dest.bank_serial, src->BankSerial);
  copy_field(dest.trading_day, src->TradingDay);
  dest.plate_serial = src->PlateSerial;
  dest.last_fragment = src->LastFragment;
  dest.session_id = src->SessionID;
  copy_field(dest.customer_name, src->CustomerName);
  dest.id_card_type = src->IdCardType;
  copy_field(dest.identified_card_no, src->IdentifiedCardNo);
  dest.cust_type = src->CustType;
  copy_field(dest.bank_account, src->BankAccount);
  copy_field(dest.bank_pass_word, src->BankPassWord);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.password, src->Password);
  dest.future_serial = src->FutureSerial;
  dest.install_id = src->InstallID;
  copy_field(dest.user_id, src->UserID);
  dest.verify_cert_no_flag = src->VerifyCertNoFlag;
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.digest, src->Digest);
  dest.bank_acc_type = src->BankAccType;
  copy_field(dest.device_id, src->DeviceID);
  dest.bank_secu_acc_type = src->BankSecuAccType;
  copy_field(dest.broker_id_by_bank, src->BrokerIDByBank);
  copy_field(dest.bank_secu_acc, src->BankSecuAcc);
  dest.bank_pwd_flag = src->BankPwdFlag;
  dest.secu_pwd_flag = src->SecuPwdFlag;
  copy_field(dest.oper_no, src->OperNo);
  dest.request_id = src->RequestID;
  dest.t_id = src->TID;
  dest.bank_use_amount = src->BankUseAmount;
  dest.bank_fetch_amount = src->BankFetchAmount;
  dest.error_id = src->ErrorID;
  copy_field(dest.error_msg, src->ErrorMsg);
  copy_field(dest.long_customer_name, src->LongCustomerName);
}

void fill_offset_setting(ctp_offset_setting &dest,
                         const CThostFtdcOffsetSettingField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.underlying_instr_id, src->UnderlyingInstrID);
  copy_field(dest.product_id, src->ProductID);
  dest.offset_type = src->OffsetType;
  dest.volume = src->Volume;
  dest.is_offset = src->IsOffset;
  dest.request_id = src->RequestID;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.exchange_serial_no, src->ExchangeSerialNo);
  copy_field(dest.exchange_product_id, src->ExchangeProductID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  dest.order_submit_status = src->OrderSubmitStatus;
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  copy_field(dest.insert_date, src->InsertDate);
  copy_field(dest.insert_time, src->InsertTime);
  copy_field(dest.cancel_time, src->CancelTime);
  dest.exec_result = src->ExecResult;
  dest.sequence_no = src->SequenceNo;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.active_user_id, src->ActiveUserID);
  dest.broker_offset_setting_seq = src->BrokerOffsetSettingSeq;
  dest.apply_src = src->ApplySrc;
}

void fill_option_instr_comm_rate(
    ctp_option_instr_comm_rate &dest,
    const CThostFtdcOptionInstrCommRateField *src) {
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
  dest.strike_ratio_by_money = src->StrikeRatioByMoney;
  dest.strike_ratio_by_volume = src->StrikeRatioByVolume;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_option_instr_trade_cost(
    ctp_option_instr_trade_cost &dest,
    const CThostFtdcOptionInstrTradeCostField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  dest.hedge_flag = src->HedgeFlag;
  dest.fixed_margin = src->FixedMargin;
  dest.mini_margin = src->MiniMargin;
  dest.royalty = src->Royalty;
  dest.exch_fixed_margin = src->ExchFixedMargin;
  dest.exch_mini_margin = src->ExchMiniMargin;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_option_self_close_action(
    ctp_option_self_close_action &dest,
    const CThostFtdcOptionSelfCloseActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.option_self_close_action_ref = src->OptionSelfCloseActionRef;
  copy_field(dest.option_self_close_ref, src->OptionSelfCloseRef);
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.option_self_close_sys_id, src->OptionSelfCloseSysID);
  dest.action_flag = src->ActionFlag;
  copy_field(dest.action_date, src->ActionDate);
  copy_field(dest.action_time, src->ActionTime);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  copy_field(dest.option_self_close_local_id, src->OptionSelfCloseLocalID);
  copy_field(dest.action_local_id, src->ActionLocalID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.order_action_status = src->OrderActionStatus;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.branch_id, src->BranchID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_option_self_close(ctp_option_self_close &dest,
                            const CThostFtdcOptionSelfCloseField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.option_self_close_ref, src->OptionSelfCloseRef);
  copy_field(dest.user_id, src->UserID);
  dest.volume = src->Volume;
  dest.request_id = src->RequestID;
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.hedge_flag = src->HedgeFlag;
  dest.opt_self_close_flag = src->OptSelfCloseFlag;
  copy_field(dest.option_self_close_local_id, src->OptionSelfCloseLocalID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  dest.order_submit_status = src->OrderSubmitStatus;
  dest.notify_sequence = src->NotifySequence;
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  copy_field(dest.option_self_close_sys_id, src->OptionSelfCloseSysID);
  copy_field(dest.insert_date, src->InsertDate);
  copy_field(dest.insert_time, src->InsertTime);
  copy_field(dest.cancel_time, src->CancelTime);
  dest.exec_result = src->ExecResult;
  copy_field(dest.clearing_part_id, src->ClearingPartID);
  dest.sequence_no = src->SequenceNo;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.user_product_info, src->UserProductInfo);
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.active_user_id, src->ActiveUserID);
  dest.broker_option_self_close_seq = src->BrokerOptionSelfCloseSeq;
  copy_field(dest.branch_id, src->BranchID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.reserve3, src->reserve3);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_order_action(ctp_order_action &dest,
                       const CThostFtdcOrderActionField *src) {
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
  copy_field(dest.action_date, src->ActionDate);
  copy_field(dest.action_time, src->ActionTime);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  copy_field(dest.order_local_id, src->OrderLocalID);
  copy_field(dest.action_local_id, src->ActionLocalID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.order_action_status = src->OrderActionStatus;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.branch_id, src->BranchID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.order_memo, src->OrderMemo);
  dest.session_req_seq = src->SessionReqSeq;
}

void fill_parked_order_action(ctp_parked_order_action &dest,
                              const CThostFtdcParkedOrderActionField *src) {
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
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.parked_order_action_id, src->ParkedOrderActionID);
  dest.user_type = src->UserType;
  dest.status = src->Status;
  dest.error_id = src->ErrorID;
  copy_field(dest.error_msg, src->ErrorMsg);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_parked_order(ctp_parked_order &dest,
                       const CThostFtdcParkedOrderField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
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
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.parked_order_id, src->ParkedOrderID);
  dest.user_type = src->UserType;
  dest.status = src->Status;
  dest.error_id = src->ErrorID;
  copy_field(dest.error_msg, src->ErrorMsg);
  dest.is_swap_order = src->IsSwapOrder;
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_product_exch_rate(ctp_product_exch_rate &dest,
                            const CThostFtdcProductExchRateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.quote_currency_id, src->QuoteCurrencyID);
  dest.exchange_rate = src->ExchangeRate;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_id, src->ProductID);
}

void fill_product(ctp_product &dest, const CThostFtdcProductField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.product_name, src->ProductName);
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.product_class = src->ProductClass;
  dest.volume_multiple = src->VolumeMultiple;
  dest.price_tick = src->PriceTick;
  dest.max_market_order_volume = src->MaxMarketOrderVolume;
  dest.min_market_order_volume = src->MinMarketOrderVolume;
  dest.max_limit_order_volume = src->MaxLimitOrderVolume;
  dest.min_limit_order_volume = src->MinLimitOrderVolume;
  dest.position_type = src->PositionType;
  dest.position_date_type = src->PositionDateType;
  dest.close_deal_type = src->CloseDealType;
  copy_field(dest.trade_currency_id, src->TradeCurrencyID);
  dest.mortgage_fund_use_range = src->MortgageFundUseRange;
  copy_field(dest.reserve2, src->reserve2);
  dest.underlying_multiple = src->UnderlyingMultiple;
  copy_field(dest.product_id, src->ProductID);
  copy_field(dest.exchange_product_id, src->ExchangeProductID);
  dest.open_limit_control_level = src->OpenLimitControlLevel;
  dest.order_freq_control_level = src->OrderFreqControlLevel;
}

void fill_product_group(ctp_product_group &dest,
                        const CThostFtdcProductGroupField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.product_id, src->ProductID);
  copy_field(dest.product_group_id, src->ProductGroupID);
}

void fill_qry_accountregister(ctp_qry_accountregister &dest,
                              const CThostFtdcQryAccountregisterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_branch_id, src->BankBranchID);
  copy_field(dest.currency_id, src->CurrencyID);
}

void fill_qry_broker_trading_algos(
    ctp_qry_broker_trading_algos &dest,
    const CThostFtdcQryBrokerTradingAlgosField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_broker_trading_params(
    ctp_qry_broker_trading_params &dest,
    const CThostFtdcQryBrokerTradingParamsField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.account_id, src->AccountID);
}

void fill_qry_cfmmc_trading_account_key(
    ctp_qry_cfmmc_trading_account_key &dest,
    const CThostFtdcQryCFMMCTradingAccountKeyField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
}

void fill_qry_classified_instrument(
    ctp_qry_classified_instrument &dest,
    const CThostFtdcQryClassifiedInstrumentField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.product_id, src->ProductID);
  dest.trading_type = src->TradingType;
  dest.class_type = src->ClassType;
}

void fill_qry_comb_action(ctp_qry_comb_action &dest,
                          const CThostFtdcQryCombActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_comb_instrument_guard(
    ctp_qry_comb_instrument_guard &dest,
    const CThostFtdcQryCombInstrumentGuardField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_comb_leg(ctp_qry_comb_leg &dest,
                       const CThostFtdcQryCombLegField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.leg_instrument_id, src->LegInstrumentID);
}

void fill_qry_comb_promotion_param(
    ctp_qry_comb_promotion_param &dest,
    const CThostFtdcQryCombPromotionParamField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_contract_bank(ctp_qry_contract_bank &dest,
                            const CThostFtdcQryContractBankField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_brch_id, src->BankBrchID);
}

void fill_qry_depth_market_data(ctp_qry_depth_market_data &dest,
                                const CThostFtdcQryDepthMarketDataField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
  dest.product_class = src->ProductClass;
}

void fill_qry_e_warrant_offset(ctp_qry_e_warrant_offset &dest,
                               const CThostFtdcQryEWarrantOffsetField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_exchange(ctp_qry_exchange &dest,
                       const CThostFtdcQryExchangeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
}

void fill_qry_exchange_margin_rate_adjust(
    ctp_qry_exchange_margin_rate_adjust &dest,
    const CThostFtdcQryExchangeMarginRateAdjustField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.reserve1, src->reserve1);
  dest.hedge_flag = src->HedgeFlag;
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_exchange_rate(ctp_qry_exchange_rate &dest,
                            const CThostFtdcQryExchangeRateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.from_currency_id, src->FromCurrencyID);
  copy_field(dest.to_currency_id, src->ToCurrencyID);
}

void fill_qry_exec_order(ctp_qry_exec_order &dest,
                         const CThostFtdcQryExecOrderField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.exec_order_sys_id, src->ExecOrderSysID);
  copy_field(dest.insert_time_start, src->InsertTimeStart);
  copy_field(dest.insert_time_end, src->InsertTimeEnd);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_for_quote(ctp_qry_for_quote &dest,
                        const CThostFtdcQryForQuoteField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.insert_time_start, src->InsertTimeStart);
  copy_field(dest.insert_time_end, src->InsertTimeEnd);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_hedge_cfm(ctp_qry_hedge_cfm &dest,
                        const CThostFtdcQryHedgeCfmField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_sys_id, src->OrderSysID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_instrument(ctp_qry_instrument &dest,
                         const CThostFtdcQryInstrumentField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.reserve3, src->reserve3);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.product_id, src->ProductID);
}

void fill_qry_instrument_order_comm_rate(
    ctp_qry_instrument_order_comm_rate &dest,
    const CThostFtdcQryInstrumentOrderCommRateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_invest_unit(ctp_qry_invest_unit &dest,
                          const CThostFtdcQryInvestUnitField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
}

void fill_qry_investor_commodity_group_spmm_margin(
    ctp_qry_investor_commodity_group_spmm_margin &dest,
    const CThostFtdcQryInvestorCommodityGroupSPMMMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.commodity_group_id, src->CommodityGroupID);
}

void fill_qry_investor_commodity_spmm_margin(
    ctp_qry_investor_commodity_spmm_margin &dest,
    const CThostFtdcQryInvestorCommoditySPMMMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.commodity_id, src->CommodityID);
}

void fill_qry_investor(ctp_qry_investor &dest,
                       const CThostFtdcQryInvestorField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
}

void fill_qry_investor_info_comm_rec(
    ctp_qry_investor_info_comm_rec &dest,
    const CThostFtdcQryInvestorInfoCommRecField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.broker_id, src->BrokerID);
}

void fill_qry_investor_portf_margin_ratio(
    ctp_qry_investor_portf_margin_ratio &dest,
    const CThostFtdcQryInvestorPortfMarginRatioField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_group_id, src->ProductGroupID);
}

void fill_qry_investor_portf_setting(
    ctp_qry_investor_portf_setting &dest,
    const CThostFtdcQryInvestorPortfSettingField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
}

void fill_qry_investor_position_combine_detail(
    ctp_qry_investor_position_combine_detail &dest,
    const CThostFtdcQryInvestorPositionCombineDetailField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.comb_instrument_id, src->CombInstrumentID);
}

void fill_qry_investor_position_detail(
    ctp_qry_investor_position_detail &dest,
    const CThostFtdcQryInvestorPositionDetailField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_investor_prod_rcams_margin(
    ctp_qry_investor_prod_rcams_margin &dest,
    const CThostFtdcQryInvestorProdRCAMSMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.comb_product_id, src->CombProductID);
  copy_field(dest.product_group_id, src->ProductGroupID);
}

void fill_qry_investor_prod_rule_margin(
    ctp_qry_investor_prod_rule_margin &dest,
    const CThostFtdcQryInvestorProdRULEMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
  dest.commodity_group_id = src->CommodityGroupID;
}

void fill_qry_investor_prod_spbm_detail(
    ctp_qry_investor_prod_spbm_detail &dest,
    const CThostFtdcQryInvestorProdSPBMDetailField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
}

void fill_qry_investor_product_group_margin(
    ctp_qry_investor_product_group_margin &dest,
    const CThostFtdcQryInvestorProductGroupMarginField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  dest.hedge_flag = src->HedgeFlag;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.product_group_id, src->ProductGroupID);
}

void fill_qry_mm_instrument_commission_rate(
    ctp_qry_mm_instrument_commission_rate &dest,
    const CThostFtdcQryMMInstrumentCommissionRateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_mm_option_instr_comm_rate(
    ctp_qry_mm_option_instr_comm_rate &dest,
    const CThostFtdcQryMMOptionInstrCommRateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_max_order_volume(ctp_qry_max_order_volume &dest,
                               const CThostFtdcQryMaxOrderVolumeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  dest.direction = src->Direction;
  dest.offset_flag = src->OffsetFlag;
  dest.hedge_flag = src->HedgeFlag;
  dest.max_volume = src->MaxVolume;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_notice(ctp_qry_notice &dest,
                     const CThostFtdcQryNoticeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
}

void fill_qry_offset_setting(ctp_qry_offset_setting &dest,
                             const CThostFtdcQryOffsetSettingField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.product_id, src->ProductID);
  dest.offset_type = src->OffsetType;
}

void fill_qry_option_instr_comm_rate(
    ctp_qry_option_instr_comm_rate &dest,
    const CThostFtdcQryOptionInstrCommRateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_option_instr_trade_cost(
    ctp_qry_option_instr_trade_cost &dest,
    const CThostFtdcQryOptionInstrTradeCostField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  dest.hedge_flag = src->HedgeFlag;
  dest.input_price = src->InputPrice;
  dest.underlying_price = src->UnderlyingPrice;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_option_self_close(ctp_qry_option_self_close &dest,
                                const CThostFtdcQryOptionSelfCloseField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.option_self_close_sys_id, src->OptionSelfCloseSysID);
  copy_field(dest.insert_time_start, src->InsertTimeStart);
  copy_field(dest.insert_time_end, src->InsertTimeEnd);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_order(ctp_qry_order &dest, const CThostFtdcQryOrderField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_sys_id, src->OrderSysID);
  copy_field(dest.insert_time_start, src->InsertTimeStart);
  copy_field(dest.insert_time_end, src->InsertTimeEnd);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_parked_order_action(
    ctp_qry_parked_order_action &dest,
    const CThostFtdcQryParkedOrderActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_parked_order(ctp_qry_parked_order &dest,
                           const CThostFtdcQryParkedOrderField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_product_exch_rate(ctp_qry_product_exch_rate &dest,
                                const CThostFtdcQryProductExchRateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_id, src->ProductID);
}

void fill_qry_product(ctp_qry_product &dest,
                      const CThostFtdcQryProductField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  dest.product_class = src->ProductClass;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_id, src->ProductID);
}

void fill_qry_product_group(ctp_qry_product_group &dest,
                            const CThostFtdcQryProductGroupField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_id, src->ProductID);
}

void fill_qry_quote(ctp_qry_quote &dest, const CThostFtdcQryQuoteField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.quote_sys_id, src->QuoteSysID);
  copy_field(dest.insert_time_start, src->InsertTimeStart);
  copy_field(dest.insert_time_end, src->InsertTimeEnd);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_rcams_comb_product_info(
    ctp_qry_rcams_comb_product_info &dest,
    const CThostFtdcQryRCAMSCombProductInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.product_id, src->ProductID);
  copy_field(dest.comb_product_id, src->CombProductID);
  copy_field(dest.product_group_id, src->ProductGroupID);
}

void fill_qry_rcams_instr_parameter(
    ctp_qry_rcams_instr_parameter &dest,
    const CThostFtdcQryRCAMSInstrParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.product_id, src->ProductID);
}

void fill_qry_rcams_inter_parameter(
    ctp_qry_rcams_inter_parameter &dest,
    const CThostFtdcQryRCAMSInterParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.product_group_id, src->ProductGroupID);
  copy_field(dest.comb_product1, src->CombProduct1);
  copy_field(dest.comb_product2, src->CombProduct2);
}

void fill_qry_rcams_intra_parameter(
    ctp_qry_rcams_intra_parameter &dest,
    const CThostFtdcQryRCAMSIntraParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.comb_product_id, src->CombProductID);
}

void fill_qry_rcams_investor_comb_position(
    ctp_qry_rcams_investor_comb_position &dest,
    const CThostFtdcQryRCAMSInvestorCombPositionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.comb_instrument_id, src->CombInstrumentID);
}

void fill_qry_rcams_short_opt_adjust_param(
    ctp_qry_rcams_short_opt_adjust_param &dest,
    const CThostFtdcQryRCAMSShortOptAdjustParamField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.comb_product_id, src->CombProductID);
}

void fill_qry_rule_instr_parameter(
    ctp_qry_rule_instr_parameter &dest,
    const CThostFtdcQryRULEInstrParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_rule_inter_parameter(
    ctp_qry_rule_inter_parameter &dest,
    const CThostFtdcQryRULEInterParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.leg1_prod_family_code, src->Leg1ProdFamilyCode);
  copy_field(dest.leg2_prod_family_code, src->Leg2ProdFamilyCode);
  dest.commodity_group_id = src->CommodityGroupID;
}

void fill_qry_rule_intra_parameter(
    ctp_qry_rule_intra_parameter &dest,
    const CThostFtdcQryRULEIntraParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
}

void fill_qry_risk_settle_invst_position(
    ctp_qry_risk_settle_invst_position &dest,
    const CThostFtdcQryRiskSettleInvstPositionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_risk_settle_product_status(
    ctp_qry_risk_settle_product_status &dest,
    const CThostFtdcQryRiskSettleProductStatusField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.product_id, src->ProductID);
}

void fill_qry_spbm_add_on_inter_parameter(
    ctp_qry_spbm_add_on_inter_parameter &dest,
    const CThostFtdcQrySPBMAddOnInterParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.leg1_prod_family_code, src->Leg1ProdFamilyCode);
  copy_field(dest.leg2_prod_family_code, src->Leg2ProdFamilyCode);
}

void fill_qry_spbm_future_parameter(
    ctp_qry_spbm_future_parameter &dest,
    const CThostFtdcQrySPBMFutureParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
}

void fill_qry_spbm_inter_parameter(
    ctp_qry_spbm_inter_parameter &dest,
    const CThostFtdcQrySPBMInterParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.leg1_prod_family_code, src->Leg1ProdFamilyCode);
  copy_field(dest.leg2_prod_family_code, src->Leg2ProdFamilyCode);
}

void fill_qry_spbm_intra_parameter(
    ctp_qry_spbm_intra_parameter &dest,
    const CThostFtdcQrySPBMIntraParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
}

void fill_qry_spbm_investor_portf_def(
    ctp_qry_spbm_investor_portf_def &dest,
    const CThostFtdcQrySPBMInvestorPortfDefField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
}

void fill_qry_spbm_option_parameter(
    ctp_qry_spbm_option_parameter &dest,
    const CThostFtdcQrySPBMOptionParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
}

void fill_qry_spbm_portf_definition(
    ctp_qry_spbm_portf_definition &dest,
    const CThostFtdcQrySPBMPortfDefinitionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.portfolio_def_id = src->PortfolioDefID;
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
}

void fill_qry_spmm_inst_param(ctp_qry_spmm_inst_param &dest,
                              const CThostFtdcQrySPMMInstParamField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_spmm_product_param(
    ctp_qry_spmm_product_param &dest,
    const CThostFtdcQrySPMMProductParamField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.product_id, src->ProductID);
}

void fill_qry_sec_agent_ac_id_map(
    ctp_qry_sec_agent_ac_id_map &dest,
    const CThostFtdcQrySecAgentACIDMapField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
}

void fill_qry_sec_agent_check_mode(
    ctp_qry_sec_agent_check_mode &dest,
    const CThostFtdcQrySecAgentCheckModeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
}

void fill_qry_sec_agent_trade_info(
    ctp_qry_sec_agent_trade_info &dest,
    const CThostFtdcQrySecAgentTradeInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.broker_sec_agent_id, src->BrokerSecAgentID);
}

void fill_qry_settlement_info_confirm(
    ctp_qry_settlement_info_confirm &dest,
    const CThostFtdcQrySettlementInfoConfirmField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
}

void fill_qry_settlement_info(ctp_qry_settlement_info &dest,
                              const CThostFtdcQrySettlementInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
}

void fill_qry_spd_apply(ctp_qry_spd_apply &dest,
                        const CThostFtdcQrySpdApplyField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_sys_id, src->OrderSysID);
  copy_field(dest.first_leg_instrument_id, src->FirstLegInstrumentID);
  copy_field(dest.second_leg_instrument_id, src->SecondLegInstrumentID);
}

void fill_qry_trade(ctp_qry_trade &dest, const CThostFtdcQryTradeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.trade_id, src->TradeID);
  copy_field(dest.trade_time_start, src->TradeTimeStart);
  copy_field(dest.trade_time_end, src->TradeTimeEnd);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.instrument_id, src->InstrumentID);
}

void fill_qry_trader_offer(ctp_qry_trader_offer &dest,
                           const CThostFtdcQryTraderOfferField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.trader_id, src->TraderID);
}

void fill_qry_trading_code(ctp_qry_trading_code &dest,
                           const CThostFtdcQryTradingCodeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.client_id, src->ClientID);
  dest.client_id_type = src->ClientIDType;
  copy_field(dest.invest_unit_id, src->InvestUnitID);
}

void fill_qry_trading_notice(ctp_qry_trading_notice &dest,
                             const CThostFtdcQryTradingNoticeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
}

void fill_qry_transfer_bank(ctp_qry_transfer_bank &dest,
                            const CThostFtdcQryTransferBankField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_brch_id, src->BankBrchID);
}

void fill_qry_transfer_serial(ctp_qry_transfer_serial &dest,
                              const CThostFtdcQryTransferSerialField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.currency_id, src->CurrencyID);
}

void fill_qry_user_session(ctp_qry_user_session &dest,
                           const CThostFtdcQryUserSessionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
}

void fill_query_cfmmc_trading_account_token(
    ctp_query_cfmmc_trading_account_token &dest,
    const CThostFtdcQueryCFMMCTradingAccountTokenField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
}

void fill_quote_action(ctp_quote_action &dest,
                       const CThostFtdcQuoteActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.quote_action_ref = src->QuoteActionRef;
  copy_field(dest.quote_ref, src->QuoteRef);
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.quote_sys_id, src->QuoteSysID);
  dest.action_flag = src->ActionFlag;
  copy_field(dest.action_date, src->ActionDate);
  copy_field(dest.action_time, src->ActionTime);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  copy_field(dest.quote_local_id, src->QuoteLocalID);
  copy_field(dest.action_local_id, src->ActionLocalID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.order_action_status = src->OrderActionStatus;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.branch_id, src->BranchID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.order_memo, src->OrderMemo);
  dest.session_req_seq = src->SessionReqSeq;
}

void fill_quote(ctp_quote &dest, const CThostFtdcQuoteField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.quote_ref, src->QuoteRef);
  copy_field(dest.user_id, src->UserID);
  dest.ask_price = src->AskPrice;
  dest.bid_price = src->BidPrice;
  dest.ask_volume = src->AskVolume;
  dest.bid_volume = src->BidVolume;
  dest.request_id = src->RequestID;
  copy_field(dest.business_unit, src->BusinessUnit);
  dest.ask_offset_flag = src->AskOffsetFlag;
  dest.bid_offset_flag = src->BidOffsetFlag;
  dest.ask_hedge_flag = src->AskHedgeFlag;
  dest.bid_hedge_flag = src->BidHedgeFlag;
  copy_field(dest.quote_local_id, src->QuoteLocalID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.reserve2, src->reserve2);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  dest.notify_sequence = src->NotifySequence;
  dest.order_submit_status = src->OrderSubmitStatus;
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  copy_field(dest.quote_sys_id, src->QuoteSysID);
  copy_field(dest.insert_date, src->InsertDate);
  copy_field(dest.insert_time, src->InsertTime);
  copy_field(dest.cancel_time, src->CancelTime);
  dest.quote_status = src->QuoteStatus;
  copy_field(dest.clearing_part_id, src->ClearingPartID);
  dest.sequence_no = src->SequenceNo;
  copy_field(dest.ask_order_sys_id, src->AskOrderSysID);
  copy_field(dest.bid_order_sys_id, src->BidOrderSysID);
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.user_product_info, src->UserProductInfo);
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.active_user_id, src->ActiveUserID);
  dest.broker_quote_seq = src->BrokerQuoteSeq;
  copy_field(dest.ask_order_ref, src->AskOrderRef);
  copy_field(dest.bid_order_ref, src->BidOrderRef);
  copy_field(dest.for_quote_sys_id, src->ForQuoteSysID);
  copy_field(dest.branch_id, src->BranchID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.reserve3, src->reserve3);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.replace_sys_id, src->ReplaceSysID);
  dest.time_condition = src->TimeCondition;
  copy_field(dest.order_memo, src->OrderMemo);
  dest.session_req_seq = src->SessionReqSeq;
}

void fill_rcams_comb_product_info(
    ctp_rcams_comb_product_info &dest,
    const CThostFtdcRCAMSCombProductInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_id, src->ProductID);
  copy_field(dest.comb_product_id, src->CombProductID);
  copy_field(dest.product_group_id, src->ProductGroupID);
}

void fill_rcams_instr_parameter(ctp_rcams_instr_parameter &dest,
                                const CThostFtdcRCAMSInstrParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_id, src->ProductID);
  dest.hedge_rate = src->HedgeRate;
}

void fill_rcams_inter_parameter(ctp_rcams_inter_parameter &dest,
                                const CThostFtdcRCAMSInterParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_group_id, src->ProductGroupID);
  dest.priority = src->Priority;
  dest.credit_rate = src->CreditRate;
  copy_field(dest.comb_product1, src->CombProduct1);
  copy_field(dest.comb_product2, src->CombProduct2);
}

void fill_rcams_intra_parameter(ctp_rcams_intra_parameter &dest,
                                const CThostFtdcRCAMSIntraParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.comb_product_id, src->CombProductID);
  dest.hedge_rate = src->HedgeRate;
}

void fill_rcams_investor_comb_position(
    ctp_rcams_investor_comb_position &dest,
    const CThostFtdcRCAMSInvestorCombPositionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.instrument_id, src->InstrumentID);
  dest.hedge_flag = src->HedgeFlag;
  dest.posi_direction = src->PosiDirection;
  copy_field(dest.comb_instrument_id, src->CombInstrumentID);
  dest.leg_id = src->LegID;
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  dest.total_amt = src->TotalAmt;
  dest.exch_margin = src->ExchMargin;
  dest.margin = src->Margin;
}

void fill_rcams_short_opt_adjust_param(
    ctp_rcams_short_opt_adjust_param &dest,
    const CThostFtdcRCAMSShortOptAdjustParamField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.comb_product_id, src->CombProductID);
  dest.hedge_flag = src->HedgeFlag;
  dest.adjust_value = src->AdjustValue;
}

void fill_rule_instr_parameter(ctp_rule_instr_parameter &dest,
                               const CThostFtdcRULEInstrParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
  dest.instrument_class = src->InstrumentClass;
  copy_field(dest.std_instrument_id, src->StdInstrumentID);
  dest.b_spec_ratio = src->BSpecRatio;
  dest.s_spec_ratio = src->SSpecRatio;
  dest.b_hedge_ratio = src->BHedgeRatio;
  dest.s_hedge_ratio = src->SHedgeRatio;
  dest.b_add_on_margin = src->BAddOnMargin;
  dest.s_add_on_margin = src->SAddOnMargin;
  dest.commodity_group_id = src->CommodityGroupID;
}

void fill_rule_inter_parameter(ctp_rule_inter_parameter &dest,
                               const CThostFtdcRULEInterParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.spread_id = src->SpreadId;
  dest.inter_rate = src->InterRate;
  copy_field(dest.leg1_prod_family_code, src->Leg1ProdFamilyCode);
  copy_field(dest.leg2_prod_family_code, src->Leg2ProdFamilyCode);
  dest.leg1_prop_factor = src->Leg1PropFactor;
  dest.leg2_prop_factor = src->Leg2PropFactor;
  dest.commodity_group_id = src->CommodityGroupID;
  copy_field(dest.commodity_group_name, src->CommodityGroupName);
}

void fill_rule_intra_parameter(ctp_rule_intra_parameter &dest,
                               const CThostFtdcRULEIntraParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
  copy_field(dest.std_instrument_id, src->StdInstrumentID);
  dest.std_instr_margin = src->StdInstrMargin;
  dest.usual_intra_rate = src->UsualIntraRate;
  dest.delivery_intra_rate = src->DeliveryIntraRate;
}

void fill_remove_parked_order_action(
    ctp_remove_parked_order_action &dest,
    const CThostFtdcRemoveParkedOrderActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.parked_order_action_id, src->ParkedOrderActionID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
}

void fill_remove_parked_order(ctp_remove_parked_order &dest,
                              const CThostFtdcRemoveParkedOrderField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.parked_order_id, src->ParkedOrderID);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
}

void fill_req_gen_sms_code(ctp_req_gen_sms_code &dest,
                           const CThostFtdcReqGenSMSCodeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.mobile, src->Mobile);
}

void fill_req_gen_user_captcha(ctp_req_gen_user_captcha &dest,
                               const CThostFtdcReqGenUserCaptchaField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
}

void fill_req_gen_user_text(ctp_req_gen_user_text &dest,
                            const CThostFtdcReqGenUserTextField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
}

void fill_req_query_account(ctp_req_query_account &dest,
                            const CThostFtdcReqQueryAccountField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trade_code, src->TradeCode);
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_branch_id, src->BankBranchID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.broker_branch_id, src->BrokerBranchID);
  copy_field(dest.trade_date, src->TradeDate);
  copy_field(dest.trade_time, src->TradeTime);
  copy_field(dest.bank_serial, src->BankSerial);
  copy_field(dest.trading_day, src->TradingDay);
  dest.plate_serial = src->PlateSerial;
  dest.last_fragment = src->LastFragment;
  dest.session_id = src->SessionID;
  copy_field(dest.customer_name, src->CustomerName);
  dest.id_card_type = src->IdCardType;
  copy_field(dest.identified_card_no, src->IdentifiedCardNo);
  dest.cust_type = src->CustType;
  copy_field(dest.bank_account, src->BankAccount);
  copy_field(dest.bank_pass_word, src->BankPassWord);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.password, src->Password);
  dest.future_serial = src->FutureSerial;
  dest.install_id = src->InstallID;
  copy_field(dest.user_id, src->UserID);
  dest.verify_cert_no_flag = src->VerifyCertNoFlag;
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.digest, src->Digest);
  dest.bank_acc_type = src->BankAccType;
  copy_field(dest.device_id, src->DeviceID);
  dest.bank_secu_acc_type = src->BankSecuAccType;
  copy_field(dest.broker_id_by_bank, src->BrokerIDByBank);
  copy_field(dest.bank_secu_acc, src->BankSecuAcc);
  dest.bank_pwd_flag = src->BankPwdFlag;
  dest.secu_pwd_flag = src->SecuPwdFlag;
  copy_field(dest.oper_no, src->OperNo);
  dest.request_id = src->RequestID;
  dest.t_id = src->TID;
  copy_field(dest.long_customer_name, src->LongCustomerName);
}

void fill_req_transfer(ctp_req_transfer &dest,
                       const CThostFtdcReqTransferField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trade_code, src->TradeCode);
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_branch_id, src->BankBranchID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.broker_branch_id, src->BrokerBranchID);
  copy_field(dest.trade_date, src->TradeDate);
  copy_field(dest.trade_time, src->TradeTime);
  copy_field(dest.bank_serial, src->BankSerial);
  copy_field(dest.trading_day, src->TradingDay);
  dest.plate_serial = src->PlateSerial;
  dest.last_fragment = src->LastFragment;
  dest.session_id = src->SessionID;
  copy_field(dest.customer_name, src->CustomerName);
  dest.id_card_type = src->IdCardType;
  copy_field(dest.identified_card_no, src->IdentifiedCardNo);
  dest.cust_type = src->CustType;
  copy_field(dest.bank_account, src->BankAccount);
  copy_field(dest.bank_pass_word, src->BankPassWord);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.password, src->Password);
  dest.install_id = src->InstallID;
  dest.future_serial = src->FutureSerial;
  copy_field(dest.user_id, src->UserID);
  dest.verify_cert_no_flag = src->VerifyCertNoFlag;
  copy_field(dest.currency_id, src->CurrencyID);
  dest.trade_amount = src->TradeAmount;
  dest.future_fetch_amount = src->FutureFetchAmount;
  dest.fee_pay_flag = src->FeePayFlag;
  dest.cust_fee = src->CustFee;
  dest.broker_fee = src->BrokerFee;
  copy_field(dest.message, src->Message);
  copy_field(dest.digest, src->Digest);
  dest.bank_acc_type = src->BankAccType;
  copy_field(dest.device_id, src->DeviceID);
  dest.bank_secu_acc_type = src->BankSecuAccType;
  copy_field(dest.broker_id_by_bank, src->BrokerIDByBank);
  copy_field(dest.bank_secu_acc, src->BankSecuAcc);
  dest.bank_pwd_flag = src->BankPwdFlag;
  dest.secu_pwd_flag = src->SecuPwdFlag;
  copy_field(dest.oper_no, src->OperNo);
  dest.request_id = src->RequestID;
  dest.t_id = src->TID;
  dest.transfer_status = src->TransferStatus;
  copy_field(dest.long_customer_name, src->LongCustomerName);
}

void fill_req_user_auth_method(ctp_req_user_auth_method &dest,
                               const CThostFtdcReqUserAuthMethodField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
}

void fill_req_user_login_with_captcha(
    ctp_req_user_login_with_captcha &dest,
    const CThostFtdcReqUserLoginWithCaptchaField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.password, src->Password);
  copy_field(dest.user_product_info, src->UserProductInfo);
  copy_field(dest.interface_product_info, src->InterfaceProductInfo);
  copy_field(dest.protocol_info, src->ProtocolInfo);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.login_remark, src->LoginRemark);
  copy_field(dest.captcha, src->Captcha);
  dest.client_ip_port = src->ClientIPPort;
  copy_field(dest.client_ip_address, src->ClientIPAddress);
}

void fill_req_user_login_with_otp(
    ctp_req_user_login_with_otp &dest,
    const CThostFtdcReqUserLoginWithOTPField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.password, src->Password);
  copy_field(dest.user_product_info, src->UserProductInfo);
  copy_field(dest.interface_product_info, src->InterfaceProductInfo);
  copy_field(dest.protocol_info, src->ProtocolInfo);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.login_remark, src->LoginRemark);
  copy_field(dest.otp_password, src->OTPPassword);
  dest.client_ip_port = src->ClientIPPort;
  copy_field(dest.client_ip_address, src->ClientIPAddress);
}

void fill_req_user_login_with_text(
    ctp_req_user_login_with_text &dest,
    const CThostFtdcReqUserLoginWithTextField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.password, src->Password);
  copy_field(dest.user_product_info, src->UserProductInfo);
  copy_field(dest.interface_product_info, src->InterfaceProductInfo);
  copy_field(dest.protocol_info, src->ProtocolInfo);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.login_remark, src->LoginRemark);
  copy_field(dest.text, src->Text);
  dest.client_ip_port = src->ClientIPPort;
  copy_field(dest.client_ip_address, src->ClientIPAddress);
}

void fill_risk_settle_invst_position(
    ctp_risk_settle_invst_position &dest,
    const CThostFtdcRiskSettleInvstPositionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.posi_direction = src->PosiDirection;
  dest.hedge_flag = src->HedgeFlag;
  dest.position_date = src->PositionDate;
  dest.yd_position = src->YdPosition;
  dest.position = src->Position;
  dest.long_frozen = src->LongFrozen;
  dest.short_frozen = src->ShortFrozen;
  dest.long_frozen_amount = src->LongFrozenAmount;
  dest.short_frozen_amount = src->ShortFrozenAmount;
  dest.open_volume = src->OpenVolume;
  dest.close_volume = src->CloseVolume;
  dest.open_amount = src->OpenAmount;
  dest.close_amount = src->CloseAmount;
  dest.position_cost = src->PositionCost;
  dest.pre_margin = src->PreMargin;
  dest.use_margin = src->UseMargin;
  dest.frozen_margin = src->FrozenMargin;
  dest.frozen_cash = src->FrozenCash;
  dest.frozen_commission = src->FrozenCommission;
  dest.cash_in = src->CashIn;
  dest.commission = src->Commission;
  dest.close_profit = src->CloseProfit;
  dest.position_profit = src->PositionProfit;
  dest.pre_settlement_price = src->PreSettlementPrice;
  dest.settlement_price = src->SettlementPrice;
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  dest.open_cost = src->OpenCost;
  dest.exchange_margin = src->ExchangeMargin;
  dest.comb_position = src->CombPosition;
  dest.comb_long_frozen = src->CombLongFrozen;
  dest.comb_short_frozen = src->CombShortFrozen;
  dest.close_profit_by_date = src->CloseProfitByDate;
  dest.close_profit_by_trade = src->CloseProfitByTrade;
  dest.today_position = src->TodayPosition;
  dest.margin_rate_by_money = src->MarginRateByMoney;
  dest.margin_rate_by_volume = src->MarginRateByVolume;
  dest.strike_frozen = src->StrikeFrozen;
  dest.strike_frozen_amount = src->StrikeFrozenAmount;
  dest.abandon_frozen = src->AbandonFrozen;
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.yd_strike_frozen = src->YdStrikeFrozen;
  copy_field(dest.invest_unit_id, src->InvestUnitID);
  dest.position_cost_offset = src->PositionCostOffset;
  dest.tas_position = src->TasPosition;
  dest.tas_position_cost = src->TasPositionCost;
}

void fill_risk_settle_product_status(
    ctp_risk_settle_product_status &dest,
    const CThostFtdcRiskSettleProductStatusField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_id, src->ProductID);
  dest.product_status = src->ProductStatus;
}

void fill_rsp_gen_sms_code(ctp_rsp_gen_sms_code &dest,
                           const CThostFtdcRspGenSMSCodeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.gen_time, src->GenTime);
}

void fill_rsp_gen_user_captcha(ctp_rsp_gen_user_captcha &dest,
                               const CThostFtdcRspGenUserCaptchaField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  dest.captcha_info_len = src->CaptchaInfoLen;
  copy_field(dest.captcha_info, src->CaptchaInfo);
}

void fill_rsp_gen_user_text(ctp_rsp_gen_user_text &dest,
                            const CThostFtdcRspGenUserTextField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  dest.user_text_seq = src->UserTextSeq;
}

void fill_rsp_transfer(ctp_rsp_transfer &dest,
                       const CThostFtdcRspTransferField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trade_code, src->TradeCode);
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_branch_id, src->BankBranchID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.broker_branch_id, src->BrokerBranchID);
  copy_field(dest.trade_date, src->TradeDate);
  copy_field(dest.trade_time, src->TradeTime);
  copy_field(dest.bank_serial, src->BankSerial);
  copy_field(dest.trading_day, src->TradingDay);
  dest.plate_serial = src->PlateSerial;
  dest.last_fragment = src->LastFragment;
  dest.session_id = src->SessionID;
  copy_field(dest.customer_name, src->CustomerName);
  dest.id_card_type = src->IdCardType;
  copy_field(dest.identified_card_no, src->IdentifiedCardNo);
  dest.cust_type = src->CustType;
  copy_field(dest.bank_account, src->BankAccount);
  copy_field(dest.bank_pass_word, src->BankPassWord);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.password, src->Password);
  dest.install_id = src->InstallID;
  dest.future_serial = src->FutureSerial;
  copy_field(dest.user_id, src->UserID);
  dest.verify_cert_no_flag = src->VerifyCertNoFlag;
  copy_field(dest.currency_id, src->CurrencyID);
  dest.trade_amount = src->TradeAmount;
  dest.future_fetch_amount = src->FutureFetchAmount;
  dest.fee_pay_flag = src->FeePayFlag;
  dest.cust_fee = src->CustFee;
  dest.broker_fee = src->BrokerFee;
  copy_field(dest.message, src->Message);
  copy_field(dest.digest, src->Digest);
  dest.bank_acc_type = src->BankAccType;
  copy_field(dest.device_id, src->DeviceID);
  dest.bank_secu_acc_type = src->BankSecuAccType;
  copy_field(dest.broker_id_by_bank, src->BrokerIDByBank);
  copy_field(dest.bank_secu_acc, src->BankSecuAcc);
  dest.bank_pwd_flag = src->BankPwdFlag;
  dest.secu_pwd_flag = src->SecuPwdFlag;
  copy_field(dest.oper_no, src->OperNo);
  dest.request_id = src->RequestID;
  dest.t_id = src->TID;
  dest.transfer_status = src->TransferStatus;
  dest.error_id = src->ErrorID;
  copy_field(dest.error_msg, src->ErrorMsg);
  copy_field(dest.long_customer_name, src->LongCustomerName);
}

void fill_rsp_user_auth_method(ctp_rsp_user_auth_method &dest,
                               const CThostFtdcRspUserAuthMethodField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  dest.usable_auth_method = src->UsableAuthMethod;
}

void fill_spbm_add_on_inter_parameter(
    ctp_spbm_add_on_inter_parameter &dest,
    const CThostFtdcSPBMAddOnInterParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.spread_id = src->SpreadId;
  dest.add_on_inter_rate_z2 = src->AddOnInterRateZ2;
  copy_field(dest.leg1_prod_family_code, src->Leg1ProdFamilyCode);
  copy_field(dest.leg2_prod_family_code, src->Leg2ProdFamilyCode);
}

void fill_spbm_future_parameter(ctp_spbm_future_parameter &dest,
                                const CThostFtdcSPBMFutureParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
  dest.cvf = src->Cvf;
  dest.time_range = src->TimeRange;
  dest.margin_rate = src->MarginRate;
  dest.lock_rate_x = src->LockRateX;
  dest.add_on_rate = src->AddOnRate;
  dest.pre_settlement_price = src->PreSettlementPrice;
  dest.add_on_lock_rate_x2 = src->AddOnLockRateX2;
}

void fill_spbm_inter_parameter(ctp_spbm_inter_parameter &dest,
                               const CThostFtdcSPBMInterParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.spread_id = src->SpreadId;
  dest.inter_rate_z = src->InterRateZ;
  copy_field(dest.leg1_prod_family_code, src->Leg1ProdFamilyCode);
  copy_field(dest.leg2_prod_family_code, src->Leg2ProdFamilyCode);
}

void fill_spbm_intra_parameter(ctp_spbm_intra_parameter &dest,
                               const CThostFtdcSPBMIntraParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
  dest.intra_rate_y = src->IntraRateY;
  dest.add_on_intra_rate_y2 = src->AddOnIntraRateY2;
}

void fill_spbm_investor_portf_def(
    ctp_spbm_investor_portf_def &dest,
    const CThostFtdcSPBMInvestorPortfDefField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.portfolio_def_id = src->PortfolioDefID;
}

void fill_spbm_option_parameter(ctp_spbm_option_parameter &dest,
                                const CThostFtdcSPBMOptionParameterField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
  dest.cvf = src->Cvf;
  dest.down_price = src->DownPrice;
  dest.delta = src->Delta;
  dest.slimi_delta = src->SlimiDelta;
  dest.pre_settlement_price = src->PreSettlementPrice;
}

void fill_spbm_portf_definition(ctp_spbm_portf_definition &dest,
                                const CThostFtdcSPBMPortfDefinitionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  dest.portfolio_def_id = src->PortfolioDefID;
  copy_field(dest.prod_family_code, src->ProdFamilyCode);
  dest.is_spbm = src->IsSPBM;
}

void fill_spmm_inst_param(ctp_spmm_inst_param &dest,
                          const CThostFtdcSPMMInstParamField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.instrument_id, src->InstrumentID);
  dest.inst_margin_cal_id = src->InstMarginCalID;
  copy_field(dest.commodity_id, src->CommodityID);
  copy_field(dest.commodity_group_id, src->CommodityGroupID);
}

void fill_spmm_product_param(ctp_spmm_product_param &dest,
                             const CThostFtdcSPMMProductParamField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.product_id, src->ProductID);
  copy_field(dest.commodity_id, src->CommodityID);
  copy_field(dest.commodity_group_id, src->CommodityGroupID);
}

void fill_sec_agent_ac_id_map(ctp_sec_agent_ac_id_map &dest,
                              const CThostFtdcSecAgentACIDMapField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.broker_sec_agent_id, src->BrokerSecAgentID);
}

void fill_sec_agent_check_mode(ctp_sec_agent_check_mode &dest,
                               const CThostFtdcSecAgentCheckModeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.currency_id, src->CurrencyID);
  copy_field(dest.broker_sec_agent_id, src->BrokerSecAgentID);
  dest.check_self_account = src->CheckSelfAccount;
}

void fill_sec_agent_trade_info(ctp_sec_agent_trade_info &dest,
                               const CThostFtdcSecAgentTradeInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.broker_sec_agent_id, src->BrokerSecAgentID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.long_customer_name, src->LongCustomerName);
}

void fill_settlement_info(ctp_settlement_info &dest,
                          const CThostFtdcSettlementInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.sequence_no = src->SequenceNo;
  copy_field(dest.content, src->Content);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.currency_id, src->CurrencyID);
}

void fill_spd_apply_action(ctp_spd_apply_action &dest,
                           const CThostFtdcSpdApplyActionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.action_date, src->ActionDate);
  copy_field(dest.action_time, src->ActionTime);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  copy_field(dest.order_local_id, src->OrderLocalID);
  copy_field(dest.action_local_id, src->ActionLocalID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  dest.order_action_status = src->OrderActionStatus;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.order_sys_id, src->OrderSysID);
  dest.request_id = src->RequestID;
  copy_field(dest.status_msg, src->StatusMsg);
  copy_field(dest.order_ref, src->OrderRef);
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
}

void fill_spd_apply(ctp_spd_apply &dest, const CThostFtdcSpdApplyField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.first_leg_instrument_id, src->FirstLegInstrumentID);
  copy_field(dest.second_leg_instrument_id, src->SecondLegInstrumentID);
  copy_field(dest.user_id, src->UserID);
  dest.volume = src->Volume;
  dest.direction = src->Direction;
  dest.request_id = src->RequestID;
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.order_ref, src->OrderRef);
  copy_field(dest.active_user_id, src->ActiveUserID);
  dest.broker_order_seq = src->BrokerOrderSeq;
  copy_field(dest.order_sys_id, src->OrderSysID);
  dest.apply_status = src->ApplyStatus;
  dest.sequence_no = src->SequenceNo;
  copy_field(dest.insert_date, src->InsertDate);
  copy_field(dest.insert_time, src->InsertTime);
  copy_field(dest.cancel_time, src->CancelTime);
  copy_field(dest.order_local_id, src->OrderLocalID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.client_id, src->ClientID);
  copy_field(dest.exchange_inst_id, src->ExchangeInstID);
  copy_field(dest.trader_id, src->TraderID);
  dest.install_id = src->InstallID;
  dest.order_submit_status = src->OrderSubmitStatus;
  dest.notify_sequence = src->NotifySequence;
  copy_field(dest.trading_day, src->TradingDay);
  dest.settlement_id = src->SettlementID;
  copy_field(dest.ip_address, src->IPAddress);
  copy_field(dest.mac_address, src->MacAddress);
  dest.cmb_type = src->CmbType;
  copy_field(dest.status_msg, src->StatusMsg);
}

void fill_trader_offer(ctp_trader_offer &dest,
                       const CThostFtdcTraderOfferField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.trader_id, src->TraderID);
  copy_field(dest.participant_id, src->ParticipantID);
  copy_field(dest.password, src->Password);
  dest.install_id = src->InstallID;
  copy_field(dest.order_local_id, src->OrderLocalID);
  dest.trader_connect_status = src->TraderConnectStatus;
  copy_field(dest.connect_request_date, src->ConnectRequestDate);
  copy_field(dest.connect_request_time, src->ConnectRequestTime);
  copy_field(dest.last_report_date, src->LastReportDate);
  copy_field(dest.last_report_time, src->LastReportTime);
  copy_field(dest.connect_date, src->ConnectDate);
  copy_field(dest.connect_time, src->ConnectTime);
  copy_field(dest.start_date, src->StartDate);
  copy_field(dest.start_time, src->StartTime);
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.max_trade_id, src->MaxTradeID);
  copy_field(dest.max_order_message_reference, src->MaxOrderMessageReference);
  dest.order_cancel_alg = src->OrderCancelAlg;
}

void fill_trading_account_password_update(
    ctp_trading_account_password_update &dest,
    const CThostFtdcTradingAccountPasswordUpdateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.old_password, src->OldPassword);
  copy_field(dest.new_password, src->NewPassword);
  copy_field(dest.currency_id, src->CurrencyID);
}

void fill_trading_code(ctp_trading_code &dest,
                       const CThostFtdcTradingCodeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.investor_id, src->InvestorID);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.exchange_id, src->ExchangeID);
  copy_field(dest.client_id, src->ClientID);
  dest.is_active = src->IsActive;
  dest.client_id_type = src->ClientIDType;
  copy_field(dest.branch_id, src->BranchID);
  dest.biz_type = src->BizType;
  copy_field(dest.invest_unit_id, src->InvestUnitID);
}

void fill_trading_notice(ctp_trading_notice &dest,
                         const CThostFtdcTradingNoticeField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  dest.investor_range = src->InvestorRange;
  copy_field(dest.investor_id, src->InvestorID);
  dest.sequence_series = src->SequenceSeries;
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.send_time, src->SendTime);
  dest.sequence_no = src->SequenceNo;
  copy_field(dest.field_content, src->FieldContent);
  copy_field(dest.invest_unit_id, src->InvestUnitID);
}

void fill_transfer_bank(ctp_transfer_bank &dest,
                        const CThostFtdcTransferBankField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_brch_id, src->BankBrchID);
  copy_field(dest.bank_name, src->BankName);
  dest.is_active = src->IsActive;
}

void fill_transfer_serial(ctp_transfer_serial &dest,
                          const CThostFtdcTransferSerialField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  dest.plate_serial = src->PlateSerial;
  copy_field(dest.trade_date, src->TradeDate);
  copy_field(dest.trading_day, src->TradingDay);
  copy_field(dest.trade_time, src->TradeTime);
  copy_field(dest.trade_code, src->TradeCode);
  dest.session_id = src->SessionID;
  copy_field(dest.bank_id, src->BankID);
  copy_field(dest.bank_branch_id, src->BankBranchID);
  dest.bank_acc_type = src->BankAccType;
  copy_field(dest.bank_account, src->BankAccount);
  copy_field(dest.bank_serial, src->BankSerial);
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.broker_branch_id, src->BrokerBranchID);
  dest.future_acc_type = src->FutureAccType;
  copy_field(dest.account_id, src->AccountID);
  copy_field(dest.investor_id, src->InvestorID);
  dest.future_serial = src->FutureSerial;
  dest.id_card_type = src->IdCardType;
  copy_field(dest.identified_card_no, src->IdentifiedCardNo);
  copy_field(dest.currency_id, src->CurrencyID);
  dest.trade_amount = src->TradeAmount;
  dest.cust_fee = src->CustFee;
  dest.broker_fee = src->BrokerFee;
  dest.availability_flag = src->AvailabilityFlag;
  copy_field(dest.operator_code, src->OperatorCode);
  copy_field(dest.bank_new_account, src->BankNewAccount);
  dest.error_id = src->ErrorID;
  copy_field(dest.error_msg, src->ErrorMsg);
}

void fill_user_password_update(ctp_user_password_update &dest,
                               const CThostFtdcUserPasswordUpdateField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.old_password, src->OldPassword);
  copy_field(dest.new_password, src->NewPassword);
}

void fill_user_session(ctp_user_session &dest,
                       const CThostFtdcUserSessionField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  dest.front_id = src->FrontID;
  dest.session_id = src->SessionID;
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  copy_field(dest.login_date, src->LoginDate);
  copy_field(dest.login_time, src->LoginTime);
  copy_field(dest.reserve1, src->reserve1);
  copy_field(dest.user_product_info, src->UserProductInfo);
  copy_field(dest.interface_product_info, src->InterfaceProductInfo);
  copy_field(dest.protocol_info, src->ProtocolInfo);
  copy_field(dest.mac_address, src->MacAddress);
  copy_field(dest.login_remark, src->LoginRemark);
  copy_field(dest.ip_address, src->IPAddress);
}

void fill_user_system_info(ctp_user_system_info &dest,
                           const CThostFtdcUserSystemInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  dest.client_system_info_len = src->ClientSystemInfoLen;
  copy_field(dest.client_system_info, src->ClientSystemInfo);
  copy_field(dest.reserve1, src->reserve1);
  dest.client_ip_port = src->ClientIPPort;
  copy_field(dest.client_login_time, src->ClientLoginTime);
  copy_field(dest.client_app_id, src->ClientAppID);
  copy_field(dest.client_public_ip, src->ClientPublicIP);
  copy_field(dest.client_login_remark, src->ClientLoginRemark);
  copy_field(dest.mac, src->MAC);
}

void fill_wechat_user_system_info(
    ctp_wechat_user_system_info &dest,
    const CThostFtdcWechatUserSystemInfoField *src) {
  std::memset(&dest, 0, sizeof(dest));
  if (src == nullptr) {
    return;
  }
  copy_field(dest.broker_id, src->BrokerID);
  copy_field(dest.user_id, src->UserID);
  dest.wechat_clt_sys_info_len = src->WechatCltSysInfoLen;
  copy_field(dest.wechat_clt_sys_info, src->WechatCltSysInfo);
  dest.client_ip_port = src->ClientIPPort;
  copy_field(dest.client_login_time, src->ClientLoginTime);
  copy_field(dest.client_app_id, src->ClientAppID);
  copy_field(dest.client_public_ip, src->ClientPublicIP);
  copy_field(dest.client_login_remark, src->ClientLoginRemark);
}

class TraderSpiAdapter final : public CThostFtdcTraderSpi {
public:
  TraderSpiAdapter(const ctp_trader_spi &callbacks, void *user_data)
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

  void OnRspAuthenticate(CThostFtdcRspAuthenticateField *auth,
                         CThostFtdcRspInfoField *rsp_info, int request_id,
                         bool is_last) override {
    if (callbacks_.on_rsp_authenticate == nullptr) {
      return;
    }
    ctp_rsp_authenticate auth_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rsp_authenticate(auth_bridge, auth);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_authenticate(auth != nullptr ? &auth_bridge : nullptr,
                                   rsp_info != nullptr ? &rsp_bridge : nullptr,
                                   request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspSettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *confirm,
                                  CThostFtdcRspInfoField *rsp_info,
                                  int request_id, bool is_last) override {
    if (callbacks_.on_rsp_settlement_info_confirm == nullptr) {
      return;
    }
    ctp_settlement_info_confirm confirm_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_settlement_info_confirm(confirm_bridge, confirm);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_settlement_info_confirm(
        confirm != nullptr ? &confirm_bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspUserLogin(CThostFtdcRspUserLoginField *login,
                      CThostFtdcRspInfoField *rsp_info, int request_id,
                      bool is_last) override {
    if (callbacks_.on_rsp_user_login == nullptr) {
      return;
    }
    ctp_rsp_user_login login_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rsp_user_login(login_bridge, login);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_user_login(login != nullptr ? &login_bridge : nullptr,
                                 rsp_info != nullptr ? &rsp_bridge : nullptr,
                                 request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspUserLogout(CThostFtdcUserLogoutField *logout,
                       CThostFtdcRspInfoField *rsp_info, int request_id,
                       bool is_last) override {
    if (callbacks_.on_rsp_user_logout == nullptr) {
      return;
    }
    ctp_user_logout logout_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_user_logout(logout_bridge, logout);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_user_logout(logout != nullptr ? &logout_bridge : nullptr,
                                  rsp_info != nullptr ? &rsp_bridge : nullptr,
                                  request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspError(CThostFtdcRspInfoField *rsp_info, int request_id,
                  bool is_last) override {
    if (callbacks_.on_rsp_error == nullptr) {
      return;
    }
    ctp_rsp_info rsp_bridge{};
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_error(rsp_info != nullptr ? &rsp_bridge : nullptr,
                            request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryTradingAccount(CThostFtdcTradingAccountField *account,
                              CThostFtdcRspInfoField *rsp_info, int request_id,
                              bool is_last) override {
    if (callbacks_.on_rsp_qry_trading_account == nullptr) {
      return;
    }
    ctp_trading_account account_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_trading_account(account_bridge, account);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_trading_account(
        account != nullptr ? &account_bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorPosition(CThostFtdcInvestorPositionField *position,
                                CThostFtdcRspInfoField *rsp_info,
                                int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_position == nullptr) {
      return;
    }
    ctp_investor_position position_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_position(position_bridge, position);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_position(
        position != nullptr ? &position_bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void
  OnRspQryInstrumentMarginRate(CThostFtdcInstrumentMarginRateField *margin_rate,
                               CThostFtdcRspInfoField *rsp_info, int request_id,
                               bool is_last) override {
    if (callbacks_.on_rsp_qry_instrument_margin_rate == nullptr) {
      return;
    }
    ctp_instrument_margin_rate margin_rate_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_instrument_margin_rate(margin_rate_bridge, margin_rate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_instrument_margin_rate(
        margin_rate != nullptr ? &margin_rate_bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void
  OnRspQryExchangeMarginRate(CThostFtdcExchangeMarginRateField *margin_rate,
                             CThostFtdcRspInfoField *rsp_info, int request_id,
                             bool is_last) override {
    if (callbacks_.on_rsp_qry_exchange_margin_rate == nullptr) {
      return;
    }
    ctp_exchange_margin_rate margin_rate_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_exchange_margin_rate(margin_rate_bridge, margin_rate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_exchange_margin_rate(
        margin_rate != nullptr ? &margin_rate_bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInstrumentCommissionRate(
      CThostFtdcInstrumentCommissionRateField *commission_rate,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_instrument_commission_rate == nullptr) {
      return;
    }
    ctp_instrument_commission_rate commission_rate_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_instrument_commission_rate(commission_rate_bridge, commission_rate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_instrument_commission_rate(
        commission_rate != nullptr ? &commission_rate_bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspOrderInsert(CThostFtdcInputOrderField *input_order,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_order_insert == nullptr) {
      return;
    }
    ctp_input_order order_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_order(order_bridge, input_order);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_order_insert(input_order != nullptr ? &order_bridge
                                                          : nullptr,
                                   rsp_info != nullptr ? &rsp_bridge : nullptr,
                                   request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspOrderAction(CThostFtdcInputOrderActionField *action,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_order_action == nullptr) {
      return;
    }
    ctp_input_order_action action_bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_order_action(action_bridge, action);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_order_action(action != nullptr ? &action_bridge : nullptr,
                                   rsp_info != nullptr ? &rsp_bridge : nullptr,
                                   request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRtnOrder(CThostFtdcOrderField *order) override {
    if (callbacks_.on_rtn_order == nullptr || order == nullptr) {
      return;
    }
    ctp_order bridge{};
    fill_order(bridge, order);
    callbacks_.on_rtn_order(&bridge, user_data_);
  }

  void OnRtnTrade(CThostFtdcTradeField *trade) override {
    if (callbacks_.on_rtn_trade == nullptr || trade == nullptr) {
      return;
    }
    ctp_trade bridge{};
    fill_trade(bridge, trade);
    callbacks_.on_rtn_trade(&bridge, user_data_);
  }

  void OnErrRtnBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer,
                                    CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_bank_to_future_by_future == nullptr) {
      return;
    }
    ctp_req_transfer bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_req_transfer(bridge, pReqTransfer);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_bank_to_future_by_future(
        pReqTransfer != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void
  OnErrRtnBatchOrderAction(CThostFtdcBatchOrderActionField *pBatchOrderAction,
                           CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_batch_order_action == nullptr) {
      return;
    }
    ctp_batch_order_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_batch_order_action(bridge, pBatchOrderAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_batch_order_action(
        pBatchOrderAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnCancelOffsetSetting(
      CThostFtdcCancelOffsetSettingField *pCancelOffsetSetting,
      CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_cancel_offset_setting == nullptr) {
      return;
    }
    ctp_cancel_offset_setting bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_cancel_offset_setting(bridge, pCancelOffsetSetting);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_cancel_offset_setting(
        pCancelOffsetSetting != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void
  OnErrRtnCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction,
                           CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_comb_action_insert == nullptr) {
      return;
    }
    ctp_input_comb_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_comb_action(bridge, pInputCombAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_comb_action_insert(
        pInputCombAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnExecOrderAction(CThostFtdcExecOrderActionField *pExecOrderAction,
                               CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_exec_order_action == nullptr) {
      return;
    }
    ctp_exec_order_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_exec_order_action(bridge, pExecOrderAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_exec_order_action(
        pExecOrderAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder,
                               CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_exec_order_insert == nullptr) {
      return;
    }
    ctp_input_exec_order bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_exec_order(bridge, pInputExecOrder);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_exec_order_insert(
        pInputExecOrder != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote,
                              CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_for_quote_insert == nullptr) {
      return;
    }
    ctp_input_for_quote bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_for_quote(bridge, pInputForQuote);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_for_quote_insert(
        pInputForQuote != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer,
                                    CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_future_to_bank_by_future == nullptr) {
      return;
    }
    ctp_req_transfer bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_req_transfer(bridge, pReqTransfer);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_future_to_bank_by_future(
        pReqTransfer != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnHedgeCfm(CThostFtdcInputHedgeCfmField *pInputHedgeCfm,
                        CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_hedge_cfm == nullptr) {
      return;
    }
    ctp_input_hedge_cfm bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_hedge_cfm(bridge, pInputHedgeCfm);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_hedge_cfm(
        pInputHedgeCfm != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnHedgeCfmAction(CThostFtdcHedgeCfmActionField *pHedgeCfmAction,
                              CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_hedge_cfm_action == nullptr) {
      return;
    }
    ctp_hedge_cfm_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_hedge_cfm_action(bridge, pHedgeCfmAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_hedge_cfm_action(
        pHedgeCfmAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void
  OnErrRtnOffsetSetting(CThostFtdcInputOffsetSettingField *pInputOffsetSetting,
                        CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_offset_setting == nullptr) {
      return;
    }
    ctp_input_offset_setting bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_offset_setting(bridge, pInputOffsetSetting);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_offset_setting(
        pInputOffsetSetting != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnOptionSelfCloseAction(
      CThostFtdcOptionSelfCloseActionField *pOptionSelfCloseAction,
      CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_option_self_close_action == nullptr) {
      return;
    }
    ctp_option_self_close_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_option_self_close_action(bridge, pOptionSelfCloseAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_option_self_close_action(
        pOptionSelfCloseAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnOptionSelfCloseInsert(
      CThostFtdcInputOptionSelfCloseField *pInputOptionSelfClose,
      CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_option_self_close_insert == nullptr) {
      return;
    }
    ctp_input_option_self_close bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_option_self_close(bridge, pInputOptionSelfClose);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_option_self_close_insert(
        pInputOptionSelfClose != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnOrderAction(CThostFtdcOrderActionField *pOrderAction,
                           CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_order_action == nullptr) {
      return;
    }
    ctp_order_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_order_action(bridge, pOrderAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_order_action(
        pOrderAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnOrderInsert(CThostFtdcInputOrderField *pInputOrder,
                           CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_order_insert == nullptr) {
      return;
    }
    ctp_input_order bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_order(bridge, pInputOrder);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_order_insert(
        pInputOrder != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnQueryBankBalanceByFuture(
      CThostFtdcReqQueryAccountField *pReqQueryAccount,
      CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_query_bank_balance_by_future == nullptr) {
      return;
    }
    ctp_req_query_account bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_req_query_account(bridge, pReqQueryAccount);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_query_bank_balance_by_future(
        pReqQueryAccount != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnQuoteAction(CThostFtdcQuoteActionField *pQuoteAction,
                           CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_quote_action == nullptr) {
      return;
    }
    ctp_quote_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_quote_action(bridge, pQuoteAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_quote_action(
        pQuoteAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnQuoteInsert(CThostFtdcInputQuoteField *pInputQuote,
                           CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_quote_insert == nullptr) {
      return;
    }
    ctp_input_quote bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_quote(bridge, pInputQuote);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_quote_insert(
        pInputQuote != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnSpdApply(CThostFtdcInputSpdApplyField *pInputSpdApply,
                        CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_spd_apply == nullptr) {
      return;
    }
    ctp_input_spd_apply bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_spd_apply(bridge, pInputSpdApply);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_spd_apply(
        pInputSpdApply != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnErrRtnSpdApplyAction(CThostFtdcSpdApplyActionField *pSpdApplyAction,
                              CThostFtdcRspInfoField *rsp_info) override {
    if (callbacks_.on_err_rtn_spd_apply_action == nullptr) {
      return;
    }
    ctp_spd_apply_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spd_apply_action(bridge, pSpdApplyAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_err_rtn_spd_apply_action(
        pSpdApplyAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, user_data_);
  }

  void OnRspBatchOrderAction(
      CThostFtdcInputBatchOrderActionField *pInputBatchOrderAction,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_batch_order_action == nullptr) {
      return;
    }
    ctp_input_batch_order_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_batch_order_action(bridge, pInputBatchOrderAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_batch_order_action(
        pInputBatchOrderAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspCancelOffsetSetting(
      CThostFtdcInputOffsetSettingField *pInputOffsetSetting,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_cancel_offset_setting == nullptr) {
      return;
    }
    ctp_input_offset_setting bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_offset_setting(bridge, pInputOffsetSetting);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_cancel_offset_setting(
        pInputOffsetSetting != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction,
                             CThostFtdcRspInfoField *rsp_info, int request_id,
                             bool is_last) override {
    if (callbacks_.on_rsp_comb_action_insert == nullptr) {
      return;
    }
    ctp_input_comb_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_comb_action(bridge, pInputCombAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_comb_action_insert(
        pInputCombAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspExecOrderAction(
      CThostFtdcInputExecOrderActionField *pInputExecOrderAction,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_exec_order_action == nullptr) {
      return;
    }
    ctp_input_exec_order_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_exec_order_action(bridge, pInputExecOrderAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_exec_order_action(
        pInputExecOrderAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder,
                            CThostFtdcRspInfoField *rsp_info, int request_id,
                            bool is_last) override {
    if (callbacks_.on_rsp_exec_order_insert == nullptr) {
      return;
    }
    ctp_input_exec_order bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_exec_order(bridge, pInputExecOrder);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_exec_order_insert(
        pInputExecOrder != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote,
                           CThostFtdcRspInfoField *rsp_info, int request_id,
                           bool is_last) override {
    if (callbacks_.on_rsp_for_quote_insert == nullptr) {
      return;
    }
    ctp_input_for_quote bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_for_quote(bridge, pInputForQuote);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_for_quote_insert(
        pInputForQuote != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspFromBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer,
                                     CThostFtdcRspInfoField *rsp_info,
                                     int request_id, bool is_last) override {
    if (callbacks_.on_rsp_from_bank_to_future_by_future == nullptr) {
      return;
    }
    ctp_req_transfer bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_req_transfer(bridge, pReqTransfer);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_from_bank_to_future_by_future(
        pReqTransfer != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspFromFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer,
                                     CThostFtdcRspInfoField *rsp_info,
                                     int request_id, bool is_last) override {
    if (callbacks_.on_rsp_from_future_to_bank_by_future == nullptr) {
      return;
    }
    ctp_req_transfer bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_req_transfer(bridge, pReqTransfer);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_from_future_to_bank_by_future(
        pReqTransfer != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspGenSMSCode(CThostFtdcRspGenSMSCodeField *pRspGenSMSCode,
                       CThostFtdcRspInfoField *rsp_info, int request_id,
                       bool is_last) override {
    if (callbacks_.on_rsp_gen_sms_code == nullptr) {
      return;
    }
    ctp_rsp_gen_sms_code bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rsp_gen_sms_code(bridge, pRspGenSMSCode);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_gen_sms_code(pRspGenSMSCode != nullptr ? &bridge
                                                             : nullptr,
                                   rsp_info != nullptr ? &rsp_bridge : nullptr,
                                   request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspGenUserCaptcha(CThostFtdcRspGenUserCaptchaField *pRspGenUserCaptcha,
                           CThostFtdcRspInfoField *rsp_info, int request_id,
                           bool is_last) override {
    if (callbacks_.on_rsp_gen_user_captcha == nullptr) {
      return;
    }
    ctp_rsp_gen_user_captcha bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rsp_gen_user_captcha(bridge, pRspGenUserCaptcha);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_gen_user_captcha(
        pRspGenUserCaptcha != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspGenUserText(CThostFtdcRspGenUserTextField *pRspGenUserText,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_gen_user_text == nullptr) {
      return;
    }
    ctp_rsp_gen_user_text bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rsp_gen_user_text(bridge, pRspGenUserText);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_gen_user_text(pRspGenUserText != nullptr ? &bridge
                                                               : nullptr,
                                    rsp_info != nullptr ? &rsp_bridge : nullptr,
                                    request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspHedgeCfm(CThostFtdcInputHedgeCfmField *pInputHedgeCfm,
                     CThostFtdcRspInfoField *rsp_info, int request_id,
                     bool is_last) override {
    if (callbacks_.on_rsp_hedge_cfm == nullptr) {
      return;
    }
    ctp_input_hedge_cfm bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_hedge_cfm(bridge, pInputHedgeCfm);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_hedge_cfm(pInputHedgeCfm != nullptr ? &bridge : nullptr,
                                rsp_info != nullptr ? &rsp_bridge : nullptr,
                                request_id, is_last ? 1 : 0, user_data_);
  }

  void
  OnRspHedgeCfmAction(CThostFtdcInputHedgeCfmActionField *pInputHedgeCfmAction,
                      CThostFtdcRspInfoField *rsp_info, int request_id,
                      bool is_last) override {
    if (callbacks_.on_rsp_hedge_cfm_action == nullptr) {
      return;
    }
    ctp_input_hedge_cfm_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_hedge_cfm_action(bridge, pInputHedgeCfmAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_hedge_cfm_action(
        pInputHedgeCfmAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void
  OnRspOffsetSetting(CThostFtdcInputOffsetSettingField *pInputOffsetSetting,
                     CThostFtdcRspInfoField *rsp_info, int request_id,
                     bool is_last) override {
    if (callbacks_.on_rsp_offset_setting == nullptr) {
      return;
    }
    ctp_input_offset_setting bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_offset_setting(bridge, pInputOffsetSetting);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_offset_setting(
        pInputOffsetSetting != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspOptionSelfCloseAction(
      CThostFtdcInputOptionSelfCloseActionField *pInputOptionSelfCloseAction,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_option_self_close_action == nullptr) {
      return;
    }
    ctp_input_option_self_close_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_option_self_close_action(bridge, pInputOptionSelfCloseAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_option_self_close_action(
        pInputOptionSelfCloseAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspOptionSelfCloseInsert(
      CThostFtdcInputOptionSelfCloseField *pInputOptionSelfClose,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_option_self_close_insert == nullptr) {
      return;
    }
    ctp_input_option_self_close bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_option_self_close(bridge, pInputOptionSelfClose);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_option_self_close_insert(
        pInputOptionSelfClose != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void
  OnRspParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction,
                         CThostFtdcRspInfoField *rsp_info, int request_id,
                         bool is_last) override {
    if (callbacks_.on_rsp_parked_order_action == nullptr) {
      return;
    }
    ctp_parked_order_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_parked_order_action(bridge, pParkedOrderAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_parked_order_action(
        pParkedOrderAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspParkedOrderInsert(CThostFtdcParkedOrderField *pParkedOrder,
                              CThostFtdcRspInfoField *rsp_info, int request_id,
                              bool is_last) override {
    if (callbacks_.on_rsp_parked_order_insert == nullptr) {
      return;
    }
    ctp_parked_order bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_parked_order(bridge, pParkedOrder);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_parked_order_insert(
        pParkedOrder != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryAccountregister(CThostFtdcAccountregisterField *pAccountregister,
                               CThostFtdcRspInfoField *rsp_info, int request_id,
                               bool is_last) override {
    if (callbacks_.on_rsp_qry_accountregister == nullptr) {
      return;
    }
    ctp_accountregister bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_accountregister(bridge, pAccountregister);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_accountregister(
        pAccountregister != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryBrokerTradingAlgos(
      CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_broker_trading_algos == nullptr) {
      return;
    }
    ctp_broker_trading_algos bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_broker_trading_algos(bridge, pBrokerTradingAlgos);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_broker_trading_algos(
        pBrokerTradingAlgos != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryBrokerTradingParams(
      CThostFtdcBrokerTradingParamsField *pBrokerTradingParams,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_broker_trading_params == nullptr) {
      return;
    }
    ctp_broker_trading_params bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_broker_trading_params(bridge, pBrokerTradingParams);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_broker_trading_params(
        pBrokerTradingParams != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryCFMMCTradingAccountKey(
      CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_cfmmc_trading_account_key == nullptr) {
      return;
    }
    ctp_cfmmc_trading_account_key bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_cfmmc_trading_account_key(bridge, pCFMMCTradingAccountKey);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_cfmmc_trading_account_key(
        pCFMMCTradingAccountKey != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryClassifiedInstrument(CThostFtdcInstrumentField *pInstrument,
                                    CThostFtdcRspInfoField *rsp_info,
                                    int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_classified_instrument == nullptr) {
      return;
    }
    ctp_instrument bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_instrument(bridge, pInstrument);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_classified_instrument(
        pInstrument != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryCombAction(CThostFtdcCombActionField *pCombAction,
                          CThostFtdcRspInfoField *rsp_info, int request_id,
                          bool is_last) override {
    if (callbacks_.on_rsp_qry_comb_action == nullptr) {
      return;
    }
    ctp_comb_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_comb_action(bridge, pCombAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_comb_action(
        pCombAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryCombInstrumentGuard(
      CThostFtdcCombInstrumentGuardField *pCombInstrumentGuard,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_comb_instrument_guard == nullptr) {
      return;
    }
    ctp_comb_instrument_guard bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_comb_instrument_guard(bridge, pCombInstrumentGuard);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_comb_instrument_guard(
        pCombInstrumentGuard != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryCombLeg(CThostFtdcCombLegField *pCombLeg,
                       CThostFtdcRspInfoField *rsp_info, int request_id,
                       bool is_last) override {
    if (callbacks_.on_rsp_qry_comb_leg == nullptr) {
      return;
    }
    ctp_comb_leg bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_comb_leg(bridge, pCombLeg);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_comb_leg(pCombLeg != nullptr ? &bridge : nullptr,
                                   rsp_info != nullptr ? &rsp_bridge : nullptr,
                                   request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryCombPromotionParam(
      CThostFtdcCombPromotionParamField *pCombPromotionParam,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_comb_promotion_param == nullptr) {
      return;
    }
    ctp_comb_promotion_param bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_comb_promotion_param(bridge, pCombPromotionParam);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_comb_promotion_param(
        pCombPromotionParam != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryContractBank(CThostFtdcContractBankField *pContractBank,
                            CThostFtdcRspInfoField *rsp_info, int request_id,
                            bool is_last) override {
    if (callbacks_.on_rsp_qry_contract_bank == nullptr) {
      return;
    }
    ctp_contract_bank bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_contract_bank(bridge, pContractBank);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_contract_bank(
        pContractBank != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData,
                               CThostFtdcRspInfoField *rsp_info, int request_id,
                               bool is_last) override {
    if (callbacks_.on_rsp_qry_depth_market_data == nullptr) {
      return;
    }
    ctp_depth_market_data bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_depth_market_data(bridge, pDepthMarketData);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_depth_market_data(
        pDepthMarketData != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryEWarrantOffset(CThostFtdcEWarrantOffsetField *pEWarrantOffset,
                              CThostFtdcRspInfoField *rsp_info, int request_id,
                              bool is_last) override {
    if (callbacks_.on_rsp_qry_e_warrant_offset == nullptr) {
      return;
    }
    ctp_e_warrant_offset bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_e_warrant_offset(bridge, pEWarrantOffset);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_e_warrant_offset(
        pEWarrantOffset != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryExchange(CThostFtdcExchangeField *pExchange,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_qry_exchange == nullptr) {
      return;
    }
    ctp_exchange bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_exchange(bridge, pExchange);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_exchange(pExchange != nullptr ? &bridge : nullptr,
                                   rsp_info != nullptr ? &rsp_bridge : nullptr,
                                   request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryExchangeMarginRateAdjust(
      CThostFtdcExchangeMarginRateAdjustField *pExchangeMarginRateAdjust,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_exchange_margin_rate_adjust == nullptr) {
      return;
    }
    ctp_exchange_margin_rate_adjust bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_exchange_margin_rate_adjust(bridge, pExchangeMarginRateAdjust);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_exchange_margin_rate_adjust(
        pExchangeMarginRateAdjust != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryExchangeRate(CThostFtdcExchangeRateField *pExchangeRate,
                            CThostFtdcRspInfoField *rsp_info, int request_id,
                            bool is_last) override {
    if (callbacks_.on_rsp_qry_exchange_rate == nullptr) {
      return;
    }
    ctp_exchange_rate bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_exchange_rate(bridge, pExchangeRate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_exchange_rate(
        pExchangeRate != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryExecOrder(CThostFtdcExecOrderField *pExecOrder,
                         CThostFtdcRspInfoField *rsp_info, int request_id,
                         bool is_last) override {
    if (callbacks_.on_rsp_qry_exec_order == nullptr) {
      return;
    }
    ctp_exec_order bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_exec_order(bridge, pExecOrder);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_exec_order(pExecOrder != nullptr ? &bridge : nullptr,
                                     rsp_info != nullptr ? &rsp_bridge
                                                         : nullptr,
                                     request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryForQuote(CThostFtdcForQuoteField *pForQuote,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_qry_for_quote == nullptr) {
      return;
    }
    ctp_for_quote bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_for_quote(bridge, pForQuote);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_for_quote(pForQuote != nullptr ? &bridge : nullptr,
                                    rsp_info != nullptr ? &rsp_bridge : nullptr,
                                    request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryHedgeCfm(CThostFtdcHedgeCfmField *pHedgeCfm,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_qry_hedge_cfm == nullptr) {
      return;
    }
    ctp_hedge_cfm bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_hedge_cfm(bridge, pHedgeCfm);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_hedge_cfm(pHedgeCfm != nullptr ? &bridge : nullptr,
                                    rsp_info != nullptr ? &rsp_bridge : nullptr,
                                    request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInstrument(CThostFtdcInstrumentField *pInstrument,
                          CThostFtdcRspInfoField *rsp_info, int request_id,
                          bool is_last) override {
    if (callbacks_.on_rsp_qry_instrument == nullptr) {
      return;
    }
    ctp_instrument bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_instrument(bridge, pInstrument);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_instrument(pInstrument != nullptr ? &bridge : nullptr,
                                     rsp_info != nullptr ? &rsp_bridge
                                                         : nullptr,
                                     request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInstrumentOrderCommRate(
      CThostFtdcInstrumentOrderCommRateField *pInstrumentOrderCommRate,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_instrument_order_comm_rate == nullptr) {
      return;
    }
    ctp_instrument_order_comm_rate bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_instrument_order_comm_rate(bridge, pInstrumentOrderCommRate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_instrument_order_comm_rate(
        pInstrumentOrderCommRate != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestUnit(CThostFtdcInvestUnitField *pInvestUnit,
                          CThostFtdcRspInfoField *rsp_info, int request_id,
                          bool is_last) override {
    if (callbacks_.on_rsp_qry_invest_unit == nullptr) {
      return;
    }
    ctp_invest_unit bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_invest_unit(bridge, pInvestUnit);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_invest_unit(
        pInvestUnit != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestor(CThostFtdcInvestorField *pInvestor,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_qry_investor == nullptr) {
      return;
    }
    ctp_investor bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor(bridge, pInvestor);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor(pInvestor != nullptr ? &bridge : nullptr,
                                   rsp_info != nullptr ? &rsp_bridge : nullptr,
                                   request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorCommodityGroupSPMMMargin(
      CThostFtdcInvestorCommodityGroupSPMMMarginField
          *pInvestorCommodityGroupSPMMMargin,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_commodity_group_spmm_margin == nullptr) {
      return;
    }
    ctp_investor_commodity_group_spmm_margin bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_commodity_group_spmm_margin(
        bridge, pInvestorCommodityGroupSPMMMargin);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_commodity_group_spmm_margin(
        pInvestorCommodityGroupSPMMMargin != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorCommoditySPMMMargin(
      CThostFtdcInvestorCommoditySPMMMarginField *pInvestorCommoditySPMMMargin,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_commodity_spmm_margin == nullptr) {
      return;
    }
    ctp_investor_commodity_spmm_margin bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_commodity_spmm_margin(bridge, pInvestorCommoditySPMMMargin);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_commodity_spmm_margin(
        pInvestorCommoditySPMMMargin != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorInfoCommRec(
      CThostFtdcInvestorInfoCommRecField *pInvestorInfoCommRec,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_info_comm_rec == nullptr) {
      return;
    }
    ctp_investor_info_comm_rec bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_info_comm_rec(bridge, pInvestorInfoCommRec);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_info_comm_rec(
        pInvestorInfoCommRec != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorPortfMarginRatio(
      CThostFtdcInvestorPortfMarginRatioField *pInvestorPortfMarginRatio,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_portf_margin_ratio == nullptr) {
      return;
    }
    ctp_investor_portf_margin_ratio bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_portf_margin_ratio(bridge, pInvestorPortfMarginRatio);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_portf_margin_ratio(
        pInvestorPortfMarginRatio != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorPortfSetting(
      CThostFtdcInvestorPortfSettingField *pInvestorPortfSetting,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_portf_setting == nullptr) {
      return;
    }
    ctp_investor_portf_setting bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_portf_setting(bridge, pInvestorPortfSetting);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_portf_setting(
        pInvestorPortfSetting != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorPositionCombineDetail(
      CThostFtdcInvestorPositionCombineDetailField
          *pInvestorPositionCombineDetail,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_position_combine_detail == nullptr) {
      return;
    }
    ctp_investor_position_combine_detail bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_position_combine_detail(bridge,
                                          pInvestorPositionCombineDetail);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_position_combine_detail(
        pInvestorPositionCombineDetail != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorPositionDetail(
      CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_position_detail == nullptr) {
      return;
    }
    ctp_investor_position_detail bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_position_detail(bridge, pInvestorPositionDetail);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_position_detail(
        pInvestorPositionDetail != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorProdRCAMSMargin(
      CThostFtdcInvestorProdRCAMSMarginField *pInvestorProdRCAMSMargin,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_prod_rcams_margin == nullptr) {
      return;
    }
    ctp_investor_prod_rcams_margin bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_prod_rcams_margin(bridge, pInvestorProdRCAMSMargin);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_prod_rcams_margin(
        pInvestorProdRCAMSMargin != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorProdRULEMargin(
      CThostFtdcInvestorProdRULEMarginField *pInvestorProdRULEMargin,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_prod_rule_margin == nullptr) {
      return;
    }
    ctp_investor_prod_rule_margin bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_prod_rule_margin(bridge, pInvestorProdRULEMargin);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_prod_rule_margin(
        pInvestorProdRULEMargin != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorProdSPBMDetail(
      CThostFtdcInvestorProdSPBMDetailField *pInvestorProdSPBMDetail,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_prod_spbm_detail == nullptr) {
      return;
    }
    ctp_investor_prod_spbm_detail bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_prod_spbm_detail(bridge, pInvestorProdSPBMDetail);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_prod_spbm_detail(
        pInvestorProdSPBMDetail != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryInvestorProductGroupMargin(
      CThostFtdcInvestorProductGroupMarginField *pInvestorProductGroupMargin,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_investor_product_group_margin == nullptr) {
      return;
    }
    ctp_investor_product_group_margin bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_investor_product_group_margin(bridge, pInvestorProductGroupMargin);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_investor_product_group_margin(
        pInvestorProductGroupMargin != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryMMInstrumentCommissionRate(
      CThostFtdcMMInstrumentCommissionRateField *pMMInstrumentCommissionRate,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_mm_instrument_commission_rate == nullptr) {
      return;
    }
    ctp_mm_instrument_commission_rate bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_mm_instrument_commission_rate(bridge, pMMInstrumentCommissionRate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_mm_instrument_commission_rate(
        pMMInstrumentCommissionRate != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryMMOptionInstrCommRate(
      CThostFtdcMMOptionInstrCommRateField *pMMOptionInstrCommRate,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_mm_option_instr_comm_rate == nullptr) {
      return;
    }
    ctp_mm_option_instr_comm_rate bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_mm_option_instr_comm_rate(bridge, pMMOptionInstrCommRate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_mm_option_instr_comm_rate(
        pMMOptionInstrCommRate != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void
  OnRspQryMaxOrderVolume(CThostFtdcQryMaxOrderVolumeField *pQryMaxOrderVolume,
                         CThostFtdcRspInfoField *rsp_info, int request_id,
                         bool is_last) override {
    if (callbacks_.on_rsp_qry_max_order_volume == nullptr) {
      return;
    }
    ctp_qry_max_order_volume bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_qry_max_order_volume(bridge, pQryMaxOrderVolume);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_max_order_volume(
        pQryMaxOrderVolume != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryNotice(CThostFtdcNoticeField *pNotice,
                      CThostFtdcRspInfoField *rsp_info, int request_id,
                      bool is_last) override {
    if (callbacks_.on_rsp_qry_notice == nullptr) {
      return;
    }
    ctp_notice bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_notice(bridge, pNotice);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_notice(pNotice != nullptr ? &bridge : nullptr,
                                 rsp_info != nullptr ? &rsp_bridge : nullptr,
                                 request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryOffsetSetting(CThostFtdcOffsetSettingField *pOffsetSetting,
                             CThostFtdcRspInfoField *rsp_info, int request_id,
                             bool is_last) override {
    if (callbacks_.on_rsp_qry_offset_setting == nullptr) {
      return;
    }
    ctp_offset_setting bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_offset_setting(bridge, pOffsetSetting);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_offset_setting(
        pOffsetSetting != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryOptionInstrCommRate(
      CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_option_instr_comm_rate == nullptr) {
      return;
    }
    ctp_option_instr_comm_rate bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_option_instr_comm_rate(bridge, pOptionInstrCommRate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_option_instr_comm_rate(
        pOptionInstrCommRate != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryOptionInstrTradeCost(
      CThostFtdcOptionInstrTradeCostField *pOptionInstrTradeCost,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_option_instr_trade_cost == nullptr) {
      return;
    }
    ctp_option_instr_trade_cost bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_option_instr_trade_cost(bridge, pOptionInstrTradeCost);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_option_instr_trade_cost(
        pOptionInstrTradeCost != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryOptionSelfClose(CThostFtdcOptionSelfCloseField *pOptionSelfClose,
                               CThostFtdcRspInfoField *rsp_info, int request_id,
                               bool is_last) override {
    if (callbacks_.on_rsp_qry_option_self_close == nullptr) {
      return;
    }
    ctp_option_self_close bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_option_self_close(bridge, pOptionSelfClose);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_option_self_close(
        pOptionSelfClose != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryOrder(CThostFtdcOrderField *pOrder,
                     CThostFtdcRspInfoField *rsp_info, int request_id,
                     bool is_last) override {
    if (callbacks_.on_rsp_qry_order == nullptr) {
      return;
    }
    ctp_order bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_order(bridge, pOrder);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_order(pOrder != nullptr ? &bridge : nullptr,
                                rsp_info != nullptr ? &rsp_bridge : nullptr,
                                request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryParkedOrder(CThostFtdcParkedOrderField *pParkedOrder,
                           CThostFtdcRspInfoField *rsp_info, int request_id,
                           bool is_last) override {
    if (callbacks_.on_rsp_qry_parked_order == nullptr) {
      return;
    }
    ctp_parked_order bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_parked_order(bridge, pParkedOrder);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_parked_order(
        pParkedOrder != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryParkedOrderAction(
      CThostFtdcParkedOrderActionField *pParkedOrderAction,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_parked_order_action == nullptr) {
      return;
    }
    ctp_parked_order_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_parked_order_action(bridge, pParkedOrderAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_parked_order_action(
        pParkedOrderAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryProduct(CThostFtdcProductField *pProduct,
                       CThostFtdcRspInfoField *rsp_info, int request_id,
                       bool is_last) override {
    if (callbacks_.on_rsp_qry_product == nullptr) {
      return;
    }
    ctp_product bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_product(bridge, pProduct);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_product(pProduct != nullptr ? &bridge : nullptr,
                                  rsp_info != nullptr ? &rsp_bridge : nullptr,
                                  request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryProductExchRate(CThostFtdcProductExchRateField *pProductExchRate,
                               CThostFtdcRspInfoField *rsp_info, int request_id,
                               bool is_last) override {
    if (callbacks_.on_rsp_qry_product_exch_rate == nullptr) {
      return;
    }
    ctp_product_exch_rate bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_product_exch_rate(bridge, pProductExchRate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_product_exch_rate(
        pProductExchRate != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryProductGroup(CThostFtdcProductGroupField *pProductGroup,
                            CThostFtdcRspInfoField *rsp_info, int request_id,
                            bool is_last) override {
    if (callbacks_.on_rsp_qry_product_group == nullptr) {
      return;
    }
    ctp_product_group bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_product_group(bridge, pProductGroup);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_product_group(
        pProductGroup != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryQuote(CThostFtdcQuoteField *pQuote,
                     CThostFtdcRspInfoField *rsp_info, int request_id,
                     bool is_last) override {
    if (callbacks_.on_rsp_qry_quote == nullptr) {
      return;
    }
    ctp_quote bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_quote(bridge, pQuote);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_quote(pQuote != nullptr ? &bridge : nullptr,
                                rsp_info != nullptr ? &rsp_bridge : nullptr,
                                request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRCAMSCombProductInfo(
      CThostFtdcRCAMSCombProductInfoField *pRCAMSCombProductInfo,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_rcams_comb_product_info == nullptr) {
      return;
    }
    ctp_rcams_comb_product_info bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rcams_comb_product_info(bridge, pRCAMSCombProductInfo);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_rcams_comb_product_info(
        pRCAMSCombProductInfo != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRCAMSInstrParameter(
      CThostFtdcRCAMSInstrParameterField *pRCAMSInstrParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_rcams_instr_parameter == nullptr) {
      return;
    }
    ctp_rcams_instr_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rcams_instr_parameter(bridge, pRCAMSInstrParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_rcams_instr_parameter(
        pRCAMSInstrParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRCAMSInterParameter(
      CThostFtdcRCAMSInterParameterField *pRCAMSInterParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_rcams_inter_parameter == nullptr) {
      return;
    }
    ctp_rcams_inter_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rcams_inter_parameter(bridge, pRCAMSInterParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_rcams_inter_parameter(
        pRCAMSInterParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRCAMSIntraParameter(
      CThostFtdcRCAMSIntraParameterField *pRCAMSIntraParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_rcams_intra_parameter == nullptr) {
      return;
    }
    ctp_rcams_intra_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rcams_intra_parameter(bridge, pRCAMSIntraParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_rcams_intra_parameter(
        pRCAMSIntraParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRCAMSInvestorCombPosition(
      CThostFtdcRCAMSInvestorCombPositionField *pRCAMSInvestorCombPosition,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_rcams_investor_comb_position == nullptr) {
      return;
    }
    ctp_rcams_investor_comb_position bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rcams_investor_comb_position(bridge, pRCAMSInvestorCombPosition);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_rcams_investor_comb_position(
        pRCAMSInvestorCombPosition != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRCAMSShortOptAdjustParam(
      CThostFtdcRCAMSShortOptAdjustParamField *pRCAMSShortOptAdjustParam,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_rcams_short_opt_adjust_param == nullptr) {
      return;
    }
    ctp_rcams_short_opt_adjust_param bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rcams_short_opt_adjust_param(bridge, pRCAMSShortOptAdjustParam);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_rcams_short_opt_adjust_param(
        pRCAMSShortOptAdjustParam != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRULEInstrParameter(
      CThostFtdcRULEInstrParameterField *pRULEInstrParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_rule_instr_parameter == nullptr) {
      return;
    }
    ctp_rule_instr_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rule_instr_parameter(bridge, pRULEInstrParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_rule_instr_parameter(
        pRULEInstrParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRULEInterParameter(
      CThostFtdcRULEInterParameterField *pRULEInterParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_rule_inter_parameter == nullptr) {
      return;
    }
    ctp_rule_inter_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rule_inter_parameter(bridge, pRULEInterParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_rule_inter_parameter(
        pRULEInterParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRULEIntraParameter(
      CThostFtdcRULEIntraParameterField *pRULEIntraParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_rule_intra_parameter == nullptr) {
      return;
    }
    ctp_rule_intra_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rule_intra_parameter(bridge, pRULEIntraParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_rule_intra_parameter(
        pRULEIntraParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRiskSettleInvstPosition(
      CThostFtdcRiskSettleInvstPositionField *pRiskSettleInvstPosition,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_risk_settle_invst_position == nullptr) {
      return;
    }
    ctp_risk_settle_invst_position bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_risk_settle_invst_position(bridge, pRiskSettleInvstPosition);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_risk_settle_invst_position(
        pRiskSettleInvstPosition != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryRiskSettleProductStatus(
      CThostFtdcRiskSettleProductStatusField *pRiskSettleProductStatus,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_risk_settle_product_status == nullptr) {
      return;
    }
    ctp_risk_settle_product_status bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_risk_settle_product_status(bridge, pRiskSettleProductStatus);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_risk_settle_product_status(
        pRiskSettleProductStatus != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySPBMAddOnInterParameter(
      CThostFtdcSPBMAddOnInterParameterField *pSPBMAddOnInterParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_spbm_add_on_inter_parameter == nullptr) {
      return;
    }
    ctp_spbm_add_on_inter_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spbm_add_on_inter_parameter(bridge, pSPBMAddOnInterParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spbm_add_on_inter_parameter(
        pSPBMAddOnInterParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySPBMFutureParameter(
      CThostFtdcSPBMFutureParameterField *pSPBMFutureParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_spbm_future_parameter == nullptr) {
      return;
    }
    ctp_spbm_future_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spbm_future_parameter(bridge, pSPBMFutureParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spbm_future_parameter(
        pSPBMFutureParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySPBMInterParameter(
      CThostFtdcSPBMInterParameterField *pSPBMInterParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_spbm_inter_parameter == nullptr) {
      return;
    }
    ctp_spbm_inter_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spbm_inter_parameter(bridge, pSPBMInterParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spbm_inter_parameter(
        pSPBMInterParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySPBMIntraParameter(
      CThostFtdcSPBMIntraParameterField *pSPBMIntraParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_spbm_intra_parameter == nullptr) {
      return;
    }
    ctp_spbm_intra_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spbm_intra_parameter(bridge, pSPBMIntraParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spbm_intra_parameter(
        pSPBMIntraParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySPBMInvestorPortfDef(
      CThostFtdcSPBMInvestorPortfDefField *pSPBMInvestorPortfDef,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_spbm_investor_portf_def == nullptr) {
      return;
    }
    ctp_spbm_investor_portf_def bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spbm_investor_portf_def(bridge, pSPBMInvestorPortfDef);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spbm_investor_portf_def(
        pSPBMInvestorPortfDef != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySPBMOptionParameter(
      CThostFtdcSPBMOptionParameterField *pSPBMOptionParameter,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_spbm_option_parameter == nullptr) {
      return;
    }
    ctp_spbm_option_parameter bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spbm_option_parameter(bridge, pSPBMOptionParameter);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spbm_option_parameter(
        pSPBMOptionParameter != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySPBMPortfDefinition(
      CThostFtdcSPBMPortfDefinitionField *pSPBMPortfDefinition,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_spbm_portf_definition == nullptr) {
      return;
    }
    ctp_spbm_portf_definition bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spbm_portf_definition(bridge, pSPBMPortfDefinition);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spbm_portf_definition(
        pSPBMPortfDefinition != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySPMMInstParam(CThostFtdcSPMMInstParamField *pSPMMInstParam,
                             CThostFtdcRspInfoField *rsp_info, int request_id,
                             bool is_last) override {
    if (callbacks_.on_rsp_qry_spmm_inst_param == nullptr) {
      return;
    }
    ctp_spmm_inst_param bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spmm_inst_param(bridge, pSPMMInstParam);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spmm_inst_param(
        pSPMMInstParam != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void
  OnRspQrySPMMProductParam(CThostFtdcSPMMProductParamField *pSPMMProductParam,
                           CThostFtdcRspInfoField *rsp_info, int request_id,
                           bool is_last) override {
    if (callbacks_.on_rsp_qry_spmm_product_param == nullptr) {
      return;
    }
    ctp_spmm_product_param bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spmm_product_param(bridge, pSPMMProductParam);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spmm_product_param(
        pSPMMProductParam != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySecAgentACIDMap(CThostFtdcSecAgentACIDMapField *pSecAgentACIDMap,
                               CThostFtdcRspInfoField *rsp_info, int request_id,
                               bool is_last) override {
    if (callbacks_.on_rsp_qry_sec_agent_ac_id_map == nullptr) {
      return;
    }
    ctp_sec_agent_ac_id_map bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_sec_agent_ac_id_map(bridge, pSecAgentACIDMap);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_sec_agent_ac_id_map(
        pSecAgentACIDMap != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySecAgentCheckMode(
      CThostFtdcSecAgentCheckModeField *pSecAgentCheckMode,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_sec_agent_check_mode == nullptr) {
      return;
    }
    ctp_sec_agent_check_mode bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_sec_agent_check_mode(bridge, pSecAgentCheckMode);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_sec_agent_check_mode(
        pSecAgentCheckMode != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySecAgentTradeInfo(
      CThostFtdcSecAgentTradeInfoField *pSecAgentTradeInfo,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_sec_agent_trade_info == nullptr) {
      return;
    }
    ctp_sec_agent_trade_info bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_sec_agent_trade_info(bridge, pSecAgentTradeInfo);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_sec_agent_trade_info(
        pSecAgentTradeInfo != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void
  OnRspQrySecAgentTradingAccount(CThostFtdcTradingAccountField *pTradingAccount,
                                 CThostFtdcRspInfoField *rsp_info,
                                 int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_sec_agent_trading_account == nullptr) {
      return;
    }
    ctp_trading_account bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_trading_account(bridge, pTradingAccount);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_sec_agent_trading_account(
        pTradingAccount != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySettlementInfo(CThostFtdcSettlementInfoField *pSettlementInfo,
                              CThostFtdcRspInfoField *rsp_info, int request_id,
                              bool is_last) override {
    if (callbacks_.on_rsp_qry_settlement_info == nullptr) {
      return;
    }
    ctp_settlement_info bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_settlement_info(bridge, pSettlementInfo);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_settlement_info(
        pSettlementInfo != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySettlementInfoConfirm(
      CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_qry_settlement_info_confirm == nullptr) {
      return;
    }
    ctp_settlement_info_confirm bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_settlement_info_confirm(bridge, pSettlementInfoConfirm);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_settlement_info_confirm(
        pSettlementInfoConfirm != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQrySpdApply(CThostFtdcSpdApplyField *pSpdApply,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_qry_spd_apply == nullptr) {
      return;
    }
    ctp_spd_apply bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_spd_apply(bridge, pSpdApply);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_spd_apply(pSpdApply != nullptr ? &bridge : nullptr,
                                    rsp_info != nullptr ? &rsp_bridge : nullptr,
                                    request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryTrade(CThostFtdcTradeField *pTrade,
                     CThostFtdcRspInfoField *rsp_info, int request_id,
                     bool is_last) override {
    if (callbacks_.on_rsp_qry_trade == nullptr) {
      return;
    }
    ctp_trade bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_trade(bridge, pTrade);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_trade(pTrade != nullptr ? &bridge : nullptr,
                                rsp_info != nullptr ? &rsp_bridge : nullptr,
                                request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQryTraderOffer(CThostFtdcTraderOfferField *pTraderOffer,
                           CThostFtdcRspInfoField *rsp_info, int request_id,
                           bool is_last) override {
    if (callbacks_.on_rsp_qry_trader_offer == nullptr) {
      return;
    }
    ctp_trader_offer bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_trader_offer(bridge, pTraderOffer);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_trader_offer(
        pTraderOffer != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryTradingCode(CThostFtdcTradingCodeField *pTradingCode,
                           CThostFtdcRspInfoField *rsp_info, int request_id,
                           bool is_last) override {
    if (callbacks_.on_rsp_qry_trading_code == nullptr) {
      return;
    }
    ctp_trading_code bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_trading_code(bridge, pTradingCode);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_trading_code(
        pTradingCode != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryTradingNotice(CThostFtdcTradingNoticeField *pTradingNotice,
                             CThostFtdcRspInfoField *rsp_info, int request_id,
                             bool is_last) override {
    if (callbacks_.on_rsp_qry_trading_notice == nullptr) {
      return;
    }
    ctp_trading_notice bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_trading_notice(bridge, pTradingNotice);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_trading_notice(
        pTradingNotice != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryTransferBank(CThostFtdcTransferBankField *pTransferBank,
                            CThostFtdcRspInfoField *rsp_info, int request_id,
                            bool is_last) override {
    if (callbacks_.on_rsp_qry_transfer_bank == nullptr) {
      return;
    }
    ctp_transfer_bank bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_transfer_bank(bridge, pTransferBank);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_transfer_bank(
        pTransferBank != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryTransferSerial(CThostFtdcTransferSerialField *pTransferSerial,
                              CThostFtdcRspInfoField *rsp_info, int request_id,
                              bool is_last) override {
    if (callbacks_.on_rsp_qry_transfer_serial == nullptr) {
      return;
    }
    ctp_transfer_serial bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_transfer_serial(bridge, pTransferSerial);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_transfer_serial(
        pTransferSerial != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQryUserSession(CThostFtdcUserSessionField *pUserSession,
                           CThostFtdcRspInfoField *rsp_info, int request_id,
                           bool is_last) override {
    if (callbacks_.on_rsp_qry_user_session == nullptr) {
      return;
    }
    ctp_user_session bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_user_session(bridge, pUserSession);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_qry_user_session(
        pUserSession != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQueryBankAccountMoneyByFuture(
      CThostFtdcReqQueryAccountField *pReqQueryAccount,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_query_bank_account_money_by_future == nullptr) {
      return;
    }
    ctp_req_query_account bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_req_query_account(bridge, pReqQueryAccount);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_query_bank_account_money_by_future(
        pReqQueryAccount != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQueryCFMMCTradingAccountToken(
      CThostFtdcQueryCFMMCTradingAccountTokenField
          *pQueryCFMMCTradingAccountToken,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_query_cfmmc_trading_account_token == nullptr) {
      return;
    }
    ctp_query_cfmmc_trading_account_token bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_query_cfmmc_trading_account_token(bridge,
                                           pQueryCFMMCTradingAccountToken);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_query_cfmmc_trading_account_token(
        pQueryCFMMCTradingAccountToken != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspQuoteAction(CThostFtdcInputQuoteActionField *pInputQuoteAction,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_quote_action == nullptr) {
      return;
    }
    ctp_input_quote_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_quote_action(bridge, pInputQuoteAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_quote_action(pInputQuoteAction != nullptr ? &bridge
                                                                : nullptr,
                                   rsp_info != nullptr ? &rsp_bridge : nullptr,
                                   request_id, is_last ? 1 : 0, user_data_);
  }

  void OnRspQuoteInsert(CThostFtdcInputQuoteField *pInputQuote,
                        CThostFtdcRspInfoField *rsp_info, int request_id,
                        bool is_last) override {
    if (callbacks_.on_rsp_quote_insert == nullptr) {
      return;
    }
    ctp_input_quote bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_quote(bridge, pInputQuote);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_quote_insert(pInputQuote != nullptr ? &bridge : nullptr,
                                   rsp_info != nullptr ? &rsp_bridge : nullptr,
                                   request_id, is_last ? 1 : 0, user_data_);
  }

  void
  OnRspRemoveParkedOrder(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder,
                         CThostFtdcRspInfoField *rsp_info, int request_id,
                         bool is_last) override {
    if (callbacks_.on_rsp_remove_parked_order == nullptr) {
      return;
    }
    ctp_remove_parked_order bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_remove_parked_order(bridge, pRemoveParkedOrder);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_remove_parked_order(
        pRemoveParkedOrder != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspRemoveParkedOrderAction(
      CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_remove_parked_order_action == nullptr) {
      return;
    }
    ctp_remove_parked_order_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_remove_parked_order_action(bridge, pRemoveParkedOrderAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_remove_parked_order_action(
        pRemoveParkedOrderAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspSpdApply(CThostFtdcInputSpdApplyField *pInputSpdApply,
                     CThostFtdcRspInfoField *rsp_info, int request_id,
                     bool is_last) override {
    if (callbacks_.on_rsp_spd_apply == nullptr) {
      return;
    }
    ctp_input_spd_apply bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_spd_apply(bridge, pInputSpdApply);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_spd_apply(pInputSpdApply != nullptr ? &bridge : nullptr,
                                rsp_info != nullptr ? &rsp_bridge : nullptr,
                                request_id, is_last ? 1 : 0, user_data_);
  }

  void
  OnRspSpdApplyAction(CThostFtdcInputSpdApplyActionField *pInputSpdApplyAction,
                      CThostFtdcRspInfoField *rsp_info, int request_id,
                      bool is_last) override {
    if (callbacks_.on_rsp_spd_apply_action == nullptr) {
      return;
    }
    ctp_input_spd_apply_action bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_input_spd_apply_action(bridge, pInputSpdApplyAction);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_spd_apply_action(
        pInputSpdApplyAction != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void
  OnRspTradingAccountPasswordUpdate(CThostFtdcTradingAccountPasswordUpdateField
                                        *pTradingAccountPasswordUpdate,
                                    CThostFtdcRspInfoField *rsp_info,
                                    int request_id, bool is_last) override {
    if (callbacks_.on_rsp_trading_account_password_update == nullptr) {
      return;
    }
    ctp_trading_account_password_update bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_trading_account_password_update(bridge, pTradingAccountPasswordUpdate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_trading_account_password_update(
        pTradingAccountPasswordUpdate != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspUserAuthMethod(CThostFtdcRspUserAuthMethodField *pRspUserAuthMethod,
                           CThostFtdcRspInfoField *rsp_info, int request_id,
                           bool is_last) override {
    if (callbacks_.on_rsp_user_auth_method == nullptr) {
      return;
    }
    ctp_rsp_user_auth_method bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_rsp_user_auth_method(bridge, pRspUserAuthMethod);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_user_auth_method(
        pRspUserAuthMethod != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRspUserPasswordUpdate(
      CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate,
      CThostFtdcRspInfoField *rsp_info, int request_id, bool is_last) override {
    if (callbacks_.on_rsp_user_password_update == nullptr) {
      return;
    }
    ctp_user_password_update bridge{};
    ctp_rsp_info rsp_bridge{};
    fill_user_password_update(bridge, pUserPasswordUpdate);
    fill_rsp_info(rsp_bridge, rsp_info);
    callbacks_.on_rsp_user_password_update(
        pUserPasswordUpdate != nullptr ? &bridge : nullptr,
        rsp_info != nullptr ? &rsp_bridge : nullptr, request_id,
        is_last ? 1 : 0, user_data_);
  }

  void OnRtnCombAction(CThostFtdcCombActionField *pCombAction) override {
    if (callbacks_.on_rtn_comb_action == nullptr || pCombAction == nullptr) {
      return;
    }
    ctp_comb_action bridge{};
    fill_comb_action(bridge, pCombAction);
    callbacks_.on_rtn_comb_action(&bridge, user_data_);
  }

  void OnRtnExecOrder(CThostFtdcExecOrderField *pExecOrder) override {
    if (callbacks_.on_rtn_exec_order == nullptr || pExecOrder == nullptr) {
      return;
    }
    ctp_exec_order bridge{};
    fill_exec_order(bridge, pExecOrder);
    callbacks_.on_rtn_exec_order(&bridge, user_data_);
  }

  void OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp) override {
    if (callbacks_.on_rtn_for_quote_rsp == nullptr || pForQuoteRsp == nullptr) {
      return;
    }
    ctp_for_quote_rsp bridge{};
    fill_for_quote_rsp(bridge, pForQuoteRsp);
    callbacks_.on_rtn_for_quote_rsp(&bridge, user_data_);
  }

  void OnRtnFromBankToFutureByFuture(
      CThostFtdcRspTransferField *pRspTransfer) override {
    if (callbacks_.on_rtn_from_bank_to_future_by_future == nullptr ||
        pRspTransfer == nullptr) {
      return;
    }
    ctp_rsp_transfer bridge{};
    fill_rsp_transfer(bridge, pRspTransfer);
    callbacks_.on_rtn_from_bank_to_future_by_future(&bridge, user_data_);
  }

  void OnRtnFromFutureToBankByFuture(
      CThostFtdcRspTransferField *pRspTransfer) override {
    if (callbacks_.on_rtn_from_future_to_bank_by_future == nullptr ||
        pRspTransfer == nullptr) {
      return;
    }
    ctp_rsp_transfer bridge{};
    fill_rsp_transfer(bridge, pRspTransfer);
    callbacks_.on_rtn_from_future_to_bank_by_future(&bridge, user_data_);
  }

  void OnRtnHedgeCfm(CThostFtdcHedgeCfmField *pHedgeCfm) override {
    if (callbacks_.on_rtn_hedge_cfm == nullptr || pHedgeCfm == nullptr) {
      return;
    }
    ctp_hedge_cfm bridge{};
    fill_hedge_cfm(bridge, pHedgeCfm);
    callbacks_.on_rtn_hedge_cfm(&bridge, user_data_);
  }

  void
  OnRtnOffsetSetting(CThostFtdcOffsetSettingField *pOffsetSetting) override {
    if (callbacks_.on_rtn_offset_setting == nullptr ||
        pOffsetSetting == nullptr) {
      return;
    }
    ctp_offset_setting bridge{};
    fill_offset_setting(bridge, pOffsetSetting);
    callbacks_.on_rtn_offset_setting(&bridge, user_data_);
  }

  void OnRtnOptionSelfClose(
      CThostFtdcOptionSelfCloseField *pOptionSelfClose) override {
    if (callbacks_.on_rtn_option_self_close == nullptr ||
        pOptionSelfClose == nullptr) {
      return;
    }
    ctp_option_self_close bridge{};
    fill_option_self_close(bridge, pOptionSelfClose);
    callbacks_.on_rtn_option_self_close(&bridge, user_data_);
  }

  void OnRtnQueryBankBalanceByFuture(
      CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount) override {
    if (callbacks_.on_rtn_query_bank_balance_by_future == nullptr ||
        pNotifyQueryAccount == nullptr) {
      return;
    }
    ctp_notify_query_account bridge{};
    fill_notify_query_account(bridge, pNotifyQueryAccount);
    callbacks_.on_rtn_query_bank_balance_by_future(&bridge, user_data_);
  }

  void OnRtnQuote(CThostFtdcQuoteField *pQuote) override {
    if (callbacks_.on_rtn_quote == nullptr || pQuote == nullptr) {
      return;
    }
    ctp_quote bridge{};
    fill_quote(bridge, pQuote);
    callbacks_.on_rtn_quote(&bridge, user_data_);
  }

  void OnRtnSpdApply(CThostFtdcSpdApplyField *pSpdApply) override {
    if (callbacks_.on_rtn_spd_apply == nullptr || pSpdApply == nullptr) {
      return;
    }
    ctp_spd_apply bridge{};
    fill_spd_apply(bridge, pSpdApply);
    callbacks_.on_rtn_spd_apply(&bridge, user_data_);
  }

private:
  ctp_trader_spi callbacks_{};
  void *user_data_{};
};

void fill_req_user_login(CThostFtdcReqUserLoginField &dest,
                         const ctp_req_user_login &src) {
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

void fill_req_user_logout(CThostFtdcUserLogoutField &dest,
                          const ctp_user_logout &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
}

void fill_req_authenticate(CThostFtdcReqAuthenticateField &dest,
                           const ctp_req_authenticate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.AuthCode, src.auth_code);
  copy_field(dest.AppID, src.app_id);
}

void fill_settlement_info_confirm(CThostFtdcSettlementInfoConfirmField &dest,
                                  const ctp_settlement_info_confirm &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ConfirmDate, src.confirm_date);
  copy_field(dest.ConfirmTime, src.confirm_time);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_qry_trading_account(CThostFtdcQryTradingAccountField &dest,
                              const ctp_qry_trading_account &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.CurrencyID, src.currency_id);
  dest.BizType = src.biz_type;
  copy_field(dest.AccountID, src.account_id);
}

void fill_qry_investor_position(CThostFtdcQryInvestorPositionField &dest,
                                const ctp_qry_investor_position &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_instrument_margin_rate(
    CThostFtdcQryInstrumentMarginRateField &dest,
    const ctp_qry_instrument_margin_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_exchange_margin_rate(CThostFtdcQryExchangeMarginRateField &dest,
                                   const ctp_qry_exchange_margin_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_instrument_commission_rate(
    CThostFtdcQryInstrumentCommissionRateField &dest,
    const ctp_qry_instrument_commission_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_input_order(CThostFtdcInputOrderField &dest,
                      const ctp_input_order &src) {
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

void fill_input_order_action(CThostFtdcInputOrderActionField &dest,
                             const ctp_input_order_action &src) {
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

void fill_accountregister(CThostFtdcAccountregisterField &dest,
                          const ctp_accountregister &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradeDay, src.trade_day);
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBranchID, src.bank_branch_id);
  copy_field(dest.BankAccount, src.bank_account);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BrokerBranchID, src.broker_branch_id);
  copy_field(dest.AccountID, src.account_id);
  dest.IdCardType = src.id_card_type;
  copy_field(dest.IdentifiedCardNo, src.identified_card_no);
  copy_field(dest.CustomerName, src.customer_name);
  copy_field(dest.CurrencyID, src.currency_id);
  dest.OpenOrDestroy = src.open_or_destroy;
  copy_field(dest.RegDate, src.reg_date);
  copy_field(dest.OutDate, src.out_date);
  dest.TID = src.t_id;
  dest.CustType = src.cust_type;
  dest.BankAccType = src.bank_acc_type;
  copy_field(dest.LongCustomerName, src.long_customer_name);
}

void fill_batch_order_action(CThostFtdcBatchOrderActionField &dest,
                             const ctp_batch_order_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.OrderActionRef = src.order_action_ref;
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ActionDate, src.action_date);
  copy_field(dest.ActionTime, src.action_time);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  copy_field(dest.ActionLocalID, src.action_local_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.OrderActionStatus = src.order_action_status;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_broker_trading_algos(CThostFtdcBrokerTradingAlgosField &dest,
                               const ctp_broker_trading_algos &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.HandlePositionAlgoID = src.handle_position_algo_id;
  dest.FindMarginRateAlgoID = src.find_margin_rate_algo_id;
  dest.HandleTradingAccountAlgoID = src.handle_trading_account_algo_id;
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_broker_trading_params(CThostFtdcBrokerTradingParamsField &dest,
                                const ctp_broker_trading_params &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.MarginPriceType = src.margin_price_type;
  dest.Algorithm = src.algorithm;
  dest.AvailIncludeCloseProfit = src.avail_include_close_profit;
  copy_field(dest.CurrencyID, src.currency_id);
  dest.OptionRoyaltyPriceType = src.option_royalty_price_type;
  copy_field(dest.AccountID, src.account_id);
}

void fill_cfmmc_trading_account_key(CThostFtdcCFMMCTradingAccountKeyField &dest,
                                    const ctp_cfmmc_trading_account_key &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.AccountID, src.account_id);
  dest.KeyID = src.key_id;
  copy_field(dest.CurrentKey, src.current_key);
}

void fill_cancel_offset_setting(CThostFtdcCancelOffsetSettingField &dest,
                                const ctp_cancel_offset_setting &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.UnderlyingInstrID, src.underlying_instr_id);
  copy_field(dest.ProductID, src.product_id);
  dest.OffsetType = src.offset_type;
  dest.Volume = src.volume;
  dest.IsOffset = src.is_offset;
  dest.RequestID = src.request_id;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.ExchangeSerialNo, src.exchange_serial_no);
  copy_field(dest.ExchangeProductID, src.exchange_product_id);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  dest.OrderActionStatus = src.order_action_status;
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.ActionLocalID, src.action_local_id);
  copy_field(dest.ActionDate, src.action_date);
  copy_field(dest.ActionTime, src.action_time);
}

void fill_comb_action(CThostFtdcCombActionField &dest,
                      const ctp_comb_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.CombActionRef, src.comb_action_ref);
  copy_field(dest.UserID, src.user_id);
  dest.Direction = src.direction;
  dest.Volume = src.volume;
  dest.CombDirection = src.comb_direction;
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.ActionLocalID, src.action_local_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  dest.ActionStatus = src.action_status;
  dest.NotifySequence = src.notify_sequence;
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  dest.SequenceNo = src.sequence_no;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.reserve3, src.reserve3);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.ComTradeID, src.com_trade_id);
  copy_field(dest.BranchID, src.branch_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_comb_instrument_guard(CThostFtdcCombInstrumentGuardField &dest,
                                const ctp_comb_instrument_guard &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.GuarantRatio = src.guarant_ratio;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_comb_leg(CThostFtdcCombLegField &dest, const ctp_comb_leg &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.CombInstrumentID, src.comb_instrument_id);
  dest.LegID = src.leg_id;
  copy_field(dest.LegInstrumentID, src.leg_instrument_id);
  dest.Direction = src.direction;
  dest.LegMultiple = src.leg_multiple;
  dest.ImplyLevel = src.imply_level;
}

void fill_comb_promotion_param(CThostFtdcCombPromotionParamField &dest,
                               const ctp_comb_promotion_param &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.CombHedgeFlag, src.comb_hedge_flag);
  dest.Xparameter = src.xparameter;
}

void fill_contract_bank(CThostFtdcContractBankField &dest,
                        const ctp_contract_bank &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBrchID, src.bank_brch_id);
  copy_field(dest.BankName, src.bank_name);
  copy_field(dest.csrcBankID, src.csrc_bank_id);
}

void fill_e_warrant_offset(CThostFtdcEWarrantOffsetField &dest,
                           const ctp_e_warrant_offset &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.Direction = src.direction;
  dest.HedgeFlag = src.hedge_flag;
  dest.Volume = src.volume;
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_exchange(CThostFtdcExchangeField &dest, const ctp_exchange &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ExchangeName, src.exchange_name);
  dest.ExchangeProperty = src.exchange_property;
}

void fill_exchange_margin_rate_adjust(
    CThostFtdcExchangeMarginRateAdjustField &dest,
    const ctp_exchange_margin_rate_adjust &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.HedgeFlag = src.hedge_flag;
  dest.LongMarginRatioByMoney = src.long_margin_ratio_by_money;
  dest.LongMarginRatioByVolume = src.long_margin_ratio_by_volume;
  dest.ShortMarginRatioByMoney = src.short_margin_ratio_by_money;
  dest.ShortMarginRatioByVolume = src.short_margin_ratio_by_volume;
  dest.ExchLongMarginRatioByMoney = src.exch_long_margin_ratio_by_money;
  dest.ExchLongMarginRatioByVolume = src.exch_long_margin_ratio_by_volume;
  dest.ExchShortMarginRatioByMoney = src.exch_short_margin_ratio_by_money;
  dest.ExchShortMarginRatioByVolume = src.exch_short_margin_ratio_by_volume;
  dest.NoLongMarginRatioByMoney = src.no_long_margin_ratio_by_money;
  dest.NoLongMarginRatioByVolume = src.no_long_margin_ratio_by_volume;
  dest.NoShortMarginRatioByMoney = src.no_short_margin_ratio_by_money;
  dest.NoShortMarginRatioByVolume = src.no_short_margin_ratio_by_volume;
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_exchange_rate(CThostFtdcExchangeRateField &dest,
                        const ctp_exchange_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.FromCurrencyID, src.from_currency_id);
  dest.FromCurrencyUnit = src.from_currency_unit;
  copy_field(dest.ToCurrencyID, src.to_currency_id);
  dest.ExchangeRate = src.exchange_rate;
}

void fill_exec_order_action(CThostFtdcExecOrderActionField &dest,
                            const ctp_exec_order_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.ExecOrderActionRef = src.exec_order_action_ref;
  copy_field(dest.ExecOrderRef, src.exec_order_ref);
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ExecOrderSysID, src.exec_order_sys_id);
  dest.ActionFlag = src.action_flag;
  copy_field(dest.ActionDate, src.action_date);
  copy_field(dest.ActionTime, src.action_time);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  copy_field(dest.ExecOrderLocalID, src.exec_order_local_id);
  copy_field(dest.ActionLocalID, src.action_local_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.OrderActionStatus = src.order_action_status;
  copy_field(dest.UserID, src.user_id);
  dest.ActionType = src.action_type;
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.BranchID, src.branch_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_exec_order(CThostFtdcExecOrderField &dest,
                     const ctp_exec_order &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExecOrderRef, src.exec_order_ref);
  copy_field(dest.UserID, src.user_id);
  dest.Volume = src.volume;
  dest.RequestID = src.request_id;
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.OffsetFlag = src.offset_flag;
  dest.HedgeFlag = src.hedge_flag;
  dest.ActionType = src.action_type;
  dest.PosiDirection = src.posi_direction;
  dest.ReservePositionFlag = src.reserve_position_flag;
  dest.CloseFlag = src.close_flag;
  copy_field(dest.ExecOrderLocalID, src.exec_order_local_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  dest.OrderSubmitStatus = src.order_submit_status;
  dest.NotifySequence = src.notify_sequence;
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.ExecOrderSysID, src.exec_order_sys_id);
  copy_field(dest.InsertDate, src.insert_date);
  copy_field(dest.InsertTime, src.insert_time);
  copy_field(dest.CancelTime, src.cancel_time);
  dest.ExecResult = src.exec_result;
  copy_field(dest.ClearingPartID, src.clearing_part_id);
  dest.SequenceNo = src.sequence_no;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.ActiveUserID, src.active_user_id);
  dest.BrokerExecOrderSeq = src.broker_exec_order_seq;
  copy_field(dest.BranchID, src.branch_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.reserve3, src.reserve3);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_fens_user_info(CThostFtdcFensUserInfoField &dest,
                         const ctp_fens_user_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  dest.LoginMode = src.login_mode;
}

void fill_for_quote(CThostFtdcForQuoteField &dest, const ctp_for_quote &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ForQuoteRef, src.for_quote_ref);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.ForQuoteLocalID, src.for_quote_local_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  copy_field(dest.InsertDate, src.insert_date);
  copy_field(dest.InsertTime, src.insert_time);
  dest.ForQuoteStatus = src.for_quote_status;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.ActiveUserID, src.active_user_id);
  dest.BrokerForQutoSeq = src.broker_for_quto_seq;
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve3, src.reserve3);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_for_quote_rsp(CThostFtdcForQuoteRspField &dest,
                        const ctp_for_quote_rsp &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ForQuoteSysID, src.for_quote_sys_id);
  copy_field(dest.ForQuoteTime, src.for_quote_time);
  copy_field(dest.ActionDay, src.action_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_front_info(CThostFtdcFrontInfoField &dest,
                     const ctp_front_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.FrontAddr, src.front_addr);
  dest.QryFreq = src.qry_freq;
  dest.FTDPkgFreq = src.ftd_pkg_freq;
}

void fill_hedge_cfm_action(CThostFtdcHedgeCfmActionField &dest,
                           const ctp_hedge_cfm_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ActionDate, src.action_date);
  copy_field(dest.ActionTime, src.action_time);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  copy_field(dest.OrderLocalID, src.order_local_id);
  copy_field(dest.ActionLocalID, src.action_local_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  dest.OrderActionStatus = src.order_action_status;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OrderSysID, src.order_sys_id);
  dest.RequestID = src.request_id;
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.OrderRef, src.order_ref);
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
}

void fill_hedge_cfm(CThostFtdcHedgeCfmField &dest, const ctp_hedge_cfm &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.UserID, src.user_id);
  dest.Volume = src.volume;
  dest.Direction = src.direction;
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.OrderRef, src.order_ref);
  copy_field(dest.ActiveUserID, src.active_user_id);
  dest.BrokerOrderSeq = src.broker_order_seq;
  copy_field(dest.OrderSysID, src.order_sys_id);
  dest.ApplyStatus = src.apply_status;
  dest.SequenceNo = src.sequence_no;
  dest.DealVolume = src.deal_volume;
  copy_field(dest.InsertDate, src.insert_date);
  copy_field(dest.InsertTime, src.insert_time);
  copy_field(dest.CancelTime, src.cancel_time);
  copy_field(dest.ReqDate, src.req_date);
  copy_field(dest.OrderLocalID, src.order_local_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  dest.OrderSubmitStatus = src.order_submit_status;
  dest.NotifySequence = src.notify_sequence;
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
}

void fill_input_batch_order_action(CThostFtdcInputBatchOrderActionField &dest,
                                   const ctp_input_batch_order_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.OrderActionRef = src.order_action_ref;
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_input_comb_action(CThostFtdcInputCombActionField &dest,
                            const ctp_input_comb_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.CombActionRef, src.comb_action_ref);
  copy_field(dest.UserID, src.user_id);
  dest.Direction = src.direction;
  dest.Volume = src.volume;
  dest.CombDirection = src.comb_direction;
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_input_exec_order_action(CThostFtdcInputExecOrderActionField &dest,
                                  const ctp_input_exec_order_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.ExecOrderActionRef = src.exec_order_action_ref;
  copy_field(dest.ExecOrderRef, src.exec_order_ref);
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ExecOrderSysID, src.exec_order_sys_id);
  dest.ActionFlag = src.action_flag;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_input_exec_order(CThostFtdcInputExecOrderField &dest,
                           const ctp_input_exec_order &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExecOrderRef, src.exec_order_ref);
  copy_field(dest.UserID, src.user_id);
  dest.Volume = src.volume;
  dest.RequestID = src.request_id;
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.OffsetFlag = src.offset_flag;
  dest.HedgeFlag = src.hedge_flag;
  dest.ActionType = src.action_type;
  dest.PosiDirection = src.posi_direction;
  dest.ReservePositionFlag = src.reserve_position_flag;
  dest.CloseFlag = src.close_flag;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_input_for_quote(CThostFtdcInputForQuoteField &dest,
                          const ctp_input_for_quote &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ForQuoteRef, src.for_quote_ref);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_input_hedge_cfm_action(CThostFtdcInputHedgeCfmActionField &dest,
                                 const ctp_input_hedge_cfm_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OrderSysID, src.order_sys_id);
  copy_field(dest.OrderRef, src.order_ref);
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  dest.RequestID = src.request_id;
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
}

void fill_input_hedge_cfm(CThostFtdcInputHedgeCfmField &dest,
                          const ctp_input_hedge_cfm &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  dest.Volume = src.volume;
  dest.Direction = src.direction;
  dest.RequestID = src.request_id;
  copy_field(dest.OrderRef, src.order_ref);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
}

void fill_input_offset_setting(CThostFtdcInputOffsetSettingField &dest,
                               const ctp_input_offset_setting &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.UnderlyingInstrID, src.underlying_instr_id);
  copy_field(dest.ProductID, src.product_id);
  dest.OffsetType = src.offset_type;
  dest.Volume = src.volume;
  dest.IsOffset = src.is_offset;
  dest.RequestID = src.request_id;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
}

void fill_input_option_self_close_action(
    CThostFtdcInputOptionSelfCloseActionField &dest,
    const ctp_input_option_self_close_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.OptionSelfCloseActionRef = src.option_self_close_action_ref;
  copy_field(dest.OptionSelfCloseRef, src.option_self_close_ref);
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OptionSelfCloseSysID, src.option_self_close_sys_id);
  dest.ActionFlag = src.action_flag;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_input_option_self_close(CThostFtdcInputOptionSelfCloseField &dest,
                                  const ctp_input_option_self_close &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.OptionSelfCloseRef, src.option_self_close_ref);
  copy_field(dest.UserID, src.user_id);
  dest.Volume = src.volume;
  dest.RequestID = src.request_id;
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.HedgeFlag = src.hedge_flag;
  dest.OptSelfCloseFlag = src.opt_self_close_flag;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_input_quote_action(CThostFtdcInputQuoteActionField &dest,
                             const ctp_input_quote_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.QuoteActionRef = src.quote_action_ref;
  copy_field(dest.QuoteRef, src.quote_ref);
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.QuoteSysID, src.quote_sys_id);
  dest.ActionFlag = src.action_flag;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.OrderMemo, src.order_memo);
  dest.SessionReqSeq = src.session_req_seq;
}

void fill_input_quote(CThostFtdcInputQuoteField &dest,
                      const ctp_input_quote &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.QuoteRef, src.quote_ref);
  copy_field(dest.UserID, src.user_id);
  dest.AskPrice = src.ask_price;
  dest.BidPrice = src.bid_price;
  dest.AskVolume = src.ask_volume;
  dest.BidVolume = src.bid_volume;
  dest.RequestID = src.request_id;
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.AskOffsetFlag = src.ask_offset_flag;
  dest.BidOffsetFlag = src.bid_offset_flag;
  dest.AskHedgeFlag = src.ask_hedge_flag;
  dest.BidHedgeFlag = src.bid_hedge_flag;
  copy_field(dest.AskOrderRef, src.ask_order_ref);
  copy_field(dest.BidOrderRef, src.bid_order_ref);
  copy_field(dest.ForQuoteSysID, src.for_quote_sys_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.ReplaceSysID, src.replace_sys_id);
  dest.TimeCondition = src.time_condition;
  copy_field(dest.OrderMemo, src.order_memo);
  dest.SessionReqSeq = src.session_req_seq;
}

void fill_input_spd_apply_action(CThostFtdcInputSpdApplyActionField &dest,
                                 const ctp_input_spd_apply_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OrderSysID, src.order_sys_id);
  copy_field(dest.OrderRef, src.order_ref);
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  dest.RequestID = src.request_id;
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
}

void fill_input_spd_apply(CThostFtdcInputSpdApplyField &dest,
                          const ctp_input_spd_apply &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.FirstLegInstrumentID, src.first_leg_instrument_id);
  copy_field(dest.SecondLegInstrumentID, src.second_leg_instrument_id);
  dest.Volume = src.volume;
  dest.Direction = src.direction;
  dest.CmbType = src.cmb_type;
  dest.RequestID = src.request_id;
  copy_field(dest.OrderRef, src.order_ref);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
}

void fill_instrument(CThostFtdcInstrumentField &dest,
                     const ctp_instrument &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentName, src.instrument_name);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.reserve3, src.reserve3);
  dest.ProductClass = src.product_class;
  dest.DeliveryYear = src.delivery_year;
  dest.DeliveryMonth = src.delivery_month;
  dest.MaxMarketOrderVolume = src.max_market_order_volume;
  dest.MinMarketOrderVolume = src.min_market_order_volume;
  dest.MaxLimitOrderVolume = src.max_limit_order_volume;
  dest.MinLimitOrderVolume = src.min_limit_order_volume;
  dest.VolumeMultiple = src.volume_multiple;
  dest.PriceTick = src.price_tick;
  copy_field(dest.CreateDate, src.create_date);
  copy_field(dest.OpenDate, src.open_date);
  copy_field(dest.ExpireDate, src.expire_date);
  copy_field(dest.StartDelivDate, src.start_deliv_date);
  copy_field(dest.EndDelivDate, src.end_deliv_date);
  dest.InstLifePhase = src.inst_life_phase;
  dest.IsTrading = src.is_trading;
  dest.PositionType = src.position_type;
  dest.PositionDateType = src.position_date_type;
  dest.LongMarginRatio = src.long_margin_ratio;
  dest.ShortMarginRatio = src.short_margin_ratio;
  dest.MaxMarginSideAlgorithm = src.max_margin_side_algorithm;
  copy_field(dest.reserve4, src.reserve4);
  dest.StrikePrice = src.strike_price;
  dest.OptionsType = src.options_type;
  dest.UnderlyingMultiple = src.underlying_multiple;
  dest.CombinationType = src.combination_type;
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.ProductID, src.product_id);
  copy_field(dest.UnderlyingInstrID, src.underlying_instr_id);
}

void fill_instrument_order_comm_rate(
    CThostFtdcInstrumentOrderCommRateField &dest,
    const ctp_instrument_order_comm_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  dest.InvestorRange = src.investor_range;
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.HedgeFlag = src.hedge_flag;
  dest.OrderCommByVolume = src.order_comm_by_volume;
  dest.OrderActionCommByVolume = src.order_action_comm_by_volume;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  dest.OrderCommByTrade = src.order_comm_by_trade;
  dest.OrderActionCommByTrade = src.order_action_comm_by_trade;
}

void fill_invest_unit(CThostFtdcInvestUnitField &dest,
                      const ctp_invest_unit &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InvestorUnitName, src.investor_unit_name);
  copy_field(dest.InvestorGroupID, src.investor_group_id);
  copy_field(dest.CommModelID, src.comm_model_id);
  copy_field(dest.MarginModelID, src.margin_model_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_investor_commodity_group_spmm_margin(
    CThostFtdcInvestorCommodityGroupSPMMMarginField &dest,
    const ctp_investor_commodity_group_spmm_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.CommodityGroupID, src.commodity_group_id);
  dest.MarginBeforeDiscount = src.margin_before_discount;
  dest.MarginNoDiscount = src.margin_no_discount;
  dest.LongRisk = src.long_risk;
  dest.ShortRisk = src.short_risk;
  dest.CloseFrozenMargin = src.close_frozen_margin;
  dest.InterCommodityRate = src.inter_commodity_rate;
  dest.MiniMarginRatio = src.mini_margin_ratio;
  dest.AdjustRatio = src.adjust_ratio;
  dest.IntraCommodityDiscount = src.intra_commodity_discount;
  dest.InterCommodityDiscount = src.inter_commodity_discount;
  dest.ExchMargin = src.exch_margin;
  dest.InvestorMargin = src.investor_margin;
  dest.FrozenCommission = src.frozen_commission;
  dest.Commission = src.commission;
  dest.FrozenCash = src.frozen_cash;
  dest.CashIn = src.cash_in;
  dest.StrikeFrozenMargin = src.strike_frozen_margin;
}

void fill_investor_commodity_spmm_margin(
    CThostFtdcInvestorCommoditySPMMMarginField &dest,
    const ctp_investor_commodity_spmm_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.CommodityID, src.commodity_id);
  dest.MarginBeforeDiscount = src.margin_before_discount;
  dest.MarginNoDiscount = src.margin_no_discount;
  dest.LongPosRisk = src.long_pos_risk;
  dest.LongOpenFrozenRisk = src.long_open_frozen_risk;
  dest.LongCloseFrozenRisk = src.long_close_frozen_risk;
  dest.ShortPosRisk = src.short_pos_risk;
  dest.ShortOpenFrozenRisk = src.short_open_frozen_risk;
  dest.ShortCloseFrozenRisk = src.short_close_frozen_risk;
  dest.IntraCommodityRate = src.intra_commodity_rate;
  dest.OptionDiscountRate = src.option_discount_rate;
  dest.PosDiscount = src.pos_discount;
  dest.OpenFrozenDiscount = src.open_frozen_discount;
  dest.NetRisk = src.net_risk;
  dest.CloseFrozenMargin = src.close_frozen_margin;
  dest.FrozenCommission = src.frozen_commission;
  dest.Commission = src.commission;
  dest.FrozenCash = src.frozen_cash;
  dest.CashIn = src.cash_in;
  dest.StrikeFrozenMargin = src.strike_frozen_margin;
}

void fill_investor(CThostFtdcInvestorField &dest, const ctp_investor &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorGroupID, src.investor_group_id);
  copy_field(dest.InvestorName, src.investor_name);
  dest.IdentifiedCardType = src.identified_card_type;
  copy_field(dest.IdentifiedCardNo, src.identified_card_no);
  dest.IsActive = src.is_active;
  copy_field(dest.Telephone, src.telephone);
  copy_field(dest.Address, src.address);
  copy_field(dest.OpenDate, src.open_date);
  copy_field(dest.Mobile, src.mobile);
  copy_field(dest.CommModelID, src.comm_model_id);
  copy_field(dest.MarginModelID, src.margin_model_id);
  dest.IsOrderFreq = src.is_order_freq;
  dest.IsOpenVolLimit = src.is_open_vol_limit;
}

void fill_investor_info_comm_rec(CThostFtdcInvestorInfoCommRecField &dest,
                                 const ctp_investor_info_comm_rec &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  dest.OrderCount = src.order_count;
  dest.OrderActionCount = src.order_action_count;
  dest.ForQuoteCnt = src.for_quote_cnt;
  dest.InfoComm = src.info_comm;
  dest.IsOptSeries = src.is_opt_series;
  copy_field(dest.ProductID, src.product_id);
  dest.InfoCnt = src.info_cnt;
}

void fill_investor_portf_margin_ratio(
    CThostFtdcInvestorPortfMarginRatioField &dest,
    const ctp_investor_portf_margin_ratio &src) {
  std::memset(&dest, 0, sizeof(dest));
  dest.InvestorRange = src.investor_range;
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.MarginRatio = src.margin_ratio;
  copy_field(dest.ProductGroupID, src.product_group_id);
}

void fill_investor_portf_setting(CThostFtdcInvestorPortfSettingField &dest,
                                 const ctp_investor_portf_setting &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.HedgeFlag = src.hedge_flag;
  dest.UsePortf = src.use_portf;
}

void fill_investor_position_combine_detail(
    CThostFtdcInvestorPositionCombineDetailField &dest,
    const ctp_investor_position_combine_detail &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.OpenDate, src.open_date);
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ComTradeID, src.com_trade_id);
  copy_field(dest.TradeID, src.trade_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.HedgeFlag = src.hedge_flag;
  dest.Direction = src.direction;
  dest.TotalAmt = src.total_amt;
  dest.Margin = src.margin;
  dest.ExchMargin = src.exch_margin;
  dest.MarginRateByMoney = src.margin_rate_by_money;
  dest.MarginRateByVolume = src.margin_rate_by_volume;
  dest.LegID = src.leg_id;
  dest.LegMultiple = src.leg_multiple;
  copy_field(dest.reserve2, src.reserve2);
  dest.TradeGroupID = src.trade_group_id;
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.CombInstrumentID, src.comb_instrument_id);
}

void fill_investor_position_detail(CThostFtdcInvestorPositionDetailField &dest,
                                   const ctp_investor_position_detail &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.HedgeFlag = src.hedge_flag;
  dest.Direction = src.direction;
  copy_field(dest.OpenDate, src.open_date);
  copy_field(dest.TradeID, src.trade_id);
  dest.Volume = src.volume;
  dest.OpenPrice = src.open_price;
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  dest.TradeType = src.trade_type;
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.CloseProfitByDate = src.close_profit_by_date;
  dest.CloseProfitByTrade = src.close_profit_by_trade;
  dest.PositionProfitByDate = src.position_profit_by_date;
  dest.PositionProfitByTrade = src.position_profit_by_trade;
  dest.Margin = src.margin;
  dest.ExchMargin = src.exch_margin;
  dest.MarginRateByMoney = src.margin_rate_by_money;
  dest.MarginRateByVolume = src.margin_rate_by_volume;
  dest.LastSettlementPrice = src.last_settlement_price;
  dest.SettlementPrice = src.settlement_price;
  dest.CloseVolume = src.close_volume;
  dest.CloseAmount = src.close_amount;
  dest.TimeFirstVolume = src.time_first_volume;
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  dest.SpecPosiType = src.spec_posi_type;
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.CombInstrumentID, src.comb_instrument_id);
}

void fill_investor_prod_rcams_margin(
    CThostFtdcInvestorProdRCAMSMarginField &dest,
    const ctp_investor_prod_rcams_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.CombProductID, src.comb_product_id);
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.ProductGroupID, src.product_group_id);
  dest.RiskBeforeDiscount = src.risk_before_discount;
  dest.IntraInstrRisk = src.intra_instr_risk;
  dest.BPosRisk = src.b_pos_risk;
  dest.SPosRisk = src.s_pos_risk;
  dest.IntraProdRisk = src.intra_prod_risk;
  dest.NetRisk = src.net_risk;
  dest.InterProdRisk = src.inter_prod_risk;
  dest.ShortOptRiskAdj = src.short_opt_risk_adj;
  dest.OptionRoyalty = src.option_royalty;
  dest.MMSACloseFrozenMargin = src.mmsa_close_frozen_margin;
  dest.CloseCombFrozenMargin = src.close_comb_frozen_margin;
  dest.CloseFrozenMargin = src.close_frozen_margin;
  dest.MMSAOpenFrozenMargin = src.mmsa_open_frozen_margin;
  dest.DeliveryOpenFrozenMargin = src.delivery_open_frozen_margin;
  dest.OpenFrozenMargin = src.open_frozen_margin;
  dest.UseFrozenMargin = src.use_frozen_margin;
  dest.MMSAExchMargin = src.mmsa_exch_margin;
  dest.DeliveryExchMargin = src.delivery_exch_margin;
  dest.CombExchMargin = src.comb_exch_margin;
  dest.ExchMargin = src.exch_margin;
  dest.UseMargin = src.use_margin;
}

void fill_investor_prod_rule_margin(CThostFtdcInvestorProdRULEMarginField &dest,
                                    const ctp_investor_prod_rule_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
  dest.InstrumentClass = src.instrument_class;
  dest.CommodityGroupID = src.commodity_group_id;
  dest.BStdPosition = src.b_std_position;
  dest.SStdPosition = src.s_std_position;
  dest.BStdOpenFrozen = src.b_std_open_frozen;
  dest.SStdOpenFrozen = src.s_std_open_frozen;
  dest.BStdCloseFrozen = src.b_std_close_frozen;
  dest.SStdCloseFrozen = src.s_std_close_frozen;
  dest.IntraProdStdPosition = src.intra_prod_std_position;
  dest.NetStdPosition = src.net_std_position;
  dest.InterProdStdPosition = src.inter_prod_std_position;
  dest.SingleStdPosition = src.single_std_position;
  dest.IntraProdMargin = src.intra_prod_margin;
  dest.InterProdMargin = src.inter_prod_margin;
  dest.SingleMargin = src.single_margin;
  dest.NonCombMargin = src.non_comb_margin;
  dest.AddOnMargin = src.add_on_margin;
  dest.ExchMargin = src.exch_margin;
  dest.AddOnFrozenMargin = src.add_on_frozen_margin;
  dest.OpenFrozenMargin = src.open_frozen_margin;
  dest.CloseFrozenMargin = src.close_frozen_margin;
  dest.Margin = src.margin;
  dest.FrozenMargin = src.frozen_margin;
}

void fill_investor_prod_spbm_detail(CThostFtdcInvestorProdSPBMDetailField &dest,
                                    const ctp_investor_prod_spbm_detail &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
  dest.IntraInstrMargin = src.intra_instr_margin;
  dest.BCollectingMargin = src.b_collecting_margin;
  dest.SCollectingMargin = src.s_collecting_margin;
  dest.IntraProdMargin = src.intra_prod_margin;
  dest.NetMargin = src.net_margin;
  dest.InterProdMargin = src.inter_prod_margin;
  dest.SingleMargin = src.single_margin;
  dest.AddOnMargin = src.add_on_margin;
  dest.DeliveryMargin = src.delivery_margin;
  dest.CallOptionMinRisk = src.call_option_min_risk;
  dest.PutOptionMinRisk = src.put_option_min_risk;
  dest.OptionMinRisk = src.option_min_risk;
  dest.OptionValueOffset = src.option_value_offset;
  dest.OptionRoyalty = src.option_royalty;
  dest.RealOptionValueOffset = src.real_option_value_offset;
  dest.Margin = src.margin;
  dest.ExchMargin = src.exch_margin;
}

void fill_investor_product_group_margin(
    CThostFtdcInvestorProductGroupMarginField &dest,
    const ctp_investor_product_group_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  dest.FrozenMargin = src.frozen_margin;
  dest.LongFrozenMargin = src.long_frozen_margin;
  dest.ShortFrozenMargin = src.short_frozen_margin;
  dest.UseMargin = src.use_margin;
  dest.LongUseMargin = src.long_use_margin;
  dest.ShortUseMargin = src.short_use_margin;
  dest.ExchMargin = src.exch_margin;
  dest.LongExchMargin = src.long_exch_margin;
  dest.ShortExchMargin = src.short_exch_margin;
  dest.CloseProfit = src.close_profit;
  dest.FrozenCommission = src.frozen_commission;
  dest.Commission = src.commission;
  dest.FrozenCash = src.frozen_cash;
  dest.CashIn = src.cash_in;
  dest.PositionProfit = src.position_profit;
  dest.OffsetAmount = src.offset_amount;
  dest.LongOffsetAmount = src.long_offset_amount;
  dest.ShortOffsetAmount = src.short_offset_amount;
  dest.ExchOffsetAmount = src.exch_offset_amount;
  dest.LongExchOffsetAmount = src.long_exch_offset_amount;
  dest.ShortExchOffsetAmount = src.short_exch_offset_amount;
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.ProductGroupID, src.product_group_id);
}

void fill_mm_instrument_commission_rate(
    CThostFtdcMMInstrumentCommissionRateField &dest,
    const ctp_mm_instrument_commission_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  dest.InvestorRange = src.investor_range;
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.OpenRatioByMoney = src.open_ratio_by_money;
  dest.OpenRatioByVolume = src.open_ratio_by_volume;
  dest.CloseRatioByMoney = src.close_ratio_by_money;
  dest.CloseRatioByVolume = src.close_ratio_by_volume;
  dest.CloseTodayRatioByMoney = src.close_today_ratio_by_money;
  dest.CloseTodayRatioByVolume = src.close_today_ratio_by_volume;
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_mm_option_instr_comm_rate(CThostFtdcMMOptionInstrCommRateField &dest,
                                    const ctp_mm_option_instr_comm_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  dest.InvestorRange = src.investor_range;
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.OpenRatioByMoney = src.open_ratio_by_money;
  dest.OpenRatioByVolume = src.open_ratio_by_volume;
  dest.CloseRatioByMoney = src.close_ratio_by_money;
  dest.CloseRatioByVolume = src.close_ratio_by_volume;
  dest.CloseTodayRatioByMoney = src.close_today_ratio_by_money;
  dest.CloseTodayRatioByVolume = src.close_today_ratio_by_volume;
  dest.StrikeRatioByMoney = src.strike_ratio_by_money;
  dest.StrikeRatioByVolume = src.strike_ratio_by_volume;
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_notice(CThostFtdcNoticeField &dest, const ctp_notice &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.Content, src.content);
  copy_field(dest.SequenceLabel, src.sequence_label);
}

void fill_notify_query_account(CThostFtdcNotifyQueryAccountField &dest,
                               const ctp_notify_query_account &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradeCode, src.trade_code);
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBranchID, src.bank_branch_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BrokerBranchID, src.broker_branch_id);
  copy_field(dest.TradeDate, src.trade_date);
  copy_field(dest.TradeTime, src.trade_time);
  copy_field(dest.BankSerial, src.bank_serial);
  copy_field(dest.TradingDay, src.trading_day);
  dest.PlateSerial = src.plate_serial;
  dest.LastFragment = src.last_fragment;
  dest.SessionID = src.session_id;
  copy_field(dest.CustomerName, src.customer_name);
  dest.IdCardType = src.id_card_type;
  copy_field(dest.IdentifiedCardNo, src.identified_card_no);
  dest.CustType = src.cust_type;
  copy_field(dest.BankAccount, src.bank_account);
  copy_field(dest.BankPassWord, src.bank_pass_word);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.Password, src.password);
  dest.FutureSerial = src.future_serial;
  dest.InstallID = src.install_id;
  copy_field(dest.UserID, src.user_id);
  dest.VerifyCertNoFlag = src.verify_cert_no_flag;
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.Digest, src.digest);
  dest.BankAccType = src.bank_acc_type;
  copy_field(dest.DeviceID, src.device_id);
  dest.BankSecuAccType = src.bank_secu_acc_type;
  copy_field(dest.BrokerIDByBank, src.broker_id_by_bank);
  copy_field(dest.BankSecuAcc, src.bank_secu_acc);
  dest.BankPwdFlag = src.bank_pwd_flag;
  dest.SecuPwdFlag = src.secu_pwd_flag;
  copy_field(dest.OperNo, src.oper_no);
  dest.RequestID = src.request_id;
  dest.TID = src.t_id;
  dest.BankUseAmount = src.bank_use_amount;
  dest.BankFetchAmount = src.bank_fetch_amount;
  dest.ErrorID = src.error_id;
  copy_field(dest.ErrorMsg, src.error_msg);
  copy_field(dest.LongCustomerName, src.long_customer_name);
}

void fill_offset_setting(CThostFtdcOffsetSettingField &dest,
                         const ctp_offset_setting &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.UnderlyingInstrID, src.underlying_instr_id);
  copy_field(dest.ProductID, src.product_id);
  dest.OffsetType = src.offset_type;
  dest.Volume = src.volume;
  dest.IsOffset = src.is_offset;
  dest.RequestID = src.request_id;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.ExchangeSerialNo, src.exchange_serial_no);
  copy_field(dest.ExchangeProductID, src.exchange_product_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  dest.OrderSubmitStatus = src.order_submit_status;
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.InsertDate, src.insert_date);
  copy_field(dest.InsertTime, src.insert_time);
  copy_field(dest.CancelTime, src.cancel_time);
  dest.ExecResult = src.exec_result;
  dest.SequenceNo = src.sequence_no;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.ActiveUserID, src.active_user_id);
  dest.BrokerOffsetSettingSeq = src.broker_offset_setting_seq;
  dest.ApplySrc = src.apply_src;
}

void fill_option_instr_comm_rate(CThostFtdcOptionInstrCommRateField &dest,
                                 const ctp_option_instr_comm_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  dest.InvestorRange = src.investor_range;
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.OpenRatioByMoney = src.open_ratio_by_money;
  dest.OpenRatioByVolume = src.open_ratio_by_volume;
  dest.CloseRatioByMoney = src.close_ratio_by_money;
  dest.CloseRatioByVolume = src.close_ratio_by_volume;
  dest.CloseTodayRatioByMoney = src.close_today_ratio_by_money;
  dest.CloseTodayRatioByVolume = src.close_today_ratio_by_volume;
  dest.StrikeRatioByMoney = src.strike_ratio_by_money;
  dest.StrikeRatioByVolume = src.strike_ratio_by_volume;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_option_instr_trade_cost(CThostFtdcOptionInstrTradeCostField &dest,
                                  const ctp_option_instr_trade_cost &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.HedgeFlag = src.hedge_flag;
  dest.FixedMargin = src.fixed_margin;
  dest.MiniMargin = src.mini_margin;
  dest.Royalty = src.royalty;
  dest.ExchFixedMargin = src.exch_fixed_margin;
  dest.ExchMiniMargin = src.exch_mini_margin;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_option_self_close_action(CThostFtdcOptionSelfCloseActionField &dest,
                                   const ctp_option_self_close_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.OptionSelfCloseActionRef = src.option_self_close_action_ref;
  copy_field(dest.OptionSelfCloseRef, src.option_self_close_ref);
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OptionSelfCloseSysID, src.option_self_close_sys_id);
  dest.ActionFlag = src.action_flag;
  copy_field(dest.ActionDate, src.action_date);
  copy_field(dest.ActionTime, src.action_time);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  copy_field(dest.OptionSelfCloseLocalID, src.option_self_close_local_id);
  copy_field(dest.ActionLocalID, src.action_local_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.OrderActionStatus = src.order_action_status;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.BranchID, src.branch_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_option_self_close(CThostFtdcOptionSelfCloseField &dest,
                            const ctp_option_self_close &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.OptionSelfCloseRef, src.option_self_close_ref);
  copy_field(dest.UserID, src.user_id);
  dest.Volume = src.volume;
  dest.RequestID = src.request_id;
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.HedgeFlag = src.hedge_flag;
  dest.OptSelfCloseFlag = src.opt_self_close_flag;
  copy_field(dest.OptionSelfCloseLocalID, src.option_self_close_local_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  dest.OrderSubmitStatus = src.order_submit_status;
  dest.NotifySequence = src.notify_sequence;
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.OptionSelfCloseSysID, src.option_self_close_sys_id);
  copy_field(dest.InsertDate, src.insert_date);
  copy_field(dest.InsertTime, src.insert_time);
  copy_field(dest.CancelTime, src.cancel_time);
  dest.ExecResult = src.exec_result;
  copy_field(dest.ClearingPartID, src.clearing_part_id);
  dest.SequenceNo = src.sequence_no;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.ActiveUserID, src.active_user_id);
  dest.BrokerOptionSelfCloseSeq = src.broker_option_self_close_seq;
  copy_field(dest.BranchID, src.branch_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.reserve3, src.reserve3);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_order_action(CThostFtdcOrderActionField &dest,
                       const ctp_order_action &src) {
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
  copy_field(dest.ActionDate, src.action_date);
  copy_field(dest.ActionTime, src.action_time);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  copy_field(dest.OrderLocalID, src.order_local_id);
  copy_field(dest.ActionLocalID, src.action_local_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.OrderActionStatus = src.order_action_status;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.BranchID, src.branch_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.OrderMemo, src.order_memo);
  dest.SessionReqSeq = src.session_req_seq;
}

void fill_parked_order_action(CThostFtdcParkedOrderActionField &dest,
                              const ctp_parked_order_action &src) {
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
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ParkedOrderActionID, src.parked_order_action_id);
  dest.UserType = src.user_type;
  dest.Status = src.status;
  dest.ErrorID = src.error_id;
  copy_field(dest.ErrorMsg, src.error_msg);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_parked_order(CThostFtdcParkedOrderField &dest,
                       const ctp_parked_order &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
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
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ParkedOrderID, src.parked_order_id);
  dest.UserType = src.user_type;
  dest.Status = src.status;
  dest.ErrorID = src.error_id;
  copy_field(dest.ErrorMsg, src.error_msg);
  dest.IsSwapOrder = src.is_swap_order;
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_product_exch_rate(CThostFtdcProductExchRateField &dest,
                            const ctp_product_exch_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.QuoteCurrencyID, src.quote_currency_id);
  dest.ExchangeRate = src.exchange_rate;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductID, src.product_id);
}

void fill_product(CThostFtdcProductField &dest, const ctp_product &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ProductName, src.product_name);
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.ProductClass = src.product_class;
  dest.VolumeMultiple = src.volume_multiple;
  dest.PriceTick = src.price_tick;
  dest.MaxMarketOrderVolume = src.max_market_order_volume;
  dest.MinMarketOrderVolume = src.min_market_order_volume;
  dest.MaxLimitOrderVolume = src.max_limit_order_volume;
  dest.MinLimitOrderVolume = src.min_limit_order_volume;
  dest.PositionType = src.position_type;
  dest.PositionDateType = src.position_date_type;
  dest.CloseDealType = src.close_deal_type;
  copy_field(dest.TradeCurrencyID, src.trade_currency_id);
  dest.MortgageFundUseRange = src.mortgage_fund_use_range;
  copy_field(dest.reserve2, src.reserve2);
  dest.UnderlyingMultiple = src.underlying_multiple;
  copy_field(dest.ProductID, src.product_id);
  copy_field(dest.ExchangeProductID, src.exchange_product_id);
  dest.OpenLimitControlLevel = src.open_limit_control_level;
  dest.OrderFreqControlLevel = src.order_freq_control_level;
}

void fill_product_group(CThostFtdcProductGroupField &dest,
                        const ctp_product_group &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.ProductID, src.product_id);
  copy_field(dest.ProductGroupID, src.product_group_id);
}

void fill_qry_accountregister(CThostFtdcQryAccountregisterField &dest,
                              const ctp_qry_accountregister &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBranchID, src.bank_branch_id);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_qry_broker_trading_algos(CThostFtdcQryBrokerTradingAlgosField &dest,
                                   const ctp_qry_broker_trading_algos &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_broker_trading_params(CThostFtdcQryBrokerTradingParamsField &dest,
                                    const ctp_qry_broker_trading_params &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.AccountID, src.account_id);
}

void fill_qry_cfmmc_trading_account_key(
    CThostFtdcQryCFMMCTradingAccountKeyField &dest,
    const ctp_qry_cfmmc_trading_account_key &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
}

void fill_qry_classified_instrument(
    CThostFtdcQryClassifiedInstrumentField &dest,
    const ctp_qry_classified_instrument &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.ProductID, src.product_id);
  dest.TradingType = src.trading_type;
  dest.ClassType = src.class_type;
}

void fill_qry_comb_action(CThostFtdcQryCombActionField &dest,
                          const ctp_qry_comb_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_comb_instrument_guard(CThostFtdcQryCombInstrumentGuardField &dest,
                                    const ctp_qry_comb_instrument_guard &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_comb_leg(CThostFtdcQryCombLegField &dest,
                       const ctp_qry_comb_leg &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.LegInstrumentID, src.leg_instrument_id);
}

void fill_qry_comb_promotion_param(CThostFtdcQryCombPromotionParamField &dest,
                                   const ctp_qry_comb_promotion_param &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_contract_bank(CThostFtdcQryContractBankField &dest,
                            const ctp_qry_contract_bank &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBrchID, src.bank_brch_id);
}

void fill_qry_depth_market_data(CThostFtdcQryDepthMarketDataField &dest,
                                const ctp_qry_depth_market_data &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  dest.ProductClass = src.product_class;
}

void fill_qry_e_warrant_offset(CThostFtdcQryEWarrantOffsetField &dest,
                               const ctp_qry_e_warrant_offset &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_exchange(CThostFtdcQryExchangeField &dest,
                       const ctp_qry_exchange &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
}

void fill_qry_exchange_margin_rate_adjust(
    CThostFtdcQryExchangeMarginRateAdjustField &dest,
    const ctp_qry_exchange_margin_rate_adjust &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_exchange_rate(CThostFtdcQryExchangeRateField &dest,
                            const ctp_qry_exchange_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.FromCurrencyID, src.from_currency_id);
  copy_field(dest.ToCurrencyID, src.to_currency_id);
}

void fill_qry_exec_order(CThostFtdcQryExecOrderField &dest,
                         const ctp_qry_exec_order &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ExecOrderSysID, src.exec_order_sys_id);
  copy_field(dest.InsertTimeStart, src.insert_time_start);
  copy_field(dest.InsertTimeEnd, src.insert_time_end);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_for_quote(CThostFtdcQryForQuoteField &dest,
                        const ctp_qry_for_quote &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InsertTimeStart, src.insert_time_start);
  copy_field(dest.InsertTimeEnd, src.insert_time_end);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_hedge_cfm(CThostFtdcQryHedgeCfmField &dest,
                        const ctp_qry_hedge_cfm &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OrderSysID, src.order_sys_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_instrument(CThostFtdcQryInstrumentField &dest,
                         const ctp_qry_instrument &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.reserve3, src.reserve3);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.ProductID, src.product_id);
}

void fill_qry_instrument_order_comm_rate(
    CThostFtdcQryInstrumentOrderCommRateField &dest,
    const ctp_qry_instrument_order_comm_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_invest_unit(CThostFtdcQryInvestUnitField &dest,
                          const ctp_qry_invest_unit &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
}

void fill_qry_investor_commodity_group_spmm_margin(
    CThostFtdcQryInvestorCommodityGroupSPMMMarginField &dest,
    const ctp_qry_investor_commodity_group_spmm_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.CommodityGroupID, src.commodity_group_id);
}

void fill_qry_investor_commodity_spmm_margin(
    CThostFtdcQryInvestorCommoditySPMMMarginField &dest,
    const ctp_qry_investor_commodity_spmm_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.CommodityID, src.commodity_id);
}

void fill_qry_investor(CThostFtdcQryInvestorField &dest,
                       const ctp_qry_investor &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
}

void fill_qry_investor_info_comm_rec(
    CThostFtdcQryInvestorInfoCommRecField &dest,
    const ctp_qry_investor_info_comm_rec &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.BrokerID, src.broker_id);
}

void fill_qry_investor_portf_margin_ratio(
    CThostFtdcQryInvestorPortfMarginRatioField &dest,
    const ctp_qry_investor_portf_margin_ratio &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductGroupID, src.product_group_id);
}

void fill_qry_investor_portf_setting(
    CThostFtdcQryInvestorPortfSettingField &dest,
    const ctp_qry_investor_portf_setting &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
}

void fill_qry_investor_position_combine_detail(
    CThostFtdcQryInvestorPositionCombineDetailField &dest,
    const ctp_qry_investor_position_combine_detail &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.CombInstrumentID, src.comb_instrument_id);
}

void fill_qry_investor_position_detail(
    CThostFtdcQryInvestorPositionDetailField &dest,
    const ctp_qry_investor_position_detail &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_investor_prod_rcams_margin(
    CThostFtdcQryInvestorProdRCAMSMarginField &dest,
    const ctp_qry_investor_prod_rcams_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.CombProductID, src.comb_product_id);
  copy_field(dest.ProductGroupID, src.product_group_id);
}

void fill_qry_investor_prod_rule_margin(
    CThostFtdcQryInvestorProdRULEMarginField &dest,
    const ctp_qry_investor_prod_rule_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
  dest.CommodityGroupID = src.commodity_group_id;
}

void fill_qry_investor_prod_spbm_detail(
    CThostFtdcQryInvestorProdSPBMDetailField &dest,
    const ctp_qry_investor_prod_spbm_detail &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
}

void fill_qry_investor_product_group_margin(
    CThostFtdcQryInvestorProductGroupMarginField &dest,
    const ctp_qry_investor_product_group_margin &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.HedgeFlag = src.hedge_flag;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.ProductGroupID, src.product_group_id);
}

void fill_qry_mm_instrument_commission_rate(
    CThostFtdcQryMMInstrumentCommissionRateField &dest,
    const ctp_qry_mm_instrument_commission_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_mm_option_instr_comm_rate(
    CThostFtdcQryMMOptionInstrCommRateField &dest,
    const ctp_qry_mm_option_instr_comm_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_max_order_volume(CThostFtdcQryMaxOrderVolumeField &dest,
                               const ctp_qry_max_order_volume &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.Direction = src.direction;
  dest.OffsetFlag = src.offset_flag;
  dest.HedgeFlag = src.hedge_flag;
  dest.MaxVolume = src.max_volume;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_notice(CThostFtdcQryNoticeField &dest,
                     const ctp_qry_notice &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
}

void fill_qry_offset_setting(CThostFtdcQryOffsetSettingField &dest,
                             const ctp_qry_offset_setting &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ProductID, src.product_id);
  dest.OffsetType = src.offset_type;
}

void fill_qry_option_instr_comm_rate(
    CThostFtdcQryOptionInstrCommRateField &dest,
    const ctp_qry_option_instr_comm_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_option_instr_trade_cost(
    CThostFtdcQryOptionInstrTradeCostField &dest,
    const ctp_qry_option_instr_trade_cost &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  dest.HedgeFlag = src.hedge_flag;
  dest.InputPrice = src.input_price;
  dest.UnderlyingPrice = src.underlying_price;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_option_self_close(CThostFtdcQryOptionSelfCloseField &dest,
                                const ctp_qry_option_self_close &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OptionSelfCloseSysID, src.option_self_close_sys_id);
  copy_field(dest.InsertTimeStart, src.insert_time_start);
  copy_field(dest.InsertTimeEnd, src.insert_time_end);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_order(CThostFtdcQryOrderField &dest, const ctp_qry_order &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OrderSysID, src.order_sys_id);
  copy_field(dest.InsertTimeStart, src.insert_time_start);
  copy_field(dest.InsertTimeEnd, src.insert_time_end);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_parked_order_action(CThostFtdcQryParkedOrderActionField &dest,
                                  const ctp_qry_parked_order_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_parked_order(CThostFtdcQryParkedOrderField &dest,
                           const ctp_qry_parked_order &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_product_exch_rate(CThostFtdcQryProductExchRateField &dest,
                                const ctp_qry_product_exch_rate &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductID, src.product_id);
}

void fill_qry_product(CThostFtdcQryProductField &dest,
                      const ctp_qry_product &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  dest.ProductClass = src.product_class;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductID, src.product_id);
}

void fill_qry_product_group(CThostFtdcQryProductGroupField &dest,
                            const ctp_qry_product_group &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductID, src.product_id);
}

void fill_qry_quote(CThostFtdcQryQuoteField &dest, const ctp_qry_quote &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.QuoteSysID, src.quote_sys_id);
  copy_field(dest.InsertTimeStart, src.insert_time_start);
  copy_field(dest.InsertTimeEnd, src.insert_time_end);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_rcams_comb_product_info(
    CThostFtdcQryRCAMSCombProductInfoField &dest,
    const ctp_qry_rcams_comb_product_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ProductID, src.product_id);
  copy_field(dest.CombProductID, src.comb_product_id);
  copy_field(dest.ProductGroupID, src.product_group_id);
}

void fill_qry_rcams_instr_parameter(CThostFtdcQryRCAMSInstrParameterField &dest,
                                    const ctp_qry_rcams_instr_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ProductID, src.product_id);
}

void fill_qry_rcams_inter_parameter(CThostFtdcQryRCAMSInterParameterField &dest,
                                    const ctp_qry_rcams_inter_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ProductGroupID, src.product_group_id);
  copy_field(dest.CombProduct1, src.comb_product1);
  copy_field(dest.CombProduct2, src.comb_product2);
}

void fill_qry_rcams_intra_parameter(CThostFtdcQryRCAMSIntraParameterField &dest,
                                    const ctp_qry_rcams_intra_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.CombProductID, src.comb_product_id);
}

void fill_qry_rcams_investor_comb_position(
    CThostFtdcQryRCAMSInvestorCombPositionField &dest,
    const ctp_qry_rcams_investor_comb_position &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.CombInstrumentID, src.comb_instrument_id);
}

void fill_qry_rcams_short_opt_adjust_param(
    CThostFtdcQryRCAMSShortOptAdjustParamField &dest,
    const ctp_qry_rcams_short_opt_adjust_param &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.CombProductID, src.comb_product_id);
}

void fill_qry_rule_instr_parameter(CThostFtdcQryRULEInstrParameterField &dest,
                                   const ctp_qry_rule_instr_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_rule_inter_parameter(CThostFtdcQryRULEInterParameterField &dest,
                                   const ctp_qry_rule_inter_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.Leg1ProdFamilyCode, src.leg1_prod_family_code);
  copy_field(dest.Leg2ProdFamilyCode, src.leg2_prod_family_code);
  dest.CommodityGroupID = src.commodity_group_id;
}

void fill_qry_rule_intra_parameter(CThostFtdcQryRULEIntraParameterField &dest,
                                   const ctp_qry_rule_intra_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
}

void fill_qry_risk_settle_invst_position(
    CThostFtdcQryRiskSettleInvstPositionField &dest,
    const ctp_qry_risk_settle_invst_position &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_risk_settle_product_status(
    CThostFtdcQryRiskSettleProductStatusField &dest,
    const ctp_qry_risk_settle_product_status &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ProductID, src.product_id);
}

void fill_qry_spbm_add_on_inter_parameter(
    CThostFtdcQrySPBMAddOnInterParameterField &dest,
    const ctp_qry_spbm_add_on_inter_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.Leg1ProdFamilyCode, src.leg1_prod_family_code);
  copy_field(dest.Leg2ProdFamilyCode, src.leg2_prod_family_code);
}

void fill_qry_spbm_future_parameter(CThostFtdcQrySPBMFutureParameterField &dest,
                                    const ctp_qry_spbm_future_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
}

void fill_qry_spbm_inter_parameter(CThostFtdcQrySPBMInterParameterField &dest,
                                   const ctp_qry_spbm_inter_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.Leg1ProdFamilyCode, src.leg1_prod_family_code);
  copy_field(dest.Leg2ProdFamilyCode, src.leg2_prod_family_code);
}

void fill_qry_spbm_intra_parameter(CThostFtdcQrySPBMIntraParameterField &dest,
                                   const ctp_qry_spbm_intra_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
}

void fill_qry_spbm_investor_portf_def(
    CThostFtdcQrySPBMInvestorPortfDefField &dest,
    const ctp_qry_spbm_investor_portf_def &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
}

void fill_qry_spbm_option_parameter(CThostFtdcQrySPBMOptionParameterField &dest,
                                    const ctp_qry_spbm_option_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
}

void fill_qry_spbm_portf_definition(CThostFtdcQrySPBMPortfDefinitionField &dest,
                                    const ctp_qry_spbm_portf_definition &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.PortfolioDefID = src.portfolio_def_id;
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
}

void fill_qry_spmm_inst_param(CThostFtdcQrySPMMInstParamField &dest,
                              const ctp_qry_spmm_inst_param &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_spmm_product_param(CThostFtdcQrySPMMProductParamField &dest,
                                 const ctp_qry_spmm_product_param &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ProductID, src.product_id);
}

void fill_qry_sec_agent_ac_id_map(CThostFtdcQrySecAgentACIDMapField &dest,
                                  const ctp_qry_sec_agent_ac_id_map &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_qry_sec_agent_check_mode(CThostFtdcQrySecAgentCheckModeField &dest,
                                   const ctp_qry_sec_agent_check_mode &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
}

void fill_qry_sec_agent_trade_info(CThostFtdcQrySecAgentTradeInfoField &dest,
                                   const ctp_qry_sec_agent_trade_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BrokerSecAgentID, src.broker_sec_agent_id);
}

void fill_qry_settlement_info_confirm(
    CThostFtdcQrySettlementInfoConfirmField &dest,
    const ctp_qry_settlement_info_confirm &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_qry_settlement_info(CThostFtdcQrySettlementInfoField &dest,
                              const ctp_qry_settlement_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_qry_spd_apply(CThostFtdcQrySpdApplyField &dest,
                        const ctp_qry_spd_apply &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OrderSysID, src.order_sys_id);
  copy_field(dest.FirstLegInstrumentID, src.first_leg_instrument_id);
  copy_field(dest.SecondLegInstrumentID, src.second_leg_instrument_id);
}

void fill_qry_trade(CThostFtdcQryTradeField &dest, const ctp_qry_trade &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.TradeID, src.trade_id);
  copy_field(dest.TradeTimeStart, src.trade_time_start);
  copy_field(dest.TradeTimeEnd, src.trade_time_end);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.InstrumentID, src.instrument_id);
}

void fill_qry_trader_offer(CThostFtdcQryTraderOfferField &dest,
                           const ctp_qry_trader_offer &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.TraderID, src.trader_id);
}

void fill_qry_trading_code(CThostFtdcQryTradingCodeField &dest,
                           const ctp_qry_trading_code &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ClientID, src.client_id);
  dest.ClientIDType = src.client_id_type;
  copy_field(dest.InvestUnitID, src.invest_unit_id);
}

void fill_qry_trading_notice(CThostFtdcQryTradingNoticeField &dest,
                             const ctp_qry_trading_notice &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
}

void fill_qry_transfer_bank(CThostFtdcQryTransferBankField &dest,
                            const ctp_qry_transfer_bank &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBrchID, src.bank_brch_id);
}

void fill_qry_transfer_serial(CThostFtdcQryTransferSerialField &dest,
                              const ctp_qry_transfer_serial &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_qry_user_session(CThostFtdcQryUserSessionField &dest,
                           const ctp_qry_user_session &src) {
  std::memset(&dest, 0, sizeof(dest));
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
}

void fill_query_cfmmc_trading_account_token(
    CThostFtdcQueryCFMMCTradingAccountTokenField &dest,
    const ctp_query_cfmmc_trading_account_token &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
}

void fill_quote_action(CThostFtdcQuoteActionField &dest,
                       const ctp_quote_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.QuoteActionRef = src.quote_action_ref;
  copy_field(dest.QuoteRef, src.quote_ref);
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.QuoteSysID, src.quote_sys_id);
  dest.ActionFlag = src.action_flag;
  copy_field(dest.ActionDate, src.action_date);
  copy_field(dest.ActionTime, src.action_time);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  copy_field(dest.QuoteLocalID, src.quote_local_id);
  copy_field(dest.ActionLocalID, src.action_local_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.OrderActionStatus = src.order_action_status;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.BranchID, src.branch_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.OrderMemo, src.order_memo);
  dest.SessionReqSeq = src.session_req_seq;
}

void fill_quote(CThostFtdcQuoteField &dest, const ctp_quote &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.QuoteRef, src.quote_ref);
  copy_field(dest.UserID, src.user_id);
  dest.AskPrice = src.ask_price;
  dest.BidPrice = src.bid_price;
  dest.AskVolume = src.ask_volume;
  dest.BidVolume = src.bid_volume;
  dest.RequestID = src.request_id;
  copy_field(dest.BusinessUnit, src.business_unit);
  dest.AskOffsetFlag = src.ask_offset_flag;
  dest.BidOffsetFlag = src.bid_offset_flag;
  dest.AskHedgeFlag = src.ask_hedge_flag;
  dest.BidHedgeFlag = src.bid_hedge_flag;
  copy_field(dest.QuoteLocalID, src.quote_local_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.reserve2, src.reserve2);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  dest.NotifySequence = src.notify_sequence;
  dest.OrderSubmitStatus = src.order_submit_status;
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.QuoteSysID, src.quote_sys_id);
  copy_field(dest.InsertDate, src.insert_date);
  copy_field(dest.InsertTime, src.insert_time);
  copy_field(dest.CancelTime, src.cancel_time);
  dest.QuoteStatus = src.quote_status;
  copy_field(dest.ClearingPartID, src.clearing_part_id);
  dest.SequenceNo = src.sequence_no;
  copy_field(dest.AskOrderSysID, src.ask_order_sys_id);
  copy_field(dest.BidOrderSysID, src.bid_order_sys_id);
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.ActiveUserID, src.active_user_id);
  dest.BrokerQuoteSeq = src.broker_quote_seq;
  copy_field(dest.AskOrderRef, src.ask_order_ref);
  copy_field(dest.BidOrderRef, src.bid_order_ref);
  copy_field(dest.ForQuoteSysID, src.for_quote_sys_id);
  copy_field(dest.BranchID, src.branch_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.reserve3, src.reserve3);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.ReplaceSysID, src.replace_sys_id);
  dest.TimeCondition = src.time_condition;
  copy_field(dest.OrderMemo, src.order_memo);
  dest.SessionReqSeq = src.session_req_seq;
}

void fill_rcams_comb_product_info(CThostFtdcRCAMSCombProductInfoField &dest,
                                  const ctp_rcams_comb_product_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductID, src.product_id);
  copy_field(dest.CombProductID, src.comb_product_id);
  copy_field(dest.ProductGroupID, src.product_group_id);
}

void fill_rcams_instr_parameter(CThostFtdcRCAMSInstrParameterField &dest,
                                const ctp_rcams_instr_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductID, src.product_id);
  dest.HedgeRate = src.hedge_rate;
}

void fill_rcams_inter_parameter(CThostFtdcRCAMSInterParameterField &dest,
                                const ctp_rcams_inter_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductGroupID, src.product_group_id);
  dest.Priority = src.priority;
  dest.CreditRate = src.credit_rate;
  copy_field(dest.CombProduct1, src.comb_product1);
  copy_field(dest.CombProduct2, src.comb_product2);
}

void fill_rcams_intra_parameter(CThostFtdcRCAMSIntraParameterField &dest,
                                const ctp_rcams_intra_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.CombProductID, src.comb_product_id);
  dest.HedgeRate = src.hedge_rate;
}

void fill_rcams_investor_comb_position(
    CThostFtdcRCAMSInvestorCombPositionField &dest,
    const ctp_rcams_investor_comb_position &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  dest.HedgeFlag = src.hedge_flag;
  dest.PosiDirection = src.posi_direction;
  copy_field(dest.CombInstrumentID, src.comb_instrument_id);
  dest.LegID = src.leg_id;
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  dest.TotalAmt = src.total_amt;
  dest.ExchMargin = src.exch_margin;
  dest.Margin = src.margin;
}

void fill_rcams_short_opt_adjust_param(
    CThostFtdcRCAMSShortOptAdjustParamField &dest,
    const ctp_rcams_short_opt_adjust_param &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.CombProductID, src.comb_product_id);
  dest.HedgeFlag = src.hedge_flag;
  dest.AdjustValue = src.adjust_value;
}

void fill_rule_instr_parameter(CThostFtdcRULEInstrParameterField &dest,
                               const ctp_rule_instr_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  dest.InstrumentClass = src.instrument_class;
  copy_field(dest.StdInstrumentID, src.std_instrument_id);
  dest.BSpecRatio = src.b_spec_ratio;
  dest.SSpecRatio = src.s_spec_ratio;
  dest.BHedgeRatio = src.b_hedge_ratio;
  dest.SHedgeRatio = src.s_hedge_ratio;
  dest.BAddOnMargin = src.b_add_on_margin;
  dest.SAddOnMargin = src.s_add_on_margin;
  dest.CommodityGroupID = src.commodity_group_id;
}

void fill_rule_inter_parameter(CThostFtdcRULEInterParameterField &dest,
                               const ctp_rule_inter_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.SpreadId = src.spread_id;
  dest.InterRate = src.inter_rate;
  copy_field(dest.Leg1ProdFamilyCode, src.leg1_prod_family_code);
  copy_field(dest.Leg2ProdFamilyCode, src.leg2_prod_family_code);
  dest.Leg1PropFactor = src.leg1_prop_factor;
  dest.Leg2PropFactor = src.leg2_prop_factor;
  dest.CommodityGroupID = src.commodity_group_id;
  copy_field(dest.CommodityGroupName, src.commodity_group_name);
}

void fill_rule_intra_parameter(CThostFtdcRULEIntraParameterField &dest,
                               const ctp_rule_intra_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
  copy_field(dest.StdInstrumentID, src.std_instrument_id);
  dest.StdInstrMargin = src.std_instr_margin;
  dest.UsualIntraRate = src.usual_intra_rate;
  dest.DeliveryIntraRate = src.delivery_intra_rate;
}

void fill_remove_parked_order_action(
    CThostFtdcRemoveParkedOrderActionField &dest,
    const ctp_remove_parked_order_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ParkedOrderActionID, src.parked_order_action_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
}

void fill_remove_parked_order(CThostFtdcRemoveParkedOrderField &dest,
                              const ctp_remove_parked_order &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ParkedOrderID, src.parked_order_id);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
}

void fill_req_gen_sms_code(CThostFtdcReqGenSMSCodeField &dest,
                           const ctp_req_gen_sms_code &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.Mobile, src.mobile);
}

void fill_req_gen_user_captcha(CThostFtdcReqGenUserCaptchaField &dest,
                               const ctp_req_gen_user_captcha &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
}

void fill_req_gen_user_text(CThostFtdcReqGenUserTextField &dest,
                            const ctp_req_gen_user_text &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
}

void fill_req_query_account(CThostFtdcReqQueryAccountField &dest,
                            const ctp_req_query_account &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradeCode, src.trade_code);
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBranchID, src.bank_branch_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BrokerBranchID, src.broker_branch_id);
  copy_field(dest.TradeDate, src.trade_date);
  copy_field(dest.TradeTime, src.trade_time);
  copy_field(dest.BankSerial, src.bank_serial);
  copy_field(dest.TradingDay, src.trading_day);
  dest.PlateSerial = src.plate_serial;
  dest.LastFragment = src.last_fragment;
  dest.SessionID = src.session_id;
  copy_field(dest.CustomerName, src.customer_name);
  dest.IdCardType = src.id_card_type;
  copy_field(dest.IdentifiedCardNo, src.identified_card_no);
  dest.CustType = src.cust_type;
  copy_field(dest.BankAccount, src.bank_account);
  copy_field(dest.BankPassWord, src.bank_pass_word);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.Password, src.password);
  dest.FutureSerial = src.future_serial;
  dest.InstallID = src.install_id;
  copy_field(dest.UserID, src.user_id);
  dest.VerifyCertNoFlag = src.verify_cert_no_flag;
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.Digest, src.digest);
  dest.BankAccType = src.bank_acc_type;
  copy_field(dest.DeviceID, src.device_id);
  dest.BankSecuAccType = src.bank_secu_acc_type;
  copy_field(dest.BrokerIDByBank, src.broker_id_by_bank);
  copy_field(dest.BankSecuAcc, src.bank_secu_acc);
  dest.BankPwdFlag = src.bank_pwd_flag;
  dest.SecuPwdFlag = src.secu_pwd_flag;
  copy_field(dest.OperNo, src.oper_no);
  dest.RequestID = src.request_id;
  dest.TID = src.t_id;
  copy_field(dest.LongCustomerName, src.long_customer_name);
}

void fill_req_transfer(CThostFtdcReqTransferField &dest,
                       const ctp_req_transfer &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradeCode, src.trade_code);
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBranchID, src.bank_branch_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BrokerBranchID, src.broker_branch_id);
  copy_field(dest.TradeDate, src.trade_date);
  copy_field(dest.TradeTime, src.trade_time);
  copy_field(dest.BankSerial, src.bank_serial);
  copy_field(dest.TradingDay, src.trading_day);
  dest.PlateSerial = src.plate_serial;
  dest.LastFragment = src.last_fragment;
  dest.SessionID = src.session_id;
  copy_field(dest.CustomerName, src.customer_name);
  dest.IdCardType = src.id_card_type;
  copy_field(dest.IdentifiedCardNo, src.identified_card_no);
  dest.CustType = src.cust_type;
  copy_field(dest.BankAccount, src.bank_account);
  copy_field(dest.BankPassWord, src.bank_pass_word);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.Password, src.password);
  dest.InstallID = src.install_id;
  dest.FutureSerial = src.future_serial;
  copy_field(dest.UserID, src.user_id);
  dest.VerifyCertNoFlag = src.verify_cert_no_flag;
  copy_field(dest.CurrencyID, src.currency_id);
  dest.TradeAmount = src.trade_amount;
  dest.FutureFetchAmount = src.future_fetch_amount;
  dest.FeePayFlag = src.fee_pay_flag;
  dest.CustFee = src.cust_fee;
  dest.BrokerFee = src.broker_fee;
  copy_field(dest.Message, src.message);
  copy_field(dest.Digest, src.digest);
  dest.BankAccType = src.bank_acc_type;
  copy_field(dest.DeviceID, src.device_id);
  dest.BankSecuAccType = src.bank_secu_acc_type;
  copy_field(dest.BrokerIDByBank, src.broker_id_by_bank);
  copy_field(dest.BankSecuAcc, src.bank_secu_acc);
  dest.BankPwdFlag = src.bank_pwd_flag;
  dest.SecuPwdFlag = src.secu_pwd_flag;
  copy_field(dest.OperNo, src.oper_no);
  dest.RequestID = src.request_id;
  dest.TID = src.t_id;
  dest.TransferStatus = src.transfer_status;
  copy_field(dest.LongCustomerName, src.long_customer_name);
}

void fill_req_user_auth_method(CThostFtdcReqUserAuthMethodField &dest,
                               const ctp_req_user_auth_method &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
}

void fill_req_user_login_with_captcha(
    CThostFtdcReqUserLoginWithCaptchaField &dest,
    const ctp_req_user_login_with_captcha &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.Password, src.password);
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.InterfaceProductInfo, src.interface_product_info);
  copy_field(dest.ProtocolInfo, src.protocol_info);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.LoginRemark, src.login_remark);
  copy_field(dest.Captcha, src.captcha);
  dest.ClientIPPort = src.client_ip_port;
  copy_field(dest.ClientIPAddress, src.client_ip_address);
}

void fill_req_user_login_with_otp(CThostFtdcReqUserLoginWithOTPField &dest,
                                  const ctp_req_user_login_with_otp &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.Password, src.password);
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.InterfaceProductInfo, src.interface_product_info);
  copy_field(dest.ProtocolInfo, src.protocol_info);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.LoginRemark, src.login_remark);
  copy_field(dest.OTPPassword, src.otp_password);
  dest.ClientIPPort = src.client_ip_port;
  copy_field(dest.ClientIPAddress, src.client_ip_address);
}

void fill_req_user_login_with_text(CThostFtdcReqUserLoginWithTextField &dest,
                                   const ctp_req_user_login_with_text &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.Password, src.password);
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.InterfaceProductInfo, src.interface_product_info);
  copy_field(dest.ProtocolInfo, src.protocol_info);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.LoginRemark, src.login_remark);
  copy_field(dest.Text, src.text);
  dest.ClientIPPort = src.client_ip_port;
  copy_field(dest.ClientIPAddress, src.client_ip_address);
}

void fill_risk_settle_invst_position(
    CThostFtdcRiskSettleInvstPositionField &dest,
    const ctp_risk_settle_invst_position &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.PosiDirection = src.posi_direction;
  dest.HedgeFlag = src.hedge_flag;
  dest.PositionDate = src.position_date;
  dest.YdPosition = src.yd_position;
  dest.Position = src.position;
  dest.LongFrozen = src.long_frozen;
  dest.ShortFrozen = src.short_frozen;
  dest.LongFrozenAmount = src.long_frozen_amount;
  dest.ShortFrozenAmount = src.short_frozen_amount;
  dest.OpenVolume = src.open_volume;
  dest.CloseVolume = src.close_volume;
  dest.OpenAmount = src.open_amount;
  dest.CloseAmount = src.close_amount;
  dest.PositionCost = src.position_cost;
  dest.PreMargin = src.pre_margin;
  dest.UseMargin = src.use_margin;
  dest.FrozenMargin = src.frozen_margin;
  dest.FrozenCash = src.frozen_cash;
  dest.FrozenCommission = src.frozen_commission;
  dest.CashIn = src.cash_in;
  dest.Commission = src.commission;
  dest.CloseProfit = src.close_profit;
  dest.PositionProfit = src.position_profit;
  dest.PreSettlementPrice = src.pre_settlement_price;
  dest.SettlementPrice = src.settlement_price;
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  dest.OpenCost = src.open_cost;
  dest.ExchangeMargin = src.exchange_margin;
  dest.CombPosition = src.comb_position;
  dest.CombLongFrozen = src.comb_long_frozen;
  dest.CombShortFrozen = src.comb_short_frozen;
  dest.CloseProfitByDate = src.close_profit_by_date;
  dest.CloseProfitByTrade = src.close_profit_by_trade;
  dest.TodayPosition = src.today_position;
  dest.MarginRateByMoney = src.margin_rate_by_money;
  dest.MarginRateByVolume = src.margin_rate_by_volume;
  dest.StrikeFrozen = src.strike_frozen;
  dest.StrikeFrozenAmount = src.strike_frozen_amount;
  dest.AbandonFrozen = src.abandon_frozen;
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.YdStrikeFrozen = src.yd_strike_frozen;
  copy_field(dest.InvestUnitID, src.invest_unit_id);
  dest.PositionCostOffset = src.position_cost_offset;
  dest.TasPosition = src.tas_position;
  dest.TasPositionCost = src.tas_position_cost;
}

void fill_risk_settle_product_status(
    CThostFtdcRiskSettleProductStatusField &dest,
    const ctp_risk_settle_product_status &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductID, src.product_id);
  dest.ProductStatus = src.product_status;
}

void fill_rsp_gen_sms_code(CThostFtdcRspGenSMSCodeField &dest,
                           const ctp_rsp_gen_sms_code &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.GenTime, src.gen_time);
}

void fill_rsp_gen_user_captcha(CThostFtdcRspGenUserCaptchaField &dest,
                               const ctp_rsp_gen_user_captcha &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  dest.CaptchaInfoLen = src.captcha_info_len;
  copy_field(dest.CaptchaInfo, src.captcha_info);
}

void fill_rsp_gen_user_text(CThostFtdcRspGenUserTextField &dest,
                            const ctp_rsp_gen_user_text &src) {
  std::memset(&dest, 0, sizeof(dest));
  dest.UserTextSeq = src.user_text_seq;
}

void fill_rsp_transfer(CThostFtdcRspTransferField &dest,
                       const ctp_rsp_transfer &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradeCode, src.trade_code);
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBranchID, src.bank_branch_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BrokerBranchID, src.broker_branch_id);
  copy_field(dest.TradeDate, src.trade_date);
  copy_field(dest.TradeTime, src.trade_time);
  copy_field(dest.BankSerial, src.bank_serial);
  copy_field(dest.TradingDay, src.trading_day);
  dest.PlateSerial = src.plate_serial;
  dest.LastFragment = src.last_fragment;
  dest.SessionID = src.session_id;
  copy_field(dest.CustomerName, src.customer_name);
  dest.IdCardType = src.id_card_type;
  copy_field(dest.IdentifiedCardNo, src.identified_card_no);
  dest.CustType = src.cust_type;
  copy_field(dest.BankAccount, src.bank_account);
  copy_field(dest.BankPassWord, src.bank_pass_word);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.Password, src.password);
  dest.InstallID = src.install_id;
  dest.FutureSerial = src.future_serial;
  copy_field(dest.UserID, src.user_id);
  dest.VerifyCertNoFlag = src.verify_cert_no_flag;
  copy_field(dest.CurrencyID, src.currency_id);
  dest.TradeAmount = src.trade_amount;
  dest.FutureFetchAmount = src.future_fetch_amount;
  dest.FeePayFlag = src.fee_pay_flag;
  dest.CustFee = src.cust_fee;
  dest.BrokerFee = src.broker_fee;
  copy_field(dest.Message, src.message);
  copy_field(dest.Digest, src.digest);
  dest.BankAccType = src.bank_acc_type;
  copy_field(dest.DeviceID, src.device_id);
  dest.BankSecuAccType = src.bank_secu_acc_type;
  copy_field(dest.BrokerIDByBank, src.broker_id_by_bank);
  copy_field(dest.BankSecuAcc, src.bank_secu_acc);
  dest.BankPwdFlag = src.bank_pwd_flag;
  dest.SecuPwdFlag = src.secu_pwd_flag;
  copy_field(dest.OperNo, src.oper_no);
  dest.RequestID = src.request_id;
  dest.TID = src.t_id;
  dest.TransferStatus = src.transfer_status;
  dest.ErrorID = src.error_id;
  copy_field(dest.ErrorMsg, src.error_msg);
  copy_field(dest.LongCustomerName, src.long_customer_name);
}

void fill_rsp_user_auth_method(CThostFtdcRspUserAuthMethodField &dest,
                               const ctp_rsp_user_auth_method &src) {
  std::memset(&dest, 0, sizeof(dest));
  dest.UsableAuthMethod = src.usable_auth_method;
}

void fill_spbm_add_on_inter_parameter(
    CThostFtdcSPBMAddOnInterParameterField &dest,
    const ctp_spbm_add_on_inter_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.SpreadId = src.spread_id;
  dest.AddOnInterRateZ2 = src.add_on_inter_rate_z2;
  copy_field(dest.Leg1ProdFamilyCode, src.leg1_prod_family_code);
  copy_field(dest.Leg2ProdFamilyCode, src.leg2_prod_family_code);
}

void fill_spbm_future_parameter(CThostFtdcSPBMFutureParameterField &dest,
                                const ctp_spbm_future_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
  dest.Cvf = src.cvf;
  dest.TimeRange = src.time_range;
  dest.MarginRate = src.margin_rate;
  dest.LockRateX = src.lock_rate_x;
  dest.AddOnRate = src.add_on_rate;
  dest.PreSettlementPrice = src.pre_settlement_price;
  dest.AddOnLockRateX2 = src.add_on_lock_rate_x2;
}

void fill_spbm_inter_parameter(CThostFtdcSPBMInterParameterField &dest,
                               const ctp_spbm_inter_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.SpreadId = src.spread_id;
  dest.InterRateZ = src.inter_rate_z;
  copy_field(dest.Leg1ProdFamilyCode, src.leg1_prod_family_code);
  copy_field(dest.Leg2ProdFamilyCode, src.leg2_prod_family_code);
}

void fill_spbm_intra_parameter(CThostFtdcSPBMIntraParameterField &dest,
                               const ctp_spbm_intra_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
  dest.IntraRateY = src.intra_rate_y;
  dest.AddOnIntraRateY2 = src.add_on_intra_rate_y2;
}

void fill_spbm_investor_portf_def(CThostFtdcSPBMInvestorPortfDefField &dest,
                                  const ctp_spbm_investor_portf_def &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.PortfolioDefID = src.portfolio_def_id;
}

void fill_spbm_option_parameter(CThostFtdcSPBMOptionParameterField &dest,
                                const ctp_spbm_option_parameter &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
  dest.Cvf = src.cvf;
  dest.DownPrice = src.down_price;
  dest.Delta = src.delta;
  dest.SlimiDelta = src.slimi_delta;
  dest.PreSettlementPrice = src.pre_settlement_price;
}

void fill_spbm_portf_definition(CThostFtdcSPBMPortfDefinitionField &dest,
                                const ctp_spbm_portf_definition &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  dest.PortfolioDefID = src.portfolio_def_id;
  copy_field(dest.ProdFamilyCode, src.prod_family_code);
  dest.IsSPBM = src.is_spbm;
}

void fill_spmm_inst_param(CThostFtdcSPMMInstParamField &dest,
                          const ctp_spmm_inst_param &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.InstrumentID, src.instrument_id);
  dest.InstMarginCalID = src.inst_margin_cal_id;
  copy_field(dest.CommodityID, src.commodity_id);
  copy_field(dest.CommodityGroupID, src.commodity_group_id);
}

void fill_spmm_product_param(CThostFtdcSPMMProductParamField &dest,
                             const ctp_spmm_product_param &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ProductID, src.product_id);
  copy_field(dest.CommodityID, src.commodity_id);
  copy_field(dest.CommodityGroupID, src.commodity_group_id);
}

void fill_sec_agent_ac_id_map(CThostFtdcSecAgentACIDMapField &dest,
                              const ctp_sec_agent_ac_id_map &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.BrokerSecAgentID, src.broker_sec_agent_id);
}

void fill_sec_agent_check_mode(CThostFtdcSecAgentCheckModeField &dest,
                               const ctp_sec_agent_check_mode &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.CurrencyID, src.currency_id);
  copy_field(dest.BrokerSecAgentID, src.broker_sec_agent_id);
  dest.CheckSelfAccount = src.check_self_account;
}

void fill_sec_agent_trade_info(CThostFtdcSecAgentTradeInfoField &dest,
                               const ctp_sec_agent_trade_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BrokerSecAgentID, src.broker_sec_agent_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.LongCustomerName, src.long_customer_name);
}

void fill_settlement_info(CThostFtdcSettlementInfoField &dest,
                          const ctp_settlement_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.SequenceNo = src.sequence_no;
  copy_field(dest.Content, src.content);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_spd_apply_action(CThostFtdcSpdApplyActionField &dest,
                           const ctp_spd_apply_action &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.ActionDate, src.action_date);
  copy_field(dest.ActionTime, src.action_time);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  copy_field(dest.OrderLocalID, src.order_local_id);
  copy_field(dest.ActionLocalID, src.action_local_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  dest.OrderActionStatus = src.order_action_status;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.OrderSysID, src.order_sys_id);
  dest.RequestID = src.request_id;
  copy_field(dest.StatusMsg, src.status_msg);
  copy_field(dest.OrderRef, src.order_ref);
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
}

void fill_spd_apply(CThostFtdcSpdApplyField &dest, const ctp_spd_apply &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.FirstLegInstrumentID, src.first_leg_instrument_id);
  copy_field(dest.SecondLegInstrumentID, src.second_leg_instrument_id);
  copy_field(dest.UserID, src.user_id);
  dest.Volume = src.volume;
  dest.Direction = src.direction;
  dest.RequestID = src.request_id;
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.OrderRef, src.order_ref);
  copy_field(dest.ActiveUserID, src.active_user_id);
  dest.BrokerOrderSeq = src.broker_order_seq;
  copy_field(dest.OrderSysID, src.order_sys_id);
  dest.ApplyStatus = src.apply_status;
  dest.SequenceNo = src.sequence_no;
  copy_field(dest.InsertDate, src.insert_date);
  copy_field(dest.InsertTime, src.insert_time);
  copy_field(dest.CancelTime, src.cancel_time);
  copy_field(dest.OrderLocalID, src.order_local_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.ClientID, src.client_id);
  copy_field(dest.ExchangeInstID, src.exchange_inst_id);
  copy_field(dest.TraderID, src.trader_id);
  dest.InstallID = src.install_id;
  dest.OrderSubmitStatus = src.order_submit_status;
  dest.NotifySequence = src.notify_sequence;
  copy_field(dest.TradingDay, src.trading_day);
  dest.SettlementID = src.settlement_id;
  copy_field(dest.IPAddress, src.ip_address);
  copy_field(dest.MacAddress, src.mac_address);
  dest.CmbType = src.cmb_type;
  copy_field(dest.StatusMsg, src.status_msg);
}

void fill_trader_offer(CThostFtdcTraderOfferField &dest,
                       const ctp_trader_offer &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.TraderID, src.trader_id);
  copy_field(dest.ParticipantID, src.participant_id);
  copy_field(dest.Password, src.password);
  dest.InstallID = src.install_id;
  copy_field(dest.OrderLocalID, src.order_local_id);
  dest.TraderConnectStatus = src.trader_connect_status;
  copy_field(dest.ConnectRequestDate, src.connect_request_date);
  copy_field(dest.ConnectRequestTime, src.connect_request_time);
  copy_field(dest.LastReportDate, src.last_report_date);
  copy_field(dest.LastReportTime, src.last_report_time);
  copy_field(dest.ConnectDate, src.connect_date);
  copy_field(dest.ConnectTime, src.connect_time);
  copy_field(dest.StartDate, src.start_date);
  copy_field(dest.StartTime, src.start_time);
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.MaxTradeID, src.max_trade_id);
  copy_field(dest.MaxOrderMessageReference, src.max_order_message_reference);
  dest.OrderCancelAlg = src.order_cancel_alg;
}

void fill_trading_account_password_update(
    CThostFtdcTradingAccountPasswordUpdateField &dest,
    const ctp_trading_account_password_update &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.OldPassword, src.old_password);
  copy_field(dest.NewPassword, src.new_password);
  copy_field(dest.CurrencyID, src.currency_id);
}

void fill_trading_code(CThostFtdcTradingCodeField &dest,
                       const ctp_trading_code &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.InvestorID, src.investor_id);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.ExchangeID, src.exchange_id);
  copy_field(dest.ClientID, src.client_id);
  dest.IsActive = src.is_active;
  dest.ClientIDType = src.client_id_type;
  copy_field(dest.BranchID, src.branch_id);
  dest.BizType = src.biz_type;
  copy_field(dest.InvestUnitID, src.invest_unit_id);
}

void fill_trading_notice(CThostFtdcTradingNoticeField &dest,
                         const ctp_trading_notice &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  dest.InvestorRange = src.investor_range;
  copy_field(dest.InvestorID, src.investor_id);
  dest.SequenceSeries = src.sequence_series;
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.SendTime, src.send_time);
  dest.SequenceNo = src.sequence_no;
  copy_field(dest.FieldContent, src.field_content);
  copy_field(dest.InvestUnitID, src.invest_unit_id);
}

void fill_transfer_bank(CThostFtdcTransferBankField &dest,
                        const ctp_transfer_bank &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBrchID, src.bank_brch_id);
  copy_field(dest.BankName, src.bank_name);
  dest.IsActive = src.is_active;
}

void fill_transfer_serial(CThostFtdcTransferSerialField &dest,
                          const ctp_transfer_serial &src) {
  std::memset(&dest, 0, sizeof(dest));
  dest.PlateSerial = src.plate_serial;
  copy_field(dest.TradeDate, src.trade_date);
  copy_field(dest.TradingDay, src.trading_day);
  copy_field(dest.TradeTime, src.trade_time);
  copy_field(dest.TradeCode, src.trade_code);
  dest.SessionID = src.session_id;
  copy_field(dest.BankID, src.bank_id);
  copy_field(dest.BankBranchID, src.bank_branch_id);
  dest.BankAccType = src.bank_acc_type;
  copy_field(dest.BankAccount, src.bank_account);
  copy_field(dest.BankSerial, src.bank_serial);
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.BrokerBranchID, src.broker_branch_id);
  dest.FutureAccType = src.future_acc_type;
  copy_field(dest.AccountID, src.account_id);
  copy_field(dest.InvestorID, src.investor_id);
  dest.FutureSerial = src.future_serial;
  dest.IdCardType = src.id_card_type;
  copy_field(dest.IdentifiedCardNo, src.identified_card_no);
  copy_field(dest.CurrencyID, src.currency_id);
  dest.TradeAmount = src.trade_amount;
  dest.CustFee = src.cust_fee;
  dest.BrokerFee = src.broker_fee;
  dest.AvailabilityFlag = src.availability_flag;
  copy_field(dest.OperatorCode, src.operator_code);
  copy_field(dest.BankNewAccount, src.bank_new_account);
  dest.ErrorID = src.error_id;
  copy_field(dest.ErrorMsg, src.error_msg);
}

void fill_user_password_update(CThostFtdcUserPasswordUpdateField &dest,
                               const ctp_user_password_update &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.OldPassword, src.old_password);
  copy_field(dest.NewPassword, src.new_password);
}

void fill_user_session(CThostFtdcUserSessionField &dest,
                       const ctp_user_session &src) {
  std::memset(&dest, 0, sizeof(dest));
  dest.FrontID = src.front_id;
  dest.SessionID = src.session_id;
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  copy_field(dest.LoginDate, src.login_date);
  copy_field(dest.LoginTime, src.login_time);
  copy_field(dest.reserve1, src.reserve1);
  copy_field(dest.UserProductInfo, src.user_product_info);
  copy_field(dest.InterfaceProductInfo, src.interface_product_info);
  copy_field(dest.ProtocolInfo, src.protocol_info);
  copy_field(dest.MacAddress, src.mac_address);
  copy_field(dest.LoginRemark, src.login_remark);
  copy_field(dest.IPAddress, src.ip_address);
}

void fill_user_system_info(CThostFtdcUserSystemInfoField &dest,
                           const ctp_user_system_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  dest.ClientSystemInfoLen = src.client_system_info_len;
  copy_field(dest.ClientSystemInfo, src.client_system_info);
  copy_field(dest.reserve1, src.reserve1);
  dest.ClientIPPort = src.client_ip_port;
  copy_field(dest.ClientLoginTime, src.client_login_time);
  copy_field(dest.ClientAppID, src.client_app_id);
  copy_field(dest.ClientPublicIP, src.client_public_ip);
  copy_field(dest.ClientLoginRemark, src.client_login_remark);
  copy_field(dest.MAC, src.mac);
}

void fill_wechat_user_system_info(CThostFtdcWechatUserSystemInfoField &dest,
                                  const ctp_wechat_user_system_info &src) {
  std::memset(&dest, 0, sizeof(dest));
  copy_field(dest.BrokerID, src.broker_id);
  copy_field(dest.UserID, src.user_id);
  dest.WechatCltSysInfoLen = src.wechat_clt_sys_info_len;
  copy_field(dest.WechatCltSysInfo, src.wechat_clt_sys_info);
  dest.ClientIPPort = src.client_ip_port;
  copy_field(dest.ClientLoginTime, src.client_login_time);
  copy_field(dest.ClientAppID, src.client_app_id);
  copy_field(dest.ClientPublicIP, src.client_public_ip);
  copy_field(dest.ClientLoginRemark, src.client_login_remark);
}

} // namespace

struct ctp_trader_handle {
  CThostFtdcTraderApi *api{};
  std::unique_ptr<TraderSpiAdapter> spi{};
};

extern "C" {

const char *ctp_trader_get_api_version(void) {
  return CThostFtdcTraderApi::GetApiVersion();
}

ctp_trader_handle *ctp_trader_create(const char *flow_path,
                                     int32_t production_mode) {
  auto *handle = new (std::nothrow) ctp_trader_handle();
  if (handle == nullptr) {
    return nullptr;
  }
  handle->api = CThostFtdcTraderApi::CreateFtdcTraderApi(
      flow_path != nullptr ? flow_path : "", production_mode != 0);
  if (handle->api == nullptr) {
    delete handle;
    return nullptr;
  }
  return handle;
}

void ctp_trader_destroy(ctp_trader_handle *handle) {
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

int32_t ctp_trader_set_spi(ctp_trader_handle *handle, const ctp_trader_spi *spi,
                           void *user_data) {
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

int32_t ctp_trader_register_front(ctp_trader_handle *handle,
                                  const char *front_address) {
  if (handle == nullptr || handle->api == nullptr || front_address == nullptr) {
    return -1;
  }
  std::string front(front_address);
  handle->api->RegisterFront(front.data());
  return 0;
}

int32_t ctp_trader_subscribe_private_topic(ctp_trader_handle *handle,
                                           int32_t resume_type,
                                           int32_t seq_no) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  handle->api->SubscribePrivateTopic(
      static_cast<THOST_TE_RESUME_TYPE>(resume_type), seq_no);
  return 0;
}

int32_t ctp_trader_subscribe_public_topic(ctp_trader_handle *handle,
                                          int32_t resume_type) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  handle->api->SubscribePublicTopic(
      static_cast<THOST_TE_RESUME_TYPE>(resume_type));
  return 0;
}

void ctp_trader_init(ctp_trader_handle *handle) {
  if (handle != nullptr && handle->api != nullptr) {
    handle->api->Init();
  }
}

int32_t ctp_trader_join(ctp_trader_handle *handle) {
  if (handle == nullptr || handle->api == nullptr) {
    return -1;
  }
  return handle->api->Join();
}

int32_t ctp_trader_req_authenticate(ctp_trader_handle *handle,
                                    const ctp_req_authenticate *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqAuthenticateField native_request{};
  fill_req_authenticate(native_request, *request);
  return handle->api->ReqAuthenticate(&native_request, request_id);
}

int32_t ctp_trader_req_settlement_info_confirm(
    ctp_trader_handle *handle, const ctp_settlement_info_confirm *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcSettlementInfoConfirmField native_request{};
  fill_settlement_info_confirm(native_request, *request);
  return handle->api->ReqSettlementInfoConfirm(&native_request, request_id);
}

int32_t ctp_trader_req_user_login(ctp_trader_handle *handle,
                                  const ctp_req_user_login *request,
                                  int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqUserLoginField native_request{};
  fill_req_user_login(native_request, *request);
  return handle->api->ReqUserLogin(&native_request, request_id);
}

int32_t ctp_trader_req_user_logout(ctp_trader_handle *handle,
                                   const ctp_user_logout *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcUserLogoutField native_request{};
  fill_req_user_logout(native_request, *request);
  return handle->api->ReqUserLogout(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_trading_account(ctp_trader_handle *handle,
                                   const ctp_qry_trading_account *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryTradingAccountField native_request{};
  fill_qry_trading_account(native_request, *request);
  return handle->api->ReqQryTradingAccount(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_investor_position(ctp_trader_handle *handle,
                                     const ctp_qry_investor_position *request,
                                     int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorPositionField native_request{};
  fill_qry_investor_position(native_request, *request);
  return handle->api->ReqQryInvestorPosition(&native_request, request_id);
}

int32_t ctp_trader_req_qry_instrument_margin_rate(
    ctp_trader_handle *handle, const ctp_qry_instrument_margin_rate *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInstrumentMarginRateField native_request{};
  fill_qry_instrument_margin_rate(native_request, *request);
  return handle->api->ReqQryInstrumentMarginRate(&native_request, request_id);
}

int32_t ctp_trader_req_qry_exchange_margin_rate(
    ctp_trader_handle *handle, const ctp_qry_exchange_margin_rate *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryExchangeMarginRateField native_request{};
  fill_qry_exchange_margin_rate(native_request, *request);
  return handle->api->ReqQryExchangeMarginRate(&native_request, request_id);
}

int32_t ctp_trader_req_qry_instrument_commission_rate(
    ctp_trader_handle *handle,
    const ctp_qry_instrument_commission_rate *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInstrumentCommissionRateField native_request{};
  fill_qry_instrument_commission_rate(native_request, *request);
  return handle->api->ReqQryInstrumentCommissionRate(&native_request,
                                                     request_id);
}

int32_t ctp_trader_req_order_insert(ctp_trader_handle *handle,
                                    const ctp_input_order *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputOrderField native_request{};
  fill_input_order(native_request, *request);
  return handle->api->ReqOrderInsert(&native_request, request_id);
}

int32_t ctp_trader_req_order_action(ctp_trader_handle *handle,
                                    const ctp_input_order_action *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputOrderActionField native_request{};
  fill_input_order_action(native_request, *request);
  return handle->api->ReqOrderAction(&native_request, request_id);
}

const char *ctp_trader_get_trading_day(ctp_trader_handle *handle) {
  if (handle == nullptr || handle->api == nullptr) {
    return nullptr;
  }
  return handle->api->GetTradingDay();
}

int32_t ctp_trader_get_front_info(ctp_trader_handle *handle,
                                  ctp_front_info *front_info) {
  if (handle == nullptr || handle->api == nullptr || front_info == nullptr) {
    return -1;
  }
  CThostFtdcFrontInfoField native_info{};
  handle->api->GetFrontInfo(&native_info);
  std::memset(front_info, 0, sizeof(*front_info));
  fill_front_info(*front_info, &native_info);
  return 0;
}

int32_t ctp_trader_register_name_server(ctp_trader_handle *handle,
                                        const char *ns_address) {
  if (handle == nullptr || handle->api == nullptr || ns_address == nullptr) {
    return -1;
  }
  std::string value(ns_address);
  handle->api->RegisterNameServer(value.data());
  return 0;
}

int32_t ctp_trader_register_fens_user_info(ctp_trader_handle *handle,
                                           const ctp_fens_user_info *request) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcFensUserInfoField native_request{};
  fill_fens_user_info(native_request, *request);
  handle->api->RegisterFensUserInfo(&native_request);
  return 0;
}

int32_t
ctp_trader_register_user_system_info(ctp_trader_handle *handle,
                                     const ctp_user_system_info *request) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcUserSystemInfoField native_request{};
  fill_user_system_info(native_request, *request);
  return handle->api->RegisterUserSystemInfo(&native_request);
}

int32_t
ctp_trader_submit_user_system_info(ctp_trader_handle *handle,
                                   const ctp_user_system_info *request) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcUserSystemInfoField native_request{};
  fill_user_system_info(native_request, *request);
  return handle->api->SubmitUserSystemInfo(&native_request);
}

int32_t ctp_trader_register_wechat_user_system_info(
    ctp_trader_handle *handle, const ctp_wechat_user_system_info *request) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcWechatUserSystemInfoField native_request{};
  fill_wechat_user_system_info(native_request, *request);
  return handle->api->RegisterWechatUserSystemInfo(&native_request);
}

int32_t ctp_trader_submit_wechat_user_system_info(
    ctp_trader_handle *handle, const ctp_wechat_user_system_info *request) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcWechatUserSystemInfoField native_request{};
  fill_wechat_user_system_info(native_request, *request);
  return handle->api->SubmitWechatUserSystemInfo(&native_request);
}

int32_t
ctp_trader_req_user_password_update(ctp_trader_handle *handle,
                                    const ctp_user_password_update *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcUserPasswordUpdateField native_request{};
  fill_user_password_update(native_request, *request);
  return handle->api->ReqUserPasswordUpdate(&native_request, request_id);
}

int32_t ctp_trader_req_trading_account_password_update(
    ctp_trader_handle *handle,
    const ctp_trading_account_password_update *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcTradingAccountPasswordUpdateField native_request{};
  fill_trading_account_password_update(native_request, *request);
  return handle->api->ReqTradingAccountPasswordUpdate(&native_request,
                                                      request_id);
}

int32_t ctp_trader_req_user_auth_method(ctp_trader_handle *handle,
                                        const ctp_req_user_auth_method *request,
                                        int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqUserAuthMethodField native_request{};
  fill_req_user_auth_method(native_request, *request);
  return handle->api->ReqUserAuthMethod(&native_request, request_id);
}

int32_t ctp_trader_req_gen_user_captcha(ctp_trader_handle *handle,
                                        const ctp_req_gen_user_captcha *request,
                                        int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqGenUserCaptchaField native_request{};
  fill_req_gen_user_captcha(native_request, *request);
  return handle->api->ReqGenUserCaptcha(&native_request, request_id);
}

int32_t ctp_trader_req_gen_user_text(ctp_trader_handle *handle,
                                     const ctp_req_gen_user_text *request,
                                     int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqGenUserTextField native_request{};
  fill_req_gen_user_text(native_request, *request);
  return handle->api->ReqGenUserText(&native_request, request_id);
}

int32_t ctp_trader_req_user_login_with_captcha(
    ctp_trader_handle *handle, const ctp_req_user_login_with_captcha *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqUserLoginWithCaptchaField native_request{};
  fill_req_user_login_with_captcha(native_request, *request);
  return handle->api->ReqUserLoginWithCaptcha(&native_request, request_id);
}

int32_t
ctp_trader_req_user_login_with_text(ctp_trader_handle *handle,
                                    const ctp_req_user_login_with_text *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqUserLoginWithTextField native_request{};
  fill_req_user_login_with_text(native_request, *request);
  return handle->api->ReqUserLoginWithText(&native_request, request_id);
}

int32_t
ctp_trader_req_user_login_with_otp(ctp_trader_handle *handle,
                                   const ctp_req_user_login_with_otp *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqUserLoginWithOTPField native_request{};
  fill_req_user_login_with_otp(native_request, *request);
  return handle->api->ReqUserLoginWithOTP(&native_request, request_id);
}

int32_t ctp_trader_req_gen_sms_code(ctp_trader_handle *handle,
                                    const ctp_req_gen_sms_code *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqGenSMSCodeField native_request{};
  fill_req_gen_sms_code(native_request, *request);
  return handle->api->ReqGenSMSCode(&native_request, request_id);
}

int32_t ctp_trader_req_parked_order_insert(ctp_trader_handle *handle,
                                           const ctp_parked_order *request,
                                           int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcParkedOrderField native_request{};
  fill_parked_order(native_request, *request);
  return handle->api->ReqParkedOrderInsert(&native_request, request_id);
}

int32_t
ctp_trader_req_parked_order_action(ctp_trader_handle *handle,
                                   const ctp_parked_order_action *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcParkedOrderActionField native_request{};
  fill_parked_order_action(native_request, *request);
  return handle->api->ReqParkedOrderAction(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_max_order_volume(ctp_trader_handle *handle,
                                    const ctp_qry_max_order_volume *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryMaxOrderVolumeField native_request{};
  fill_qry_max_order_volume(native_request, *request);
  return handle->api->ReqQryMaxOrderVolume(&native_request, request_id);
}

int32_t
ctp_trader_req_remove_parked_order(ctp_trader_handle *handle,
                                   const ctp_remove_parked_order *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcRemoveParkedOrderField native_request{};
  fill_remove_parked_order(native_request, *request);
  return handle->api->ReqRemoveParkedOrder(&native_request, request_id);
}

int32_t ctp_trader_req_remove_parked_order_action(
    ctp_trader_handle *handle, const ctp_remove_parked_order_action *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcRemoveParkedOrderActionField native_request{};
  fill_remove_parked_order_action(native_request, *request);
  return handle->api->ReqRemoveParkedOrderAction(&native_request, request_id);
}

int32_t ctp_trader_req_exec_order_insert(ctp_trader_handle *handle,
                                         const ctp_input_exec_order *request,
                                         int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputExecOrderField native_request{};
  fill_input_exec_order(native_request, *request);
  return handle->api->ReqExecOrderInsert(&native_request, request_id);
}

int32_t
ctp_trader_req_exec_order_action(ctp_trader_handle *handle,
                                 const ctp_input_exec_order_action *request,
                                 int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputExecOrderActionField native_request{};
  fill_input_exec_order_action(native_request, *request);
  return handle->api->ReqExecOrderAction(&native_request, request_id);
}

int32_t ctp_trader_req_for_quote_insert(ctp_trader_handle *handle,
                                        const ctp_input_for_quote *request,
                                        int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputForQuoteField native_request{};
  fill_input_for_quote(native_request, *request);
  return handle->api->ReqForQuoteInsert(&native_request, request_id);
}

int32_t ctp_trader_req_quote_insert(ctp_trader_handle *handle,
                                    const ctp_input_quote *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputQuoteField native_request{};
  fill_input_quote(native_request, *request);
  return handle->api->ReqQuoteInsert(&native_request, request_id);
}

int32_t ctp_trader_req_quote_action(ctp_trader_handle *handle,
                                    const ctp_input_quote_action *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputQuoteActionField native_request{};
  fill_input_quote_action(native_request, *request);
  return handle->api->ReqQuoteAction(&native_request, request_id);
}

int32_t
ctp_trader_req_batch_order_action(ctp_trader_handle *handle,
                                  const ctp_input_batch_order_action *request,
                                  int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputBatchOrderActionField native_request{};
  fill_input_batch_order_action(native_request, *request);
  return handle->api->ReqBatchOrderAction(&native_request, request_id);
}

int32_t ctp_trader_req_option_self_close_insert(
    ctp_trader_handle *handle, const ctp_input_option_self_close *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputOptionSelfCloseField native_request{};
  fill_input_option_self_close(native_request, *request);
  return handle->api->ReqOptionSelfCloseInsert(&native_request, request_id);
}

int32_t ctp_trader_req_option_self_close_action(
    ctp_trader_handle *handle,
    const ctp_input_option_self_close_action *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputOptionSelfCloseActionField native_request{};
  fill_input_option_self_close_action(native_request, *request);
  return handle->api->ReqOptionSelfCloseAction(&native_request, request_id);
}

int32_t ctp_trader_req_comb_action_insert(ctp_trader_handle *handle,
                                          const ctp_input_comb_action *request,
                                          int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputCombActionField native_request{};
  fill_input_comb_action(native_request, *request);
  return handle->api->ReqCombActionInsert(&native_request, request_id);
}

int32_t ctp_trader_req_offset_setting(ctp_trader_handle *handle,
                                      const ctp_input_offset_setting *request,
                                      int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputOffsetSettingField native_request{};
  fill_input_offset_setting(native_request, *request);
  return handle->api->ReqOffsetSetting(&native_request, request_id);
}

int32_t
ctp_trader_req_cancel_offset_setting(ctp_trader_handle *handle,
                                     const ctp_input_offset_setting *request,
                                     int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputOffsetSettingField native_request{};
  fill_input_offset_setting(native_request, *request);
  return handle->api->ReqCancelOffsetSetting(&native_request, request_id);
}

int32_t ctp_trader_req_spd_apply(ctp_trader_handle *handle,
                                 const ctp_input_spd_apply *request,
                                 int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputSpdApplyField native_request{};
  fill_input_spd_apply(native_request, *request);
  return handle->api->ReqSpdApply(&native_request, request_id);
}

int32_t
ctp_trader_req_spd_apply_action(ctp_trader_handle *handle,
                                const ctp_input_spd_apply_action *request,
                                int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputSpdApplyActionField native_request{};
  fill_input_spd_apply_action(native_request, *request);
  return handle->api->ReqSpdApplyAction(&native_request, request_id);
}

int32_t ctp_trader_req_hedge_cfm(ctp_trader_handle *handle,
                                 const ctp_input_hedge_cfm *request,
                                 int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputHedgeCfmField native_request{};
  fill_input_hedge_cfm(native_request, *request);
  return handle->api->ReqHedgeCfm(&native_request, request_id);
}

int32_t
ctp_trader_req_hedge_cfm_action(ctp_trader_handle *handle,
                                const ctp_input_hedge_cfm_action *request,
                                int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcInputHedgeCfmActionField native_request{};
  fill_input_hedge_cfm_action(native_request, *request);
  return handle->api->ReqHedgeCfmAction(&native_request, request_id);
}

int32_t ctp_trader_req_qry_order(ctp_trader_handle *handle,
                                 const ctp_qry_order *request,
                                 int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryOrderField native_request{};
  fill_qry_order(native_request, *request);
  return handle->api->ReqQryOrder(&native_request, request_id);
}

int32_t ctp_trader_req_qry_trade(ctp_trader_handle *handle,
                                 const ctp_qry_trade *request,
                                 int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryTradeField native_request{};
  fill_qry_trade(native_request, *request);
  return handle->api->ReqQryTrade(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor(ctp_trader_handle *handle,
                                    const ctp_qry_investor *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorField native_request{};
  fill_qry_investor(native_request, *request);
  return handle->api->ReqQryInvestor(&native_request, request_id);
}

int32_t ctp_trader_req_qry_trading_code(ctp_trader_handle *handle,
                                        const ctp_qry_trading_code *request,
                                        int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryTradingCodeField native_request{};
  fill_qry_trading_code(native_request, *request);
  return handle->api->ReqQryTradingCode(&native_request, request_id);
}

int32_t ctp_trader_req_qry_user_session(ctp_trader_handle *handle,
                                        const ctp_qry_user_session *request,
                                        int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryUserSessionField native_request{};
  fill_qry_user_session(native_request, *request);
  return handle->api->ReqQryUserSession(&native_request, request_id);
}

int32_t ctp_trader_req_qry_exchange(ctp_trader_handle *handle,
                                    const ctp_qry_exchange *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryExchangeField native_request{};
  fill_qry_exchange(native_request, *request);
  return handle->api->ReqQryExchange(&native_request, request_id);
}

int32_t ctp_trader_req_qry_product(ctp_trader_handle *handle,
                                   const ctp_qry_product *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryProductField native_request{};
  fill_qry_product(native_request, *request);
  return handle->api->ReqQryProduct(&native_request, request_id);
}

int32_t ctp_trader_req_qry_instrument(ctp_trader_handle *handle,
                                      const ctp_qry_instrument *request,
                                      int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInstrumentField native_request{};
  fill_qry_instrument(native_request, *request);
  return handle->api->ReqQryInstrument(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_depth_market_data(ctp_trader_handle *handle,
                                     const ctp_qry_depth_market_data *request,
                                     int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryDepthMarketDataField native_request{};
  fill_qry_depth_market_data(native_request, *request);
  return handle->api->ReqQryDepthMarketData(&native_request, request_id);
}

int32_t ctp_trader_req_qry_trader_offer(ctp_trader_handle *handle,
                                        const ctp_qry_trader_offer *request,
                                        int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryTraderOfferField native_request{};
  fill_qry_trader_offer(native_request, *request);
  return handle->api->ReqQryTraderOffer(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_settlement_info(ctp_trader_handle *handle,
                                   const ctp_qry_settlement_info *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySettlementInfoField native_request{};
  fill_qry_settlement_info(native_request, *request);
  return handle->api->ReqQrySettlementInfo(&native_request, request_id);
}

int32_t ctp_trader_req_qry_transfer_bank(ctp_trader_handle *handle,
                                         const ctp_qry_transfer_bank *request,
                                         int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryTransferBankField native_request{};
  fill_qry_transfer_bank(native_request, *request);
  return handle->api->ReqQryTransferBank(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor_position_detail(
    ctp_trader_handle *handle, const ctp_qry_investor_position_detail *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorPositionDetailField native_request{};
  fill_qry_investor_position_detail(native_request, *request);
  return handle->api->ReqQryInvestorPositionDetail(&native_request, request_id);
}

int32_t ctp_trader_req_qry_notice(ctp_trader_handle *handle,
                                  const ctp_qry_notice *request,
                                  int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryNoticeField native_request{};
  fill_qry_notice(native_request, *request);
  return handle->api->ReqQryNotice(&native_request, request_id);
}

int32_t ctp_trader_req_qry_settlement_info_confirm(
    ctp_trader_handle *handle, const ctp_qry_settlement_info_confirm *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySettlementInfoConfirmField native_request{};
  fill_qry_settlement_info_confirm(native_request, *request);
  return handle->api->ReqQrySettlementInfoConfirm(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor_position_combine_detail(
    ctp_trader_handle *handle,
    const ctp_qry_investor_position_combine_detail *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorPositionCombineDetailField native_request{};
  fill_qry_investor_position_combine_detail(native_request, *request);
  return handle->api->ReqQryInvestorPositionCombineDetail(&native_request,
                                                          request_id);
}

int32_t ctp_trader_req_qry_cfmmc_trading_account_key(
    ctp_trader_handle *handle, const ctp_qry_cfmmc_trading_account_key *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryCFMMCTradingAccountKeyField native_request{};
  fill_qry_cfmmc_trading_account_key(native_request, *request);
  return handle->api->ReqQryCFMMCTradingAccountKey(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_e_warrant_offset(ctp_trader_handle *handle,
                                    const ctp_qry_e_warrant_offset *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryEWarrantOffsetField native_request{};
  fill_qry_e_warrant_offset(native_request, *request);
  return handle->api->ReqQryEWarrantOffset(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor_product_group_margin(
    ctp_trader_handle *handle,
    const ctp_qry_investor_product_group_margin *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorProductGroupMarginField native_request{};
  fill_qry_investor_product_group_margin(native_request, *request);
  return handle->api->ReqQryInvestorProductGroupMargin(&native_request,
                                                       request_id);
}

int32_t ctp_trader_req_qry_exchange_margin_rate_adjust(
    ctp_trader_handle *handle,
    const ctp_qry_exchange_margin_rate_adjust *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryExchangeMarginRateAdjustField native_request{};
  fill_qry_exchange_margin_rate_adjust(native_request, *request);
  return handle->api->ReqQryExchangeMarginRateAdjust(&native_request,
                                                     request_id);
}

int32_t ctp_trader_req_qry_exchange_rate(ctp_trader_handle *handle,
                                         const ctp_qry_exchange_rate *request,
                                         int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryExchangeRateField native_request{};
  fill_qry_exchange_rate(native_request, *request);
  return handle->api->ReqQryExchangeRate(&native_request, request_id);
}

int32_t ctp_trader_req_qry_sec_agent_ac_id_map(
    ctp_trader_handle *handle, const ctp_qry_sec_agent_ac_id_map *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySecAgentACIDMapField native_request{};
  fill_qry_sec_agent_ac_id_map(native_request, *request);
  return handle->api->ReqQrySecAgentACIDMap(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_product_exch_rate(ctp_trader_handle *handle,
                                     const ctp_qry_product_exch_rate *request,
                                     int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryProductExchRateField native_request{};
  fill_qry_product_exch_rate(native_request, *request);
  return handle->api->ReqQryProductExchRate(&native_request, request_id);
}

int32_t ctp_trader_req_qry_product_group(ctp_trader_handle *handle,
                                         const ctp_qry_product_group *request,
                                         int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryProductGroupField native_request{};
  fill_qry_product_group(native_request, *request);
  return handle->api->ReqQryProductGroup(&native_request, request_id);
}

int32_t ctp_trader_req_qry_mm_instrument_commission_rate(
    ctp_trader_handle *handle,
    const ctp_qry_mm_instrument_commission_rate *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryMMInstrumentCommissionRateField native_request{};
  fill_qry_mm_instrument_commission_rate(native_request, *request);
  return handle->api->ReqQryMMInstrumentCommissionRate(&native_request,
                                                       request_id);
}

int32_t ctp_trader_req_qry_mm_option_instr_comm_rate(
    ctp_trader_handle *handle, const ctp_qry_mm_option_instr_comm_rate *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryMMOptionInstrCommRateField native_request{};
  fill_qry_mm_option_instr_comm_rate(native_request, *request);
  return handle->api->ReqQryMMOptionInstrCommRate(&native_request, request_id);
}

int32_t ctp_trader_req_qry_instrument_order_comm_rate(
    ctp_trader_handle *handle,
    const ctp_qry_instrument_order_comm_rate *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInstrumentOrderCommRateField native_request{};
  fill_qry_instrument_order_comm_rate(native_request, *request);
  return handle->api->ReqQryInstrumentOrderCommRate(&native_request,
                                                    request_id);
}

int32_t ctp_trader_req_qry_sec_agent_trading_account(
    ctp_trader_handle *handle, const ctp_qry_trading_account *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryTradingAccountField native_request{};
  fill_qry_trading_account(native_request, *request);
  return handle->api->ReqQrySecAgentTradingAccount(&native_request, request_id);
}

int32_t ctp_trader_req_qry_sec_agent_check_mode(
    ctp_trader_handle *handle, const ctp_qry_sec_agent_check_mode *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySecAgentCheckModeField native_request{};
  fill_qry_sec_agent_check_mode(native_request, *request);
  return handle->api->ReqQrySecAgentCheckMode(&native_request, request_id);
}

int32_t ctp_trader_req_qry_sec_agent_trade_info(
    ctp_trader_handle *handle, const ctp_qry_sec_agent_trade_info *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySecAgentTradeInfoField native_request{};
  fill_qry_sec_agent_trade_info(native_request, *request);
  return handle->api->ReqQrySecAgentTradeInfo(&native_request, request_id);
}

int32_t ctp_trader_req_qry_option_instr_trade_cost(
    ctp_trader_handle *handle, const ctp_qry_option_instr_trade_cost *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryOptionInstrTradeCostField native_request{};
  fill_qry_option_instr_trade_cost(native_request, *request);
  return handle->api->ReqQryOptionInstrTradeCost(&native_request, request_id);
}

int32_t ctp_trader_req_qry_option_instr_comm_rate(
    ctp_trader_handle *handle, const ctp_qry_option_instr_comm_rate *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryOptionInstrCommRateField native_request{};
  fill_qry_option_instr_comm_rate(native_request, *request);
  return handle->api->ReqQryOptionInstrCommRate(&native_request, request_id);
}

int32_t ctp_trader_req_qry_exec_order(ctp_trader_handle *handle,
                                      const ctp_qry_exec_order *request,
                                      int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryExecOrderField native_request{};
  fill_qry_exec_order(native_request, *request);
  return handle->api->ReqQryExecOrder(&native_request, request_id);
}

int32_t ctp_trader_req_qry_for_quote(ctp_trader_handle *handle,
                                     const ctp_qry_for_quote *request,
                                     int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryForQuoteField native_request{};
  fill_qry_for_quote(native_request, *request);
  return handle->api->ReqQryForQuote(&native_request, request_id);
}

int32_t ctp_trader_req_qry_quote(ctp_trader_handle *handle,
                                 const ctp_qry_quote *request,
                                 int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryQuoteField native_request{};
  fill_qry_quote(native_request, *request);
  return handle->api->ReqQryQuote(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_option_self_close(ctp_trader_handle *handle,
                                     const ctp_qry_option_self_close *request,
                                     int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryOptionSelfCloseField native_request{};
  fill_qry_option_self_close(native_request, *request);
  return handle->api->ReqQryOptionSelfClose(&native_request, request_id);
}

int32_t ctp_trader_req_qry_invest_unit(ctp_trader_handle *handle,
                                       const ctp_qry_invest_unit *request,
                                       int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestUnitField native_request{};
  fill_qry_invest_unit(native_request, *request);
  return handle->api->ReqQryInvestUnit(&native_request, request_id);
}

int32_t ctp_trader_req_qry_comb_instrument_guard(
    ctp_trader_handle *handle, const ctp_qry_comb_instrument_guard *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryCombInstrumentGuardField native_request{};
  fill_qry_comb_instrument_guard(native_request, *request);
  return handle->api->ReqQryCombInstrumentGuard(&native_request, request_id);
}

int32_t ctp_trader_req_qry_comb_action(ctp_trader_handle *handle,
                                       const ctp_qry_comb_action *request,
                                       int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryCombActionField native_request{};
  fill_qry_comb_action(native_request, *request);
  return handle->api->ReqQryCombAction(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_transfer_serial(ctp_trader_handle *handle,
                                   const ctp_qry_transfer_serial *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryTransferSerialField native_request{};
  fill_qry_transfer_serial(native_request, *request);
  return handle->api->ReqQryTransferSerial(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_accountregister(ctp_trader_handle *handle,
                                   const ctp_qry_accountregister *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryAccountregisterField native_request{};
  fill_qry_accountregister(native_request, *request);
  return handle->api->ReqQryAccountregister(&native_request, request_id);
}

int32_t ctp_trader_req_qry_contract_bank(ctp_trader_handle *handle,
                                         const ctp_qry_contract_bank *request,
                                         int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryContractBankField native_request{};
  fill_qry_contract_bank(native_request, *request);
  return handle->api->ReqQryContractBank(&native_request, request_id);
}

int32_t ctp_trader_req_qry_parked_order(ctp_trader_handle *handle,
                                        const ctp_qry_parked_order *request,
                                        int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryParkedOrderField native_request{};
  fill_qry_parked_order(native_request, *request);
  return handle->api->ReqQryParkedOrder(&native_request, request_id);
}

int32_t ctp_trader_req_qry_parked_order_action(
    ctp_trader_handle *handle, const ctp_qry_parked_order_action *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryParkedOrderActionField native_request{};
  fill_qry_parked_order_action(native_request, *request);
  return handle->api->ReqQryParkedOrderAction(&native_request, request_id);
}

int32_t ctp_trader_req_qry_trading_notice(ctp_trader_handle *handle,
                                          const ctp_qry_trading_notice *request,
                                          int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryTradingNoticeField native_request{};
  fill_qry_trading_notice(native_request, *request);
  return handle->api->ReqQryTradingNotice(&native_request, request_id);
}

int32_t ctp_trader_req_qry_broker_trading_params(
    ctp_trader_handle *handle, const ctp_qry_broker_trading_params *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryBrokerTradingParamsField native_request{};
  fill_qry_broker_trading_params(native_request, *request);
  return handle->api->ReqQryBrokerTradingParams(&native_request, request_id);
}

int32_t ctp_trader_req_qry_broker_trading_algos(
    ctp_trader_handle *handle, const ctp_qry_broker_trading_algos *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryBrokerTradingAlgosField native_request{};
  fill_qry_broker_trading_algos(native_request, *request);
  return handle->api->ReqQryBrokerTradingAlgos(&native_request, request_id);
}

int32_t ctp_trader_req_query_cfmmc_trading_account_token(
    ctp_trader_handle *handle,
    const ctp_query_cfmmc_trading_account_token *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQueryCFMMCTradingAccountTokenField native_request{};
  fill_query_cfmmc_trading_account_token(native_request, *request);
  return handle->api->ReqQueryCFMMCTradingAccountToken(&native_request,
                                                       request_id);
}

int32_t ctp_trader_req_qry_classified_instrument(
    ctp_trader_handle *handle, const ctp_qry_classified_instrument *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryClassifiedInstrumentField native_request{};
  fill_qry_classified_instrument(native_request, *request);
  return handle->api->ReqQryClassifiedInstrument(&native_request, request_id);
}

int32_t ctp_trader_req_qry_comb_promotion_param(
    ctp_trader_handle *handle, const ctp_qry_comb_promotion_param *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryCombPromotionParamField native_request{};
  fill_qry_comb_promotion_param(native_request, *request);
  return handle->api->ReqQryCombPromotionParam(&native_request, request_id);
}

int32_t ctp_trader_req_qry_risk_settle_invst_position(
    ctp_trader_handle *handle,
    const ctp_qry_risk_settle_invst_position *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRiskSettleInvstPositionField native_request{};
  fill_qry_risk_settle_invst_position(native_request, *request);
  return handle->api->ReqQryRiskSettleInvstPosition(&native_request,
                                                    request_id);
}

int32_t ctp_trader_req_qry_risk_settle_product_status(
    ctp_trader_handle *handle,
    const ctp_qry_risk_settle_product_status *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRiskSettleProductStatusField native_request{};
  fill_qry_risk_settle_product_status(native_request, *request);
  return handle->api->ReqQryRiskSettleProductStatus(&native_request,
                                                    request_id);
}

int32_t ctp_trader_req_qry_spbm_future_parameter(
    ctp_trader_handle *handle, const ctp_qry_spbm_future_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySPBMFutureParameterField native_request{};
  fill_qry_spbm_future_parameter(native_request, *request);
  return handle->api->ReqQrySPBMFutureParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_spbm_option_parameter(
    ctp_trader_handle *handle, const ctp_qry_spbm_option_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySPBMOptionParameterField native_request{};
  fill_qry_spbm_option_parameter(native_request, *request);
  return handle->api->ReqQrySPBMOptionParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_spbm_intra_parameter(
    ctp_trader_handle *handle, const ctp_qry_spbm_intra_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySPBMIntraParameterField native_request{};
  fill_qry_spbm_intra_parameter(native_request, *request);
  return handle->api->ReqQrySPBMIntraParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_spbm_inter_parameter(
    ctp_trader_handle *handle, const ctp_qry_spbm_inter_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySPBMInterParameterField native_request{};
  fill_qry_spbm_inter_parameter(native_request, *request);
  return handle->api->ReqQrySPBMInterParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_spbm_portf_definition(
    ctp_trader_handle *handle, const ctp_qry_spbm_portf_definition *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySPBMPortfDefinitionField native_request{};
  fill_qry_spbm_portf_definition(native_request, *request);
  return handle->api->ReqQrySPBMPortfDefinition(&native_request, request_id);
}

int32_t ctp_trader_req_qry_spbm_investor_portf_def(
    ctp_trader_handle *handle, const ctp_qry_spbm_investor_portf_def *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySPBMInvestorPortfDefField native_request{};
  fill_qry_spbm_investor_portf_def(native_request, *request);
  return handle->api->ReqQrySPBMInvestorPortfDef(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor_portf_margin_ratio(
    ctp_trader_handle *handle,
    const ctp_qry_investor_portf_margin_ratio *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorPortfMarginRatioField native_request{};
  fill_qry_investor_portf_margin_ratio(native_request, *request);
  return handle->api->ReqQryInvestorPortfMarginRatio(&native_request,
                                                     request_id);
}

int32_t ctp_trader_req_qry_investor_prod_spbm_detail(
    ctp_trader_handle *handle, const ctp_qry_investor_prod_spbm_detail *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorProdSPBMDetailField native_request{};
  fill_qry_investor_prod_spbm_detail(native_request, *request);
  return handle->api->ReqQryInvestorProdSPBMDetail(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor_commodity_spmm_margin(
    ctp_trader_handle *handle,
    const ctp_qry_investor_commodity_spmm_margin *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorCommoditySPMMMarginField native_request{};
  fill_qry_investor_commodity_spmm_margin(native_request, *request);
  return handle->api->ReqQryInvestorCommoditySPMMMargin(&native_request,
                                                        request_id);
}

int32_t ctp_trader_req_qry_investor_commodity_group_spmm_margin(
    ctp_trader_handle *handle,
    const ctp_qry_investor_commodity_group_spmm_margin *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorCommodityGroupSPMMMarginField native_request{};
  fill_qry_investor_commodity_group_spmm_margin(native_request, *request);
  return handle->api->ReqQryInvestorCommodityGroupSPMMMargin(&native_request,
                                                             request_id);
}

int32_t
ctp_trader_req_qry_spmm_inst_param(ctp_trader_handle *handle,
                                   const ctp_qry_spmm_inst_param *request,
                                   int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySPMMInstParamField native_request{};
  fill_qry_spmm_inst_param(native_request, *request);
  return handle->api->ReqQrySPMMInstParam(&native_request, request_id);
}

int32_t
ctp_trader_req_qry_spmm_product_param(ctp_trader_handle *handle,
                                      const ctp_qry_spmm_product_param *request,
                                      int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySPMMProductParamField native_request{};
  fill_qry_spmm_product_param(native_request, *request);
  return handle->api->ReqQrySPMMProductParam(&native_request, request_id);
}

int32_t ctp_trader_req_qry_spbm_add_on_inter_parameter(
    ctp_trader_handle *handle,
    const ctp_qry_spbm_add_on_inter_parameter *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySPBMAddOnInterParameterField native_request{};
  fill_qry_spbm_add_on_inter_parameter(native_request, *request);
  return handle->api->ReqQrySPBMAddOnInterParameter(&native_request,
                                                    request_id);
}

int32_t ctp_trader_req_qry_rcams_comb_product_info(
    ctp_trader_handle *handle, const ctp_qry_rcams_comb_product_info *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRCAMSCombProductInfoField native_request{};
  fill_qry_rcams_comb_product_info(native_request, *request);
  return handle->api->ReqQryRCAMSCombProductInfo(&native_request, request_id);
}

int32_t ctp_trader_req_qry_rcams_instr_parameter(
    ctp_trader_handle *handle, const ctp_qry_rcams_instr_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRCAMSInstrParameterField native_request{};
  fill_qry_rcams_instr_parameter(native_request, *request);
  return handle->api->ReqQryRCAMSInstrParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_rcams_intra_parameter(
    ctp_trader_handle *handle, const ctp_qry_rcams_intra_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRCAMSIntraParameterField native_request{};
  fill_qry_rcams_intra_parameter(native_request, *request);
  return handle->api->ReqQryRCAMSIntraParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_rcams_inter_parameter(
    ctp_trader_handle *handle, const ctp_qry_rcams_inter_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRCAMSInterParameterField native_request{};
  fill_qry_rcams_inter_parameter(native_request, *request);
  return handle->api->ReqQryRCAMSInterParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_rcams_short_opt_adjust_param(
    ctp_trader_handle *handle,
    const ctp_qry_rcams_short_opt_adjust_param *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRCAMSShortOptAdjustParamField native_request{};
  fill_qry_rcams_short_opt_adjust_param(native_request, *request);
  return handle->api->ReqQryRCAMSShortOptAdjustParam(&native_request,
                                                     request_id);
}

int32_t ctp_trader_req_qry_rcams_investor_comb_position(
    ctp_trader_handle *handle,
    const ctp_qry_rcams_investor_comb_position *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRCAMSInvestorCombPositionField native_request{};
  fill_qry_rcams_investor_comb_position(native_request, *request);
  return handle->api->ReqQryRCAMSInvestorCombPosition(&native_request,
                                                      request_id);
}

int32_t ctp_trader_req_qry_investor_prod_rcams_margin(
    ctp_trader_handle *handle,
    const ctp_qry_investor_prod_rcams_margin *request, int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorProdRCAMSMarginField native_request{};
  fill_qry_investor_prod_rcams_margin(native_request, *request);
  return handle->api->ReqQryInvestorProdRCAMSMargin(&native_request,
                                                    request_id);
}

int32_t ctp_trader_req_qry_rule_instr_parameter(
    ctp_trader_handle *handle, const ctp_qry_rule_instr_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRULEInstrParameterField native_request{};
  fill_qry_rule_instr_parameter(native_request, *request);
  return handle->api->ReqQryRULEInstrParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_rule_intra_parameter(
    ctp_trader_handle *handle, const ctp_qry_rule_intra_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRULEIntraParameterField native_request{};
  fill_qry_rule_intra_parameter(native_request, *request);
  return handle->api->ReqQryRULEIntraParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_rule_inter_parameter(
    ctp_trader_handle *handle, const ctp_qry_rule_inter_parameter *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryRULEInterParameterField native_request{};
  fill_qry_rule_inter_parameter(native_request, *request);
  return handle->api->ReqQryRULEInterParameter(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor_prod_rule_margin(
    ctp_trader_handle *handle, const ctp_qry_investor_prod_rule_margin *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorProdRULEMarginField native_request{};
  fill_qry_investor_prod_rule_margin(native_request, *request);
  return handle->api->ReqQryInvestorProdRULEMargin(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor_portf_setting(
    ctp_trader_handle *handle, const ctp_qry_investor_portf_setting *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorPortfSettingField native_request{};
  fill_qry_investor_portf_setting(native_request, *request);
  return handle->api->ReqQryInvestorPortfSetting(&native_request, request_id);
}

int32_t ctp_trader_req_qry_investor_info_comm_rec(
    ctp_trader_handle *handle, const ctp_qry_investor_info_comm_rec *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryInvestorInfoCommRecField native_request{};
  fill_qry_investor_info_comm_rec(native_request, *request);
  return handle->api->ReqQryInvestorInfoCommRec(&native_request, request_id);
}

int32_t ctp_trader_req_qry_comb_leg(ctp_trader_handle *handle,
                                    const ctp_qry_comb_leg *request,
                                    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryCombLegField native_request{};
  fill_qry_comb_leg(native_request, *request);
  return handle->api->ReqQryCombLeg(&native_request, request_id);
}

int32_t ctp_trader_req_qry_offset_setting(ctp_trader_handle *handle,
                                          const ctp_qry_offset_setting *request,
                                          int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryOffsetSettingField native_request{};
  fill_qry_offset_setting(native_request, *request);
  return handle->api->ReqQryOffsetSetting(&native_request, request_id);
}

int32_t ctp_trader_req_qry_spd_apply(ctp_trader_handle *handle,
                                     const ctp_qry_spd_apply *request,
                                     int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQrySpdApplyField native_request{};
  fill_qry_spd_apply(native_request, *request);
  return handle->api->ReqQrySpdApply(&native_request, request_id);
}

int32_t ctp_trader_req_qry_hedge_cfm(ctp_trader_handle *handle,
                                     const ctp_qry_hedge_cfm *request,
                                     int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcQryHedgeCfmField native_request{};
  fill_qry_hedge_cfm(native_request, *request);
  return handle->api->ReqQryHedgeCfm(&native_request, request_id);
}

int32_t
ctp_trader_req_from_bank_to_future_by_future(ctp_trader_handle *handle,
                                             const ctp_req_transfer *request,
                                             int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqTransferField native_request{};
  fill_req_transfer(native_request, *request);
  return handle->api->ReqFromBankToFutureByFuture(&native_request, request_id);
}

int32_t
ctp_trader_req_from_future_to_bank_by_future(ctp_trader_handle *handle,
                                             const ctp_req_transfer *request,
                                             int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqTransferField native_request{};
  fill_req_transfer(native_request, *request);
  return handle->api->ReqFromFutureToBankByFuture(&native_request, request_id);
}

int32_t ctp_trader_req_query_bank_account_money_by_future(
    ctp_trader_handle *handle, const ctp_req_query_account *request,
    int32_t request_id) {
  if (handle == nullptr || handle->api == nullptr || request == nullptr) {
    return -1;
  }
  CThostFtdcReqQueryAccountField native_request{};
  fill_req_query_account(native_request, *request);
  return handle->api->ReqQueryBankAccountMoneyByFuture(&native_request,
                                                       request_id);
}

} // extern "C"
