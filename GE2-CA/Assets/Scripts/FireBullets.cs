using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
	
	public GameObject bulletSpawnPoint;
	public GameObject bulletPrefab;

	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Fire ()
	{
		GameObject bullet = GameObject.Instantiate<GameObject> (bulletPrefab);
		bullet.transform.position = bulletSpawnPoint.transform.position;
		bullet.transform.rotation = transform.rotation;

	}
}
