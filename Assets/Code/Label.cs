using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour {

    public Transform following;

	void Update () {
		transform.position = following.transform.position + Vector3.up;
	}
}
