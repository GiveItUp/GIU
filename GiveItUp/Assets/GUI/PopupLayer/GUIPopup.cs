using UnityEngine;
using System.Collections;

public class GUIPopup : GUILayer {

    public GameObject blackBGPrefab;
    public GameObject blockBGPrefab;

    protected void BlackGUIBehind()
    {
        if (blackBGPrefab != null)
        {
            GameObject black = GameObject.Instantiate(blackBGPrefab) as GameObject;
            black.transform.parent = this.transform;
            black.transform.localPosition = new Vector3(0, 0, 1);
        }
    }

    protected void BlockGUIBehind()
    {
        if (blockBGPrefab != null)
        {
            GameObject block = GameObject.Instantiate(blockBGPrefab) as GameObject;
            block.transform.parent = this.transform;
            block.transform.localPosition = new Vector3(0, 0, 1);
        }
	}

    
	protected override bool IsTopLayer ()
	{
		//Debug.Log (PopupLayer.GetActualPopupZ() + " -- " + transform.localPosition.z);
		return PopupLayer.GetActualPopupZ() == transform.localPosition.z;
	}
}
