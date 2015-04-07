using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OptionsGUI : GUIPopup
{
	
	public PackedSprite ps_bgr;
	public SpriteText lbl_title;
	public UIButton btn_menu;
	public UIButton btn_like;
	public UIButton btn_rate;
	public UIButton btn_sounds_on;
	public UIButton btn_sounds_off;
	public UIButton btn_restore_purchase;
	public PackedSprite ps_rate_notification;
	public UIScrollList gamecredits_scr_list;
	public GameCreditsTitle p_GameCreditsTitle;
	public GameCreditsName p_GameCreditsName;
	public GameObject p_GameCreditsStartEmpty;
	public GameObject p_GameCreditsEmpty;
	public JoyStickMenu joyOptionMenu;

	#region Init
	public void Init ()
	{
		_inputEnabled = false;
		ps_bgr.SetSize (GfxSettings.Instance ().GetScreenWidth () + 1, GfxSettings.Instance ().GetScreenHeight () + 1);
		InitLabels ();
		InitButtons ();
		InitScrollList ();

		RefreshSoundButtons ();
		
		ps_rate_notification.gameObject.SetActive (User.NeedRateNotififaction);

		StartCoroutine (PlayShowAnim ());
	}
	#endregion	

	float gamecredits_scr_list_actualpos = 0;
	float speed = 70f;
	#region Unity Methods
	void Update ()
	{
		base.Update ();
		
		if (Input.GetMouseButtonUp (0)) {
			gamecredits_scr_list_actualpos = gamecredits_scr_list.ScrollPosition;
		}
		if (!Input.GetMouseButton (0)) {
			gamecredits_scr_list.ScrollListTo (gamecredits_scr_list_actualpos);
			gamecredits_scr_list_actualpos += Time.deltaTime * speed / gamecredits_scr_list.ContentExtents;
			
			//Debug.Log("ss = " + gamecredits_scr_list_actualpos);
			
			if (gamecredits_scr_list.ScrollPosition >= 1.0f)
				gamecredits_scr_list_actualpos = 0f;
		}
		
	}
	#endregion
	
	#region Methods

	private IEnumerator PlayShowAnim ()
	{
		ComponentAnimation_Prepare (btn_sounds_on.transform);
		ComponentAnimation_Prepare (btn_sounds_off.transform);
		ComponentAnimation_Prepare (btn_menu.transform);
		//ComponentAnimation_Prepare (btn_like.transform);
		//ComponentAnimation_Prepare (btn_rate.transform);
		//ComponentAnimation_Prepare (btn_restore_purchase.transform);

		yield return StartCoroutine (ComponentAnimation_Show (btn_menu.transform, 0.10f));

		if (User.IsSoundEnabled)
			yield return StartCoroutine (ComponentAnimation_Show (btn_sounds_on.transform, 0.07f));
		else
			yield return StartCoroutine (ComponentAnimation_Show (btn_sounds_off.transform, 0.07f));

		yield return new WaitForSeconds (0.2f);

		//yield return StartCoroutine(ComponentAnimation_Show (btn_rate.transform, 0.07f));
		//yield return StartCoroutine(ComponentAnimation_Show (btn_like.transform, 0.11f));
		//yield return StartCoroutine(ComponentAnimation_Show (btn_restore_purchase.transform, 0.10f));

		_inputEnabled = true;

		InitBackButton (() => {
			StartCoroutine (OnMenu ()); });
		joyOptionMenu.enabled = true;
	}

	private IEnumerator PlayHideAnim ()
	{
		joyOptionMenu.enabled = false;
		yield return StartCoroutine (ComponentAnimation_Hide (btn_menu.transform, 0.07f));
		//yield return StartCoroutine(ComponentAnimation_Hide (btn_restore_purchase.transform, 0.07f));
		//yield return StartCoroutine(ComponentAnimation_Hide (btn_like.transform, 0.09f));
		//yield return StartCoroutine(ComponentAnimation_Hide (btn_rate.transform, 0.09f));

		if (User.IsSoundEnabled) {
			yield return StartCoroutine (ComponentAnimation_Hide (btn_sounds_on.transform, 0.06f));
			joyOptionMenu.btns [0] = btn_sounds_on.gameObject;
		} else {
			yield return StartCoroutine (ComponentAnimation_Hide (btn_sounds_off.transform, 0.05f));
			joyOptionMenu.btns [0] = btn_sounds_off.gameObject;
		}

		yield return new WaitForSeconds (0.2f);
	}

	private void InitLabels ()
	{
		lbl_title.Text = TextManager.Get ("Settings");
	}

	private void InitButtons ()
	{
		btn_menu.SetInputDelegate (OnBtn_Menu_Input);
		btn_restore_purchase.SetInputDelegate (OnBtn_RestorePurchase_Input);
		btn_like.SetInputDelegate (OnBtn_Like_Input);
		btn_rate.SetInputDelegate (OnBtn_Rate_Input);

		btn_sounds_on.SetInputDelegate (OnBtn_SoundsOn_Input);
		btn_sounds_off.SetInputDelegate (OnBtn_SoundsOff_Input);
	}

	private void InitScrollList ()
	{
		if (gamecredits_scr_list != null) {
			gamecredits_scr_list.ClearList (true);
		}
		
		List<GameCredits> credits = GameCredits.GetGameCredits ();
		if (credits != null) {
			GameObject gcse = GameObject.Instantiate (p_GameCreditsStartEmpty) as GameObject;
			gamecredits_scr_list.AddItem (gcse.GetComponent<UIListItemContainer> ());
			
			foreach (GameCredits gc in credits) {
				if (gc != null) {
					GameCreditsTitle gct = GameObject.Instantiate (p_GameCreditsTitle) as GameCreditsTitle;
					gct.Init (gc.Title);
					gamecredits_scr_list.AddItem (gct.GetComponent<UIListItemContainer> ());
					
					if (gc.Names != null) {
						foreach (string name in gc.Names) {
							GameCreditsName lbl_name = GameObject.Instantiate (p_GameCreditsName) as GameCreditsName;
							lbl_name.Init (name);
							gamecredits_scr_list.AddItem (lbl_name.GetComponent<UIListItemContainer> ());
						}
					}
					
					GameObject gce = GameObject.Instantiate (p_GameCreditsEmpty) as GameObject;
					gamecredits_scr_list.AddItem (gce.GetComponent<UIListItemContainer> ());
				}
			}
			GameObject gcse2 = GameObject.Instantiate (p_GameCreditsStartEmpty) as GameObject;
			gamecredits_scr_list.AddItem (gcse2.GetComponent<UIListItemContainer> ());
		}
	}

	private void RefreshSoundButtons ()
	{
		btn_sounds_on.gameObject.SetActive (User.IsSoundEnabled);
		btn_sounds_off.gameObject.SetActive (!User.IsSoundEnabled);
		if (User.IsSoundEnabled) {
			joyOptionMenu.btns [0] = btn_sounds_on.gameObject;
		} else {
			joyOptionMenu.btns [0] = btn_sounds_off.gameObject;
		}
	}

	#endregion
	
	#region Handlers
	
	void OnBtn_Menu_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			StartCoroutine (OnMenu ());
			
			break;
		}
	}
	
	private IEnumerator OnMenu ()
	{
		if (_inputEnabled) {
			_inputEnabled = false;
			SoundManager.PlayButtonBackSound ();
			
			yield return StartCoroutine (PlayHideAnim ());
			
			CGame.popupLayer.CloseOptionsGUI ();
			CGame.menuLayer.ShowMainMenuGUI ();
		}
	}
	
	void OnBtn_RestorePurchase_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			OnRestorePurchase ();
			
			break;
		}
	}
	
	private void OnRestorePurchase ()
	{
		if (_inputEnabled) {
			SoundManager.PlayButtonTapSound ();

			//PluginManager.iap.RestorePurchase ();
		}
	}

	void OnBtn_Like_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			OnLike ();
			
			break;
		}
	}
	
	private void OnLike ()
	{
		if (_inputEnabled) {
			SoundManager.PlayButtonTapSound ();
			#if !UNITY_EDITOR
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				CGame.popupLayer.ShowInfoPopupGUI (TextManager.Get ("Please check your internet connection!"));
				return;
			}
			#endif

			FacebookLike ();
		}
	}

	void FacebookLike ()
	{

		#if UNITY_EDITOR
        Application.OpenURL("https://www.facebook.com/275295995837831");

		#elif UNITY_IPHONE
            
		StartCoroutine(IOSFacebookLike());
 
		#else
//		if (checkPackageAppIsPresent ("com.facebook.katana")) {
//			Application.OpenURL ("fb://page/275295995837831"); //there is Facebook app installed so let's use it
//		} else {
//			Application.OpenURL ("https://www.facebook.com/275295995837831"); // no Facebook app - use built-in web browser
//		}
		#endif
	}
    
