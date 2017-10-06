using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AgentMovement))]
public class DynamicPathFollow : MonoBehaviour {

	public Path my_path;
    int path_index = 0;
    float arrival_radius = 1f;

	protected AgentMovement agent;


	Vector2 targetPosition = Vector2.zero;
	protected virtual void Awake(){
		agent = GetComponent<AgentMovement> ();
	}

	protected virtual void Update () {
		if (my_path == null) {
			return;
		}

        targetPosition = my_path.points[path_index].position;
		agent.targetDirection = (Vector3)(targetPosition) -  transform.position;


        if (Vector3.Distance(transform.position, targetPosition) < arrival_radius) {
            path_index = (path_index + 1)%my_path.points.Count;
        }
	}

	void LateUpdate(){
		//PlayerDebug.DrawLine (transform.position,  my_path.points[path_index].position, Color.red);
	}
}
