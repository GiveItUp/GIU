using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	bool isActive = false;
	bool isJumpNeed = false;
	bool isJumpedOnThisColumn = false;
	public Pislogas anims;
	Vector3 pos;
	float radius;
	float angle;
	[HideInInspector]
	public float
		speed;
	[HideInInspector]
	public float
		t;
	Vector3 pFrom, pTo, vFrom, vTo;
	[HideInInspector]
	public float
		GAME_SPEED_SCALE = 1.25f; // itt hiaba adsz neki erteket
	float INPUT_JUMP_THRESHOLD = 0.3f;
	float jumpTimeout = 0f;
    
    
	public delegate void OnJumpedDelegate ();

	public OnJumpedDelegate OnJumped;
	public PackedSprite placcs;

	void Start ()
	{
		GAME_SPEED_SCALE = 1.18f;
		//GAME_SPEED_SCALE = 0.9f;
		radius = transform.localScale.y * 0.5f;
	}

	public void Init ()
	{
		Column c0 = CGame.gamelogic.world.GetFirstColumn ();

		pos = c0.states [0].pivots [0].transform.position + Vector3.up * radius;
		b_pos = pos;

		GetComponent<Rigidbody2D>().MovePosition (pos);
		transform.position = pos;
		CGame.cameraLogic.transform.position = new Vector3 (pos.x + 2f, 1.65f, -13f);

		angle = 0;
		t = 1;
		speed = 2.5f;
		DoJump ();
		User.PlayCount++;
		
		PluginManager.social.ReportAchievement (eAchievement.play_game_50, (float)User.PlayCount / 50f * 100f);
		PluginManager.social.ReportAchievement (eAchievement.play_game_100, (float)User.PlayCount / 100f * 100f);
		PluginManager.social.ReportAchievement (eAchievement.play_game_200, (float)User.PlayCount / 200f * 100f);
		PluginManager.social.ReportAchievement (eAchievement.play_game_500, (float)User.PlayCount / 500f * 100f);
		PluginManager.social.ReportAchievement (eAchievement.play_game_1000, (float)User.PlayCount / 1000f * 100f);

		PluginManager.social.SubmitScore (eLeaderboard.Attempts, (long)User.PlayCount);
	}
		
	[HideInInspector]
	public bool
		updateRunning = true;

	void Update ()
	{
		if (!isActive || !updateRunning)
			return;
//#if !UNITY_EDITOR
//        if (Input.touchCount > 0)
//#else
		if ((Input.touchCount > 0||Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (JoyStickConfig.a) || Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown (0)) && !isFailSwitch && Tutorial.isInputEnabled) {
//#endif
			if (t > 1 - INPUT_JUMP_THRESHOLD) { // Ha a holtpont elott van a tap
				isJumpNeed = true;
			} else if (t < INPUT_JUMP_THRESHOLD && !isJumpedOnThisColumn) { // Ha a holtpont utan van a tap
				isJumpNeed = true;
				DoJump (true);
			}
			isJumpedOnThisColumn = true;
		}

#if UNITY_ANDROID2
		NotFixedUpdate();
		transform.position = b_pos;
#else
		transform.position = Vector3.Lerp (transform.position, b_pos, 0.5f);
#endif
	}

	Vector3 b_pos = Vector3.zero;
	float b_angle = 0f;
	//int	prevSamples;
	///float prevMusicTime;
	public double prevMusicTimeDouble;

#if UNITY_ANDROID2
    void NotFixedUpdate()
#else
	void FixedUpdate ()
