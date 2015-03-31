using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveObjects : MonoBehaviour 
{
	public float dist = 10f;
	public Transform prefab;
	public List<Transform> wayPoints = new List<Transform>();
	

	
	
	[ContextMenu ("CreateObjects")]
	public void CreateObjects()
	{
		List<Vector3> cPoints = new List<Vector3>();
		
		Vector3 currentPoint;
		Vector3 nextPoint;
		Vector3 currentPos;
		int posId;	
		
		var path = new Transform[wayPoints.Count];
		for (int i = 0; i < wayPoints.Count; i++)
		{
			path[i] = wayPoints[i].transform;
		}
		IEnumerable<Vector3> sequence = Interpolate.NewCatmullRom(path, 5, false);
		foreach (var el in sequence)
		cPoints.Add (el - this.transform.position);
		
		
		currentPoint = cPoints[0];
		nextPoint = cPoints[1];
		currentPos = currentPoint;
		posId = 1;
		float tempDist = dist;
		while(true)
		{
			if (Vector3.Distance(currentPos, currentPoint) > Vector3.Distance(currentPoint, nextPoint))
			{
				tempDist = tempDist - Mathf.Abs(Vector3.Distance(currentPos, currentPoint) - Vector3.Distance(currentPoint, nextPoint));
				currentPos = nextPoint;
				currentPoint = nextPoint;
				posId++;
				if (posId >= cPoints.Count)
					return;
				nextPoint = cPoints[posId];
				
				Vector3 dir2 = nextPoint - currentPoint;
				dir2.Normalize();
				currentPos += dir2 * tempDist;
				
				Transform go = GameObject.Instantiate(prefab) as Transform;
				go.parent = this.transform;
				go.localPosition = currentPos;
			}
			else
			{
				tempDist = dist;
				Vector3 dir = nextPoint - currentPoint;
				dir.Normalize();
				currentPos += dir * dist;
			}
		}
	}
	
	
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		foreach (var wayPoint in wayPoints)
		{
			Gizmos.DrawSphere(wayPoint.transform.position, 20f);
		}
		if (wayPoints.Count <= 1)
			return;
	
			var path = new Transform[wayPoints.Count];
			for (int i = 0; i < wayPoints.Count; i++)
				path[i] = wayPoints[i].transform;
			IEnumerable<Vector3> sequence = Interpolate.NewCatmullRom(path, 10, false);
			
			var firstPoint = path[0].position;
			var segmentStart = firstPoint;
		
			//sequence.MoveNext();
			int count = 0;
    		foreach (var segmentEnd in sequence) 
			{
				count++;
      			Gizmos.DrawLine(segmentStart, segmentEnd);
      			segmentStart = segmentEnd;
      			//if (segmentStart == firstPoint) { break; }
    		}
	}
	
	
	[ContextMenu ("AddWayPoint")]
	public void AddWayPoint()
	{
		GameObject go = new GameObject("waypoint");
		go.transform.parent = this.transform;
		go.transform.localPosition = Vector3.zero;
		wayPoints.Add(go.transform);
	}
	
	[ContextMenu ("DeleteWayPoint")]
	public void DeleteWayPoint()
	{
		if (wayPoints.Count > 0)
		{
			GameObject.DestroyImmediate(wayPoints[wayPoints.Count -1].gameObject);
			wayPoints.RemoveAt(wayPoints.Count - 1);
		}
	}
	
	[ContextMenu ("DeleteAllWayPoints")]
	public void DeleteAllWayPoints()
	{
		foreach (var go in wayPoints)
			GameObject.DestroyImmediate(go.gameObject);
		wayPoints.Clear();
	}
}
