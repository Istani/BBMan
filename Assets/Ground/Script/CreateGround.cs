using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum FeldTyp
{
		nothing,
		portal,

}

public class CreateGround : MonoBehaviour
{
		public GameObject Cube;
		public GameObject Rahmen;

		public GameObject Ground;
		GameObject[] Raender = new GameObject[4];
		Vector3 StartZone = new Vector3 (50, 2, 50);

		public Dictionary<Vector2, FeldTyp> Felder = new Dictionary<Vector2, FeldTyp> ();
		
		public void GenerateLevel (int PlayerCount)
		{
				StartZone = new Vector3 (PlayerCount * 7, 2, PlayerCount * 7);
				GameObject TmpObject;
				// Vorherige Blöcke löschen
				foreach (Transform child in transform) {
						Network.Destroy (child.gameObject.GetComponent<NetworkView> ().viewID);
				}
				//Boden erstellen
				for (int x=0; x<StartZone.x; x++) {
						for (int y=1; y<StartZone.y; y++) {
								for (int z=0; z<StartZone.z; z++) {
										TmpObject = (GameObject)Network.Instantiate (Ground, new Vector3 (x, -y, z), Quaternion.Euler (new Vector3 (0, 0, 0)), 0);
										TmpObject.transform.parent = transform;
								}
						}
				}

				// Spielfeld Rahmen
				Transform GroundParent = GameObject.Find ("Ground").transform;
				TmpObject = (GameObject)Network.Instantiate (Rahmen, new Vector3 (-1, 1, StartZone.z / 2), Quaternion.Euler (new Vector3 (0, 0, 0)), 0);
				TmpObject.transform.parent = GroundParent;
				Raender [0] = TmpObject;
				TmpObject = (GameObject)Network.Instantiate (Rahmen, new Vector3 (StartZone.x, 1, StartZone.z / 2), Quaternion.Euler (new Vector3 (0, 0, 0)), 0);
				TmpObject.transform.parent = GroundParent;
				Raender [1] = TmpObject;
				TmpObject = (GameObject)Network.Instantiate (Rahmen, new Vector3 (StartZone.x / 2, 1, -1), Quaternion.Euler (new Vector3 (0, 0, 0)), 0);
				TmpObject.transform.parent = GroundParent;
				Raender [2] = TmpObject;
				TmpObject = (GameObject)Network.Instantiate (Rahmen, new Vector3 (StartZone.x / 2, 1, StartZone.z), Quaternion.Euler (new Vector3 (0, 0, 0)), 0);
				TmpObject.transform.parent = GroundParent;
				Raender [3] = TmpObject;

				// Zerren
				Raender [0].transform.localScale = new Vector3 (1, StartZone.y * 2, StartZone.z + 1);
				Raender [1].transform.localScale = new Vector3 (1, StartZone.y * 2, StartZone.z + 1);
				Raender [2].transform.localScale = new Vector3 (StartZone.x + 1, StartZone.y * 2, 1);
				Raender [3].transform.localScale = new Vector3 (StartZone.x + 1, StartZone.y * 2, 1);

				// Nun den Inhalt
				int rand = 3;
				for (int x=rand; x<StartZone.x-rand; x++) {
						for (int z=rand; z<StartZone.z-rand; z++) {
								Felder.Add (new Vector2 (x, z), FeldTyp.nothing);
						}
				}
				int tmp_player_count = PlayerCount - 2;
				while (tmp_player_count>0) {
						Vector2[] FKeys = new Vector2[Felder.Keys.Count];
						FKeys = Felder.Keys.ToArray ();
						int rng = Random.Range (0, FKeys.Length + 1);
						if (Felder [FKeys [rng - 1]] == FeldTyp.nothing) {
								tmp_player_count -= 1;
								Felder [FKeys [rng - 1]] = FeldTyp.portal;
						}
				}
				foreach (Vector2 EXPOS in Felder.Keys) {
						TmpObject = (GameObject)Network.Instantiate (Cube, new Vector3 (EXPOS.x, 0, EXPOS.y), Quaternion.Euler (new Vector3 (0, 0, 0)), 0);
						TmpObject.transform.parent = transform;
				}
		}
}
