using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
		float max_speed = 6;
		Vector3 bewegung;
		Vector3 last_pos;

		void Start ()
		{
				// Check for skills?	
		}
	
		// Update is called once per frame
		void Update ()
		{
				// Bewegung festlegen
				bewegung.z = Input.GetAxis ("Vertical") * max_speed;
				transform.Rotate (new Vector3 (0, Input.GetAxis ("Horizontal") * max_speed / 2, 0));
				if (Input.GetButtonUp ("Jump")) {
						bewegung.y = max_speed * 20;
				}
				//Debug.Log (bewegung);

				// Bewegung ausführen
				last_pos = transform.position;
				bewegung.x = bewegung.x - (Time.deltaTime);
				if (bewegung.x < 0) {
						bewegung.x = 0;
				}
				bewegung.z = bewegung.z - (Time.deltaTime * 5);
				if (bewegung.z < 0) {
						bewegung.z = 0;
				}
				bewegung.y = bewegung.y;
				transform.Translate (bewegung * Time.deltaTime);
				bewegung.y = 0;
				
		}
}
