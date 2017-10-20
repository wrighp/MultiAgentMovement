using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderEmergent : MonoBehaviour {
	
	public bool transferLeadership = true;
    public List<FollowerEmergent> my_followers = new List<FollowerEmergent>();

	void OnDestroy(){

		LeaderEmergent leader = null;

		for (int i = 0; i < my_followers.Count; i++) {
			FollowerEmergent follower = my_followers [i];
			//Create new leader by copying values of this leader
			if (i == 0 && transferLeadership) {
				leader = follower.gameObject.AddComponent<LeaderEmergent> ();
				leader.my_followers = my_followers.GetRange (1, my_followers.Count - 1);
                Destroy (follower);

				DynamicPathFollow pathFollow = leader.gameObject.AddComponent<DynamicPathFollow> ();
				DynamicPathFollow thisFollow = GetComponent<DynamicPathFollow> ();
				pathFollow.my_path = thisFollow.my_path;
				pathFollow.path_index = thisFollow.path_index;

				leader.GetComponent<Rigidbody2D>().mass = GetComponent<Rigidbody2D>().mass;

			}

		}
	}
}
