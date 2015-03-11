using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
		float max_speed = 6;
		Vector3 bewegung;

		void Start ()
		{
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (networkView.isMine) {
						// Bewegung festlegen
						bewegung.z = Input.GetAxis ("Vertical") * max_speed;
						transform.Rotate (new Vector3 (0, Input.GetAxis ("Horizontal") * max_speed / 2, 0));
						//if (Input.GetButtonUp ("Jump")) {
						//		bewegung.y = max_speed * 10;
						//}
						//Debug.Log (bewegung);

						// Bewegung ausführen
						bewegung.x = bewegung.x - (Time.deltaTime);
						if (bewegung.x < 0) {
								bewegung.x = 0;
						}
						bewegung.z = bewegung.z - (Time.deltaTime * 5);
						if (bewegung.z < 0) {
								bewegung.z = 0;
						}
						bewegung.y = 0;
						transform.Translate (bewegung * Time.deltaTime);
						
				}
		}

		[RPC]
		void SetGameObjectName (string Name)
		{
				gameObject.name = Name;
		}
}
