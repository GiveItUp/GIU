using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectBallContrl : MonoBehaviour {
	public List<UIButton> balls = new List<UIButton>();
	public UIButton uiButton;
	private Vector3[] posList = {Vector3.zero,new Vector3(-115,0,0),new Vector3(-200,0,0)};
	void Awake()
	{
		string ballName = "Go_BlackBall";
		if(PlayerPrefs.HasKey("ballName"))
		{
			ballName = PlayerPrefs.GetString("ballName");
		}

		while (balls[0].name!=ballName) {
			UIButton temp = balls[0];
			balls.Remove(temp);
			balls.Add(temp);
		}
		ResetOrderbyBalls();
	}
	// Use this for initialization
	void Start () {
		foreach (UIButton item in balls) {
			item.SetInputDelegate (OnBtn_Start_Input);	
		}
		uiButton.SetInputDelegate(OnBtn_Start_Input);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	float y =0;
	void OnBtn_Start_Input (ref POINTER_INFO ptr)
	{
		if(ptr.inputDelta.y>0)
			return;
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.DRAG:
			y += ptr.inputDelta.y;
			ChangeSacle();
			break;
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
		case POINTER_INFO.INPUT_EVENT.RELEASE:
			y = 0;
			ResetOrderbyBalls();
			break;
		}
	}

	void ChangeSacle()
	{
		for (int i = 1; i < balls.Count; i++) {

			UIButton item = balls[i];
			item.transform.parent.localScale += new Vector3(0.008f,0.008f,0);
			balls[0].transform.parent.localScale -= new Vector3(0.008f,0.008f,0);
		}

		for (int i = 1; i < balls.Count; i++) {

			UIButton item = balls[i];
			item.transform.parent.localPosition += new Vector3(4f/i+1f*(i-1),0,0);
			if(i == 1 && item.transform.parent.localPosition.x >0)
			{
				UIButton temp = balls[0];
				balls.Remove(temp);
				balls.Add(temp);
				ResetOrderbyBalls();
				return;
			}
			balls[0].transform.parent.localPosition += new Vector3(0.5f,0,1);
		}
	}

	void ResetOrderbyBalls()
	{
		for (int i = 0; i < balls.Count; i++) {
			balls[i].transform.parent.localScale = Vector3.one -  new Vector3(0.25f,0.25f,0) * i;
			balls[i].transform.parent.localPosition = posList[i];
		}
		MainMenuGUI.inst.ChangeBall(balls[0].name);
	}
}
