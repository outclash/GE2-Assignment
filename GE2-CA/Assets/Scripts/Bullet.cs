﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 50;
	// Use this for initialization
	void Start ()
	{
		Invoke ("KillMe", 3);
	}

	void KillMe ()
	{
		GameObject.Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Translate (0, 0, speed * Time.deltaTime);
	}
		
}
