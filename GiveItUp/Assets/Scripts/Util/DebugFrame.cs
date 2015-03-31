using UnityEngine;
using System.Collections;

public class DebugFrame : MonoBehaviour {

    public Color color;
    public float w;
    public float h;

	void OnDrawGizmos()
    {
    
        Gizmos.color = color;
        
        /*
        Vector3 bl = new Vector3(renderer.bounds.min.x, renderer.bounds.min.y, 0);
        Vector3 br = new Vector3(renderer.bounds.max.x, renderer.bounds.min.y, 0);
        Vector3 tr = new Vector3(renderer.bounds.max.x, renderer.bounds.max.y, 0);
        Vector3 tl = new Vector3(renderer.bounds.min.x, renderer.bounds.max.y, 0);
        */
        
        Vector3 bl = new Vector3( -w, -h, 0);
        Vector3 br = new Vector3( w, -h, 0);
        Vector3 tr = new Vector3( w, h, 0);
        Vector3 tl = new Vector3( -w, h, 0);
        
        Gizmos.DrawSphere(bl, 10);
        Gizmos.DrawSphere(br, 10);
        Gizmos.DrawSphere(tl, 10);
        Gizmos.DrawSphere(tr, 10);
        
        Gizmos.DrawLine( bl, br );
        Gizmos.DrawLine( br, tr );
        Gizmos.DrawLine( tr, tl );
        Gizmos.DrawLine( tl, bl );
        
        
    }
}
