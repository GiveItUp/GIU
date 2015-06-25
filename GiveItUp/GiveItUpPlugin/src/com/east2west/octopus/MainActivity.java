package com.east2west.octopus;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

import mm.purchasesdk.OnPurchaseListener;
import mm.purchasesdk.Purchase;
import net.slidingmenu.tools.AdManager;
import net.slidingmenu.tools.os.OffersManager;
import net.slidingmenu.tools.os.PointsManager;
import net.slidingmenu.tools.st.SpotManager;
import net.slidingmenu.tools.video.VideoAdManager;
import net.slidingmenu.tools.video.listener.VideoAdListener;

import org.json.JSONObject;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;
import cn.egame.terminal.paysdk.EgamePay;
import cn.egame.terminal.paysdk.EgamePayListener;
import cn.egame.terminal.sdk.log.EgameAgent;
import cn.sharesdk.unity3d.ShareSDKUtils;

import com.duoku.platform.single.DKPlatform;
import com.duoku.platform.single.DKPlatformSettings;
import com.duoku.platform.single.DkErrorCode;
import com.duoku.platform.single.DkProtocolKeys;
import com.duoku.platform.single.callback.IDKSDKCallBack;
import com.ktplay.open.KTAccountManager;
import com.ktplay.open.KTAccountManager.KTLoginListener;
import com.ktplay.open.KTError;
import com.ktplay.open.KTLeaderboard;
import com.ktplay.open.KTLeaderboardPaginator;
import com.ktplay.open.KTPlay;
import com.ktplay.open.KTUser;
import com.unicom.dcLoader.Utils;
import com.unicom.dcLoader.Utils.UnipayPayResultListener;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
//import com.chinaMobile.MobileAgent;

//import com.mediav.ads.sdk.adcore.Mvad;

public class MainActivity extends UnityPlayerActivity {
	public static MainActivity instance = null;
	private Context context;

	private String buyID;
	private String payID;
	public Purchase purchase;
	private ProgressDialog mProgressDialog;
	private IAPListener mListener;

	public static int ChinaType = Const.ChinaNull;
	private String UnlockGame;
	private String BuyCharacter;

	public static boolean isBaiduChannel = false;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		ShareSDKUtils.prepare(this.getApplicationContext());
		Log.e("Unity", "MainActivity Activity");
		instance = this;
		context = this;
		KTPlay.startWithAppKey(this, Const.KtplayKey, Const.KtplaySecret);
		// 设置appid, appsecret, 是否测试模式)
		AdManager.getInstance(this).init(Const.YoumiID, Const.YoumiSecret,
				false);
		// 插屏广告初始
		SpotManager.getInstance(this).loadSpotAds();
		SpotManager.getInstance(this).setSpotOrientation(
				SpotManager.ORIENTATION_LANDSCAPE);
		SpotManager.getInstance(this)
				.setAnimationType(SpotManager.ANIM_ADVANCE);
		// 积分
		OffersManager.getInstance(this).onAppLaunch();

		switch (ChinaType) {
		case Const.ChinaMobile:
			mProgressDialog = new ProgressDialog(this);
			Log.e("Unity", "MainActivity Activity3");
			mProgressDialog.setIndeterminate(true);
			mProgressDialog.setMessage("Please wait...");
			IAPHandler iapHandler = new IAPHandler(this);
			mListener = new IAPListener(this, iapHandler);
			purchase = Purchase.getInstance();
			try {
				purchase.setAppInfo(Const.MobileID, Const.MobileKey);
			} catch (Exception e1) {
				e1.printStackTrace();
			}

			try {
				Log.e("Unity", "MainActivity Activity4");
				purchase.init(this, mListener);
				Log.e("Unity", "MainActivity Activity5");
			} catch (Exception e) {
				Log.e("Unity", "MainActivity Activity6");
				e.printStackTrace();
			}
			showProgressDialog();
			UnlockGame = Const.ydUnlockGame;
			BuyCharacter = Const.ydBuyCharacter;
			// 初始化有数
			// MobileAgent.init(this, APPID, "MobileMarket");
			Log.e("Unity", "plugin ready");
			break;
		case Const.ChinaTelecom:
			UnlockGame = Const.dxUnlockGame;
			BuyCharacter = Const.dxBuyCharacter;
			EgamePay.init(this);
			Log.e("Unity", "EgamePay.init");
			break;
		case Const.ChinaUnicom:
			UnlockGame = Const.ltUnlockGame;
			BuyCharacter = Const.ltBuyCharacter;
			break;
		}

