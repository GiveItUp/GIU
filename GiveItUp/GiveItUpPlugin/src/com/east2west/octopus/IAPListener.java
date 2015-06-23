package com.east2west.octopus;

import java.util.HashMap;

import mm.purchasesdk.OnPurchaseListener;
import mm.purchasesdk.Purchase;
import mm.purchasesdk.PurchaseCode;
import android.content.Context;
import android.os.Message;
import android.util.Log;

public class IAPListener implements OnPurchaseListener {
	private final String TAG = "IAPListener";
	private MainActivity context;
	private IAPHandler iapHandler;

	public IAPListener(Context context, IAPHandler iapHandler) {
		this.context = (MainActivity) context;
		this.iapHandler = iapHandler;
	}

	@Override
	public void onAfterApply() {

	}

	@Override
	public void onAfterDownload() {

	}

	@Override
	public void onBeforeApply() {

	}

	@Override
	public void onBeforeDownload() {

	}

	@Override
	public void onInitFinish(int code) {
		Log.d(TAG, "Init finish, status code = " + code);
		Message message = iapHandler.obtainMessage(IAPHandler.INIT_FINISH);
		String result = "初始化结果：" + Purchase.getReason(code);
		message.obj = result;
		message.sendToTarget();
	}

	@Override
	public void onBillingFinish(int code, HashMap arg1) {
		Log.d(TAG, "billing finish, status code = " + code);
		String result = "订购结果：订购成功！";
		Message message = iapHandler.obtainMessage(IAPHandler.BILL_FINISH);
		// 此次订购的orderID
		String orderID = null;
		// 商品的paycode
		String paycode = null;
		// 商品的有效期(仅租赁类型商品有�?
		String leftday = null;
		// 商品的交�?ID，用户可以根据这个交易ID，查询商品是否已经交�?
		String tradeID = null;
		
		String ordertype = null;
		if (code == PurchaseCode.ORDER_OK || (code == PurchaseCode.AUTH_OK) ||(code == PurchaseCode.WEAK_ORDER_OK)) {
			/**
			 * 商品购买成功或�?已经购买�?此时会返回商品的paycode，orderID,以及剩余时间(租赁类型商品)
			 */
			if (arg1 != null) {
				leftday = (String) arg1.get(OnPurchaseListener.LEFTDAY);
				if (leftday != null && leftday.trim().length() != 0) {
					result = result + ",剩余时间 �?" + leftday;
				}
				orderID = (String) arg1.get(OnPurchaseListener.ORDERID);
				if (orderID != null && orderID.trim().length() != 0) {
					result = result + ",OrderID �?" + orderID;
				}
				paycode = (String) arg1.get(OnPurchaseListener.PAYCODE);
				if (paycode != null && paycode.trim().length() != 0) {
					result = result + ",Paycode:" + paycode;
				}
				tradeID = (String) arg1.get(OnPurchaseListener.TRADEID);
				if (tradeID != null && tradeID.trim().length() != 0) {
					result = result + ",tradeID:" + tradeID;
				}
				ordertype = (String) arg1.get(OnPurchaseListener.ORDERTYPE);
				if (tradeID != null && tradeID.trim().length() != 0) {
					result = result + ",ORDERTYPE:" + ordertype;
				}
			}
            context.MMpaySuccess(paycode);  
		} else {
			/**
			 * 表示订购失败�?
			 */;
            context.MMpayFaild();
		}
		context.dismissProgressDialog();
		System.out.println(result);

	}

	@Override
	public void onQueryFinish(int code, HashMap arg1) {
		Log.d(TAG, "license finish, status code = " + code);
		Message message = iapHandler.obtainMessage(IAPHandler.QUERY_FINISH);
		String result = "查询成功,该商品已购买";
		// 此次订购的orderID
		String orderID = null;
		// 商品的paycode
		String paycode = null;
		// 商品的有效期(仅租赁类型商品有�?
		String leftday = null;
		if (code != PurchaseCode.QUERY_OK) {
			/**
			 * 查询不到商品购买的相关信�?
			 */
			result = "查询结果" + Purchase.getReason(code);
		} else {
			/**
			 * 查询到商品的相关信息�?
			 * 此时你可以获得商品的paycode，orderid，以及商品的有效期leftday（仅租赁类型商品可以返回�?
			 */
			leftday = (String) arg1.get(OnPurchaseListener.LEFTDAY);
			if (leftday != null && leftday.trim().length() != 0) {
				result = result + ",剩余时间" + leftday;
			}
			orderID = (String) arg1.get(OnPurchaseListener.ORDERID);
			if (orderID != null && orderID.trim().length() != 0) {
				result = result + ",OrderID �?" + orderID;
			}
			paycode = (String) arg1.get(OnPurchaseListener.PAYCODE);
			if (paycode != null && paycode.trim().length() != 0) {
				result = result + ",Paycode:" + paycode;
			}
		}
		System.out.println(result);
		context.dismissProgressDialog();
	}

	

	@Override
	public void onUnsubscribeFinish(int code) {
		// TODO Auto-generated method stub
		String result = Purchase.getReason(code);
		System.out.println(result);
		context.dismissProgressDialog();
	}
}
