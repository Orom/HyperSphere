﻿using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collInfo)
	{
		if (collInfo.gameObject.tag == "TheBall") {
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().StartNewRound();
		}
	}
}
