﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetPursue : SteeringBehaviour {

    public Boid leader;
    private Vector3 offset;
    Vector3 worldtarget;

    // Use this for initialization
    public void OnDrawGizmos()
    {
        if (isActiveAndEnabled && Application.isPlaying)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(transform.position, worldtarget);
        }
    }


    void Start () {
        offset = transform.position - leader.transform.position;
//		Debug.Log (transform.position);
//		Debug.Log (leader.transform.position);
//		Debug.Log (offset);
       // offset = Quaternion.Inverse(leader.transform.rotation) * offset;
//		Debug.Log (offset);
//		Debug.Log (Quaternion.Inverse(leader.transform.rotation));
    }

    // Update is called once per frame
    void Update () {
		
	}

    public override Vector3 Calculate()
    {
		worldtarget = leader.transform.position+offset;
//		Debug.Log (worldtarget);
        float dist = Vector3.Distance(worldtarget
            , transform.position);
        float time = dist / boid.maxSpeed;

        Vector3 targetPos = worldtarget + (leader.velocity * time);
//		Debug.Log (targetPos);
        return boid.ArriveForce(targetPos);

    }
}
