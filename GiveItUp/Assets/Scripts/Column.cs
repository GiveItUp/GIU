using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Column : MonoBehaviour
{

	[System.Serializable]
	public class ColumnState
	{
		public Transform[] pivots;
	}

	public List<Transform> greenGlows;
    public Material Mat_GreenBottom;

	public Transform sprite;
	
	public bool isFinishColumn = false;

    public bool isCatchesJumpDelegate = false;
    
    
    public int currentState = 0;
    public int previousState = 0;
    public ColumnState[] states;
    public Animation anim;
    public Animation fall_anim;
    private void Start()
	{
		if(MainMenuGUI.flowerFlg == 1 && greenGlows.Count>0)
		{
			if(greenGlows[0] != null){
				MeshRenderer meshRenderer = greenGlows[0].GetComponent<MeshRenderer>();

				meshRenderer.material = Resources.Load<Material>("Glow_1");
			}
		}
	}

    public virtual void SetColor(int c)
    {
		if (greenGlows != null && greenGlows.Count > 0 && c == 1)

		{
			foreach (var ps in greenGlows)
			{
                if (ps != null)
                    ps.gameObject.SetActive(true);
			}
		}

		if (sprite != null  && greenGlows.Count > 0 && c == 1)
            sprite.GetComponent<Renderer>().material = Mat_GreenBottom;

	    if (c == 1 && anim != null)
	        anim.Play("platform_anim");
	}

    public void PlayFallAnim()
    {
        if (fall_anim != null)
            fall_anim.Play("platform_fall");
    }

    public virtual void PlayAnimation()
    {
        if (GetComponent<Animation>() != null && GetComponent<Animation>().GetClipCount() > 1)
            GetComponent<Animation>().Play();
    }
    
    public virtual void OnJumped()
    {
    	
    }
    
    protected virtual void IncreaseState()
    {
    	previousState = currentState;
    	currentState++;
    	if (currentState > states.Length - 1)
    	{
    		currentState = 0;
    	}
    }
}
