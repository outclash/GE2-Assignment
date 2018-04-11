﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

	public List<Vector3> waypoints = new List<Vector3> ();
	public GameObject mothership = null;
	public int next = 0;
	public bool looped = true;
	//public int count;
	//public Vector3 range;
	public bool isRandom = true;

	public void OnDrawGizmos ()
	{
		int count;
		if (isRandom) {
			count = looped ? (waypoints.Count + 1) : waypoints.Count;
			Gizmos.color = Color.red;
			for (int i = 1; i < count; i++) {
				Vector3 prev = waypoints [i - 1];
				Vector3 next = waypoints [i % waypoints.Count];
				Gizmos.DrawLine (prev, next);
				Gizmos.DrawSphere (prev, 1);
				Gizmos.DrawSphere (next, 1);
			}
		} else {
			count = looped ? (transform.childCount + 1) : transform.childCount;
			Gizmos.color = Color.cyan;
			for (int i = 1; i < count; i++)
			{
				Transform prev = transform.GetChild(i - 1);
				Transform next = transform.GetChild(i % transform.childCount);
				Gizmos.DrawLine(prev.transform.position, next.transform.position);
				Gizmos.DrawSphere(prev.position, 1);
				Gizmos.DrawSphere(next.position, 1);
			}
		}

	}

	// Use this for initialization
	void Start ()
	{
		waypoints.Clear ();
		if (isRandom) {
			int count = 10;
			Vector3 range = mothership.GetComponent<Collider> ().bounds.size;
			for (int i = 0; i < count; i++) {
				Vector3 offset = Random.onUnitSphere;
				offset.x = offset.x * range.x;
				offset.y = offset.y * range.y;
				offset.z = offset.z * range.z;
				waypoints.Add (mothership.transform.position + offset);
			}
		} else {
			//add path from gameobject
			int count = transform.childCount;
			for (int i = 0; i < count; i++) {
				waypoints.Add (transform.GetChild (i).position);
			}
//			waypoints.Add (new Vector3 (300, 50, 0));
//			waypoints.Add (new Vector3 (100, 50, 0));
//			waypoints.Add (new Vector3 (0, 50, 0));
//			waypoints.Add (new Vector3 (-80, 50, 0));
//			waypoints.Add (new Vector3 (-100, 75, 0));
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public Vector3 NextWaypoint ()
	{
		return waypoints [next];
	}

	public void AdvanceToNext ()
	{
		if (looped) {
			next = (next + 1) % waypoints.Count;
		} else {
			if (next != waypoints.Count - 1) {
				next++;
			}
		}
	}

	public bool IsLast ()
	{
		return next == waypoints.Count - 1;
	}
}
