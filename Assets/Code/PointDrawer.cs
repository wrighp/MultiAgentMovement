using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDrawer : MonoBehaviour {

	void LateUpdate(){
		PlayerDebug.DrawCircle(transform.position, 0.25f, Color.blue);
	}

	void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
