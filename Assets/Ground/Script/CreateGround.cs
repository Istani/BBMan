using UnityEngine;
using System.Collections;

public class CreateGround : MonoBehaviour
{
		public GameObject Cube;
		public GameObject Rahmen;
		GameObject[] Raender = new GameObject[4];
		Vector3 StartZone = new Vector3 (10, 10, 10);
		
		public void GenerateLevel ()
		{
				GameObject TmpObject;
				// Vorherige Blöcke löschen
				foreach (Transform child in transform) {
						Destroy (child.gameObject);
				}
				//Neue Blöcke erstellen
				for (int x=0; x<StartZone.x; x++) {
						for (int y=0; y<StartZone.y; y++) {
								for (int z=0; z<StartZone.z; z++) {
										TmpObject = (GameObject)Network.Instantiate (Cube, new Vector3 (x, -y, z), Quaternion.Euler (new Vector3 (0, 0, 0)), 0);
										TmpObject.transform.parent = transform;
								}
						}
				}

				// Spielfeld Rahmen
				Transform GroundParent = GameObject.Find ("Ground").transform;
				TmpObject = (GameObject)Instantiate (Rahmen, new Vector3 (-1, 1, StartZone.z / 2), Quaternion.Euler (new Vector3 (0, 0, 0)));
				TmpObject.transform.parent = GroundParent;
				Raender [0] = TmpObject;
				TmpObject = (GameObject)Instantiate (Rahmen, new Vector3 (StartZone.x, 1, StartZone.z / 2), Quaternion.Euler (new Vector3 (0, 0, 0)));
				TmpObject.transform.parent = GroundParent;
				Raender [1] = TmpObject;
				TmpObject = (GameObject)Instantiate (Rahmen, new Vector3 (StartZone.x / 2, 1, -1), Quaternion.Euler (new Vector3 (0, 0, 0)));
				TmpObject.transform.parent = GroundParent;
				Raender [2] = TmpObject;
				TmpObject = (GameObject)Instantiate (Rahmen, new Vector3 (StartZone.x / 2, 1, StartZone.z), Quaternion.Euler (new Vector3 (0, 0, 0)));
				TmpObject.transform.parent = GroundParent;
				Raender [3] = TmpObject;

				// Zerren
				Raender [0].transform.localScale = new Vector3 (1, StartZone.y * 2, StartZone.z + 1);
				Raender [1].transform.localScale = new Vector3 (1, StartZone.y * 2, StartZone.z + 1);
				Raender [2].transform.localScale = new Vector3 (StartZone.x + 1, StartZone.y * 2, 1);
				Raender [3].transform.localScale = new Vector3 (StartZone.x + 1, StartZone.y * 2, 1);
		}
}
