using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionDetection : MonoBehaviour
{
		List<GameObject> TocuhObjects = new List<GameObject> ();
		float TimeToExplode = 3.0f;
		// Use this for initialization
		void Update ()
		{
				if (networkView.isMine) { // weils nur der Server Spawnt ist es immer der Server
						TimeToExplode -= Time.deltaTime;
						if (TimeToExplode <= 0) {
								TimeToExplode = 99;
								DoExplosiveDmg ();
						}
				}
		}
		void OnTriggerEnter (Collider other)
		{
				if (networkView.isMine) {
						TocuhObjects.Add (other.gameObject);
				}
		}

		void OnTriggerExit (Collider other)
		{
				if (networkView.isMine) {
						TocuhObjects.Remove (other.gameObject);
				}
		}

		public void DoExplosiveDmg ()
		{
				if (networkView.isMine) {
						foreach (GameObject Objects in TocuhObjects) {
								if (Objects.layer == 9) {
										// Abfrage was fürn Object das ist, wenn Portal dann Spawn Portal ;)
										Network.Destroy (Objects.gameObject.GetComponent<NetworkView> ().viewID);
								}
								// Was wenn es Spieler sind
						}

						Network.Destroy (transform.parent.gameObject.GetComponent<NetworkView> ().viewID);
				}
		}
}
