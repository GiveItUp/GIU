using UnityEngine;
using System.Collections;

public class IngameBgr : MonoBehaviour {

	public GameObject go_bgr;
	public GameObject go_bgr_1;
	private GameObject go_bgr_temp;
	private bool m_failed;
	//public GameObject go_bgr_failed;
	public SpriteText lbl_progress;
	
	private float _lastProgress = -1;

	public void SetProgress(float progress)
	{
		if(_lastProgress != progress)
		{
			lbl_progress.Text = "" + Mathf.RoundToInt(progress * 100f) + "%";
		}
	}

	public void SetBgr(bool failed)
	{
		m_failed = failed;
		go_bgr_temp.GetComponent<Renderer>().material.color = failed ? new Color(0.5f, 0f, 0f) : new Color (0.5f, 0.5f, 0.5f);
		go_bgr_temp.transform.localScale = new Vector3 ((7f * (float)Screen.width) / (float)Screen.height, 7, 1);
	}

	public void ChangeBg(int index)
	{
		if(index == 1)
		{
			go_bgr.SetActive(false);
			go_bgr_1.SetActive(true);
			go_bgr_temp = go_bgr_1;
		}else
		{
			go_bgr.SetActive(true);
			go_bgr_1.SetActive(false);
			go_bgr_temp = go_bgr;
		}

	}

	private float animProgress = -1;
	private bool _success = false;
	public void PlayFadeAnim(bool success)
	{
		_success = success;
		animProgress = 0;
	}

	void Update()
	{
		if(animProgress >= 0)
		{
			if(animProgress > 1)
				animProgress = 1;

			go_bgr_temp.GetComponent<Renderer>().material.color = Color.Lerp(_success ? new Color(0f, 0.5f, 0f) : new Color(0.5f, 0f, 0f), new Color (0.5f, 0.5f, 0.5f), animProgress);

			if(animProgress == 1)
				animProgress = -1;
			else
				animProgress += Time.deltaTime;
		}
	}
}