		if (isBaiduChannel) {
			initBaiduSDK();
			Log.e("Unity", "initBaiduSDK Success");
		}
	}

	private void initBaiduSDK() {
		IDKSDKCallBack initcompletelistener = new IDKSDKCallBack() {
			@Override
			public void onResponse(String paramString) {
				Log.e("Unity", paramString);
				try {
					JSONObject jsonObject = new JSONObject(paramString);
					// 返回的操作状态码
					int mFunctionCode = jsonObject
							.getInt(DkProtocolKeys.FUNCTION_CODE);
					// 初始化完
					if (mFunctionCode == DkErrorCode.BDG_CROSSRECOMMEND_INIT_FINSIH) {
						DKPlatform.getInstance().bdgameInit(instance,
								new IDKSDKCallBack() {
									@Override
									public void onResponse(String arg0) {
										// TODO Auto-generated method stub
										Log.e("Unity", "pinxuan sdk init Success");
									}
								});
					}
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		};

		// 初始化
		DKPlatform.getInstance().init(this, true,
				DKPlatformSettings.SdkMode.SDK_BASIC, null, null,
				initcompletelistener);
	}

	@Override
	protected void onPause() {
		Log.e("Unity", "MainActivity onPause");
		// TODO Auto-generated method stub
		super.onPause();
		switch (ChinaType) {
		case Const.ChinaMobile:
			// MobileAgent.onPause(this);
			break;
		case Const.ChinaTelecom:
			EgameAgent.onPause(this);
			break;
		case Const.ChinaUnicom:
			Utils.getInstances().onPause(this);
			break;
		}
		// 百度
		if (isBaiduChannel) {
			DKPlatform.getInstance().pauseBaiduMobileStatistic(this);
		}
	}

	@Override
	protected void onResume() {
		Log.e("Unity", "MainActivity onResume");
		// TODO Auto-generated method stub
		super.onResume();
		switch (ChinaType) {
		case Const.ChinaMobile:
			// MobileAgent.onResume(this);
			break;
		case Const.ChinaTelecom:
			EgameAgent.onResume(this);
			break;
		case Const.ChinaUnicom:
			Utils.getInstances().onResume(this);
			break;
		}
		// 百度
		if (isBaiduChannel) {
			DKPlatform.getInstance().pauseBaiduMobileStatistic(this);
		}
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();
		// 调用插屏，开屏，退屏时退出。
		SpotManager.getInstance(context).onDestroy();
		// 调用视频广告时退出。
		VideoAdManager.getInstance(context).onDestroy();
		// DynamicSdkManager.getInstance(this).onAppDestroy();
		OffersManager.getInstance(this).onAppExit();
	}

	private void showProgressDialog() {
		if (mProgressDialog == null) {
			mProgressDialog = new ProgressDialog(MainActivity.this);
			mProgressDialog.setIndeterminate(true);
			mProgressDialog.setMessage("Please wait.....");
		}
		if (!mProgressDialog.isShowing()) {
			mProgressDialog.show();
		}
	}

	public void dismissProgressDialog() {
		if (mProgressDialog != null && mProgressDialog.isShowing()) {
			mProgressDialog.dismiss();
		}

	}

	public static Object getInstance() {
		Log.e("", "instance");
		return instance;
	}

	private byte[] InputStreamToByte(InputStream is) throws IOException {
		ByteArrayOutputStream bytestream = new ByteArrayOutputStream();
		int ch;
		while ((ch = is.read()) != -1) {
			bytestream.write(ch);
		}
		byte imgdata[] = bytestream.toByteArray();
		bytestream.close();
		return imgdata;
	}

	public void Buy(final String pid) {

		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				buy(pid);
			}
		});
	}

	public void buy(String pid) {
		buyID = pid;
		switch (pid) {
		case "UnlockGame":
			payID = UnlockGame;
			break;
		case "BuyCharacter":
			payID = BuyCharacter;
			break;
		default:
			break;
		}
		Log.e("Unity", pid + "----" + payID);
		if(payID == null)
			UnityPlayer.UnitySendMessage("CGame", "OnPurchaseFailed",
					buyID);
			
		switch (ChinaType) {
		case Const.ChinaMobile:
			order(instance, payID, mListener);
			break;
		case Const.ChinaTelecom:
			HashMap<String, String> payParams = new HashMap<String, String>();
			payParams.put(EgamePay.PAY_PARAMS_KEY_TOOLS_ALIAS, payID);
			EgamePay.pay(instance, payParams, new EgamePayListener() {
				@Override
				public void payCancel(Map<String, String> arg0) {
					// TODO Auto-generated method stub
					Log.e("Unity", "payCancel");
					UnityPlayer.UnitySendMessage("CGame", "OnPurchaseFailed",
							buyID);
				}

				@Override
				public void payFailed(Map<String, String> arg0, int arg1) {
					// TODO Auto-generated method stub
					Log.e("Unity", "payFailed" + arg1);
					UnityPlayer.UnitySendMessage("CGame", "OnPurchaseFailed",
							buyID);
				}

				@Override
				public void paySuccess(Map<String, String> arg0) {
					// TODO Auto-generated method stub pid
					Log.e("Unity", "paySuccess");
					UnityPlayer.UnitySendMessage("CGame", "OnPurchaseSuccess",
							buyID);
				}

			});
			break;
		case Const.ChinaUnicom:
			Log.e("Unity", "ChinaUnicom");
			Utils.getInstances().pay(instance, payID,
					new UnipayPayResultListener() {
						@Override
						public void PayResult(String code, int flag, int flag1,
								String error) {
							// TODO Auto-generated method stub
							if (flag == 1) {
								UnityPlayer.UnitySendMessage("CGame",
										"OnPurchaseSuccess", buyID);
							} else if (flag == 2) {
								UnityPlayer.UnitySendMessage("CGame",
										"OnPurchaseFailed", buyID);
							} else if (flag == 3) {
								UnityPlayer.UnitySendMessage("CGame",
										"OnPurchaseFailed", buyID);
							}
							Log.e("Unity", payID + "---" + code + "---" + flag
									+ "---" + flag1 + "---" + error);
						}
					});
			break;
		default:
			UnityPlayer.UnitySendMessage("CGame", "OnPurchaseFailed", buyID);
			break;
		}
	}

	public void order(Context context, String mPaycode,
			OnPurchaseListener listener) {
		Log.e("Unity", "order");
		try {
			purchase.order(context, mPaycode, mListener);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public void MMpaySuccess(String code) {
		if (code == null)
			return;
		Log.e("Unity", "MMpaySuccess" + code);
		try {
			UnityPlayer.UnitySendMessage("CGame", "OnPurchaseSuccess", buyID);
		} catch (Exception e) {
			e.printStackTrace();
		} catch (Error e) {
			e.printStackTrace();
		}
	}

	public void MMpayFaild() {
		Log.e("Unity", "MMpayFaild");
		try {
			UnityPlayer.UnitySendMessage("CGame", "OnPurchaseFailed", buyID);
			Log.e("Unity", "messageSent");
		} catch (Exception e) {
			e.printStackTrace();
		} catch (Error e) {
			e.printStackTrace();
		}
	}

	// 请求视频广告
	public void RequestVideoAd() {
		Log.e("Unity", "Request Video Ad");
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				requestVideoAd();
			}
		});
	}

	private void requestVideoAd() {
		VideoAdManager.getInstance(this).requestVideoAd();
	}

	// 显示视频广告
	public void ShowVideoAds() {
		Log.e("Unity", "Show Video Ads");
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				showVideoAds();
			}
		});
	}

	private void showVideoAds() {
		VideoAdManager.getInstance(this).showVideo(this, new VideoAdListener() {
			// 视频播放失败
			@Override
			public void onVideoPlayFail() {
				Log.d("unity", "failed");
			}

			// 视频播放完成
			@Override
			public void onVideoPlayComplete() {
				Log.d("unity", "complete");
				UnityPlayer
						.UnitySendMessage("CGame", "OnVideoPlayComplete", "");
			}

			// 视频播放完成的记录向服务器发送是否成功
			@Override
			public void onVideoCallback(boolean callback) {
				// 视屏播放记录发送是否回调成功
				Log.d("unity", "completeEffect:" + callback);
			}

			// 视频播放中途退出
			@Override
			public void onVideoPlayInterrupt() {
				Log.d("unity", "interrupt");
				Toast.makeText(context, "视频未播放完成，无法获取奖励", Toast.LENGTH_SHORT)
						.show();
			}

			@Override
			public void onDownloadComplete(String id) {
				Log.e("unity", "download complete: " + id);
			}

			@Override
			public void onNewApkDownloadStart() {
				Log.e("unity", "开始下载");
			}

			@Override
			public void onVideoLoadComplete() {
				Log.e("unity", "视频加载完成");
			}
		});
	}

	// 显示积分广告
	public void ShowOffersAds() {
		Log.e("Unity", "Show Offers Ads");
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				showOffersAd();
			}
		});
	}

	private void showOffersAd() {
		OffersManager.getInstance(this).showOffersWall();
		Log.e("Unity", "AD");
	}

	// 查询积分
	public void QueryPoints() {
		Log.e("Unity", "QueryPoints");
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				queryPoints();
			}
		});
	}

	private void queryPoints() {
		int myPointBalance = PointsManager.getInstance(this).queryPoints();
		UnityPlayer.UnitySendMessage("CGame", "QueryPoints",
				String.valueOf(myPointBalance));
		Log.e("Unity", "AD");
	}

	// 扣除积分
	public void SpendPoints(final int amount) {
		Log.e("Unity", "SpendPoints" + amount);
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				spendPoints(amount);
			}
		});
	}

	private void spendPoints(int amount) {
		boolean isSuccess = PointsManager.getInstance(this).spendPoints(amount);
		String str = "false";
		if (isSuccess)
			str = "true";
		UnityPlayer.UnitySendMessage("CGame", "SpendPoints", str);
		Log.e("Unity", "AD");
	}

	// 增加积分
	public void AwardPoints(final int amount) {
		Log.e("Unity", "AwardPoints" + amount);
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				awardPoints(amount);
			}
		});
	}

	private void awardPoints(int amount) {
		boolean isSuccess = PointsManager.getInstance(this).awardPoints(amount);
		String str = "false";
		if (isSuccess)
			str = "true";
		UnityPlayer.UnitySendMessage("CGame", "AwardPoints", str);
		Log.e("Unity", "AD");
	}

	// 显示插屏广告
	public void ShowAds() {
		Log.e("Unity", "show ADs");
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				showAd();
			}
		});
	}

	private void showAd() {
		SpotManager.getInstance(this).showSpotAds(this);
		Log.e("Unity", "AD");
	}

	public void ExitGame() {
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				exitGame();
			}
		});
	}

	private void exitGame() {
		if (isBaiduChannel) {
			Log.e("Unity", "isBaiduChannel is true");
			DKPlatform.getInstance().bdgameExit(instance, new IDKSDKCallBack() {
				@Override
				public void onResponse(String paramString) {
					finish();
					android.os.Process.killProcess(android.os.Process.myPid());
				}
			});
		}
	}

	public void ShowLeaderboards(final String score) {
		UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				KTPlayLogin(score);
			}
		});
	}

	private static long score;

	// 登录
	public void KTPlayLogin(String s) {
		Log.e("Unity", "score:" + s);
		score = Integer.valueOf(s);
		boolean isLoggedIn = KTAccountManager.isLoggedIn();
		if (isLoggedIn) {
			KTLeaderboard.reportScore(score, Const.KtplayLeaderboardId,
					onReportScoreListener);
		} else {
			KTAccountManager.showLoginView(true, getListener());
		}
	}

	// 登录回调
	private KTLoginListener loginListener;

	public KTLoginListener getListener() {
		if (loginListener == null) {
			loginListener = new KTLoginListener() {
				@Override
				public void onLoginResult(boolean isSuccess, KTUser user,
						KTError error) {
					if (isSuccess) {
						// 上传分数
						KTLeaderboard.reportScore(score, Const.KtplayLeaderboardId,
								onReportScoreListener);
					} else {
						// Toast.makeText(activity, error.description,
						// Toast.LENGTH_SHORT).show();
					}
				}
			};
		}
		return loginListener;
	}

	// 创建排行榜回�?上传分数
	KTLeaderboard.OnReportScoreListener onReportScoreListener = new KTLeaderboard.OnReportScoreListener() {
		@Override
		public void onReportScoreResult(boolean isSuccess,
				String leaderboardId, long score, KTError error) {
			// TODO Auto-generated method stub
			if (isSuccess) {
				// 获取游戏排⾏榜数�?
				KTLeaderboard
						.globalLeaderboard(leaderboardId, 0, 100, listener);
			} else {
				// Toast.makeText(activity, error.description,
				// Toast.LENGTH_SHORT)
				// .show();
			}
		}
	};

	public static String[] name;
	private static String[] gold;
	public static String[] str;
	// 创建回调 获取排行�?
	KTLeaderboard.OnGetLeaderboardListener listener = new KTLeaderboard.OnGetLeaderboardListener() {
		@Override
		public void onGetLeaderboardResult(boolean isSuccess,
				String leaderboardId, KTLeaderboardPaginator leaderboard,
				KTError error) {
			// TODO Auto-generated method stub
			if (isSuccess) {
				ArrayList<KTUser> users = leaderboard.getUsers();
				int size = users.size();
				Log.e("Unity", "Count" + size);
				if (size == 0) {
					Toast.makeText(instance, "暂无排行榜", Toast.LENGTH_SHORT)
							.show();
				} else {
					name = new String[size];
					gold = new String[size];
					str = new String[size];

					for (int i = 0; i < size; i++) {
						name[i] = users.get(i).getNickname().toString();
						gold[i] = users.get(i).getScore().toString();
						str[i] = "第" + (i + 1) + "名:" + name[i] + "     通关数:"
								+ gold[i];
						Log.e("Unity", i + "---" + name[i] + "==" + gold[i]);
					}
					KTPlayShowList(leaderboard.getMyRank(),
							leaderboard.getMyScore());
				}
			} else {
				Toast.makeText(instance, "暂无排行榜", Toast.LENGTH_SHORT).show();
			}
		}
	};

	private void KTPlayShowList(int rank, String score) {
		/**
		 * 测试 // name = new String[100]; // gold = new String[100]; // for(int i
		 * = 0;i<100;i++){ // name[i] = "Gold list + " + i; // gold[i] = "" + i;
		 * }
		 */
		if (name.length == 0 || gold.length == 0) {
			Toast.makeText(instance, "暂无排行榜数据", Toast.LENGTH_SHORT).show();
			Log.e("Unity", "暂无排行榜数据");
		} else {
			Intent intent = new Intent();
			intent.setClassName(this,
					"com.east2west.octopus.LeaderboardsActivity");
			startActivity(intent);
		}
	}
}
