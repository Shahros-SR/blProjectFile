using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerManager :Photon.MonoBehaviour {
	int[] shuffledCards = new int[52];
	int[] sortedCards = new int[52];
	int[] cardsOnTable = new int[4];
	int[] playerSequence = new int[4];
	int[] ranking = new int[4];
	bool[] didWin = new bool[4];
	bool[] isBotActive = new bool[4];

	string[] nameOfPlayers=new string[4];
	Transform[] playerPositions=new Transform[4];
    Transform[] cardsStartingPositions = new Transform[4];


	PhotonView pv;
	int turn;
	int cardsDistributed;
	int drawnCards;
	int winners;
	int playersPlayingGame;
	string suitToBePlayed=null;
	bool isCardToBeDrawn=true;
	bool firstTurn;
	float timer;


	public static MultiplayerManager instance;
	public Text centerText;
	public InputField userInput;
    public Transform[] cardsToBeDrawnPosition =new Transform[4];
	public Transform[] tablePositions;
	public Image[] totalCards;
	public Text[] playerNames;


	void Awake(){
		instance = this;
		pv = GetComponent<PhotonView> ();
		playersPlayingGame = 4;
		drawnCards = 0;
		firstTurn = true;
		turn = 1;
		cardsDistributed = 0;
		winners = 0;
		for (int i = 0; i < 4; i++) {
			isBotActive [i] = false;
		}

		PhotonNetwork.playerName = PlayerPrefs.GetString ("playerName");
		pv.RPC ("getName", PhotonTargets.All, PhotonNetwork.playerName,PhotonNetwork.player.ID);
		//string PlName = PhotonNetwork.playerName;
		//int x = PhotonNetwork.player.ID;
		//centerText.text = PlName;
	
	}

	// Use this for initialization
	void Start () 
	{

		Debug.Log ("I am player: " + PhotonNetwork.player.ID);

		if (PhotonNetwork.isMasterClient) {

			ShuffleCards ();
			foreach (var player in PhotonNetwork.playerList) {
				DistributeCards (player.ID,player);
				for(int i=0;i<4;i++){
					pv.RPC ("setNames", PhotonTargets.All);
				}
			}
			//pv.RPC ("displayCards", PhotonTargets.All);

		} else
			Debug.Log ("I am not master");
	
		//Remove errors from here...
	
		if (PhotonNetwork.player.ID == 1)
		{
			Debug.Log (" i am player 1");
			playerPositions [0] = tablePositions [0];
			playerPositions [1] = tablePositions [1];
			playerPositions [2] = tablePositions [2];
			playerPositions [3] = tablePositions [3];

            cardsStartingPositions[0] = cardsToBeDrawnPosition[0];
            cardsStartingPositions[1] = cardsToBeDrawnPosition[1];
            cardsStartingPositions[2] = cardsToBeDrawnPosition[2];
            cardsStartingPositions[3] = cardsToBeDrawnPosition[3];

        }
		else if (PhotonNetwork.player.ID == 2) {
			Debug.Log (" i am player 2");
			playerPositions [0] = tablePositions [3];
			playerPositions [1] = tablePositions [0];
			playerPositions [2] = tablePositions [1];
            playerPositions [3] = tablePositions [2];

            cardsStartingPositions[0] = cardsToBeDrawnPosition[3];
            cardsStartingPositions[1] = cardsToBeDrawnPosition[0];
            cardsStartingPositions[2] = cardsToBeDrawnPosition[1];
            cardsStartingPositions[3] = cardsToBeDrawnPosition[2];
            

        }
		else if (PhotonNetwork.player.ID == 3) {
			Debug.Log (" i am player 3");
			playerPositions [0] = tablePositions [2];
			playerPositions [1] = tablePositions [3];
			playerPositions [2] = tablePositions [0];
			playerPositions [3] = tablePositions [1];

            cardsStartingPositions[0] = cardsToBeDrawnPosition[2];
            cardsStartingPositions[1] = cardsToBeDrawnPosition[3];
            cardsStartingPositions[2] = cardsToBeDrawnPosition[0];
            cardsStartingPositions[3] = cardsToBeDrawnPosition[1];

        } 
		else if(PhotonNetwork.player.ID==4){
			Debug.Log (" i am player 4");
			playerPositions [0] = tablePositions [1];
			playerPositions [1] = tablePositions [2];
			playerPositions [2] = tablePositions [3];
			playerPositions [3] = tablePositions [0];

            cardsStartingPositions[0] = cardsToBeDrawnPosition[1];
            cardsStartingPositions[1] = cardsToBeDrawnPosition[2];
            cardsStartingPositions[2] = cardsToBeDrawnPosition[3];
            cardsStartingPositions[3] = cardsToBeDrawnPosition[0];
        }
			

	}



	// Update is called once per frame
	void Update () {


		if (PhotonNetwork.isMasterClient) 
		{
				if (isCardToBeDrawn == true) {

					if (timer > 2f) {
						if (drawnCards == 0 && firstTurn == false) {
							pv.RPC ("PutCardOutOfTable", PhotonTargets.All, false);
						}

						isCardToBeDrawn = false;
						timer = 0;
						if (isBotActive [turn - 1] == true) {
						
							if (didWin [turn - 1] == false) {
								pv.RPC ("setTurn", PhotonTargets.All, turn);
								pv.RPC ("botDrawCard", PhotonTargets.MasterClient, turn, suitToBePlayed);
							} else {
								turn++;
								if (turn > 4) {
									turn = 1;
								}
								isCardToBeDrawn = true;
							}
						} else {
							foreach (PhotonPlayer player in PhotonNetwork.playerList) {
								Debug.Log ("turn of Player: " + turn + " and it won." + didWin [turn - 1]);
								if (player.ID == turn) {
									if (didWin [turn - 1] == false) {
										pv.RPC ("setTurn", PhotonTargets.All, turn);
										//pv.RPC ("turnTeller", PhotonTargets.All, turn);
										pv.RPC ("GiveTurn", player, suitToBePlayed);
									} else {
										turn++;
										if (turn > 4) {
											turn = 1;
										}
										isCardToBeDrawn = true;
										break;
									}
								}
							}
						} 
					} else {
						timer += Time.deltaTime;
					}

					if (firstTurn == true) {
						pv.RPC ("setNames", PhotonTargets.All);
					}
				}

		}
	}

	void pileCards()
	{
		for (int i = 0; i < 52; i++) 
		{
			sortedCards [i] = i;
			//Debug.Log ("Sorted: " + sortedCards [i]);
		}
	}

	//shuffle the cards deck.
	void ShuffleCards()
	{
		pileCards ();
		int limit = 52;
		int rand = 0;
		for (int i = 0; i < 52; i++) 
		{
			rand = Random.Range (0,limit);
			shuffledCards [i] = sortedCards [rand];
			for (int j = rand; j < limit-1; j++) 
			{
				sortedCards [j] = sortedCards [j + 1];
			}
			//Debug.Log ("Shuffled: " + shuffledCards [i]);
			limit--;
		}
	}

	void DistributeCards(int player,PhotonPlayer pl)
	{
		for (int c = cardsDistributed; c <= cardsDistributed+12; c++) 
		{
			pv.RPC ("RecieveCards", PhotonTargets.All, shuffledCards [c],player);
			//Debug.Log(CardsScript.instance.GetCardRank(shuffledCards[c]));
			if (CardsScript.instance.GetCardRank (shuffledCards [c]) == 14 && CardsScript.instance.GetCardSuits (shuffledCards [c]) == "spades") 
			{
				turn = player;
				Debug.Log ("turn wil be: " + turn);
			}
		}
		pv.RPC ("SetCards", pl);
		cardsDistributed += 13;
	}


	public void TurnManager(int card , bool thola){
		int prevTurn;
		if (suitToBePlayed == null) {
			suitToBePlayed = CardsScript.instance.GetCardSuits (card);
		}
		prevTurn = turn;
		Debug.Log ("player " + turn + " drawing " + card);
		cardsOnTable [drawnCards] = card;
		playerSequence [drawnCards] = turn;
		drawnCards++;
		turn++;


		if (thola == true && firstTurn == false) {
			Debug.Log ("Giving thola");
			int max = CardsScript.instance.GetCardRank (cardsOnTable [0]);
			turn = playerSequence [0];
			for (int i = 1; i < drawnCards-1; i++) {
				if (max < CardsScript.instance.GetCardRank (cardsOnTable [i])) {
					max = CardsScript.instance.GetCardRank (cardsOnTable [i]);

					turn = playerSequence [i];
				}
			}
			giveThola(turn);
			pv.RPC("tholaTeller",PhotonTargets.All,prevTurn,turn);
			drawnCards = 0;
			suitToBePlayed = null;

		//	pv.RPC ("PutCardOutOfTable",PhotonTargets.All);
		}
		else if (drawnCards == playersPlayingGame) {
			int max = CardsScript.instance.GetCardRank (cardsOnTable [0]);
			turn = playerSequence [0];
			for (int i = 1; i < playersPlayingGame; i++) {
				if (max < CardsScript.instance.GetCardRank (cardsOnTable [i])) {
					max = CardsScript.instance.GetCardRank (cardsOnTable [i]);
					turn = playerSequence [i];
				}
			}
			drawnCards = 0;
			suitToBePlayed = null;
			firstTurn = false;
		//	pv.RPC ("PutCardOutOfTable",PhotonTargets.All);

		}
		if (turn > 4)
			turn = 1;
		
		pv.RPC ("checkForWinners", PhotonTargets.All);


		if (winners == 3) {
			pv.RPC ("PutCardOutOfTable",PhotonTargets.All,false);
			pv.RPC ("showFinalResult", PhotonTargets.All);
		} else {
			Debug.Log ("next is: " + turn);
			isCardToBeDrawn = true;
		}

	}

	public void SetPositionOfCardOnTable(int card,int player)
	{
        float pos = Random.Range(30, 60);
		totalCards [card].transform.position = cardsStartingPositions[player - 1].position;
        if(player%2==0)
            iTween.RotateTo(totalCards[card].gameObject, new Vector3(0f, 0f,pos), 1f);
        else
            iTween.RotateTo(totalCards[card].gameObject, new Vector3(0f, 0f, -pos), 1f);

        iTween.MoveTo(totalCards[card].gameObject, playerPositions[player - 1].position, 1f);
	}


	[PunRPC]
	void setNames(){
		if (PhotonNetwork.player.ID == 1) {
			playerNames [0].text = PhotonNetwork.playerName;
			playerNames [1].text = nameOfPlayers [1];
			playerNames [2].text = nameOfPlayers [2];
			playerNames [3].text = nameOfPlayers [3];

		} else if (PhotonNetwork.player.ID == 4) {
			playerNames [0].text = PhotonNetwork.playerName;
			playerNames [1].text = nameOfPlayers [0];
			playerNames [2].text = nameOfPlayers [1];
			playerNames [3].text = nameOfPlayers [2];

		} else if (PhotonNetwork.player.ID == 3) {
			playerNames [0].text = PhotonNetwork.playerName;
			playerNames [1].text = nameOfPlayers [3];
			playerNames [2].text = nameOfPlayers [0];
			playerNames [3].text = nameOfPlayers [1];

		} else if (PhotonNetwork.player.ID == 2) {
			playerNames [0].text = PhotonNetwork.playerName;
			playerNames [1].text = nameOfPlayers [2];
			playerNames [2].text = nameOfPlayers [3];
			playerNames [3].text = nameOfPlayers [0];
		}
	}

	[PunRPC]
	void GetCardFromClient(int card, bool thola,PhotonPlayer player)
	{
		
		pv.RPC ("RemoveCard", PhotonTargets.All, turn,card);
		pv.RPC ("SetCards", player);
		TurnManager (card,thola);
	}

	[PunRPC]
	void getCardFromBot(int card, int player,bool thola){
		pv.RPC ("RemoveCard", PhotonTargets.All, turn,card);
		TurnManager (card,thola);
	}
	void giveThola(int turn){

		for(int i=0;i<drawnCards;i++) {
			pv.RPC ("RecieveCards", PhotonTargets.All, cardsOnTable[i], turn);
		}
		pv.RPC ("SetCards", PhotonTargets.All);
	}

	[PunRPC]
	void tholaTeller(int givenBy, int givenTo){
		centerText.text = "Thola by player: " + givenBy + " To player: " + givenTo;
	}

	[PunRPC]
	void PutCardOutOfTable(bool setFirstturn){
		firstTurn = false;
		foreach (var c in totalCards) {
			c.transform.localPosition= new Vector3 (0, 0, 0);
			c.transform.localRotation = new Quaternion (0, 0, 0, 0);
		}
	}
	[PunRPC]
	void checkForWinners(){
		for (int i = 0; i < 4; i++) {
			if (didWin [i] == false) {
				didWin[i] = PlayerCardManager.instance.CheckForWinners (i);
				if (didWin [i] == true) {
					ranking [winners] = i + 1;
					winners++;
					playersPlayingGame--;
				}
			}
				
		}
	}

	[PunRPC]
	void getName(string name,int player){
		Debug.Log (name + " is player " + player);
		nameOfPlayers [player - 1]=name;
		centerText.text += "\n" + name;
	
	}

	[PunRPC]
	void turnTeller(int player){
		centerText.text = "Turn of Player: " + player;
	}

	[PunRPC]
	void showFinalResult (){
		
		centerText.text = "First is player: " + ranking [0]+"\n";
		centerText.text += "Second is player: " + ranking [1]+"\n";
		centerText.text += "Third is player: " + ranking [2]+"\n";

		for (int i = 0; i < 4; i++) {
			if (didWin [i] == false) {
				centerText.text += "Loser is player: " + (i + 1);
				break;
			}
		}
		PlayerPrefs.SetString ("roomName", null);
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player){
		Debug.Log (player.ID + " is disconnected.");
		isBotActive [(player.ID) - 1] = true;
		if (PhotonNetwork.isMasterClient && didWin[turn-1] == false) {
			if (turn == player.ID) {
				pv.RPC ("botDrawCard", PhotonTargets.MasterClient, turn, suitToBePlayed);
			}
			centerText.text = "turn: "+turn+"playerID: "+player.ID;
		}
		Debug.Log (player.ID + " is now bot " + isBotActive [(player.ID) - 1]);
		//centerText.text = player.ID + " is disconnected.";
	}

	[PunRPC]
	void setTurn(int turnOfPlayer){
		turn = turnOfPlayer;
		centerText.text = "turn: " + turn;
	}

}
