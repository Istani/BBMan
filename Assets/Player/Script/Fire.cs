﻿using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
		public GameObject Bomb;
		// Use this for initialization
		void Start ()
		{
				// Check for skills?
		}
	
		// Update is called once per frame
		void Update ()
		{
				GameObject TmpBomb;
				if (Input.GetButtonUp ("Fire1")) {
						TmpBomb = (GameObject)Instantiate (Bomb, transform.position, transform.rotation);
						TmpBomb.transform.parent = GameObject.Find ("Bomben").transform;
				}
		}
}