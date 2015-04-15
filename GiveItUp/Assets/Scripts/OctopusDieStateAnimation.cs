using UnityEngine;
using System.Collections;

public class OctopusDieStateAnimation : AnimationBase {
	public PackedSprite packedSprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Ouch ()
	{
		packedSprite.defaultAnim = Random.Range (0,3);
	}
}
