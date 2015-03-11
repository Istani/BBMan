using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
		public GameObject Bomb;
		// Use this for initialization
		void Start ()
		{
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (networkView.isMine) {
						
						if (Input.GetButtonUp ("Fire1")) {
								networkView.RPC ("LayBomb", RPCMode.Server); // Damit die Bombe übern Server läuft
						}
				}
		}

		[RPC]
		public void LayBomb ()
		{
				GameObject TmpBomb;
				TmpBomb = (GameObject)Network.Instantiate (Bomb, transform.position, transform.rotation, 0);
				TmpBomb.transform.parent = GameObject.Find ("Bomben").transform;
		}
}
