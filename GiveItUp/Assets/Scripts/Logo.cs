using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour
{
	GameObject go;
	// Use this for initialization
	IEnumerator Start ()
	{
		go = GameObject.Instantiate (Resources.Load ("Prefabs/Monstar/logo1")) as GameObject;
		yield return new WaitForSeconds (1);
		Destroy (go);
		go = GameObject.Instantiate (Resources.Load ("Prefabs/Monstar/logo2")) as GameObject;
		yield return new WaitForSeconds (1);
		Destroy (go);
		go = GameObject.Instantiate (Resources.Load ("Prefabs/Monstar/logo3")) as GameObject;
		yield return new WaitForSeconds (1);
		Destroy (go);
		go = GameObject.Instantiate (Resources.Load ("Prefabs/Monstar/logo4")) as GameObject;
		Application.LoadLevel (1);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
