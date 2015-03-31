using UnityEngine;
using System.Collections;

public class RecordMarker : MonoBehaviour {

	public float minSat;
	public float maxSat;
	private float lastSat;
	private float targetSat;

	public Color minColor;
	public Color maxColor;
	private Color lastColor;
	private Color targetColor;

	public float minTime;
	public float maxTime;
	private float currTime;

	private Material mat;
	private float t = 2;

	private Vector2[] uvs = {
		new Vector2 (0, 0),
		new Vector2 (1, 1),
		new Vector2 (1, 0),
		new Vector2 (0, 1f)
	};

	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer>().material;
		MeshFilter mf = GetComponent<MeshFilter> ();

		mf.mesh.uv = uvs;

	}
	
	// Update is called once per frame
	void Update () {

		if (t > 1)
		{
			t -= 1;

			lastSat = targetSat;
			lastColor = targetColor;

			currTime = Random.Range(minTime, maxTime);
			targetSat = Random.Range(minSat, maxSat);
			targetColor = Color.Lerp(minColor, maxColor, Random.Range(0f,1.0f));

		}

		mat.SetFloat ("_Sat", Mathf.Lerp (lastSat, targetSat, t));
		mat.SetColor ("_Color", Color.Lerp (lastColor, targetColor, t));

		t += Time.deltaTime / currTime;

	}
}
