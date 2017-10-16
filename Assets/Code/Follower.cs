using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AgentMovement))]
public class Follower : MonoBehaviour {

	public List<Vector3> path_points = new List<Vector3>();
	public List<float> points_radii = new List<float>();

	float slowdown_radius = 3f;

    const float max_accel_peak = 50f;   // 10
    const float max_speed_peak = 5f;    // 2
    const float turn_speed_peak = 50f;  // 10


    protected AgentMovement agent;
    [HideInInspector]
    public Leader my_leader;

	protected virtual void Awake(){
		agent = GetComponent<AgentMovement> ();
	}

    protected virtual void Update () {
		
		Vector2 targetPosition = Vector2.zero;

		float radius = GetComponent<CircleCollider2D>().radius;
		for(int i = path_points.Count - 1; i >= 0; i--){
			if (Vector2.Distance(transform.position, path_points[i]) < points_radii[i] + radius) {
				//If in circle, remove all path points before this (and this one), as we have made it past them
				path_points.RemoveRange(0,i+1);
				points_radii.RemoveRange(0,i+1);
				break;
			}
		}
		if(path_points.Count <= 1){
			targetPosition = my_leader.get_slot_position(my_leader.my_followers.IndexOf(this));
		}
		else{
			targetPosition = path_points[0];
		}

		agent.targetDirection = (Vector3)(targetPosition) -  transform.position;
        PlayerDebug.DrawLine(transform.position, targetPosition, Color.blue);

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

	}

}
