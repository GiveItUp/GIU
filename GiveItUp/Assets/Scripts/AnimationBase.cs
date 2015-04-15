using UnityEngine;
using System.Collections;

public class AnimationBase : MonoBehaviour {
	void Awake()
	{
	}

	public virtual void Ouch()
	{

	}

	public virtual Vector3 jumpOffset
	{
		get{return Vector3.zero;}
	}
}
