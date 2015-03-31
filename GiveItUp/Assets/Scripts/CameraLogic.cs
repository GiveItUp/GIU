using UnityEngine;
using System.Collections;

public class CameraLogic : MonoBehaviour
{
    public Transform ball;

    void LateUpdate()
    {
		if (ball != null)
		{
#if UNITY_ANDROID
			transform.position = new Vector3(ball.position.x + 2f, 1.65f, -13f);
#else
			transform.position = Vector3.Lerp(transform.position, new Vector3(ball.position.x + 2f, 1.65f, -13f), 0.5f);
#endif
		}
    }
}
