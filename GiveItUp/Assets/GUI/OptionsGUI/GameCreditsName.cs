using UnityEngine;
using System.Collections;

public class GameCreditsName : MonoBehaviour {

	public SpriteText lbl_name;

    private string _name;

    #region Init
    public void Init(string name)
    {
        _name = name;
        InitLabels();
    }
    #endregion

    #region Methods
    private void InitLabels()
	{
		//lbl_name.borderColor = new Color (0,0,0);
		//lbl_name.topClipPosition = new Vector3(0,280,0);
		//lbl_name.bottomClipPosition = new Vector3(0,-280,0);
		lbl_name.Text = TextManager.Get(_name);
		//RefreshLocalization();
    }
	/*
	public void RefreshLocalization()
	{
		lbl_name.SetText(_name);

		if(lbl_name.GetNumberOfLines() > 0)
			GetComponent<PackedSprite>().SetSize(0,lbl_name.GetNumberOfLines() * 40);
		else
			GetComponent<PackedSprite>().SetSize(0, 20);
	}*/
    #endregion

    #region Handlers
    #endregion
}
