﻿using UnityEngine;
using System.Collections;

public class followCenter : MonoBehaviour {
	public GameObject player;
	//public GameObject changeto;
	//private bool changeing;
	// Use this for initialization
	void Start () {
	
	}

	void changeCenter(GameObject newplayer){
		player = newplayer;
	}
	// Update is called once per frame
	void Update () {
		//if (changeing) {
		if(player!=null) transform.position = new Vector3 (Mathf.Lerp (transform.position.x, player.transform.position.x, Time.deltaTime * 5), Mathf.Lerp (transform.position.y, player.transform.position.y, Time.deltaTime * 5), 0);
		//}
		//transform.position = player.transform.position;
	
	}
}
