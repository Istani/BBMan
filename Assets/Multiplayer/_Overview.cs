using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _Overview : MonoBehaviour
{
		//GUI_Helper GUI_ZoD = new GUI_Helper ();
		string GameKennzeichen = "BBMan";
		//string IP = "127.0.0.1";
		int Port = 25000;
		bool NAT = true;

		//string connectTo = "";
		float StartTimer = 0.0f;
		float StartCooldown = 1f;
		float UpdateTimer = 0.0f;
		float UpdateCooldown = 5f;
		Vector2 scrollPosition = Vector2.zero;
		bool server_not_connect = false;
		public bool GameToStart = false;
		bool StartSoon = false;
		
		public GameObject Player;
		public Dictionary<NetworkPlayer, string> PlayerNames = new Dictionary<NetworkPlayer, string> ();
		string PName = "";

		// Use this for initialization
		void Start ()
		{
				//IP = Network.player.externalIP;
				MasterServer.ipAddress = "81.20.139.46";
				MasterServer.port = 23466;
				Network.natFacilitatorIP = MasterServer.ipAddress;
				Network.natFacilitatorPort = 23467;
				int rng = Random.Range (0, Port);
				string tmp = "";
				tmp = PlayerPrefs.GetString ("Player_Name");
				if (tmp == "" || tmp == null) {
						PName = "Player " + rng.ToString ();
				} else {
						PName = tmp;
				}
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
				if (Network.isServer && StartSoon) {
						StartTimer -= Time.deltaTime;
						if (StartTimer <= 0) {
								networkView.RPC ("GameStart", RPCMode.All);
								StartSoon = false;
						}
				}
		}

		void OnGUI ()
		{
				
				if (Network.isServer || Network.isClient) {
						if (Network.isServer && GameToStart == false && StartSoon == false) {
								Rect AnzeigeGamemnue = new Rect (50, 50, Screen.width - 100, Screen.height - 100);
								GUI.Box (AnzeigeGamemnue, "");
								GUILayout.BeginArea (AnzeigeGamemnue);
								int i = 0;
								foreach (NetworkPlayer p in PlayerNames.Keys) {
										i++;
										GUILayout.Label (i + ": " + PlayerNames [p]);
								}
								if (GUILayout.Button ("Start")) {
										StartTimer = StartCooldown;
										StartSoon = true;
								}
								GUILayout.EndArea ();
						} 
						if (Network.isClient && GameToStart == false) {
								Rect AnzeigeGamemnue = new Rect (50, 50, Screen.width - 100, Screen.height - 100);
								GUI.Box (AnzeigeGamemnue, "");
								GUILayout.BeginArea (AnzeigeGamemnue);
								GUILayout.Label ("Wait for the Game to Start...");
								GUILayout.EndArea ();
						} 
				} else {
						Rect AnzeigeGamemnue = new Rect (50, 50, Screen.width - 100, Screen.height - 100);
						GUI.Box (AnzeigeGamemnue, "");
						GUILayout.BeginArea (AnzeigeGamemnue);
						if (server_not_connect) {
								GUILayout.Label ("Keine Verbindung zum Master Server!");
						} else {
								GUILayout.BeginHorizontal ();
								PName = GUILayout.TextField (PName);
								if (GUILayout.Button ("Spiel erstellen")) {
										PlayerPrefs.SetString ("Player_Name", PName);
										Network.InitializeServer (31, Port, NAT); 
										MasterServer.RegisterHost (GameKennzeichen, "Beta", "Dev");
										PlayerNames.Add (Network.player, PName);
								}
								GUILayout.EndHorizontal ();
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
														PlayerPrefs.SetString ("Player_Name", PName);
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
				networkView.RPC ("PlayerAdd", RPCMode.Server, Network.player, PName);
		}
		void OnPlayerDisconnected (NetworkPlayer player)
		{
				PlayerNames.Remove (player);
				Network.RemoveRPCs (player);
				if (GameToStart) {
						Network.DestroyPlayerObjects (player);
				}
		}

		void OnDisconnectedFromServer ()
		{
				GameToStart = false;
				Application.LoadLevel (0);
		}

		[RPC]
		public void PlayerAdd (NetworkPlayer player, string PN)
		{
				PlayerNames.Add (player, PN);
		}

		[RPC]
		public void GameStart ()
		{
				if (Network.isServer) {
						MasterServer.UnregisterHost ();
						GameObject.Find ("Ground").GetComponent<CreateGround> ().GenerateLevel (PlayerNames.Count);
				}
				GameObject obj = (GameObject)Network.Instantiate (Player, new Vector3 (0, 0, 0), Quaternion.identity, 0);
				obj.name = PName;
				obj.transform.FindChild ("Camera").gameObject.SetActive (true);
				GameToStart = true;
		}
	
}
