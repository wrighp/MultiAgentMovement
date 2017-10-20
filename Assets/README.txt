⦁	What did you use for obstacle avoidance?
	Cone check

⦁	What are the heuristics for the agents to go through the tunnels
	Leaders create a set of paths for each agent based on its slot position and the width of the currently traversible area.
	This funnels agents into the tunnels. The emergent formation has no way of doing this, so they just swarm around the tunnels.

⦁	Did you use any additional heuristics?
	The path points are more dense in smaller areas, which helps to make sure the agents will find their way to the openings.

⦁	What are the differences in the three groups’ performances?
	The two-level formation tends to perform the best, followed closely by the scalable formation (which makes sense, since it's very similar).
	The emergent formation performs the least well.