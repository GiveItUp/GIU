using UnityEngine;
using System.Collections;

public class GameCreditsTitle : MonoBehaviour {

	public SpriteText lbl_title;

    private string title;

    #region Init
    public void Init(string title_)
    {
        title = title_;
        InitLabels();
    }
    #endregion

    #region Methods
    private void InitLabels()
	{
		//lbl_title.borderColor = new Color (0,0,0);
		//lbl_title.topClipPosition = new Vector3(0,280,0);
		//lbl_title.bottomClipPosition = new Vector3(0,-280,0);
		lbl_title.Text = TextManager.Get(title);
		//RefreshLocalization();
	}
	/*
	public void RefreshLocalization()
	{
		lbl_title.SetText(title);

		if(lbl_title.GetNumberOfLines() > 0)
			GetComponent<PackedSprite>().SetSize(0,lbl_title.GetNumberOfLines() * 20);
		else
			GetComponent<PackedSprite>().SetSize(0, 10);
	}*/
    #endregion

    #region Handlers
    #endregion
}
