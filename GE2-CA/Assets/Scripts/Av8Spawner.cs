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

	private void Start ()
	{
		float thetaInc = (Mathf.PI * 2) / leaders;
		for (int i = 1; i <= leaders; i++) {

			float theta = thetaInc * i;
			Vector3 newpos = new Vector3 (Mathf.Sin (theta) * gapL, 0, Mathf.Cos (theta) * gapL);
			Debug.Log (newpos);
			CreateLeaders (newpos);
		}

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
		leader.transform.position = this.transform.TransformPoint(newpos);
		leader.transform.rotation = this.transform.rotation;

		Path path = leader.AddComponent<Path> ();
		path.target = GameObject.Find ("Mothership");
		FollowPath fpath = leader.AddComponent<FollowPath> ();
		fpath.path = path;
		fpath.enabled = !fpath.enabled;
		//add obstaclavoid
		//Seek seek = leader.AddComponent<Seek>();
		//add path pathfollow
		//seek.target = leader.transform.position + leader.transform.forward * 1000;

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

		OffsetPursue op = follower.AddComponent<OffsetPursue> ();
		op.leader = leader;
	}


	// Update is called once per frame
	void Update ()
	{

	}
}
