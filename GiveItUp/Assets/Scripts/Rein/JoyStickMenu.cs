using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class JoyStickMenu : MonoBehaviour
{
	public List<GameObject> btns;
	public int numPerLine = 0;
	public bool isHorizontal = true;
	public int defaultIndex = 0;
	public bool scaleSelected = true;
	public bool isLoopIndex=false;
	Vector3[] originalScales;//=new Vector3[btns.Length];
	int selectedIndex = 0;
	float rightTimer = 0f;
	float leftTimer = 0f;
	float downTimer = 0f;
	float upTimer = 0f;
	bool isSupportJoystick = false;

	void Start ()
	{
		if (PlayerPrefs.GetInt ("TVMode", 0) == 1) {
			isSupportJoystick = true;
			//Debug.Log (isSupportJoystick);
		}
		if (isSupportJoystick) {
			selectedIndex = defaultIndex;
			if (scaleSelected) {
				originalScales = new Vector3[btns.Count];
				//Debug.Log (originalScales);
				for (int i=0; i<btns.Count; i++) {
					originalScales [i] = btns [i].transform.localScale;
				}
			}
		}
		Time.timeScale = 1;
	}

	void Update ()
	{
		if (isSupportJoystick) {
			if (Input.GetAxis ("Horizontal") >= JoyStickConfig.menuSensitivity) {
				//Debug.Log (rightTimer);
				rightTimer += Time.deltaTime;
				if (rightTimer > JoyStickConfig.detectDelay) {
					rightTimer = 0;
					OnRightBtn ();
				}
			} else {
				rightTimer = 0;
			}

			if (Input.GetAxis ("Horizontal") <= -JoyStickConfig.menuSensitivity) {
				leftTimer += Time.deltaTime;
				if (leftTimer > JoyStickConfig.detectDelay) {
					leftTimer = 0;
					OnLeftBtn ();

				}
			} else {
				leftTimer = 0;
			}

			if (Input.GetAxis ("Vertical") >= JoyStickConfig.menuSensitivity) {
				//Debug.Log (rightTimer);
				downTimer += Time.deltaTime;
				if (downTimer > JoyStickConfig.detectDelay) {
					downTimer = 0;
					OnDownBtn ();
				}
			} else {
				downTimer = 0;
			}

			if (Input.GetAxis ("Vertical") <= -JoyStickConfig.menuSensitivity) {
				upTimer += Time.deltaTime;
				if (upTimer > JoyStickConfig.detectDelay) {
					upTimer = 0;
					OnUpBtn ();

				}
			} else {
				upTimer = 0;
			}

			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				OnLeftBtn ();
			}
		
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				OnRightBtn ();
			}

			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				OnUpBtn ();
			}

			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				OnDownBtn ();
			}

			if (scaleSelected) {
				for (int i=0; i<btns.Count; i++) {
					//Debug.Log(originalScales);
					btns [i].transform.localScale = originalScales [i];
				}
				btns [selectedIndex].transform.localScale = originalScales [selectedIndex] * JoyStickConfig.menuBtnSelectedScale;
			}
			OnSelectedBtn (btns [selectedIndex]);
			OnSelectedBtn (selectedIndex);
			if (Input.GetKeyUp (KeyCode.Space) || Input.GetKeyUp (KeyCode.Return) || Input.GetKeyUp (KeyCode.JoystickButton0) || Input.GetKeyUp (KeyCode.Menu)) {
				btns [selectedIndex].SendMessage ("OnClick");
				//gameObject.SetActive (false);
				if (scaleSelected) {
					btns [selectedIndex].transform.localScale = originalScales [selectedIndex];
				}
			}
		
			if (Input.GetKeyUp (KeyCode.Escape) || Input.GetKeyUp (KeyCode.JoystickButton1)) {
				OnBackBtn ();
				//gameObject.SetActive (false);
				if (scaleSelected) {
					btns [selectedIndex].transform.localScale = originalScales [selectedIndex];
				}
			}
		}
	}

	public virtual void OnUp ()
	{
			
	}
		
	public virtual void OnDown ()
	{
			
	}

	public virtual void OnSelectedBtn (GameObject selectedBtn)
	{
			
	}

	public virtual void OnSelectedBtn (int index)
	{
			
	}

	public virtual void OnBackBtn ()
	{
	
	}

	public void SetSelectedIndex (int i)
	{
		selectedIndex = i;
	}

	void MoveToNext ()
	{
		selectedIndex++;
//		if (!btns [selectedIndex].activeInHierarchy) {
//			selectedIndex++;
//		}
		ClampIndex ();
	}

	void MoveToLast ()
	{
		selectedIndex--;
//		if (!btns [selectedIndex].activeInHierarchy) {
//			selectedIndex--;
//		}
		ClampIndex ();
	}

	void MoveToNextLine ()
	{
		selectedIndex += numPerLine;
		ClampIndex ();
		//Debug.Log(selectedIndex);
	}

	void MoveToLastLine ()
	{
		selectedIndex -= numPerLine;
		ClampIndex ();
	}

	void ClampIndex(){
		if (isLoopIndex) {
			selectedIndex = selectedIndex<0? btns.Count+selectedIndex: (selectedIndex> btns.Count - 1? selectedIndex-btns.Count:Mathf.Clamp (selectedIndex, 0, btns.Count - 1));
		} else {
			selectedIndex = Mathf.Clamp (selectedIndex, 0, btns.Count - 1);
		}
	}

	void OnRightBtn ()
	{
		if (isHorizontal) {
			MoveToNext ();
		} else {
			MoveToNextLine ();
		}
		//Debug.Log("OnRightBtn" +selectedIndex);
	}

	void OnLeftBtn ()
	{
		if (isHorizontal) {
			MoveToLast ();
		} else {
			MoveToLastLine ();
		}
		//Debug.Log("OnLeftBtn" +selectedIndex);
	}

	void OnUpBtn ()
	{
		if (isHorizontal) {
			MoveToLastLine ();
		} else {
			MoveToLast ();
		}
		OnUp ();
		//Debug.Log("OnUpBtn" +selectedIndex);
	}

	void OnDownBtn ()
	{
		if (isHorizontal) {
			MoveToNextLine ();
		} else {
			MoveToNext ();
		}
		OnDown ();
		//Debug.Log("OnDownBtn" +selectedIndex);
	}
}
