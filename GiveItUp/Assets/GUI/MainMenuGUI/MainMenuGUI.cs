using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuGUI : GUIMenu
{
	
	public PackedSprite ps_bgr;
	public PackedSprite ps_logo;
	public PackedSprite ps_info;
	public Transform selectBall;
	public PackedSprite selectBall_bg;
	public UIButton flower_logo;
	public UIButton btn_start;
	public UIButton btn_start_random;
	public UIButton btn_gamecenter;
	public UIButton btn_moregames;
	public UIButton btn_unlock_all;
	public UIButton btn_remove_ads;
	public UIButton btn_options;
	public UIButton btn_share;
	public UIButton btn_prevpage;
	public UIButton btn_nextpage;
	public SpriteText lbl_title;
	public SpriteText lbl_dailychallenge_progress;
	public SpriteText lbl_dailychallenge_time;
	public GameObject playerGroupTitle;
	public GameObject playerGroupTitleBG;
	public GameObject go_ball;
	public GameObject go_levels_1;
	public GameObject go_levels_2;
	public GameObject go_levels_3;
	public JoyStickMenu joyMainMenu;
	public LevelItemGUI p_LevelItemGUI;
	//public PackedSprite bgGameObject;
	private List<LevelItemGUI> levelItemGUIs = new List<LevelItemGUI> ();
	private int _selectedStageIndex = 0;
	private Stage _selectedStage;
	private LevelItemGUI _selectedLevelItemGUI;
	public static MainMenuGUI inst;
	private int page;
	private int pageSize = 9;
	private Wechat wechat;
	private void Awake()
	{
		inst = this;
	}

	#region Init
	public void Init ()
	{
		btn_prevpage.gameObject.SetActive(false);
		btn_nextpage.gameObject.SetActive(false);

		_inputEnabled = false;
		int width = GfxSettings.Instance ().GetScreenWidth () + 1;
		int height = GfxSettings.Instance ().GetScreenHeight () + 1;
		ps_bgr.SetSize (width, height);
		//bgGameObject.transform.localScale = new Vector3(width , height , 1);
		InitLabels ();
		InitButtons ();
		joyMainMenu.btns.Add (btn_unlock_all.gameObject);
		btn_start_random.gameObject.SetActive (true);
		InitLevels ();

		Refresh ();
		PositionThis ();

		//ChangeLevel(User.ActualStage >= User.LastPlayedStage ? User.LastPlayedStage : User.ActualStage);
		ChangeLevel (User.LastPlayedStage);
		//Debug.Log ("act = " + User.LastPlayedStage);
	//	actualPage = User.LastPlayedStage < 18 ? 0 : 1;
		page = PlayerPrefs.GetInt("page",0);
		actualPage = page;
		//ChangeLevels (page);

		StartCoroutine (PlayShowAnim ());

		wechat = Camera.main.gameObject.AddComponent<Wechat> ();
	}
	#endregion

	#region Unity Methods

	#if TEST_BUILD
	void OnGUI()
	{
		if(GUI.Button(new Rect(200,100,100,100), "Reset user"))
		{
			User.Reset();
		}

		if(GUI.Button(new Rect(200,220,100,100), "Complete lvl" + (User.ActualStage + 1)))
		{
			User.CompleteStage(User.ActualStage);
			CGame.menuLayer.CloseMainMenuGUI();
			CGame.menuLayer.ShowMainMenuGUI();
		}
	}
	#endif

	void LateUpdate ()
	{
		System.DateTime now = System.DateTime.Now;
		lbl_dailychallenge_time.Text = (23 - now.Hour).ToString ("00") + ":" + (59 - now.Minute).ToString ("00") + ":" + (59 - now.Second).ToString ("00");

		joyMainMenu.enabled = _inputEnabled;
	}
	
	void OnDestroy ()
	{
		#if UNITY_ANDROID
		EtceteraAndroidManager.alertButtonClickedEvent -= ExitGame;
		#endif
	}

	#endregion
	
	#region Methods

	private IEnumerator PlayShowAnim ()
	{
		ComponentAnimation_Prepare (ps_logo.transform);
		ComponentAnimation_Prepare (ps_info.transform);
		ComponentAnimation_Prepare (btn_start.transform);
		ComponentAnimation_Prepare (selectBall.transform);
		ComponentAnimation_Prepare (selectBall_bg.transform);
		ComponentAnimation_Prepare (flower_logo.transform);
		//if (User.HasIAP_UnlockAll) {
			ComponentAnimation_Prepare (btn_start_random.transform);
		//}
		ComponentAnimation_Prepare (btn_gamecenter.transform);
		//ComponentAnimation_Prepare (btn_moregames.transform);
		ComponentAnimation_Prepare (btn_unlock_all.transform);
		//ComponentAnimation_Prepare (btn_remove_ads.transform);
		ComponentAnimation_Prepare (btn_options.transform);
		ComponentAnimation_Prepare (btn_share.transform);

		//ComponentAnimation_Prepare (btn_prevpage.transform);
		//ComponentAnimation_Prepare (btn_nextpage.transform);

		ComponentAnimation_Prepare (go_ball.transform);
		
		foreach (var lit in levelItemGUIs)
			ComponentAnimation_Prepare (lit.transform);

		yield return new WaitForSeconds (0.2f);
		
		yield return StartCoroutine (ComponentAnimation_Show (ps_logo.transform, 0.2f));
		
		yield return StartCoroutine (ComponentAnimation_Show (btn_start.transform, 0.1f));

		//if (User.HasIAP_UnlockAll) {
			yield return StartCoroutine (ComponentAnimation_Show (btn_start_random.transform, 0.1f));
		//}

		yield return StartCoroutine (ComponentAnimation_Show (btn_options.transform, 0.2f));

		yield return StartCoroutine(ComponentAnimation_Show (btn_gamecenter.transform, 0.11f));

		//yield return StartCoroutine(ComponentAnimation_Show (btn_moregames.transform, 0.12f));

		//		if(!User.HasIAP_RemoveAds && !User.IsPremium)
		//			yield return StartCoroutine(ComponentAnimation_Show (btn_remove_ads.transform, 0.0f, false));
		if (!User.HasIAP_UnlockAll)
			yield return StartCoroutine (ComponentAnimation_Show (btn_unlock_all.transform, 0.1f, false));

		//SoundManager.Instance.Play(SoundManager.eSoundClip.GUI_PopupShowComponent, 1);

		yield return StartCoroutine (ShowLevels (page));

		yield return StartCoroutine (ComponentAnimation_Show (flower_logo.transform, 0.15f));
		yield return StartCoroutine (ComponentAnimation_Show (selectBall_bg.transform, 0.15f));
		yield return StartCoroutine (ComponentAnimation_Show (selectBall.transform, 0.15f));
		yield return StartCoroutine (ComponentAnimation_Show (btn_share.transform, 0.15f));
		yield return StartCoroutine (ComponentAnimation_Show (go_ball.transform, 0.15f));

		#if UNITY_ANDROID
		EtceteraAndroidManager.alertButtonClickedEvent += ExitGame;
		InitBackButton (() => { EtceteraAndroid.showAlert ("退出游戏", "您想退出游戏吗", "是", "否"); });
		#endif

		_inputEnabled = true;

		if (PlayerPrefs.GetInt ("TVMode", 0) != 1) {
			btn_start.GetComponent<Animation>().Play ("ButtonAnim");
		}

//		float timer = 0;
//		while (timer<5f && _inputEnabled == true) {
//			if (Input.touchCount == 0 && !Input.anyKey) {
//				timer += Time.deltaTime;
//			} else {
//				timer = 0;
//			}
//			yield return new WaitForSeconds (Time.deltaTime);
//		}
		yield return StartCoroutine (ComponentAnimation_Show (ps_info.transform, 0.2f));
		joyMainMenu.enabled = true;
	}

	private IEnumerator PlayHideAnim ()
	{
		//StopCoroutine ("PlayShowAnim");
		joyMainMenu.enabled = false;
		yield return StartCoroutine (ComponentAnimation_Hide (go_ball.transform, 0.15f));
		
		SoundManager.Instance.Play (SoundManager.eSoundClip.GUI_Button_back, 1);
		
		yield return StartCoroutine (HideLevels (page));
		
		//yield return StartCoroutine(ComponentAnimation_Hide (btn_moregames.transform, 0.11f, false ));
		//if (User.HasIAP_UnlockAll) {
			yield return StartCoroutine (ComponentAnimation_Hide (btn_start_random.transform, 0.11f));
		//}
		yield return StartCoroutine (ComponentAnimation_Hide (btn_start.transform, 0.11f));
		yield return StartCoroutine(ComponentAnimation_Hide (btn_gamecenter.transform, 0.11f));
		
		if (!User.HasIAP_UnlockAll)
			yield return StartCoroutine (ComponentAnimation_Hide (btn_unlock_all.transform, 0.1f, false));
		
//		if(!User.HasIAP_RemoveAds && !User.IsPremium)
//			yield return StartCoroutine(ComponentAnimation_Hide (btn_remove_ads.transform, 0.0f, false));

		yield return StartCoroutine (ComponentAnimation_Hide (btn_options.transform, 0.1f));

		yield return StartCoroutine (ComponentAnimation_Hide (ps_logo.transform, 0.03f));
		
		yield return StartCoroutine (ComponentAnimation_Hide (flower_logo.transform, 0.15f));
		yield return StartCoroutine (ComponentAnimation_Hide (selectBall.transform, 0.15f));
		yield return StartCoroutine (ComponentAnimation_Hide (selectBall_bg.transform, 0.15f));
		yield return StartCoroutine (ComponentAnimation_Hide (btn_share.transform, 0.1f));

		if (ps_info.gameObject.activeInHierarchy) {
			yield return StartCoroutine (ComponentAnimation_Hide (ps_info.transform, 0.03f));
		}
		yield return new WaitForSeconds (0.3f);
	}

	private void InitLabels ()
	{
		lbl_title.Text = "ImpossiBall";
		lbl_dailychallenge_progress.Text = User.actualRandomLevelRecord == 0 ? "0%" : User.actualRandomLevelScore + "%";
	}

	private void InitButtons ()
	{
		btn_start.SetInputDelegate (OnBtn_Start_Input);
		btn_start_random.SetInputDelegate (OnBtn_StartRandom_Input);
		btn_gamecenter.SetInputDelegate (OnBtn_GameCenter_Input);
		btn_moregames.SetInputDelegate (OnBtn_MoreGames_Input);
		btn_unlock_all.SetInputDelegate (OnBtn_UnlockAll_Input);
		btn_remove_ads.SetInputDelegate (OnBtn_RemoveAds_Input);
		btn_options.SetInputDelegate (OnBtn_Options_Input);
		btn_share.SetInputDelegate (OnBtn_Share_Input);
		flower_logo.SetInputDelegate (OnBtn_flower_logo_Input);
		btn_prevpage.SetInputDelegate (OnBtn_PrevPage_Input);
		btn_nextpage.SetInputDelegate (OnBtn_NextPage_Input);
	}

	private void InitLevels ()
	{
		/*
		level_scr_list.ClearList (true);

		if(CGame.Instance.p_Gamelogic.world._worlds.Count > 0)
		{
			int levelsOnPageCount = 9;
			int page = 0;
			while(true)
			{
				if(CGame.Instance.p_Gamelogic.world._worlds.Count > page * levelsOnPageCount)
				{
					List<World.Stage> levels = CGame.Instance.p_Gamelogic.world._worlds.GetRange (page * levelsOnPageCount, Mathf.Min(9,CGame.Instance.p_Gamelogic.world._worlds.Count - (page * levelsOnPageCount)));

					LevelPackGUI lp = GameObject.Instantiate(p_LevelPackGUI) as LevelPackGUI;
					lp.Init(levels);
					levelPackGUIs.Add(lp);

					level_scr_list.AddItem(lp.GetComponent<UIListItemContainer>());

					page++;
					//Debug.Log ("levels count = " + levels.Count);
				}
				else
					break;
			}
		}*/
		/*
		List<World.Stage> levels = CGame.Instance.p_Gamelogic.world._worlds.GetRange (0, Mathf.Min(9,CGame.Instance.p_Gamelogic.world._worlds.Count));

		LevelPackGUI lp = GameObject.Instantiate(p_LevelPackGUI) as LevelPackGUI;
		lp.transform.parent = this.transform;
		lp.transform.localPosition = new Vector3 (0,0,-1);
		lp.Init(levels);
		*/
		for (int i = 0; i < Storage.Instance._worlds.Count && i < CGame.LEVEL_COUNT; i++) {
			LevelItemGUI lit = GameObject.Instantiate (p_LevelItemGUI) as LevelItemGUI;
			if (i < pageSize)
				lit.transform.parent = go_levels_1.transform;
			else if (i < 2*pageSize)
				lit.transform.parent = go_levels_2.transform;
			else
				lit.transform.parent = go_levels_3.transform;
			lit.transform.localPosition = new Vector3 (-430 + (i % pageSize) * 110, 0, -1);
			lit.Init (i, Storage.Instance._worlds [i], ChangeLevel);
			levelItemGUIs.Add (lit);
			if ((User.GetLevelScore(User.ActualStage)>=100 && User.ActualStage - lit._index ==-1)|| User.ActualStage - lit._index >=0 || User.HasIAP_UnlockAll) {
				//Debug.Log(User.ActualStage +" "+ lit._index);
				joyMainMenu.btns.Add (lit.gameObject);
			}
		}
	}

	private LevelItemGUI GetLevelItemGUI (int index)
	{
		foreach (var lit in levelItemGUIs)
			if (lit._index == index)
				return lit;
		return null;
	}
	int a=0;
	int actualPage {
		get{return a;}
		set{
			if(a != 1 && value == 1)
			{
				a = 1; 
			}
			a = value;

		}
	}

	private IEnumerator ChangeLevels (int page)
	{
		btn_prevpage.gameObject.SetActive(false);
		btn_nextpage.gameObject.SetActive(false);
		yield return StartCoroutine (HideLevels (actualPage));

		actualPage = page;
		PlayerPrefs.SetInt("page",page);
		PlayerPrefs.Save();

		go_levels_1.gameObject.SetActive (actualPage == 0);
		go_levels_2.gameObject.SetActive (actualPage == 1);
		go_levels_3.gameObject.SetActive (actualPage == 2);

		yield return StartCoroutine (ShowLevels (actualPage));
	}

	private IEnumerator ShowLevels (int showPage)
	{
		int startIndex = showPage*pageSize;//actualPage == 0 ? 0 : 9;
		int endIndex = (showPage+1)*pageSize;//actualPage == 0 ? 9 : 18;
		//foreach (var lit in levelItemGUIs)
		for (int i = startIndex; i < levelItemGUIs.Count && i < endIndex; i++)
			yield return StartCoroutine (ComponentAnimation_Show (levelItemGUIs [i].transform, 0.07f));
		if(actualPage!=0 && actualPage != 2)
		{
			btn_prevpage.gameObject.SetActive(true);
		}
		if(actualPage!= 1 && actualPage != 2)
		{
			btn_nextpage.gameObject.SetActive(true);
		}
	}

	private IEnumerator HideLevels (int showPage)
	{
//		if(actualPage == 0)
//			yield return StartCoroutine(ComponentAnimation_Hide (btn_nextpage.transform, 0.1f));
//		else
//			yield return StartCoroutine(ComponentAnimation_Hide (btn_prevpage.transform, 0.1f));

		int startIndex = ((actualPage+1)*pageSize > levelItemGUIs.Count ? levelItemGUIs.Count : (actualPage+1)*pageSize)-1;//actualPage == 0 ? 8 : 17;
		int endIndex = actualPage*pageSize;//actualPage == 0 ? 0 : 9;
		for (int i = startIndex; i >= endIndex; i--)
			yield return StartCoroutine (ComponentAnimation_Hide (levelItemGUIs [i].transform, 0.07f, false));
	}

	private void ChangeLevel (int stageIndex)
	{
		if (stageIndex >= Storage.Instance._worlds.Count || stageIndex >= CGame.LEVEL_COUNT) {
			stageIndex = Mathf.Min (CGame.LEVEL_COUNT - 1, Storage.Instance._worlds.Count - 1);
		}

		//Debug.Log ("stage = " + stageIndex);
		if (_selectedStageIndex != stageIndex) {
			_selectedStageIndex = stageIndex;
			_selectedStage = Storage.Instance._worlds [stageIndex];
			_selectedLevelItemGUI = GetLevelItemGUI (stageIndex);
			go_ball.transform.parent = _selectedLevelItemGUI.go_ball_position.transform;
			go_ball.transform.position = _selectedLevelItemGUI.go_ball_position.transform.position;
			go_ball.transform.localScale = Vector3.one;

			foreach (var lig in levelItemGUIs) {
				if (lig != null) {
					lig.SetSelected (_selectedLevelItemGUI._index == lig._index);
				}
			}
		} else {
			_selectedStageIndex = stageIndex;
			_selectedStage = Storage.Instance._worlds [stageIndex];
			_selectedLevelItemGUI = GetLevelItemGUI (stageIndex);
			go_ball.transform.parent = _selectedLevelItemGUI.go_ball_position.transform;
			go_ball.transform.position = _selectedLevelItemGUI.go_ball_position.transform.position;
			go_ball.transform.localScale = Vector3.one;
			foreach (var lig in levelItemGUIs) {
				if (lig != null) {
					lig.SetSelected (_selectedLevelItemGUI._index == lig._index);
				}
			}
			if((User.GetLevelScore(User.ActualStage)>=100 && User.ActualStage - _selectedStageIndex ==-1)|| User.ActualStage - _selectedStageIndex >=0||User.HasIAP_UnlockAll){
				StartCoroutine (OnStart ());
			}
		}
	}

	public void RefreshGameCenterFriends ()
	{
		foreach (var lig in levelItemGUIs) {
			if (lig != null) {
				lig.InitFriends ();
				if (_selectedLevelItemGUI != null)
					lig.SetSelected (_selectedLevelItemGUI._index == lig._index);
			}
		}
	}

	public void RefreshScreenSize ()
	{
		if (ps_bgr != null)
			ps_bgr.SetSize (GfxSettings.Instance ().GetScreenWidth () + 1, GfxSettings.Instance ().GetScreenHeight () + 1);
	}

	public void Refresh ()
	{
		//btn_remove_ads.gameObject.SetActive (!User.HasIAP_RemoveAds && !User.IsPremium);
		btn_unlock_all.gameObject.SetActive (!User.HasIAP_UnlockAll);
		btn_start_random.gameObject.SetActive (User.HasIAP_UnlockAll);
		PositionThis ();
	}

	private void ExitGame (string s)
	{
		if (s == "是")
			Application.Quit ();
	}

	private void PositionThis ()
	{
		float w = GfxSettings.Instance ().GetScreenWidth () / 2f;
		float h = GfxSettings.Instance ().GetScreenHeight () / 2f;

		btn_remove_ads.transform.localPosition = new Vector3 (w - 75, h - 70, -1);
		btn_unlock_all.transform.localPosition = new Vector3 (w - 80, User.HasIAP_RemoveAds || User.IsPremium ? h - 70 : h - 180, -1);

		btn_options.transform.localPosition = new Vector3 (w - 80, h - 65, -1);
		
		btn_prevpage.transform.localPosition = new Vector3 (-w + 70, -h + 40, -1);
		btn_nextpage.transform.localPosition = new Vector3 (w - 70, -h + 40, -1);
		playerGroupTitle.transform.localPosition = new Vector3 (0, -h + 30, -1);
		playerGroupTitleBG.transform.localScale = new Vector3(2*w-60,45,1);
	}
	#endregion
	
	#region Handlers

	void OnBtn_Start_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			StartCoroutine (OnStart ());
			
			break;
		}
	}
	
	private IEnumerator OnStart ()
	{
		if (_inputEnabled) {
			_inputEnabled = false;
			SoundManager.PlayButtonPlaySound ();

			yield return StartCoroutine (PlayHideAnim ());
			
			CGame.Instance.DestroyMenuMusic ();
			CGame.menuLayer.CloseMainMenuGUI ();
			CGame.Instance.InitGamelogic (_selectedStageIndex, _selectedStage);
		}
	}
	
	void OnBtn_StartRandom_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled) {
				return;
			}
			
			StartCoroutine (OnStartRandom ());
			break;
		}
	}
	
	private IEnumerator OnStartRandom ()
	{
		if (_inputEnabled) {
			_inputEnabled = false;
			SoundManager.PlayButtonPlaySound ();
			
			yield return StartCoroutine (PlayHideAnim ());
			
			CGame.Instance.DestroyMenuMusic ();
			CGame.menuLayer.CloseMainMenuGUI ();
			CGame.Instance.InitGamelogicRandom ();
		}
	}

	void OnBtn_GameCenter_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			OnGameCenter ();
			
			break;
		}
	}
	
	private void OnGameCenter ()
	{
		if (_inputEnabled) {
			SoundManager.PlayButtonTapSound ();
			PluginManager.social.ShowLeaderboards ();
		}
	}
	
	void OnBtn_MoreGames_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			OnMoreGames ();
			
			break;
		}
	}
	
	private void OnMoreGames ()
	{
		if (_inputEnabled) {
			SoundManager.PlayButtonTapSound ();
			//Upsight.sendContentRequest ("more_games", true, false);
		}
	}
	
	void OnBtn_UnlockAll_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			OnUnlockAll ();
			
			break;
		}
	}
	
	private void OnUnlockAll ()
	{
		if (_inputEnabled) {
			SoundManager.PlayButtonTapSound ();
			#if !UNITY_EDITOR
			if ( Application.internetReachability == NetworkReachability.NotReachable ||PluginManager.iap.GetIAPProduct (eIAP.UnlockAll) == null) {//
				CGame.popupLayer.ShowInfoPopupGUI (TextManager.Get ("Please check your internet connection!"));
				return;
			}
			#endif
			PluginManager.iap.PurchaseProduct(eIAP.UnlockAll);
			//CGame.popupLayer.ShowInfoPopupGUI (TextManager.Get ("Unlock all info"));
			#if UNITY_ANDROID
			EtceteraAndroidManager.alertButtonClickedEvent += UnlockAll;
			EtceteraAndroid.showAlert ("解锁全部关卡", TextManager.Get ("Unlock all info"), "好的", "取消");
			#endif
		}
	}

	void UnlockAll (string s)
	{
		if (s == "好的") {
			if (PlayerPrefs.GetInt ("DebugMode", 0) != 1) {
				CGame.popupLayer.ShowPurchaseLoadingGUI ();
				ReinPluginManager.Purchase ("UnlockAll");
			} else {
				CGame.Instance.gameObject.SendMessage ("OnPurchaseSuccess");
			}
		}
	}
	
	void OnBtn_RemoveAds_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			OnRemoveAds ();
			
			break;
		}
	}
	
	private void OnRemoveAds ()
	{
		if (_inputEnabled) {
			SoundManager.PlayButtonTapSound ();
//			#if !UNITY_EDITOR
//			if (Application.internetReachability == NetworkReachability.NotReachable || PluginManager.iap.GetIAPProduct (eIAP.NoAds) == null) {
//				CGame.popupLayer.ShowInfoPopupGUI (TextManager.Get ("Please check your internet connection!"));
//				return;
//			}
//			#endif
			CGame.popupLayer.ShowPurchaseLoadingGUI ();
			//PluginManager.iap.PurchaseProduct (eIAP.NoAds);
		}
	}
	
	void OnBtn_Options_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			StartCoroutine (OnOptions ());
			
			break;
		}
	}

	void OnBtn_Share_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			wechat.Share ();
			
			break;
		}
	}

	void OnBtn_flower_logo_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.TAP:
				if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
					return;
				if(!PlayerPrefs.HasKey("flower"))
				{
					PlayerPrefs.SetInt("flower",page);
					PlayerPrefs.Save();
					page = 2;
				}else
				{
					page = PlayerPrefs.GetInt("flower");
					PlayerPrefs.DeleteKey("flower");
				}
				
				
				StartCoroutine (ChangeLevels (page));

				break;
		}
	}
	
	private IEnumerator OnOptions ()
	{
		if (_inputEnabled) {
			_inputEnabled = false;
			SoundManager.PlayButtonTapSound ();
			
			yield return StartCoroutine (PlayHideAnim ());
			
			CGame.menuLayer.CloseMainMenuGUI ();
			CGame.popupLayer.ShowOptionsGUI ();
		}
	}
	
	void OnBtn_PrevPage_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			StartCoroutine (OnPrevPage ());
			
			break;
		}
	}
	
	private IEnumerator OnPrevPage ()
	{
		if (_inputEnabled) {
			//_inputEnabled = false;
			SoundManager.PlayButtonTapSound ();
			page--;
			if(page<0)
			{
				page = 0;
			}else
			{
				yield return StartCoroutine (ChangeLevels (page));
			}
		}
	}
	
	void OnBtn_NextPage_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			
			StartCoroutine (OnNextPage ());
			
			break;
		}
	}
	
	private IEnumerator OnNextPage ()
	{
		if (_inputEnabled) {
			//_inputEnabled = false;
			SoundManager.PlayButtonTapSound ();
			page++;
			if(page>2)
			{
				page = 2;
			}else
			{
				yield return StartCoroutine (ChangeLevels (page));
			}
		}
	}
	#endregion

	public void OnPlayBtn ()
	{
		StartCoroutine (OnStart ());
	}

	public void ChangeBall(string ballName)
	{
		PlayerPrefs.SetString("ballName",ballName);
		PlayerPrefs.Save();
		CGame.ballName = ballName.Split(new char[]{'_'})[1];
		GameObject temp = go_ball;
		GameObject gb = Resources.Load("Prefabs/Monstar/"+ballName) as GameObject;
		go_ball = GameObject.Instantiate(gb) as GameObject;
		if(temp!=null)
		{
			go_ball.transform.parent = temp.transform.parent;
			GameObject.Destroy(temp);
		}
		else
		{
			go_ball.transform.parent = this.transform;
		}
		go_ball.transform.localScale= Vector3.one;
		go_ball.transform.localPosition= Vector3.zero;
		btn_start = go_ball.transform.Find("btn_start").GetComponent<UIButton>();
		btn_start.SetInputDelegate (OnBtn_Start_Input);
	}
}
