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
								networkView.RPC ("LayBomb", RPCMode.Server, transform.position); // Damit die Bombe übern Server läuft
						}
				}
		}
		
		[RPC]
		public void LayBomb (Vector3 Pos)
		{
				Network.Instantiate (Bomb, Pos, transform.rotation, 0);
		}
		
}
