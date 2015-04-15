	using UnityEngine;
using System.Collections;

public class OctopusJumpStateAnimation : AnimationBase {
	public Pislogas jumpAnim;
	public Pislogas eyeAnim;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Ouch ()
	{

	}

	public override Vector3 jumpOffset {
		get {
			return new Vector3(0,-0.2f,0);
		}
	}
}
