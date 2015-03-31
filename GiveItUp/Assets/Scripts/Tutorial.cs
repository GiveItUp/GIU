using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public static Tutorial instance;

	public enum eTutorialState
	{
		simple,
		stair,
		stair1,
		stair2,
		stair3,
		down,
		hideDown,
		dontJump,
		finalJump,
		goodLuck,
		theEnd
	}

	private delegate void UpdateDelegate();
	private UpdateDelegate UpdateMethod;
	
	
	public static bool isInputEnabled = true;
	private static bool isStopped = false;

	private bool isSkipState = false;

	private float fixedDeltaTime;

	void SetState( eTutorialState state )
	{
		isSkipState = false;

		switch (state)
		{
			case eTutorialState.simple:
				UpdateMethod = Simple;
				isInputEnabled = false;
				isStopped = false;
				if (PlayerPrefs.GetInt("TUTORIAL 1", 0) != 0)
				{
					//SetState(eTutorialState.stair);
					Debug.Log("skip simple");
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 1", 1);
				break;
			case eTutorialState.stair:
				UpdateMethod = Stair;
				if (PlayerPrefs.GetInt("TUTORIAL 2", 0) != 0)
				{
					//SetState(eTutorialState.stair1);
					Debug.Log("skip stair");
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 2", 1);
				break;
			case eTutorialState.stair1:
				UpdateMethod = Stair1;
				if (PlayerPrefs.GetInt("TUTORIAL 3", 0) != 0)
				{
					//SetState(eTutorialState.stair2);
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 3", 1);
				break;
			case eTutorialState.stair2:
				UpdateMethod = Stair2;
				if (PlayerPrefs.GetInt("TUTORIAL 4", 0) != 0)
				{
					//SetState(eTutorialState.stair3);
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 4", 1);
				break;
			case eTutorialState.stair3:
				UpdateMethod = Stair3;
				if (PlayerPrefs.GetInt("TUTORIAL 5", 0) != 0)
				{
					//SetState(eTutorialState.down);
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 5", 1);
				break;
			case eTutorialState.down:
				UpdateMethod = Down;
				if (PlayerPrefs.GetInt("TUTORIAL 6", 0) != 0)
				{
					//SetState(eTutorialState.hideDown);
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 6", 1);
				break;
			case eTutorialState.hideDown:
				UpdateMethod = HideDown;
				if (PlayerPrefs.GetInt("TUTORIAL 7", 0) != 0)
				{
					//SetState(eTutorialState.dontJump);
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 7", 1);
				break;
			case eTutorialState.dontJump:
				UpdateMethod = DontJump;
				if (PlayerPrefs.GetInt("TUTORIAL 8", 0) != 0)
				{
					//SetState(eTutorialState.finalJump);
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 8", 1);
				break;
			case eTutorialState.finalJump:
				UpdateMethod = FinalJump;
				if (PlayerPrefs.GetInt("TUTORIAL 9", 0) != 0)
				{
					//SetState(eTutorialState.goodLuck);
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 9", 1);
				break;
			case eTutorialState.goodLuck:
				UpdateMethod = GoodLuck;
				if (PlayerPrefs.GetInt("TUTORIAL 10", 0) != 0)
				{
					//SetState(eTutorialState.theEnd);
					isSkipState = true;
				}
				//PlayerPrefs.SetInt("TUTORIAL 10", 1);
				break;
			case eTutorialState.theEnd:
				UpdateMethod = TheEnd;
				break;
		}
	}
	

	// Use this for initialization
	void Start () {

		if (PlayerPrefs.GetInt("HasPlayedTutorial", 0) != 0 || CGame.gamelogic.GetStageIndex() != 0)
		{
			Destroy(this);
			return;
		}

		fixedDeltaTime = Time.fixedDeltaTime;
		SetState(eTutorialState.simple);
		CGame.gamelogic.hud.lbl_stage.Text = " ";
		CGame.gamelogic.hud.lbl_tries.Text = " ";
	}
	
	void Update()
	{
		UpdateMethod();
	}

	IEnumerator EnableInput(bool enab)
	{
		float t = 0;
		while ( t < 1 )
		{
			yield return 0;
			
			t += Time.deltaTime * (1f/Time.timeScale);
		}
		
		isInputEnabled = enab;
	}

	IEnumerator SetTimeScale(float timeScale)
	{
		if (timeScale == 1) {
						CGame.gamelogic.ball.updateRunning = true;
			CGame.gamelogic.ball.prevMusicTimeDouble = AudioSettings.dspTime;
				}else
			CGame.gamelogic.ball.updateRunning = false;

		yield return 0;
		Time.timeScale = timeScale;

		if (timeScale == 1)
		{
			if(CGame.gamelogic.state == Gamelogic.eState.ingame)
				CGame.gamelogic.music.Play();
		}
		else
		{
			CGame.gamelogic.music.Pause();
		}
	}
	
	IEnumerator LerpTimescale( float to, float duration)
	{
		float t = 0;
		float from = Time.timeScale;
		
		while ( t < duration)
		{
			Time.timeScale = Mathf.Lerp( from, to, t / duration);
			if (Time.timeScale <= to)
			{
				Time.timeScale = to;
			}
			Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
						
			yield return 0;
			
			t += Time.deltaTime * (1f/Time.timeScale);
		}
		
		Time.timeScale = to;
		Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
	}

	private float tDiff;

	void Simple()
	{
		if ( !isStopped && CGame.gamelogic.ball.transform.position.x > 7.8f)
		{
			if (CGame.gamelogic.ball.t > 0.5f)
			{
				tDiff = -(1 - CGame.gamelogic.ball.t);
				CGame.gamelogic.ball.t = 1;
			}
			else
			{
				tDiff = CGame.gamelogic.ball.t;
				CGame.gamelogic.ball.t = 0;
			}
			
			CGame.gamelogic.ball.UpdatePos();
			isStopped = true;
			StartCoroutine(EnableInput(true));
			StartCoroutine(SetTimeScale(0.0001f));

			CGame.popupLayer.ShowTutorialGUI("Tap at touchdown to make Blob jump. The rhythm of the music may help.", new Vector2(250, 50));
		}
		if (isStopped || isSkipState)
		{
			if (((Input.GetMouseButtonDown(0)||Input.GetKeyDown(JoyStickConfig.a)||Input.GetKeyDown (KeyCode.UpArrow)||Input.GetKeyDown (KeyCode.Return)) && isInputEnabled) || isSkipState)
			{
				PlayerPrefs.SetInt("TUTORIAL 1", 1);
				isStopped = false;
				isInputEnabled = true;
				StartCoroutine(SetTimeScale(1f));
				SetState(eTutorialState.stair);
				//StartCoroutine(EnableInput(e));
				CGame.popupLayer.CloseTutorialGUI();
			}
		}
	}
	

	void Stair()
	{
		if ( !isStopped && CGame.gamelogic.ball.transform.position.x > 30.5f)
		{
			isInputEnabled = false;
		}
		if ( !isStopped && CGame.gamelogic.ball.transform.position.x > 31.8f)
		{
			if (CGame.gamelogic.ball.t > 0.5f)
			{
				tDiff = -(1 - CGame.gamelogic.ball.t);
				CGame.gamelogic.ball.t = 1;
			}
			else
			{
				tDiff = CGame.gamelogic.ball.t;
				CGame.gamelogic.ball.t = 0;
			}
			
			CGame.gamelogic.ball.UpdatePos();
			
			isStopped = true;
			StartCoroutine(EnableInput(true));
			StartCoroutine(SetTimeScale(0.0001f));

			CGame.popupLayer.ShowTutorialGUI("Tap to jump up. Blob knows what to do.", new Vector2(250, 50));
		}
		if (isStopped || isSkipState)
		{
			if (((Input.GetMouseButtonDown(0)||Input.GetKeyDown(JoyStickConfig.a)||Input.GetKeyDown (KeyCode.UpArrow)||Input.GetKeyDown (KeyCode.Return)) && isInputEnabled) || isSkipState)
			{
				PlayerPrefs.SetInt("TUTORIAL 2", 1);
				isStopped = false;
				StartCoroutine(SetTimeScale(1f));
				SetState(eTutorialState.stair1);

				CGame.popupLayer.CloseTutorialGUI();
			}
		}
	}

	void Stair1()
	{
		if ( !isStopped && CGame.gamelogic.ball.transform.position.x > 54.5f)
		{
			isInputEnabled = false;
		}
		if ( !isStopped && CGame.gamelogic.ball.transform.position.x > 55.8f)
		{
			if (CGame.gamelogic.ball.t > 0.5f)
			{
				tDiff = -(1 - CGame.gamelogic.ball.t);
				CGame.gamelogic.ball.t = 1;
			}
			else
			{
				tDiff = CGame.gamelogic.ball.t;
				CGame.gamelogic.ball.t = 0;
			}
			
			CGame.gamelogic.ball.UpdatePos();
			
			isStopped = true;
			StartCoroutine(EnableInput(true));
			StartCoroutine(SetTimeScale(0.0001f));

			CGame.popupLayer.ShowTutorialGUI("It's slightly harder: tap three times in a row to the rhythm of the jump.", new Vector2(-300, 100));

		}
		if (isStopped || isSkipState)
		{
			if (((Input.GetMouseButtonDown(0)||Input.GetKeyDown(JoyStickConfig.a)||Input.GetKeyDown (KeyCode.UpArrow)||Input.GetKeyDown (KeyCode.Return)) && isInputEnabled) || isSkipState)
			{
				PlayerPrefs.SetInt("TUTORIAL 3", 1);
				isStopped = false;
				StartCoroutine(SetTimeScale(1f));
				SetState(eTutorialState.stair2);
			}
		}
	}
	
	void Stair2()
	{
		if ( !isStopped && CGame.gamelogic.ball.transform.position.x > 56.8f)
		{
			if (CGame.gamelogic.ball.t > 0.5f)
			{
				tDiff = -(1 - CGame.gamelogic.ball.t);
				CGame.gamelogic.ball.t = 1;
			}
			else
			{
				tDiff = CGame.gamelogic.ball.t;
				CGame.gamelogic.ball.t = 0;
			}
			
			CGame.gamelogic.ball.UpdatePos();
			
			isStopped = true;
			StartCoroutine(SetTimeScale(0.0001f));

			CGame.popupLayer.ShowTutorialGUI("TAP", new Vector2(-230, 15));
		}
		if (isStopped || isSkipState)
		{
			if (((Input.GetMouseButtonDown(0)||Input.GetKeyDown(JoyStickConfig.a)||Input.GetKeyDown (KeyCode.UpArrow)||Input.GetKeyDown (KeyCode.Return)) && isInputEnabled) || isSkipState)
			{
				PlayerPrefs.SetInt("TUTORIAL 4", 1);
				isStopped = false;
				StartCoroutine(SetTimeScale(1f));
				SetState(eTutorialState.stair3);

			}
		}
		
	}
	
	void Stair3()
	{
		if ( !isStopped && CGame.gamelogic.ball.transform.position.x > 57.8f)
		{
			if (CGame.gamelogic.ball.t > 0.5f)
			{
				tDiff = -(1 - CGame.gamelogic.ball.t);
				CGame.gamelogic.ball.t = 1;
			}
			else
			{
				tDiff = 1 - CGame.gamelogic.ball.t;
				CGame.gamelogic.ball.t = 0;
			}
			
			CGame.gamelogic.ball.UpdatePos();
			
			isStopped = true;
			StartCoroutine(SetTimeScale(0.0001f));

			CGame.popupLayer.ShowTutorialGUI("TAP", new Vector2(-220, 115));
		}
		if (isStopped || isSkipState)
		{
			if (((Input.GetMouseButtonDown(0)||Input.GetKeyDown(JoyStickConfig.a)||Input.GetKeyDown (KeyCode.UpArrow)||Input.GetKeyDown (KeyCode.Return)) && isInputEnabled) || isSkipState)
			{
				PlayerPrefs.SetInt("TUTORIAL 5", 1);
				isStopped = false;
				StartCoroutine(SetTimeScale(1f));
				SetState(eTutorialState.down);

				CGame.popupLayer.CloseTutorialGUI();
			}
		}
	}

	void Down()
	{
		if ( (!CGame.popupLayer.IsTutorialOpen() && CGame.gamelogic.ball.transform.position.x > 59f) || isSkipState)
		{
			if (!isSkipState)
				CGame.popupLayer.ShowTutorialGUI("Blob is jumping down by itself.", new Vector2(250, 220));
			SetState (eTutorialState.hideDown);
			PlayerPrefs.SetInt("TUTORIAL 6", 1);
		}
	}
	
	void HideDown()
	{
		if ( (CGame.popupLayer.IsTutorialOpen() && CGame.gamelogic.ball.transform.position.x > 67f) || isSkipState)
		{
			CGame.popupLayer.CloseTutorialGUI();
			SetState (eTutorialState.dontJump);
			PlayerPrefs.SetInt("TUTORIAL 7", 1);
		}
	}
	
	void DontJump()
	{
		if ( (!CGame.popupLayer.IsTutorialOpen() && CGame.gamelogic.ball.transform.position.x > 111f) || isSkipState)
		{
			if (!isSkipState)
				CGame.popupLayer.ShowTutorialGUI("You don't always have to jump: take care of spikes hanging down.", new Vector2(250, 220));
			SetState (eTutorialState.finalJump);
			PlayerPrefs.SetInt("TUTORIAL 8", 1);
		}

	}

	void FinalJump()
	{
		if ( !isStopped && CGame.gamelogic.ball.transform.position.x > 125.5f)
		{
			isInputEnabled = false;
		}
		if ( !isStopped && CGame.gamelogic.ball.transform.position.x > 128.8f)
		{
			if (CGame.gamelogic.ball.t > 0.5f)
			{
				tDiff = -(1 - CGame.gamelogic.ball.t);
				CGame.gamelogic.ball.t = 1;
			}
			else
			{
				tDiff = 1 - CGame.gamelogic.ball.t;
				CGame.gamelogic.ball.t = 0;
			}
			
			CGame.gamelogic.ball.UpdatePos();
			
			isStopped = true;
			//isInputEnabled = true;
			StartCoroutine(EnableInput(true));
			//Time.timeScale = 0.0001f;
			StartCoroutine(SetTimeScale(0.0001f));

			CGame.popupLayer.ShowTutorialGUI("Tap now!", new Vector2(250, 220));
		}
		if (isStopped || isSkipState)
		{
			if (((Input.GetMouseButtonDown(0)||Input.GetKeyDown(JoyStickConfig.a)||Input.GetKeyDown (KeyCode.UpArrow)||Input.GetKeyDown (KeyCode.Return)) && isInputEnabled) || isSkipState)
			{
				PlayerPrefs.SetInt("TUTORIAL 9", 1);
				isStopped = false;
				StartCoroutine(SetTimeScale(1f));
				SetState(eTutorialState.goodLuck);

				CGame.popupLayer.CloseTutorialGUI();
			}
		}
	}

	void GoodLuck()
	{
		if ( !CGame.popupLayer.IsTutorialOpen() && CGame.gamelogic.ball.transform.position.x > 131.8f)
		{
			CGame.popupLayer.ShowTutorialGUI("GOOD LUCK", new Vector2(250, 220));
		}
		if ((CGame.popupLayer.IsTutorialOpen() && CGame.gamelogic.ball.transform.position.x > 140.8f) || isSkipState)
		{
			PlayerPrefs.SetInt("TUTORIAL 10", 1);
			CGame.popupLayer.CloseTutorialGUI();
			SetState(eTutorialState.theEnd);
		}
	}

	void TheEnd()
	{

	}
}
