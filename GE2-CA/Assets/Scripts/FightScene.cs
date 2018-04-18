using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : MonoBehaviour
{

	public GameObject fsSpwn;
	public GameObject av8prefab;
	public GameObject enemyprefab;
	public GameObject expPrefab;
	private List<Path> paths = new List<Path> ();
	private List<GameObject> eships = new List<GameObject> ();
	private List<GameObject> av8ships = new List<GameObject> ();
	// Use this for initialization
	void Start ()
	{
		paths.Clear ();
		eships.Clear ();
		av8ships.Clear ();
		foreach (Path p in gameObject.GetComponentsInChildren(typeof(Path))) {
			paths.Add (p);
		}


		//Creates the enemy to get shot by
		GameObject leader = GameObject.Instantiate<GameObject> (enemyprefab);
		leader.transform.parent = this.transform;
		leader.transform.position = this.transform.GetChild (1).position; //position at the start of the epath
		leader.transform.rotation = this.transform.GetChild (1).rotation; 
		FollowPath fpath = leader.AddComponent<FollowPath> ();
		fpath.path = paths [1]; //follows the epath
		fpath.enabled = fpath.enabled;
		eships.Add (leader);

		//Creates the enemy that will shoot the av8 plane
		leader = GameObject.Instantiate<GameObject> (enemyprefab);
		leader.transform.parent = this.transform;
		leader.transform.position = this.transform.GetChild (0).TransformPoint (new Vector3 (0, 0, -10)); //position at the start of the apath with offset
		leader.transform.rotation = this.transform.GetChild (0).rotation;
		eships.Add (leader);

		//Creates the av8 plane that will shoot the enemy
		leader = GameObject.Instantiate<GameObject> (av8prefab);
		leader.transform.parent = this.transform;
		leader.transform.position = this.transform.position;
		leader.transform.rotation = this.transform.rotation;
		//Add boid and seek to the  bombs(missiles)
		int count = leader.transform.Find ("Bombs").gameObject.transform.childCount;
		for (int i = 0; i < count; i++) {
			Boid b = leader.transform.Find ("Bombs").GetChild (i).gameObject.AddComponent<Boid> ();
			b.maxSpeed = 45f;
			b.explosionPrefab = expPrefab;
			Pursue sb = leader.transform.Find ("Bombs").GetChild (i).gameObject.AddComponent<Pursue> ();
			sb.target = eships [0].GetComponent<Boid> ();
			sb.enabled = !sb.enabled;
		}
		av8ships.Add (leader);

		//Creates the av8 plane that will get shot by
		leader = GameObject.Instantiate<GameObject> (av8prefab);
		leader.transform.parent = this.transform;
		leader.transform.position = this.transform.GetChild (0).position; //position at the start of the apath
		leader.transform.rotation = this.transform.GetChild (0).rotation; 
		fpath = leader.AddComponent<FollowPath> ();
		fpath.path = paths [0]; //follows the apath
		fpath.enabled = !fpath.enabled;
		av8ships.Add (leader);

		StartCoroutine (Enemykill ());
	}


	public IEnumerator Enemykill ()
	{
		yield return new WaitForSeconds (2);
		//first plane shoot enemy
		av8ships [0].transform.Find ("Bombs").GetChild (5).gameObject.GetComponent<Pursue> ().enabled = enabled;
		yield return new WaitForSeconds (1);
		av8ships [0].transform.Find ("Bombs").GetChild (5).gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		StartCoroutine (Av8Kill ());
		yield break;
	}

	public IEnumerator Av8Kill ()//first plane shoot enemy
	{
		yield return new WaitForSeconds (1);
		av8ships [1].GetComponent<FollowPath> ().enabled = enabled;
		yield return new WaitForSeconds (1.7f);
		eships [1].GetComponent<FireBullets> ().Fire();
		yield break;
	}

	void Update(){
		if (av8ships [1] != null) {
			eships [1].transform.LookAt (av8ships [1].transform);
		}
	}
}
