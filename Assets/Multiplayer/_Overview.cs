using UnityEngine;
using System.Collections;

public class _Overview : MonoBehaviour
{
		GUI_Helper GUI_ZoD = new GUI_Helper ();
		string GameKennzeichen = "BBMan";
		string IP = "127.0.0.1";
		int Port = 25000;
		bool NAT = true;

		string connectTo = "";

		float UpdateTimer = 0.0f;
		float UpdateCooldown = 5f;
		Vector2 scrollPosition = Vector2.zero;
		public bool server_not_connect = false;
		
		public GameObject Player;
		// Use this for initialization
		void Start ()
		{
				IP = Network.player.externalIP;
				MasterServer.ipAddress = "81.20.139.46";
				MasterServer.port = 23466;
				Network.natFacilitatorIP = MasterServer.ipAddress;
				Network.natFacilitatorPort = 23467;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Network.isServer || Network.isClient) {

				} else {
						if (UpdateTimer <= 0) {
								MasterServer.RequestHostList (GameKennzeichen);
								UpdateTimer = UpdateCooldown;
						}
						UpdateTimer -= Time.deltaTime;
				}
		}

		void OnGUI ()
		{
				
				if (Network.isServer || Network.isClient) {
			
				} else {
						Rect AnzeigeGamemnue = new Rect (50, 50, Screen.width - 100, Screen.height - 100);
						GUI.Box (AnzeigeGamemnue, "");
						GUILayout.BeginArea (AnzeigeGamemnue);
						if (server_not_connect) {
								GUILayout.Label ("Keine Verbindung zum Master Server!");
						} else {
								if (GUILayout.Button ("Spiel erstellen")) {
										Network.InitializeServer (3, Port, NAT); 
										MasterServer.RegisterHost (GameKennzeichen, "Beta", "Dev");
										Network.Instantiate (Player, new Vector3 (1, 2, 0), Quaternion.identity, 0);
								}
								HostData[] data = MasterServer.PollHostList ();
								if (data.Length == 0 && server_not_connect == false) {
										GUILayout.Label ("Keine offene Spiele gefunden!");
										GUILayout.Label ("Warte einen Augenblick oder erstelle selbst ein Spiel!");
								}
								foreach (HostData element in data) {
										scrollPosition = GUILayout.BeginScrollView (scrollPosition);
										GUILayout.BeginHorizontal ();
										if (element.connectedPlayers < element.playerLimit) { 
												if (GUILayout.Button ("Connect")) {
														Network.Connect (element);        
												}
												var name = element.gameName + " (Players: " + element.connectedPlayers + " / " + element.playerLimit + ")";
												GUILayout.Label (name);  
												GUILayout.Space (5);
												GUILayout.Space (5);
												GUILayout.Label (element.comment);
												GUILayout.Space (5);
												GUILayout.FlexibleSpace ();
										}
										GUILayout.EndHorizontal ();  
										GUILayout.EndScrollView ();
								}
						}
						GUILayout.EndArea ();
				}
				
		}
	
		void OnFailedToConnectToMasterServer (NetworkConnectionError info)
		{
				server_not_connect = true;
		}
		void OnConnectedToServer ()
		{
				Debug.Log ("test");
				Network.Instantiate (Player, new Vector3 (1, 2, 0), Quaternion.identity, 0);
		}
		void OnPlayerDisconnected (NetworkPlayer player)
		{
				Network.RemoveRPCs (player);
				Network.DestroyPlayerObjects (player);
		}
	
	
}
