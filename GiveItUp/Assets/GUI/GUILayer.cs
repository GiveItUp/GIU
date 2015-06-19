using UnityEngine;
using System.Collections;

public class GUILayer : MonoBehaviour {

	[System.NonSerialized]
	public bool _inputEnabled = true;

	public delegate void dClick();
	private dClick _click;
	
	protected bool forceShow = false;
	
	protected void InitBackButton(dClick click)
	{
		if(click != null)
		{
			_click = click;
		}
	}

	protected virtual bool IsTopLayer()
	{
		return true;
	}

	protected virtual void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(JoyStickConfig.b))
		{
			if(_click != null && IsTopLayer())
				_click();
		}
	}

	protected void ComponentAnimation_Prepare(Transform t)
	{
		if (t.GetComponent<Animation>() == null)
			t.gameObject.AddComponent<Animation>();
		
		t.GetComponent<Animation>().playAutomatically = false;
		t.GetComponent<Animation>().AddClip(Storage.Instance.animShowGUI, "ShowAnimBubble");
		t.GetComponent<Animation>().AddClip(Storage.Instance.animHideGUI, "HideAnimBubble");
		
		t.gameObject.SetActive(false);
	}
	
	protected IEnumerator ComponentAnimation_Show(Transform t, float wait)
	{
		yield return StartCoroutine (ComponentAnimation_Show(t, wait, true));
	}
	
	protected IEnumerator ComponentAnimation_Show(Transform t, float wait, bool needSound)
	{
		t.gameObject.SetActive(true);
		if (!forceShow && t.GetComponent<Animation>() != null)
			t.GetComponent<Animation>().Play("ShowAnimBubble");

		if(needSound)
			SoundManager.Instance.Play(SoundManager.eSoundClip.GUI_PopupShowComponent, 1);
		
		if (wait > 0 && !forceShow)
			yield return new WaitForSeconds(wait);
	}
	
	protected IEnumerator ComponentAnimation_Hide(Transform t, float wait)
	{
		yield return StartCoroutine (ComponentAnimation_Hide(t, wait, true));
	}
	
	protected IEnumerator ComponentAnimation_Hide(Transform t, float wait, bool needSound)
	{
		t.gameObject.SetActive(true);
		Animation animation = t.GetComponent<Animation>();
		if (!forceShow && animation != null)
		{
			if(animation.GetClip("HideAnimBubble")!=null)
			{
				animation.Play("HideAnimBubble");
			}
		}
		else
		{
			t.gameObject.SetActive(false);
		}
		if(needSound)
			SoundManager.Instance.Play(SoundManager.eSoundClip.GUI_Button_back, 1);
		
		if (wait > 0 && !forceShow)
			yield return new WaitForSeconds(wait);
	}


}
