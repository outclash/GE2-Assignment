using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIcontrol : MonoBehaviour
{

	public GameObject a8spwn;
	public GameObject mothership;
	private List<GameObject> av8s = new List<GameObject> ();
	//list of enemy

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
		yield return new WaitForSeconds (10);
		//add all av8 plane to list
		av8s.Clear ();
		for (int i = 0; i < a8spwn.transform.childCount; i++) {
			av8s.Add (a8spwn.transform.GetChild (i).gameObject);
		}
		StartCoroutine (FireMissiles ());
		Debug.Log ("ab8count: " + av8s.Count);
	}

	public IEnumerator FireMissiles ()
	{
		for (int i = 0; i < av8s.Count; i++) {
			for (int j = 0; j < 3; j++) { //fire first 3 missilbe of each av8
				av8s [i].transform.Find ("Body/Bombs").GetChild (j).gameObject.GetComponent<Seek> ().enabled = enabled;
				yield return new WaitForSeconds (1);
			}
		}
		Debug.Log ("Missiles Fired!");
		yield break;
	}
	// Update is called once per frame
	void Update ()
	{

	}
}
