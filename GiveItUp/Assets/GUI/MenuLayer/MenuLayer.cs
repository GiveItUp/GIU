using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuLayer : MonoBehaviour {
	
	public MainMenuGUI p_MainMenuGUI;

	#region MainMenuGUI
	private MainMenuGUI mainMenuGUI;
	
	public void ShowMainMenuGUI()
	{
		CloseMainMenuGUI();

		CGame.Instance.PlayMenuMusic();

		mainMenuGUI = GameObject.Instantiate(p_MainMenuGUI) as MainMenuGUI;
		mainMenuGUI.transform.parent = transform;
		mainMenuGUI.transform.localPosition = new Vector3(0, 0, 0);
		mainMenuGUI.Init();
	}

	public bool HasMainMenu() { return mainMenuGUI != null; }
	
	public void MainMenuGUI_Refresh()
	{
		if (mainMenuGUI != null)
			mainMenuGUI.Refresh ();
	}
	
	public void MainMenuGUI_RefreshScreenSize()
	{
		if (mainMenuGUI != null)
			mainMenuGUI.RefreshScreenSize ();
	}

	public void MainMenuGUI_RefreshGameCenterFriends()
	{
		if (mainMenuGUI != null)
			mainMenuGUI.RefreshGameCenterFriends ();
	}
	
	public void CloseMainMenuGUI()
	{
		if (mainMenuGUI != null)
		{
			GameObject.Destroy(mainMenuGUI.gameObject);
		}
		mainMenuGUI = null;
	}
	#endregion

	#region Others
	public void RefreshUserCashInfoGUI()
	{
	}
	#endregion

}
