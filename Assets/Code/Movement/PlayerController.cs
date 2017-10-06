using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AgentMovement))]
public class PlayerController : MonoBehaviour {

	AgentMovement agent;
	Vector3 position;

	void Awake(){
		agent = GetComponent<AgentMovement>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		position.z = 0;
		agent.targetDirection = position - agent.transform.position;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(position, 0.125f);
	}
}
