using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : Photon.MonoBehaviour {

	PhotonView pv;
	int ping;
 

	public GameObject mainMenu;
    public GameObject txtField;
    public Text totalPlayersText;
    public Text pingText;
    public Text coinTxt;
    public Text gemTxt;
    public Text playerName;
	public Button multiPlayerBtn;


	void Start(){
        playerName.text = PlayerPrefs.GetString("playerName");
        coinTxt.text = comunication.InfoControl.coinAmount.ToString();
        gemTxt.text = comunication.InfoControl.gemAmount.ToString();
        PlayerPrefs.SetString ("roomName", "");
		pv = GetComponent<PhotonView> ();
		PhotonNetwork.ConnectUsingSettings ("version 0.0.1");
		PhotonNetwork.JoinLobby ();
	}

	void OnJoinedLobby(){
        playerName.text = PlayerPrefs.GetString("playerName");
        ping = PhotonNetwork.GetPing ();
		pingText.text = "Ping: " + ping;
		//if (ping < 300) {
			multiPlayerBtn.interactable = true;
		//}
		string pName = PlayerPrefs.GetString ("playerName");
		Debug.Log ("Joined Lobby!");
		Debug.Log ("player Name: "+ pName);
		if (String.IsNullOrEmpty(pName)) {
			txtField.SetActive (true);

		} else {
			mainMenu.SetActive (true);
		}
	}

	public void PlayMultiPlayerGame()
	{
		mainMenu.SetActive (false);
		if (PlayerPrefs.GetString ("roomName")!="") {
			PhotonNetwork.JoinRoom (PlayerPrefs.GetString ("roomName"));
			PlayerPrefs.SetInt ("roomEntryNumber", 0);
			Debug.Log ("i found a room.");
		}
		else
		PhotonNetwork.JoinRandomRoom ();
	}

	public void PlaySinglePlayerGame(){
		SceneManager.LoadScene (2);
	}

	void OnPhotonRandomJoinFailed(){

		Debug.Log ("creating room...");
		PhotonNetwork.CreateRoom("room1");

	}

	void OnJoinedRoom(){

		if (PhotonNetwork.isMasterClient)
			Debug.Log ("I am master");
		else
			Debug.Log ("I am not master");
		totalPlayersText.gameObject.SetActive (true);
		totalPlayersText.text = "Total Players: " + PhotonNetwork.playerList.Length;

	}

	void OnPhotonPlayerConnected(PhotonPlayer player){
		if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length==4) {
			Debug.Log ("Starting Game");
			pv.RPC ("StartMultiPlayerGame", PhotonTargets.AllBuffered);
		}
		totalPlayersText.text = "Total Players: " + PhotonNetwork.playerList.Length;
	}

	[PunRPC]
	void StartMultiPlayerGame()
	{
		//
	//	if (PhotonNetwork.isMasterClient)
	//		Debug.Log ("master");
		PhotonNetwork.LoadLevel (1);
		//totalPlayersText.text="Game is going to start";
	}

}
