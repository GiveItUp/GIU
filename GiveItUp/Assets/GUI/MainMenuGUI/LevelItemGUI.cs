using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelItemGUI : MonoBehaviour
{
	
	public UIButton btn_active;
	public UIButton btn_inactive;
	public UIButton btn_completed;
	public PackedSprite ps_bgr_shine;
	public PackedSprite ps_number_shine;
	public SpriteText lbl_index;
	public SpriteText lbl_score;
	public GameObject go_friendpanel;
	public List<GameObject> friendTexturesOnLevel;
	public GameObject go_ball_position;
	[System.NonSerialized]
	public int
		_index;
	private Stage _level;
	private System.Action<int> _onClick;
	private bool _hasFriendOnLevel = false;

	#region Init
	public void Init (int index, Stage level, System.Action<int> onClick)
	{
		_index = index;
		_level = level;
		_onClick = onClick;

		InitLabels ();
		InitButtons ();
		InitFriends ();
	}
	#endregion
	
	#region Methods
	private void InitLabels ()
	{
		lbl_index.Text = "" + (_index + 1);
		lbl_score.Text = User.GetLevelScore (_index) + "%";
	}

	private void InitButtons ()
	{
		btn_active.SetInputDelegate (OnBtn_Start_Input);
		btn_completed.SetInputDelegate (OnBtn_Start_Input);

		bool isenabled = User.ActualStage == _index;
		bool completed = true;//User.GetLevelScore (_index) >= 100 || User.HasIAP_UnlockAll;
		
		btn_active.gameObject.SetActive (false);
		btn_inactive.gameObject.SetActive (!completed && !isenabled);
		btn_completed.gameObject.SetActive (completed || isenabled);
		
		//btn_active.gameObject.SetActive (!completed && isenabled);
		//btn_inactive.gameObject.SetActive (!completed && !isenabled);
		//btn_completed.gameObject.SetActive (completed && !isenabled);

		ps_bgr_shine.gameObject.SetActive (completed || isenabled);
		ps_number_shine.gameObject.SetActive (completed || isenabled);

		lbl_score.gameObject.SetActive (completed || isenabled);

		go_ball_position.transform.localPosition = new Vector3 (0, completed || isenabled ? 90 : 46, -10);
	}

	public void InitFriends ()
	{
		_hasFriendOnLevel = false;
		List<Texture2D> textures = CGame.Instance.GetGameCenterFriendTexturesOnStage (_index);
		if (textures != null) {
			for (int i = 0; i < 3; i++) {
				if (textures.Count > i && textures [i] != null) {
					friendTexturesOnLevel [i].gameObject.SetActive (true);
					friendTexturesOnLevel [i].GetComponent<Renderer>().material.mainTexture = textures [i];
					_hasFriendOnLevel = true;
				} else {
					friendTexturesOnLevel [i].gameObject.SetActive (false);
				}
			}
		}

		if (!_hasFriendOnLevel) {
			go_friendpanel.gameObject.SetActive (false);
		}
	}

	public void SetSelected (bool selected)
	{
		//Debug.Log ("index = "+_index+" - selected = " + selected);
		if (selected)
			go_friendpanel.gameObject.SetActive (false);
		else
			go_friendpanel.gameObject.SetActive (_hasFriendOnLevel);
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
			
			OnStart ();
			
			break;
		}
	}
	
	private void OnStart ()
	{
		if (_onClick != null) {
			SoundManager.PlayButtonTapSound ();
			_onClick (_index);
		}
	}
	#endregion

	void OnClick ()
	{
		OnStart ();
	}
}
