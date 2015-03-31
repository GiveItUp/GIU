using UnityEngine;
using System.Collections;

public class GUIMenu : GUILayer {

	protected override bool IsTopLayer ()
	{
		return !PopupLayer.HasOpenedPopup();
	}
}
