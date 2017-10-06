using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    public List<Transform> points = new List<Transform>();
    public bool looping = false;
    public Color path_color;

    void LateUpdate() {
        if (points.Count >= 2) {
            for (int i = 1; i < points.Count; i++) {
				PlayerDebug.DrawLine(points[i-1].transform.position, points[i].transform.position, path_color);
            }

            if (looping) {
			    PlayerDebug.DrawLine(points[0].transform.position, points[points.Count-1].transform.position, path_color*2f);
            }
        }
    }
}
