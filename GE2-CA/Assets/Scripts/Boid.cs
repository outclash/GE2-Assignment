﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
	List<SteeringBehaviour> behaviours = new List<SteeringBehaviour> ();
    
	public Vector3 force = Vector3.zero;
	public Vector3 acceleration = Vector3.zero;
	public Vector3 velocity = Vector3.zero;
	public float mass = 1;
	public float maxSpeed = 5.0f;
    
	public GameObject explosionPrefab = null;
	// Use this for initialization
	void Start ()
	{

		SteeringBehaviour[] behaviours = GetComponents<SteeringBehaviour> ();

		foreach (SteeringBehaviour b in behaviours) {
			this.behaviours.Add (b);
		}
	}

	public Vector3 SeekForce (Vector3 target)
	{
		Vector3 desired = target - transform.position;
		desired.Normalize ();
		desired *= maxSpeed;
		return desired - velocity;
	}

	public Vector3 ArriveForce (Vector3 target, float slowingDistance = 50.0f, float deceleration = 1.0f)
	{
		Vector3 toTarget = target - transform.position;

		float distance = toTarget.magnitude;
		if (distance == 0) {
			return Vector3.zero;
		}
		float ramped = maxSpeed * (distance / (slowingDistance * deceleration));

		float clamped = Mathf.Min (ramped, maxSpeed);
		Vector3 desired = clamped * (toTarget / distance);

		return desired - velocity;
	}


	Vector3 Calculate ()
	{
		force = Vector3.zero;

		foreach (SteeringBehaviour b in behaviours) {
			if (b.isActiveAndEnabled) {
				force += b.Calculate () * b.weight;
			}
		}


		return force;
	}

	
	// Update is called once per frame
	void Update ()
	{
		force = Calculate ();
		Vector3 newAcceleration = force / mass;

		float smoothRate = Mathf.Clamp (9.0f * Time.deltaTime, 0.15f, 0.4f) / 2.0f;
		acceleration = Vector3.Lerp (acceleration, newAcceleration, smoothRate);
        
		velocity += acceleration * Time.deltaTime;

		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);

		Vector3 globalUp = new Vector3 (0, 0.2f, 0);
		Vector3 accelUp = acceleration * 0.05f;
		Vector3 bankUp = accelUp + globalUp;
		smoothRate = Time.deltaTime * 3f;
		Vector3 tempUp = transform.up;
		tempUp = Vector3.Lerp (tempUp, bankUp, smoothRate);

		if (velocity.magnitude > 0.0001f) {
			transform.LookAt (transform.position + velocity, tempUp);
			velocity *= 0.99f;
		}
		transform.position += velocity * Time.deltaTime;        
	}
		
	public void ExplodeMyParts()
	{
		foreach (Transform t in this.GetComponentsInChildren<Transform>())
		{
			Rigidbody rb = t.gameObject.GetComponent<Rigidbody>();
			if (rb == null)
			{
				rb = t.gameObject.AddComponent<Rigidbody>();
			}
			rb.useGravity = true;
			rb.isKinematic = false;
			Vector3 v = new Vector3(
				Random.Range(-5, 5)
				, Random.Range(5, 10)
				, Random.Range(-5, 5)
			);
			rb.velocity = v;
		}
		Destroy(this.gameObject, 5);
	}

	void OnCollisionEnter (Collision other)
	{

		if (other.gameObject.CompareTag("EnemyBullet") && this.gameObject.CompareTag ("Av8s")) {
			Debug.Log ("hit av8");
			ExplodeMyParts ();
			//add explosion fx
			GameObject go = Instantiate(explosionPrefab,this.gameObject.transform.position,Quaternion.identity);
			go.transform.localScale = new Vector3 (1, 1, 1);
			Destroy (go, 3);
		}
		if (this.gameObject.CompareTag("Missiles") && other.gameObject.CompareTag("Mothership")) {
			Debug.Log ("explode");
			//add explosion fx
			GameObject go = Instantiate(explosionPrefab,this.gameObject.transform.position,Quaternion.identity);
			Destroy(this.gameObject);
			Destroy (go, 3);
		}
		if (this.gameObject.CompareTag("Missiles") && other.gameObject.CompareTag("Enemy")) {
			Debug.Log ("hit enemy");
			//add explosion fx
			GameObject go = Instantiate(explosionPrefab,this.gameObject.transform.position,Quaternion.identity);
			Destroy(this.gameObject);
			Destroy (go, 3);
		}
	}
}
