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
		foreach (var item in packedSprites) {
			item.gameObject.SetActive(false);
		}
		int index = 1;//Random.Range (0,3);
		packedSprites [index].PlayAnim (0);
		packedSprites [index].gameObject.SetActive (true);
	}
}
