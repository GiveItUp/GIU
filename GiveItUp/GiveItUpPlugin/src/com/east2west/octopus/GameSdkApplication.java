package com.east2west.octopus;

//import a.b.c.DynamicSdkManager;
import android.app.Application;
import android.content.Context;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.telephony.TelephonyManager;
import android.util.Log;

import com.baidu.frontia.FrontiaApplication;
import com.unicom.dcLoader.Utils;
import com.unicom.dcLoader.Utils.UnipayPayResultListener;

public class GameSdkApplication extends Application {
	@Override
	public void onCreate() {
		super.onCreate();
//		MainActivity.ChinaType = checkSIM();// 三网
		MainActivity.ChinaType = Const.ChinaMobile;// 移动单网
		if (MainActivity.ChinaType == Const.ChinaUnicom) {
			Log.e("Unity", "GameSdkApplication onCreate");
			Utils.getInstances().initSDK(this, new UnipayPayResultListener() {
				@Override
				public void PayResult(String arg0, int arg1, int arg2,
						String arg3) {
				}
			});
		}
		ApplicationInfo appInfo = null;
		try {
			appInfo = this.getPackageManager().getApplicationInfo(
					getPackageName(), PackageManager.GET_META_DATA);
		} catch (NameNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		MainActivity.isBaiduChannel = false;
		// 判断是否是百度四渠道
		int msg = appInfo.metaData.getInt("EGAME_CHANNEL");
		if (msg == Const.BAIDUZHUSHOU || msg == Const.NINEONE
				|| msg == Const.TIEBA || msg == Const.DUOKU) {
			MainActivity.isBaiduChannel = true;
			FrontiaApplication.initFrontiaApplication(this
					.getApplicationContext());
		}
	}

	public int checkSIM() {
		try {
			TelephonyManager telManager = (TelephonyManager) getSystemService(Context.TELEPHONY_SERVICE);
			String operator = telManager.getSimOperator();
			if (operator != null) {
				Log.e("Unity", operator);
				if (operator.equals("46000") || operator.equals("46002")
						|| operator.equals("46007")) {
					// 中国移动
					return Const.ChinaMobile;
				} else if (operator.equals("46001")) {
					// 中国联通
					return Const.ChinaUnicom;
				} else if (operator.equals("46003") || operator.equals("46005")
						|| operator.equals("20404")) {// 20404 英国Vodafone) {
					// 中国电信
					return Const.ChinaTelecom;
				}
			}
			else{
				Log.e("Unity", "operator is null");
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return Const.ChinaNull;
	}
}
