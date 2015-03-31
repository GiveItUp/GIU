using UnityEngine;
using System.Collections;

public class Initialzation : MonoBehaviour
{
	[SerializeField]
	bool
		isDebugMode = false;
	[SerializeField]
	bool
		isTvMode = false;

	void Start ()
	{
		if (isDebugMode) {
			PlayerPrefs.SetInt ("DebugMode", 1);
		} else {
			PlayerPrefs.SetInt ("DebugMode", 0);
		}

		if (isTvMode) {
			PlayerPrefs.SetInt ("TVMode", 1);
		} else {
			PlayerPrefs.SetInt ("TVMode", 0);
		}
	}
}
