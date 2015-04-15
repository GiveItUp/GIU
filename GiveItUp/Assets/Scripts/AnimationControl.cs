using UnityEngine;
using System.Collections;

public class AnimationControl : MonoBehaviour {
	public AnimationBase jumpAnimation;
	public AnimationBase dieAnimation;
	public AnimationBase successfulAnimation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void JumpOuch()
	{
		if (jumpAnimation == null)
			return;
		jumpAnimation.Ouch ();
	}

	public Vector3 jumpOffset
	{
		get{return jumpAnimation.jumpOffset;}
	}

	public void DieAnimation()
	{
		if (dieAnimation != null) {
			dieAnimation.gameObject.SetActive(true);
			dieAnimation.Ouch ();
		}
		if (jumpAnimation != null) {
			jumpAnimation.gameObject.SetActive (false);
		}
	}

	public void SuccessfulAnimation()
	{
		if (successfulAnimation != null) {
			successfulAnimation.gameObject.SetActive(true);
			successfulAnimation.Ouch ();
		}
		if (jumpAnimation != null) {
			jumpAnimation.gameObject.SetActive (false);
		}
	}
}
