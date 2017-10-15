using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour {

    const float max_radius = 2f;
    const float radius_step = 0.02f;
    const float agent_radius_buffer = 0.2f;

    bool blocked;
    float current_radius = 0f;

    LayerMask obstacles_mask;

    public List<Follower> my_followers = new List<Follower>();

    const float path_step_frequency = 1f;
    float path_step_timer = 0f;

    void Start () {
        // set up the mask to include only obstacles
        string[] layers = new string[1];
        layers[0] = "Obstacles";
        obstacles_mask = LayerMask.GetMask(layers);

        for (int i = 0; i < my_followers.Count; i++) {
            my_followers[i].my_leader = this;
        }
    }

    void Update () {

        blocked = false;
        current_radius = 0f;
        
        while (!blocked && current_radius < max_radius) {
            current_radius += radius_step;
            Collider2D[] hit_colliders = Physics2D.OverlapCircleAll(transform.position, current_radius, obstacles_mask);

            if (hit_colliders.Length > 0) {
                blocked = true;
            }
        }

        PlayerDebug.DrawCircle(transform.position, current_radius, new Color(1f,1f,0f,0.5f));



        for (int i = 0; i < my_followers.Count; i++) {
            float angle = (((float)i)/my_followers.Count)*Mathf.PI*2f + (transform.eulerAngles.z * Mathf.Deg2Rad);
            Vector3 newpos = transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f))*(current_radius - agent_radius_buffer);

            //PlayerDebug.DrawLine(transform.position, newpos, Color.black);
            //PlayerDebug.DrawCircle(newpos, agent_radius_buffer, new Color(0f,0f,0f,0.5f));
        }


        path_step_timer += Time.deltaTime;
        if (path_step_timer > path_step_frequency) {
            path_step_timer -= path_step_frequency;

            for (int i = 0; i < my_followers.Count; i++) {
                my_followers[i].path_points.Enqueue(get_slot_position(i));
            }
        }
    }

    public Vector3 get_slot_position (int index) {
        float angle = (((float)index)/my_followers.Count)*Mathf.PI*2f + (transform.eulerAngles.z * Mathf.Deg2Rad);
        return transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f))*(current_radius - agent_radius_buffer);
    }

}
