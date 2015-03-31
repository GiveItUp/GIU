using UnityEngine;
using System.Collections;

public class PopupLayer : MonoBehaviour {

    private static float ACTUAL_Z = 0.0f;
	private static float DISTANCE_Z = 500f;

	public void Init()
	{
		ACTUAL_Z = 0;
	}

	
	public ResultsGUI p_ResultsGUI;
	public ResultsSuccessGUI p_ResultsSuccessGUI;
	public InfoPopupGUI p_InfoPopupGUI;
	public UpdatePopupGUI p_UpdatePopupGUI;
	public PurchaseLoadingGUI p_PurchaseLoadingGUI;
	public OptionsGUI p_OptionsGUI;
	public TutorialGUI p_TutorialGUI;
	
	#region ResultsGUI
	private ResultsGUI resultsGUI;
	
	public void ShowResultsGUI(Results results)
	{
		CloseResultsGUI();
		
		ACTUAL_Z -= DISTANCE_Z;
		resultsGUI = GameObject.Instantiate(p_ResultsGUI) as ResultsGUI;
		resultsGUI.transform.parent = transform;
		resultsGUI.transform.localPosition = new Vector3(0, 0, ACTUAL_Z);
		resultsGUI.Init(results);
	}
	
	public void CloseResultsGUI()
	{
		if (resultsGUI != null)
		{
			ACTUAL_Z += DISTANCE_Z;
			GameObject.Destroy(resultsGUI.gameObject);
		}
		resultsGUI = null;
	}
	#endregion
	
	#region ResultsSuccessGUI
	private ResultsSuccessGUI resultsSuccessGUI;
	
	public void ShowResultsSuccessGUI(Results results)
	{
		CloseResultsSuccessGUI();
		
		ACTUAL_Z -= DISTANCE_Z;
		resultsSuccessGUI = GameObject.Instantiate(p_ResultsSuccessGUI) as ResultsSuccessGUI;
		resultsSuccessGUI.transform.parent = transform;
		resultsSuccessGUI.transform.localPosition = new Vector3(0, 0, ACTUAL_Z);
		resultsSuccessGUI.Init(results);
	}
	
	public void CloseResultsSuccessGUI()
	{
		if (resultsSuccessGUI != null)
		{
			ACTUAL_Z += DISTANCE_Z;
			GameObject.Destroy(resultsSuccessGUI.gameObject);
		}
		resultsSuccessGUI = null;
	}
	#endregion
	
	#region InfoPopupGUI
	private InfoPopupGUI infoPopupGUI;
	
	public void ShowInfoPopupGUI(string text)
	{
		CloseInfoPopupGUI();
		
		ACTUAL_Z -= DISTANCE_Z;
		infoPopupGUI = GameObject.Instantiate(p_InfoPopupGUI) as InfoPopupGUI;
		infoPopupGUI.transform.parent = transform;
		infoPopupGUI.transform.localPosition = new Vector3(0, 0, ACTUAL_Z);
		infoPopupGUI.Init(text);
	}
	
	public void CloseInfoPopupGUI()
	{
		if (infoPopupGUI != null)
		{
			ACTUAL_Z += DISTANCE_Z;
			GameObject.Destroy(infoPopupGUI.gameObject);
		}
		infoPopupGUI = null;
	}
	#endregion

	#region UpdatePopupGUI
	private UpdatePopupGUI updatePopupGUI;
	
	public void ShowUpdatePopupGUI()
	{
		CloseUpdatePopupGUI();
		
		ACTUAL_Z -= DISTANCE_Z;
		updatePopupGUI = GameObject.Instantiate(p_UpdatePopupGUI) as UpdatePopupGUI;
		updatePopupGUI.transform.parent = transform;
		updatePopupGUI.transform.localPosition = new Vector3(0, 0, ACTUAL_Z);
		updatePopupGUI.Init();
	}
	
	public void CloseUpdatePopupGUI()
	{
		if (updatePopupGUI != null)
		{
			ACTUAL_Z += DISTANCE_Z;
			GameObject.Destroy(updatePopupGUI.gameObject);
		}
		updatePopupGUI = null;
	}
	#endregion

	#region TutorialGUI
	private TutorialGUI tutorialGUI;
	public void ShowTutorialGUI(string title, Vector2 pos)
	{
		CloseTutorialGUI();
		
		ACTUAL_Z -= DISTANCE_Z;
		tutorialGUI = GameObject.Instantiate(p_TutorialGUI) as TutorialGUI;
		tutorialGUI.transform.parent = transform;
		tutorialGUI.transform.localPosition = new Vector3(0, 0, ACTUAL_Z);
		tutorialGUI.Init(title, pos);
	}
	
	public bool IsTutorialOpen()
	{
		return tutorialGUI != null;
	}
	
	public void CloseTutorialGUI()
	{
		if (tutorialGUI != null)
		{
			ACTUAL_Z += DISTANCE_Z;
			GameObject.Destroy(tutorialGUI.gameObject);
		}
		tutorialGUI = null;
	}
	#endregion
	
	#region PurchaseLoadingGUI
	private PurchaseLoadingGUI purchaseLoadingGUI;
	
	public void ShowPurchaseLoadingGUI()
	{
		ClosePurchaseLoadingGUI();
		
		ACTUAL_Z -= DISTANCE_Z;
		purchaseLoadingGUI = GameObject.Instantiate(p_PurchaseLoadingGUI) as PurchaseLoadingGUI;
		purchaseLoadingGUI.transform.parent = transform;
		purchaseLoadingGUI.transform.localPosition = new Vector3(0, 0, ACTUAL_Z);
		purchaseLoadingGUI.Init();
	}
	
	public void ClosePurchaseLoadingGUI()
	{
		if (purchaseLoadingGUI != null)
		{
			ACTUAL_Z += DISTANCE_Z;
			GameObject.Destroy(purchaseLoadingGUI.gameObject);
		}
		purchaseLoadingGUI = null;
	}
	#endregion
	
	#region OptionsGUI
	private OptionsGUI optionsGUI;
	
	public void ShowOptionsGUI()
	{
		CloseOptionsGUI();
		
		ACTUAL_Z -= DISTANCE_Z;
		optionsGUI = GameObject.Instantiate(p_OptionsGUI) as OptionsGUI;
		optionsGUI.transform.parent = transform;
		optionsGUI.transform.localPosition = new Vector3(0, 0, ACTUAL_Z);
		optionsGUI.Init();
	}
	
	public void CloseOptionsGUI()
	{
		if (optionsGUI != null)
		{
			ACTUAL_Z += DISTANCE_Z;
			GameObject.Destroy(optionsGUI.gameObject);
		}
		optionsGUI = null;
	}
	#endregion


    #region Others
    public static bool HasOpenedPopup()
    {
        return ACTUAL_Z != 0;
    }

	public static float GetActualPopupZ()
	{
		return ACTUAL_Z;
	}
	public void RefreshUserCashInfoGUI()
	{

	}
	#endregion
}
