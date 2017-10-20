using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerEmergent : MonoBehaviour {


    public Path my_path;
    public static int path_index = 0;
    float arrival_radius = 1f;


    // 3 things:
    // - steer in direction of nearby boids
    // - avoid colliding with nearby boids 
    // - move towards average position of nearby boids

    const float separation_amount = 4f;
    const float alignment_amount = 1f;
    const float cohesion_amount = 3f;

    const float local_range = 1f;

    Vector3 separation_direction = new Vector3(0f,0f,0f);
    Vector3 alignment_direction = new Vector3(0f,0f,0f);
    Vector3 cohesion_direction = new Vector3(0f,0f,0f);

    public Boid my_boid;
    protected AgentMovement agent;

    protected virtual void Awake(){
		agent = GetComponent<AgentMovement> ();
	}

    void Update () {
        List<Boid> local_boids = new List<Boid>();
        Boid[] all_boids = Object.FindObjectsOfType<Boid>();

        // make a list of nearby boids
        for (int i = 0; i < all_boids.Length; i++) {
            if (all_boids[i] != my_boid) {
                if (Vector3.Distance(transform.position, all_boids[i].transform.position) < local_range) {
                    local_boids.Add(all_boids[i]);
                }
            }
        }

        separation_direction = new Vector3(0f,0f,0f);

        // sum the squares of the vectors to the local boids
        for (int i = 0; i < local_boids.Count; i++) {
            Vector3 diff = transform.position - local_boids[i].transform.position;
            separation_direction += diff.normalized * (Mathf.Pow(diff.magnitude, 2f));
        }

        separation_direction = separation_direction.normalized;


        /*
        alignment_direction = new Vector3(0f,0f,0f);

        // sum the directions of the local boids
        for (int i = 0; i < local_boids.Count; i++) {
            Vector3 dir = local_boids[i].rb.velocity;
            alignment_direction += dir;
        }

        alignment_direction += (Vector3)my_leader.rb.velocity * 10f;
        alignment_direction = alignment_direction.normalized;
        */



        if (my_path == null) {
			return;
		}
        alignment_direction = (my_path.points[path_index].position - transform.position).normalized;

        if (Vector3.Distance(transform.position, my_path.points[path_index].position) < arrival_radius) {
            path_index = (path_index + 1)%my_path.points.Count;
        }






        cohesion_direction = new Vector3(0f,0f,0f);

        // sum the vectors from the local boids
        for (int i = 0; i < local_boids.Count; i++) {
            Vector3 diff = local_boids[i].transform.position - transform.position;
            cohesion_direction += diff;
        }

        cohesion_direction = cohesion_direction.normalized;



        agent.targetDirection = (separation_direction*separation_amount + alignment_direction*alignment_amount + cohesion_direction*cohesion_amount);
    }


}
