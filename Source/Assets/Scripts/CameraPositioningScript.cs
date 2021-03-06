﻿using UnityEngine;
using System.Collections;

public class CameraPositioningScript : MonoBehaviour {
	public GameObject target;
	public GameObject motherNode;
	public Vector3 currentNodePos;
	public Vector3 otherNodePos;
	private Transform oldSecond;
	private int oldSecondIndex;
	private int currentSecondIndex;
	private Transform currentSecond;
	public bool usingOld;
	public float cameraHeight = 2.0f;
	Transform[] directionNodes;
	public float lerpything;
	public float d;

	private Vector3 initialPos;

	public Vector3 GetCurrentNodePosition()
	{
		return currentNodePos;
	}

	public void ResetToStart(){
		transform.position = initialPos;
		}

	// Use this for initialization
	void Start () {
		initialPos = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(target == null)
			target = GameObject.FindGameObjectWithTag("TheBall");

		directionNodes = motherNode.GetComponentsInChildren<Transform>();
		float minDistance = 10000.0f;
		float secondDistance = 10000.0f;
		Transform closest = null;
		Transform secondClosest = null;
		int n = 0;
		int closestIndex = 0;
		int secondClosestIndex = 0;
		foreach (Transform node in directionNodes) {
			
			float dist = Vector3.Distance(target.transform.position, node.position);
			if(dist < minDistance){
				secondClosest = closest;
				secondDistance = minDistance;
				closest = node;
				secondClosestIndex = closestIndex;
				closestIndex = n;
				minDistance = dist;
			} else {
				if(secondClosest == null){
					secondClosest = node;
					secondDistance = dist;
					 secondClosestIndex = n;
				} else {
					if (dist < secondDistance){
						secondClosest = node;
						secondDistance = dist;
						 secondClosestIndex = n;
					}
				}
			}
			n++;
		}
		
		
		
		closestIndex--;
		secondClosestIndex--;
		
		if(currentSecond != secondClosest){
			oldSecond = currentSecond;
			oldSecondIndex = currentSecondIndex;
		}
		
		currentSecond = secondClosest;
		currentSecondIndex = secondClosestIndex;
							
		MeshRenderer[] meshRenderers;
		meshRenderers = motherNode.GetComponentsInChildren<MeshRenderer>();
		for(int i = 0; i < meshRenderers.Length; i++)
		{
			if(i == closestIndex)
				meshRenderers[i].material.SetColor("_Color", Color.red);
			else if(i == secondClosestIndex)
				meshRenderers[i].material.SetColor("_Color", Color.black);
			else if(i == oldSecondIndex)
				meshRenderers[i].material.SetColor("_Color", Color.yellow);
			else
				meshRenderers[i].material.SetColor("_Color", Color.white);
		}
		
		currentNodePos = closest.position;

		Transform secondClosestToUse = currentSecond;

		usingOld = false;
		if(oldSecond != null && Vector3.Dot(target.rigidbody.velocity, closest.position - target.transform.position) > 0.0f && 
			Vector3.Dot(target.rigidbody.velocity, currentSecond.position - target.transform.position) > 0.0f)
		{
			secondClosestToUse = oldSecond;
			usingOld = true;
		}
		otherNodePos= secondClosestToUse.position;
		float D =Vector3.Distance(closest.position, secondClosestToUse.position);
		Vector3 targetRelPos = closest.InverseTransformPoint(target.transform.position);
		//d = Vector3.Distance(target.transform.position, closest.position);
		d = Mathf.Abs(targetRelPos.y);
		lerpything= 1.0f - (D - d)/D;
		Vector3 cameraOffset = Vector3.Lerp(-closest.up, -secondClosestToUse.up, lerpything);
		cameraOffset.y = 0;
		cameraOffset.Normalize ();
		cameraOffset *= 1.5f;
		cameraOffset.y += cameraHeight;

		transform.position = Vector3.Lerp (transform.position, target.transform.position + cameraOffset, Time.deltaTime * 8.0f);
		Quaternion oldRot = transform.rotation;
		transform.LookAt (transform.position - cameraOffset + new Vector3(0,0.8f,0));
		Quaternion newRot = transform.rotation;
		transform.rotation = Quaternion.Lerp(oldRot,newRot,2.0f*Time.deltaTime);


	}

	void SetTarget(GameObject target)
	{
		this.target = target;
	}

}
