using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionDetection : MonoBehaviour
{
		CreateGround GroundScript;
		List<GameObject> TocuhObjects = new List<GameObject> ();
		float TimeToExplode = 3.0f;

		void Start ()
		{
				if (networkView.isMine) {
						GroundScript = GameObject.Find ("Ground").GetComponent<CreateGround> ();
				}
		}

		void Update ()
		{
				
				SphereCollider SCol = gameObject.GetComponent<SphereCollider> ();
				SCol.radius = 2f;
				TimeToExplode -= Time.deltaTime;

				if (TimeToExplode <= 0) {
						TimeToExplode = 99;
						if (networkView.isMine) {
								DoExplosiveDmg ();
						}
				}
		}
		void OnTriggerEnter (Collider other)
		{
				TocuhObjects.Add (other.gameObject);
		}

		void OnTriggerExit (Collider other)
		{
				TocuhObjects.Remove (other.gameObject);
		}

		public void DoExplosiveDmg ()
		{
				foreach (GameObject Objects in TocuhObjects) {
						if (Objects.layer == 9) {
								// Abfrage was fürn Object das ist, wenn Portal dann Spawn Portal ;)
								Vector3 SpawnVector = Objects.transform.position;
								Vector2 CheckVector = new Vector2 (SpawnVector.x, SpawnVector.z);
								Network.Destroy (Objects.gameObject.GetComponent<NetworkView> ().viewID);
								if (GroundScript.Felder [CheckVector] == FeldTyp.portal) {
										//Spawn Portal
								}
						}
						// Was wenn es Spieler sind
				}

				Network.Destroy (transform.parent.gameObject.GetComponent<NetworkView> ().viewID);
		}
}
