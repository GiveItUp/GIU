﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectBallContrl : MonoBehaviour {
	public List<UIButton> balls = new List<UIButton>();
	private Vector3[] posList = {Vector3.zero,new Vector3(-115,0,0),new Vector3(-200,0,0)};
	private string defalutBallName = "";
	void Awake()
	{
		defalutBallName = "Go_BlackBall";
		if(PlayerPrefs.HasKey("ballName"))
		{
			defalutBallName = PlayerPrefs.GetString("ballName");
		}

		while (balls[balls.Count-1].name!=defalutBallName) {
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
		//uiButton.SetInputDelegate(OnBtn_Start_Input);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnBtn_Start_Input (ref POINTER_INFO ptr)
	{
		switch (ptr.evt) {
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (!((UIButton)(ptr.targetObj)).IsReleaseEnabled)
				return;
			ChangeSacle(ptr.targetObj.gameObject.GetComponent<UIButton>());
			
			break;
		}

	}

	public void SelectedRed()
	{
		string ballName = "Go_PinkBall";
		while (balls[balls.Count-1].name!=ballName) {
			UIButton temp = balls[0];
			balls.Remove(temp);
			balls.Add(temp);
		}
		ResetOrderbyBalls();
	}

	public void SelectedBlack()
	{
		while (balls[balls.Count-1].name!=defalutBallName) {
			UIButton temp = balls[0];
			balls.Remove(temp);
			balls.Add(temp);
		}
		ResetOrderbyBalls();
	}

	void ChangeSacle(UIButton temp)
	{
		defalutBallName = temp.name;
		balls.Remove(temp);
		balls.Add(temp);
		ResetOrderbyBalls();
	}

	void ResetOrderbyBalls()
	{
		for (int i = 0; i < balls.Count; i++) {
			balls[i].gameObject.SetActive(false);
			if(i==0)
			{
				balls[i].gameObject.SetActive(true);
			}
		}
		MainMenuGUI.inst.ChangeBall(balls[balls.Count-1].name);
	}
}
