using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AgentMovement))]
public class DynamicPursue : MonoBehaviour {
	public float timeStep = .5f; //how many seconds ahead will agent look to future position
	public Transform target;
	protected Rigidbody2D targetBody;
	protected AgentMovement agent;
	protected AgentMovement targetAgent;

	Vector2 targetPosition = Vector2.zero;
	protected virtual void Awake(){
		agent = GetComponent<AgentMovement> ();
	}

    protected virtual void Start () {
       
    }

	protected virtual void Update () {
		if (target == null) {
			return;
		}
		targetPosition = target.position;
		if (targetBody == null) {
			targetBody = target.GetComponent<Rigidbody2D> ();
		}
		else {
			Vector2 acceleratedMovement = Vector2.zero;
			if (targetAgent == null) {
				targetAgent = target.GetComponent<AgentMovement> ();
			} else {
				//dt = 1/2 a * t^2
				float displacement;
				displacement = .5f * targetAgent.acceleration * targetAgent.maxAcceleration;
				displacement *= timeStep * timeStep;
				acceleratedMovement = target.up * displacement;
			}
			targetPosition = targetPosition + targetBody.velocity * timeStep + acceleratedMovement;
		}

		agent.targetDirection = (Vector3)(targetPosition) -  transform.position;
	}

	void LateUpdate(){
		PlayerDebug.DrawLine (target.position, targetPosition, Color.red);
		PlayerDebug.DrawCircle(targetPosition, .125f, Color.red);
	}

	protected virtual void OnDrawGizmos(){
		if (target == null || !enabled) {
			return;
		}
		Gizmos.color = Color.red;
		Gizmos.DrawSphere( targetPosition,.125f);
	}
}
