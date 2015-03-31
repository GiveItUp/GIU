using UnityEngine;
using System.Collections;

public class GrowingColumn : Column
{

	public float growSpeed = 1;
	public Transform grower;
	public Transform head;

	public float startScale = 0.29f;
	
	public int jumpsBeforeGrow = 2;

	private bool activated = false;

	public bool isReverse = false;

	void Start()
	{
		if (isReverse)
		{
			grower.localScale = new Vector3 (1, 1, 1);
			head.localScale = new Vector3(1, 1, 1);
		}
		else
		{
			grower.localScale = new Vector3 (1, startScale, 1);
			head.localScale = new Vector3(1, 1f/startScale, 1);
		}
	}
	
	public override void PlayAnimation()
	{
		
	}
	

	public override void OnJumped()
	{
		if ( !activated && Mathf.Abs (CGame.gamelogic.ball.transform.position.x - transform.position.x) + 0.5f <= jumpsBeforeGrow)
		{
			activated = true;
			if (isReverse)
			{
				StartCoroutine(Reverse());
			}
			else
			{
				StartCoroutine(Grow());
			}
		}
	}


	private IEnumerator Grow()
	{
		float t = startScale;

		while (t < 1)
		{
			grower.localScale = new Vector3(1,t,1);
			head.localScale = new Vector3(1, 1f/t, 1);
			t += Time.deltaTime * growSpeed;
			yield return 0;
		}

		grower.localScale = new Vector3(1,1,1);
		head.localScale = new Vector3(1, 1, 1);
	}
	
	private IEnumerator Reverse()
	{
		float t = 1;
		
		while (t > startScale)
		{
			grower.localScale = new Vector3(1,t,1);
			head.localScale = new Vector3(1, 1f/t, 1);
			t -= Time.deltaTime * growSpeed;
			yield return 0;
		}
		
		grower.localScale = new Vector3(1,startScale,1);
		head.localScale = new Vector3(1, 1f/startScale, 1);
	}
}
