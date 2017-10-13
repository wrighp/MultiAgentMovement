using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AgentMovement))]
public class Follower : MonoBehaviour {

    public Queue<Vector3> path_points = new Queue<Vector3>();
    float arrival_radius = 1f;

    protected AgentMovement agent;

    bool has_target = false;
	Vector2 targetPosition = Vector2.zero;

	protected virtual void Awake(){
		agent = GetComponent<AgentMovement> ();
	}

    protected virtual void Update () {

        agent.targetDirection = (Vector3)(targetPosition) -  transform.position;

        if (has_target) {
            if (Vector3.Distance(transform.position, targetPosition) < arrival_radius) {
                has_target = false;
            }
        }
        else {
            if (path_points.Count > 0) {
                targetPosition = path_points.Dequeue();
                has_target = true;
            }
            else {
                targetPosition = transform.position;
            }

        }


        PlayerDebug.DrawLine(transform.position, targetPosition, Color.blue);
	}

}
