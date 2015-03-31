using UnityEngine;
using System;
using System.Collections;

public class Btn : MonoBehaviour
{
	Action onClick;
	Action onHold;
	Action onRelease;

	public void Init (Action onBtnClick)
	{
		onClick = onBtnClick;
	}

	public void Init (Action onBtnClick, Action onBtnHold, Action onBtnRelease)
	{
		onClick = onBtnClick;
		onHold = onBtnHold;
		onRelease = onBtnRelease;
	}

//		public void InitCoroutine (Action onBtnClick)
//		{
//			onClickBtn = onBtnClick;
//		}

	void OnMouseDown ()
	{
		if (onClick != null) {
			onClick ();
		}
//			if (onClickBtn != null) {
//				StartCoroutine (onClickBtn());
//			}
		//Debug.Log ("OnMouseDown" + name);
	}

	void OnMouseOver ()
	{
		if (onHold != null) {
			onHold ();
		}
	}

	void OnMouseExit ()
	{
		if (onRelease != null) {
			onRelease ();
		}
	}

	void OnMouseUp ()
	{
		if (onRelease != null) {
			onRelease ();
		}
	}

//		IEnumerator OnClickBtn(){
//			onClickBtn();
//		}
	public void SimulateClick ()
	{
		if (onClick != null) {
			onClick ();
		}
	}
		
}
