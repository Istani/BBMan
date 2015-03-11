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
				TimeToExplode -= Time.deltaTime;
				if (TimeToExplode <= 0) {
						DoExplosiveDmg ();
						TimeToExplode = 99;
				}
		}
		void OnTriggerEnter (Collider other)
		{
				Debug.Log (other.name);
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
								Destroy (Objects);
						}
				}

				Destroy (transform.parent.gameObject);
		}
}
