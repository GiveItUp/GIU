using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {

	public float interval = 0.5f;

	private int count = 0;
	private float time = 0;
	private int fps;

	
	// Update is called once per frame
	void Update () {
	
		time += Time.deltaTime;
		count++;
		if (time > interval)
		{
			fps = (int)(count/time);
			count = 0;
			time = 0;
		}
	}

#if TEST_BUILD
	void OnGUI()
	{
		GUI.Label (new Rect (10, 10, 100, 50), fps.ToString ());
	}
#endif
}
