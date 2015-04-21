using UnityEngine;
using System.Collections;

public class OctopusDieStateAnimation : AnimationBase {
	public PackedSprite[] packedSprites;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public override void Ouch ()
	{
		packedSprites [0].gameObject.SetActive (true);
	}
}
