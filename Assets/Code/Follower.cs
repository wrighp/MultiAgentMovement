using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AgentMovement))]
public class Follower : MonoBehaviour {

    public Queue<Vector3> path_points = new Queue<Vector3>();
    float arrival_radius = 1f;
    float slowdown_radius = 3f;

    const float max_accel_peak = 50f;   // 10
    const float max_speed_peak = 5f;    // 2
    const float turn_speed_peak = 50f;  // 10


    protected AgentMovement agent;
    [HideInInspector]
    public Leader my_leader;

    bool has_target = false;
	Vector2 targetPosition = Vector2.zero;

	protected virtual void Awake(){
		agent = GetComponent<AgentMovement> ();
	}

    protected virtual void Update () {

        if (path_points.Count > 0) {
            agent.maxAcceleration = max_accel_peak;
            agent.maxSpeed = max_speed_peak;
            agent.turnSpeed = turn_speed_peak;
        }
        else {
            float dist = Vector3.Distance(transform.position, targetPosition);

            if (dist < slowdown_radius) {

                float lerpval = Mathf.InverseLerp(0f, slowdown_radius, dist);

                agent.maxAcceleration = Mathf.Lerp(10f, max_accel_peak, lerpval);
                agent.maxSpeed = Mathf.Lerp(2f, max_speed_peak, lerpval);
                agent.turnSpeed = Mathf.Lerp(10f, turn_speed_peak, lerpval);
            }
            else {
                agent.maxAcceleration = max_accel_peak;
                agent.maxSpeed = max_speed_peak;
                agent.turnSpeed = turn_speed_peak;
            }
        }


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
                targetPosition = my_leader.get_slot_position(my_leader.my_followers.IndexOf(this));
            }

        }


        PlayerDebug.DrawLine(transform.position, targetPosition, Color.blue);
	}

}