#if UNITY_IPHONE
    IEnumerator IOSFacebookLike()
    {
		WWW www = new WWW("fb://page/275295995837831");
		yield return www;
		
		if (www.error == null)
		{
			
		}
		else
		{
			Application.OpenURL("https://www.facebook.com/275295995837831");
		}
    }
#endif

#if UNITY_ANDROID
    private bool checkPackageAppIsPresent(string package)
    {
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        //take the list of all packages on the device
        AndroidJavaObject appList = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);
        int num = appList.Call<int>("size");
        for (int i = 0; i < num; i++)
        {
            AndroidJavaObject appInfo = appList.Call<AndroidJavaObject>("get", i);
            string packageNew = appInfo.Get<string>("packageName");
            if (packageNew.CompareTo(package) == 0)
            {
                return true;
            }
        }
        return false;
    }
#endif
	
	void OnBtn_Rate_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			OnRate ();
			
			break;
		}
	}
	
	private void OnRate ()
	{
		if (_inputEnabled) {
			SoundManager.PlayButtonTapSound ();
			
			User.NeedRateNotififaction = false;
			
			CGame.Instance.AskForReviewNow ();
		}
	}
	
	void OnBtn_SoundsOn_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			OnSoundsOn ();
			
			break;
		}
	}
	
	private void OnSoundsOn ()
	{
		if (_inputEnabled) {
			SoundManager.PlayButtonTapSound ();
			User.IsSoundEnabled = false;
			User.Save ();
			RefreshSoundButtons ();
			CGame.Instance.RefreshMute ();
		}
	}
	
	void OnBtn_SoundsOff_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			OnSoundsOff ();
			
			break;
		}
	}
	
	private void OnSoundsOff ()
	{
		if (_inputEnabled) {
			SoundManager.PlayButtonTapSound ();
			User.IsSoundEnabled = true;
			User.Save ();
			RefreshSoundButtons ();
			CGame.Instance.RefreshMute ();
		}
	}

	#endregion
}
