using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AgentMovement))]
public class CollisionPrediction : MonoBehaviour {

    AgentMovement my_agent;
    AgentMovement[] all_agents;
    const float collision_distance = 0.3f;
    const float vision_range = 2f;

    void Start () {
        my_agent = GetComponent<AgentMovement>();
        all_agents = Object.FindObjectsOfType<AgentMovement>();
    }

    public void Update () {
        for (int i = 0; i < all_agents.Length; i++) {
            if (all_agents[i] != my_agent) {
                if (Vector3.Distance(all_agents[i].transform.position, transform.position) <= vision_range) {
                    if (Vector3.Dot(get_position_at_time_of_approach_to(all_agents[i])-transform.position, my_agent.rb.velocity) > 0) {
                        if (get_distance_at_time_of_approach_to(all_agents[i]) <= collision_distance) {

                            Vector2 vectorDirection = all_agents[i].transform.position - transform.position;
			                Vector2 forwardDirection = transform.up;

			                Vector3 cross = Vector3.Cross(vectorDirection, forwardDirection);
			                float sign = Mathf.Sign(cross.z);

                            my_agent.targetDirection = (Quaternion.Euler(0,0,90f * sign) *transform.up).normalized;
                        }
                    }
                }
            }
        }
    }

    public float get_closest_time_of_approach_to (AgentMovement other_agent) {

        Vector3 w_0 = my_agent.transform.position - other_agent.transform.position;
        Vector3 u = my_agent.rb.velocity;
        Vector3 v = other_agent.rb.velocity;


        float time_at_cpa;

        if ((u-v).magnitude == 0f) {
            time_at_cpa = 0f;
        }
        else {
            time_at_cpa = (Vector3.Dot(-w_0,(u-v)))/(Mathf.Pow((u-v).magnitude, 2));
        }

        return time_at_cpa;
    }

    public Vector3 get_position_at_time_of_approach_to (AgentMovement other_agent) {
        Vector3 u = my_agent.rb.velocity;
        Vector3 v = other_agent.rb.velocity;
        float time_at_cpa = get_closest_time_of_approach_to(other_agent);
        return (my_agent.transform.position + (u*time_at_cpa));
    }

    public float get_distance_at_time_of_approach_to (AgentMovement other_agent) {
        
        Vector3 u = my_agent.rb.velocity;
        Vector3 v = other_agent.rb.velocity;

        float time_at_cpa = get_closest_time_of_approach_to(other_agent);

        return ((my_agent.transform.position + (u*time_at_cpa)) - (other_agent.transform.position + (v*time_at_cpa))).magnitude;
    }

    /*
    void OnDrawGizmos () {
		//In case of scene exit
		if(all_agents == null){
			return;
		}
        for (int i = 0; i < all_agents.Length; i++) {
            if (all_agents[i] != my_agent) {
                if (Vector3.Dot(get_position_at_time_of_approach_to(all_agents[i])-transform.position, my_agent.rb.velocity) > 0) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, get_position_at_time_of_approach_to(all_agents[i]));
                    Gizmos.DrawWireSphere(get_position_at_time_of_approach_to(all_agents[i]), get_distance_at_time_of_approach_to(all_agents[i]));
                }
            }
        }
    }*/

}
