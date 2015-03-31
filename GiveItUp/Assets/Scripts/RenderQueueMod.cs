using UnityEngine;
using System.Collections;

public class RenderQueueMod : MonoBehaviour 
{
    public int q = 1000;
    public bool shared = true;
	void Awake () 
    {
        if (shared)
            GetComponent<Renderer>().sharedMaterial.renderQueue = q;
        else
            GetComponent<Renderer>().material.renderQueue = q;
	}

#if UNITY_EDITOR
    [ContextMenu ("setValue")]
    public void SetValue()
    {
        GetComponent<Renderer>().sharedMaterial.renderQueue = q;
    }
#endif
	
}
