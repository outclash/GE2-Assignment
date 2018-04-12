using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIcontrol : MonoBehaviour
{
	//set up on the inspector
	public GameObject a8spwn;
	//av8spawner
	public GameObject emspwn;
	//enemyspawner
	public GameObject fsSpwn;
	//fightscenespawner
	public GameObject mothership;
	//deactivate kr and enei12


	private List<GameObject> av8s = new List<GameObject> ();
	private List<GameObject> enemies = new List<GameObject> ();

	void Awake ()
	{
		//a8spwn = GameObject.Find ("av8Spawner").GetComponent<Av8Spawner> ();
	}
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (Setships ());
	}

	public IEnumerator Setships ()
	{  
		av8s.Clear ();
		enemies.Clear ();
		fsSpwn.SetActive(false);
		//to make sure everything is loading before executing this
		yield return new WaitForSeconds (10);
		//disable fightscene for now

		//add all av8 plane to list
		for (int i = 0; i < a8spwn.transform.childCount; i++) {
			av8s.Add (a8spwn.transform.GetChild (i).gameObject);
		}
		//add all enemy ships to list
		for (int i = 0; i < emspwn.transform.childCount; i++) {
			enemies.Add (emspwn.transform.GetChild (i).gameObject);
		}
		StartCoroutine (FireMissiles ());
		Debug.Log ("ab8count: " + av8s.Count);
	}

	public IEnumerator FireMissiles ()
	{
		for (int i = 0; i < av8s.Count; i++) {
			for (int j = 0; j < 3; j++) { //fire first 3 missilbe of each av8
				av8s [i].transform.Find ("Bombs").GetChild (j).gameObject.GetComponent<Seek> ().enabled = enabled;
				yield return new WaitForSeconds (1);
				av8s [i].transform.Find ("Bombs").GetChild (j).gameObject.GetComponent<Rigidbody> ().isKinematic = false;

			}
		}
		Debug.Log ("Missiles Fired!");
		StartCoroutine (Pathfollow ());
		yield break;
	}

	public IEnumerator Pathfollow ()
	{
		//add some timing
		foreach (GameObject av in av8s) {
			foreach (GameObject es in enemies) {
				if (av.name != "follower") {
					//disable previous behaviour of leader
					av.GetComponent<Arrive> ().enabled = !enabled;
				} else {
					//disable previous behaviour of follower
					av.GetComponent<OffsetPursue> ().enabled = !enabled;
				}
				//enable follow path script and obstacle avoidance for av8 planes
				av.GetComponent<FollowPath> ().enabled = enabled;
				av.GetComponent<ObstacleAvoidance> ().enabled = enabled;
				av.GetComponent<Boid> ().maxSpeed = 30;
				//disable previous behaviour of enemies
				es.GetComponent<Wander>().enabled = !enabled;
				//make seek script of enemies to follow each one of av8 planes
				Seek sk = es.GetComponent<Seek>();
				sk.targetGameObject = av;
				sk.enabled = !sk.enabled;
			}
		}
		yield break;
	}
	// Update is called once per frame
	void Update ()
	{

	}
}
