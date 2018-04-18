using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Av8Spawner : MonoBehaviour
{
	public float leaders = 3;
	public float gapL = 10;
	public float gap = 5;
	public float followers = 1;
	public GameObject prefab;
	public GameObject krprefab;
	public GameObject mothership;
	public GameObject expPrefab; //explosion prefab place on boid of missiles
	private Vector3 mshipsize;
	//Could create the list here and use at the AI script aswell

	private void Awake ()
	{
		mothership = GameObject.Find ("Mothership");
		mshipsize = mothership.GetComponent<Collider> ().bounds.size;
		for (int i = 1; i <= leaders; i++) {
			Vector3 rndPos = new Vector3 (Random.Range ((-leaders * gapL), (leaders * gapL)), Random.Range ((-leaders * gapL), (leaders * gapL)), 0);
			CreateLeaders (rndPos);
		}

		//Create K.Russel plane
		GameObject leader = GameObject.Instantiate<GameObject> (krprefab);
		leader.transform.position = this.transform.TransformPoint (new Vector3(0,-50,0));
		leader.transform.rotation = this.transform.rotation;

		//add components path,pathfollow,obsavoidance,
		//Path path = leader.AddComponent<Path> ();
		//path.isRandom = false;
		FollowPath fpath = leader.AddComponent<FollowPath> ();
		fpath.path = GameObject.Find("KRpath").GetComponent<Path>();
		fpath.enabled = fpath.enabled;
		ObstacleAvoidance obavd = leader.AddComponent<ObstacleAvoidance> ();
		leader.GetComponent<Boid> ().maxSpeed = 30;

//		float thetaInc = (Mathf.PI * 2) / leaders;
//		for (int i = 1; i <= leaders; i++) {
//
//			float theta = thetaInc * i;
//			Vector3 newpos = new Vector3 (Mathf.Sin (theta) * gapL, 0, Mathf.Cos (theta) * gapL);
//			Debug.Log (newpos);
//			CreateLeaders (newpos);
//		}

//		for (int i = 1; i <=leaders; i++)
//		{	
//			Vector3 offset = new Vector3(- gapL, 0, 0);
//			gapL += gapL;
//			CreateLeaders(offset);
//		}
	}

	void CreateLeaders (Vector3 newpos)
	{
		GameObject leader = GameObject.Instantiate<GameObject> (prefab);
		leader.transform.parent = this.transform;
		leader.transform.position = this.transform.TransformPoint (newpos);
		leader.transform.rotation = this.transform.rotation;

		//add components path,pathfollow,seek,arrive,obsavoidance,
		Arrive arive = leader.AddComponent<Arrive> ();
		arive.targetPosition = leader.transform.position + leader.transform.forward * 100;
		Seek seek = leader.AddComponent<Seek> ();
		seek.enabled = !seek.enabled;
		Path path = leader.AddComponent<Path> ();
		path.mothership = mothership;
		FollowPath fpath = leader.AddComponent<FollowPath> ();
		fpath.path = path;
		fpath.enabled = !fpath.enabled;
		ObstacleAvoidance obavd = leader.AddComponent<ObstacleAvoidance> ();
		obavd.enabled = !obavd.enabled;

		//Add boid and seek to the  bombs(missiles)
		int count = leader.transform.Find ("Bombs").gameObject.transform.childCount;
		for (int i = 0; i < count; i++) {
			Boid b = leader.transform.Find ("Bombs").GetChild (i).gameObject.AddComponent<Boid> ();
			b.maxSpeed = 25f;
			b.explosionPrefab = expPrefab;
			Seek sb = leader.transform.Find ("Bombs").GetChild (i).gameObject.AddComponent<Seek> ();
			sb.target = RandomTarget ();
			sb.enabled = !sb.enabled;
		}
			

		for (int i = 1; i <= followers; i++) {
			Vector3 offset = new Vector3 (gap * i, 0, -gap * i);
			CreateFollower (offset, leader.GetComponent<Boid> ());
			offset = new Vector3 (-gap * i, 0, -gap * i);
			CreateFollower (offset, leader.GetComponent<Boid> ());            
		}
	}

	void CreateFollower (Vector3 offset, Boid leader)
	{
		GameObject follower = GameObject.Instantiate<GameObject> (prefab);
		follower.transform.position = leader.transform.TransformPoint (offset);
		follower.transform.parent = this.transform;
		follower.transform.rotation = this.transform.rotation;
		follower.name = "follower";

		OffsetPursue op = follower.AddComponent<OffsetPursue> ();
		op.leader = leader;
		Seek seek = follower.AddComponent<Seek> ();
		seek.enabled = !seek.enabled;
		Path path = follower.AddComponent<Path> ();
		path.mothership = mothership;
		FollowPath fpath = follower.AddComponent<FollowPath> ();
		fpath.path = path;
		fpath.enabled = !fpath.enabled;
		ObstacleAvoidance obavd = follower.AddComponent<ObstacleAvoidance> ();
		obavd.enabled = !obavd.enabled;

		//Add boid and seek to the  bombs(missiles)
		int count = follower.transform.Find ("Bombs").gameObject.transform.childCount;
		for (int i = 0; i < count; i++) {
			Boid b = follower.transform.Find ("Bombs").GetChild (i).gameObject.AddComponent<Boid> ();
			b.maxSpeed = 25f;
			b.explosionPrefab = expPrefab;
			Seek sb = follower.transform.Find ("Bombs").GetChild (i).gameObject.AddComponent<Seek> ();
			sb.target = RandomTarget ();
			sb.enabled = !sb.enabled;
		}
	}

	Vector3 RandomTarget () //missile launch random position of mothership
	{
		return new Vector3 (mothership.transform.position.x,
			mothership.transform.position.y,
			Random.Range (-mshipsize.z / 2, mshipsize.z / 2) + mothership.transform.position.z
		);
	}
	// Update is called once per frame
	void Update ()
	{

	}
}