#endif
	{
		if (!isActive || !updateRunning)
			return;
#if UNITY_ANDROID2
        float dt = Time.deltaTime;
#else
		float dt = Time.fixedDeltaTime;
#endif
		//variációk ütemezésre:
		///v1
		///int curSamples = CGame.gamelogic.music.timeSamples;
		///float dt2 = (curSamples-prevSamples)/((float)CGame.gamelogic.music.audio.clip.frequency);
		//v2
		//float musicTime = CGame.gamelogic.music.time;
		//float dt2=musicTime-prevMusicTime;
		////v3
		double musicTimeDouble = AudioSettings.dspTime;
		float dt2 = (float)(musicTimeDouble - prevMusicTimeDouble);
		if (prevMusicTimeDouble == 0.0)
			dt2 = 0f;
		//	Debug.Log( "dt" + dt+ " " + dt2 );
		///prevSamples = curSamples;
		//prevMusicTime=musicTime;
		prevMusicTimeDouble = musicTimeDouble;
		dt = dt2;
		if (!CGame.gamelogic.music.isPlaying)
			dt = 0f;

		angle -= speed * 40f * dt;

		t += dt * speed * GAME_SPEED_SCALE;
		if (t >= 1f) {        

			t -= 1f;
            
			DoJump ();

			if (CGame.gamelogic.OnJumped != null)
				CGame.gamelogic.OnJumped ();
            
			if (CGame.gamelogic.world.IsItTheEnd (transform.position.x))
				CGame.gamelogic.ChangeState (Gamelogic.eState.results_success);
		}

		pos = Util.Hermite (pFrom, vFrom, pTo, vTo, t);
		b_pos = pos;
		b_angle = angle;
//		GetComponent<Rigidbody2D>().MoveRotation (angle);
	}
    
	public void UpdatePos ()
	{
		pos = Util.Hermite (pFrom, vFrom, pTo, vTo, t);
		transform.position = new Vector3 (pos.x, pos.y, 0.0f);
		b_pos = transform.position;
	}

	bool isFailSwitch = false;

	void DoJump (bool lateInput = false)
	{
		//Android sound delay debug
		//itt derült ki, hogy a rendszer azt hiszi, jó helyen tart a hanglejátszással, de a hangszórókból bizony megkésve jön a hang :(
		//PC-n és jó nag késésű HTC-n is hasonlóak a logolt értékek
		//Debug.Log("jump" + syncStep+" "+ CGame.gamelogic.music.timeSamples);
		//syncStep++;

		User.JumpCount++;
		Column c0 = CGame.gamelogic.world.GetCurrentColumn (pos.x);
		Column c1 = CGame.gamelogic.world.GetNextColumn (pos.x, 1);
		Column c2 = CGame.gamelogic.world.GetNextColumn (pos.x, 2);

		c0.SetColor (1);
        
		Vector3 p0;
		Vector3 p1;
		Vector3 p2;
        
		p0 = c0.states [c0.currentState].pivots [0].transform.position;
		if (c0.states [c0.currentState].pivots.Length >= 2) {
			p1 = c0.states [c0.currentState].pivots [1].transform.position;
		} else {
			p1 = c1.states [c1.currentState].pivots [0].transform.position;
		}
		if (c0.states [c0.currentState].pivots.Length >= 3) {
			p2 = c0.states [c0.currentState].pivots [2].transform.position;
		} else {
			if (c1.states [c1.currentState].pivots.Length >= 2) {
				p2 = c1.states [c1.currentState].pivots [1].transform.position;
			} else {
				p2 = c2.states [c2.currentState].pivots [0].transform.position;
			}
		}
			
				
		isFailSwitch = false;
		if (c0 is SwitchingColumn) {
		
			if (lateInput) {
				pFrom = p0;
				vFrom = new Vector3 (1.5f, 4f, 0f);
				vTo = new Vector3 (1f, -3f, 0f);
				pTo = pFrom + Vector3.right * 2;
				isJumpNeed = false;
				return;
			}

			SwitchingColumn sc = (SwitchingColumn)c0;
			if (sc.currentState == 1) {
				p1 = p0 + new Vector3 (0, -2, 0);
				if (sc.numJumps % sc.jumpsPerSwitch == 0)
					sc.numJumps--;
				isFailSwitch = true;
			} else {
				if (sc.numJumps % sc.jumpsPerSwitch == 1) {
					p1 = p0 + new Vector3 (1, -2, 0);
				} else {

				}
			}
		}
		Column cp = CGame.gamelogic.world.GetNextColumn (pos.x, -1);
		if (cp is SwitchingColumn) {
			SwitchingColumn sc = (SwitchingColumn)cp;
			if (sc.currentState == 0) {
				p1 = p0 + new Vector3 (0, -2, 0);
				if (sc.numJumps % sc.jumpsPerSwitch == 0)
					sc.numJumps--;
				isFailSwitch = true;
			}
		}
		
		
		float y0 = p0.y;
		float y1 = p1.y;
		float y2 = p2.y;
        
		isJumpedOnThisColumn = false;
		
		pFrom = p0;
		pTo = p1;

		vFrom = new Vector3 (1f, 2.5f, 0f);
		vTo = new Vector3 (1f, -2.5f, 0f);

		speed = 2.5f;
		
		bool isJumped = false;
		if (isJumpNeed && !c1.isFinishColumn) {
			if (y0 < y1) {//up
				if (y1 == y0 + 1f) {
					//JUMP UP!
					anims.Ouch ();
					int level = (int)(y1 - 0.5f);
					if (level == 0) {
						CGame.gamelogic.currentUpSoundPack = Random.Range (0, CGame.gamelogic.upSounds.Count);
					}
					CGame.gamelogic.upSounds [CGame.gamelogic.currentUpSoundPack].sounds [level].Play ();
#if UNITY_ANDROID	//átugorjuk a felfutást, így kicsit kisebbnek hat a lag..bocs KZ
					CGame.gamelogic.upSounds[CGame.gamelogic.currentUpSoundPack].sounds[level].timeSamples=1800;
#endif
					isJumped = true;
					vFrom = new Vector3 (0.6f, 5f, 0f);
					vTo = new Vector3 (1f, -2f, 0f);

					User.JumpUpCount++;
                                        
				}
			} else if (y0 >= y2) {//forward

				//JUMP FORWARD!
				anims.Ouch ();

				CGame.gamelogic.scratchSounds [CGame.gamelogic.currentScratchSoundPack].sounds [CGame.gamelogic.currentScratchSound].Play ();
#if UNITY_ANDROID	//átugorjuk a felfutást, így kicsit kisebbnek hat a lag..bocs KZ
				CGame.gamelogic.scratchSounds[CGame.gamelogic.currentScratchSoundPack].sounds[CGame.gamelogic.currentScratchSound].timeSamples=1800;
#endif
				CGame.gamelogic.currentScratchSound++;
				if (CGame.gamelogic.currentScratchSound >= CGame.gamelogic.scratchSounds [CGame.gamelogic.currentScratchSoundPack].sounds.Count)
					CGame.gamelogic.currentScratchSound = 0;

				isJumped = true;
				pTo = p2;
				vFrom = new Vector3 (1.5f, 4f, 0f);
				vTo = new Vector3 (1f, -3f, 0f);

			} else if (y0 < y2) {//forward but wall ahead
				//JUMP FORWARD!
				Collider2D[] cs = c2.GetComponentsInChildren<Collider2D> ();
				foreach (Collider2D c in cs) {
					c.gameObject.layer = LayerMask.NameToLayer ("Die");
				}
				isJumped = true;
				pTo = p2;
				pTo.y = pFrom.y;
				vFrom = new Vector3 (1.5f, 4f, 0f);
				vTo = new Vector3 (1f, -1.5f, 0f);
			}
		}
		
		if (isJumpNeed && c2 is GrowingColumn) {
			if (((GrowingColumn)c2).isReverse) {
				((GrowingColumn)c2).jumpsBeforeGrow++;
				((GrowingColumn)c2).OnJumped ();
			}
		}

		if (!isJumped) {        
			if (y0 > y1) {
				//FALL DOWN!
				float y = y0 - y1;
				vFrom = new Vector3 (1.4f + 0.5f * (y0 - y1), 2f + 0.5f * (y0 - y1), 0f);
				vTo = new Vector3 (1f, -4f - y, 0f);
			} else if (y1 == y0) {
				pTo = pFrom + Vector3.right;
			} else if (y1 > y0) {//wall ahead
				pTo = pFrom + new Vector3 (1, -0.5f, 0);
				vFrom = new Vector3 (1.5f, 2.5f, 0f);
			}
		}

		if (isFailSwitch) {
			vFrom = Vector3.down * 5;
			vTo = Vector3.down * 3;
		}

		pFrom += Vector3.up * radius;
		pTo += Vector3.up * radius;

		isJumpNeed = false;


		Column c20 = CGame.gamelogic.world.GetNextColumn (pos.x, 20);
		if (c20 != null) {
			c20.PlayAnimation ();
		}
       	
	}

	void OnCollisionEnter2D (Collision2D c)
	{
//		if (PlayerPrefs.GetInt ("DebugMode", 0) != 1) {
			if (c.collider.gameObject.layer == 8) {
				Die ();
				User.DieCount++;
				PluginManager.social.ReportAchievement (eAchievement.die_50, (float)User.DieCount / 50f * 100f);
				PluginManager.social.ReportAchievement (eAchievement.die_100, (float)User.DieCount / 100f * 100f);
				PluginManager.social.ReportAchievement (eAchievement.die_200, (float)User.DieCount / 200f * 100f);
				PluginManager.social.ReportAchievement (eAchievement.die_500, (float)User.DieCount / 500f * 100f);
				PluginManager.social.ReportAchievement (eAchievement.die_1000, (float)User.DieCount / 1000f * 100f);
			} else {
				if (c.contacts [0].point.x > pos.x + 0.1f) {
					Die ();
				}
			}
//		} else {
//			DoJump (true);
//		}
	}
	
	void Die ()
	{
		if (CGame.gamelogic != null && CGame.gamelogic.state == Gamelogic.eState.ingame) {
			GetComponent<Collider2D>().enabled = false;

			//Debug.Log("DIE");
			speed = 0f;
			CGame.gamelogic.ChangeState (Gamelogic.eState.results_fail);
			placcs.transform.eulerAngles = new Vector3 (0f, 0f, -30f);
			placcs.gameObject.SetActive (true);
			GetComponent<Animation>().Play ("ball_scale");
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.CompareTag ("JumpOverTrigger")) {
			//Debug.Log("EXIT GENYÓ");
			if (CGame.gamelogic.state == Gamelogic.eState.ingame) {
				User.JumpOverSpikeCount++;
			}
		}
	}

	public void SetActive (bool a)
	{
		isActive = a;
	}
}
