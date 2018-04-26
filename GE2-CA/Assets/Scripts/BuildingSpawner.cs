using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{


	public GameObject house1;
	public GameObject house2;
	public GameObject house3;

	public int xInc = 50;
	public int zInc = 50;
	public int offset = 250;
	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				GameObject house = GameObject.Instantiate<GameObject> (RandomHouse ());
				house.transform.parent = this.transform;
				house.transform.position = new Vector3 ((i * xInc)-offset, 0, (j * zInc)-offset);
				house.transform.localScale = RandomVec3 ();
				//xInc += 10;
				//zInc += 10;
			}
		}
	}

	//Returns random house prefab
	private GameObject RandomHouse ()
	{
		int option = Random.Range (1, 4);
		switch (option) {
		default:
			return null;
			break;
		case 1:
			return house1;
			break;
		case 2:
			return house2;
			break;
		case 3:
			return house3;
			break;
		}
	}

	private Vector3 RandomVec3(){
		return new Vector3 (Random.Range(5,10),Random.Range(5,10),2.0f);
	}

}
