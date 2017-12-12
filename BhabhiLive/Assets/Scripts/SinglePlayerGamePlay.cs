using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerGamePlay : MonoBehaviour {
	int[] shuffledCards = new int[52];
	int[] sortedCards = new int[52];
	int[] cardsOnTable=new int[4];
	int[] playerSequence = new int[4];
	int[] ranking = new int[4];
	bool[] didWin=new bool[4];

	List<int> player1Cards=new List<int>();
	List<int> player2Cards=new List<int>();
	List<int> player3Cards=new List<int>();
	List<int> player4Cards=new List<int>();

	int turn;
	int cardsDistributed;
	int drawnCards;
	int winners;
	int playersPlayingGame;
	string suitToBePlayed=null;
	bool isCardToBeDrawn=true;
	bool thola;
	bool firstTurn;
	float timer;

	public static SinglePlayerGamePlay instance;
	public RectTransform panelRectTransform;
	public Button[] inHandCards;
	public Sprite[] cardsSprites;
	public Image[] totalCards;
	public Transform[] playerPositions;
	public Transform[] tablePositions;
	public Text[] totalPlayerCards;
	Transform[] cardsStartingPositions = new Transform[4];
	public Text centerText;
    public Text playerName, coinTxt, gemTxt;
	// Use this for initialization
	void Awake(){
		instance = this;
        playerName.text = PlayerPrefs.GetString("playerName");
        coinTxt.text = comunication.InfoControl.coinAmount.ToString();
        gemTxt.text = comunication.InfoControl.gemAmount.ToString();
    }

	void Start () {
		timer = 0;
		playersPlayingGame = 4;
		turn = 1;
		cardsDistributed = 0;
		drawnCards = 0;
		firstTurn = true;
		ShuffleCards ();

		for(int i=1;i<5;i++)
			DistributeCards (i);

		Player1SetCards ();
		player2Cards.Sort ();
		player3Cards.Sort ();
		player4Cards.Sort ();
	
		for (int i = 0; i < 4; i++) {
			didWin [i] = false;
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		
		if (isCardToBeDrawn == true) {
			centerText.text="Turn of: "+turn;
			if (timer > 3f ) {
				timer = 0;
				isCardToBeDrawn = false;
                if (drawnCards == 0)
                    setTotalcards();
             

                if (turn == 1) {
					if (didWin [turn - 1] == false) {
						Player1GetDrawableCards ();
					} else {
						turn++;
						isCardToBeDrawn = true;
					}
				} else {
					if (didWin [turn - 1] == false) {
						botDrawCard ();
					} else {
						turn++;
						isCardToBeDrawn = true;
					}
					if (turn > 4) {
						turn = 1;
						isCardToBeDrawn = true;
					}

				}
			} else {
				timer += Time.deltaTime;
			}
		}
		
	}

	void pileCards()
	{
		for (int i = 0; i < 52; i++) 
		{
			sortedCards [i] = i;
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


	void DistributeCards(int player)
	{
		for (int i = cardsDistributed; i <= cardsDistributed + 12; i++) {
			if (player == 1) {
				player1Cards.Add (shuffledCards[i]);
			} else if (player == 2) {
				player2Cards.Add (shuffledCards [i]);
			} else if(player == 3) {
				player3Cards.Add (shuffledCards [i]);
			} else if(player == 4) {
				player4Cards.Add (shuffledCards [i]);
			}
			if (shuffledCards[i] == 0) {
				turn = player;
			///	Debug.Log ("turn of: " + turn);
			}
		}

		cardsDistributed += 13;
	}

	void Player1SetCards()
	{
		int x = 0;
		player1Cards.Sort ();
		panelRectTransform.sizeDelta = new Vector3 (0,0,0);
		foreach (var sprite in inHandCards) {
			sprite.GetComponent<Image> ().sprite = null;
		}
			foreach (var card in player1Cards) {
				panelRectTransform.sizeDelta = new Vector2 (panelRectTransform.sizeDelta.x  + 250, panelRectTransform.sizeDelta.y);
				inHandCards [x].gameObject.SetActive (true);
				inHandCards [x].name = card.ToString ();
				inHandCards [x].GetComponent<Image> ().sprite = cardsSprites [card];
				x++;
			}
			panelRectTransform.position =new Vector3( panelRectTransform.sizeDelta.x/2,panelRectTransform.position.y,0);
	}

	void Player1RemoveCard(int card)
	{
		foreach (var btn in inHandCards) {
			btn.interactable = false;
		}
		foreach (var btn in inHandCards) {
			btn.gameObject.SetActive (false);
		}
		player1Cards.Remove (card);
		Player1SetCards ();
	
	}

	void Player1GetDrawableCards(){
		thola = true;
		int numberOfCardsToActive=0;
		Debug.Log ("suit to be played: " + suitToBePlayed);
		foreach (var n in player1Cards)
			//Debug.Log ("p1 card: " + n);
		if (suitToBePlayed == null) {
			foreach (var btn in inHandCards) {
				
				if (numberOfCardsToActive < player1Cards.Count) {
					//Debug.Log ("Enabling1: " + btn.name);
					if (btn.name == "0") {
						btn.interactable = true;
						thola = false;
						break;
					} else {
						btn.interactable = true;
						thola = false;
					}
				} else
					break;
				numberOfCardsToActive++;
			}
			numberOfCardsToActive = 0;
		} else if (suitToBePlayed != null) {
			foreach (var btn in inHandCards) {
				if (numberOfCardsToActive < player1Cards.Count) {
					if (CardsScript.instance.GetCardSuits (int.Parse (btn.name)) == suitToBePlayed) {
						//Debug.Log ("Enabling2: " + btn.name);
						btn.interactable = true;
						thola = false;
					}
					numberOfCardsToActive++;
				} else
					break;
			}
			numberOfCardsToActive = 0;
		} 
		if (thola == true) {
			foreach (var btn in inHandCards) {
				if (numberOfCardsToActive < player1Cards.Count) {
					//Debug.Log ("Enabling3: " + btn.name);
					btn.interactable = true;
				}
				else
					break;
			}

		}
	}

	public void drawCardOnTable(int card){

        if (suitToBePlayed == null) {
			suitToBePlayed = CardsScript.instance.GetCardSuits (card);
		}
		cardsOnTable [drawnCards] = card;
		playerSequence [drawnCards] = turn;
        SetPositionOfCard(card);
        drawnCards++;
		turn++;


		if (thola == true && firstTurn == false) {
            Debug.Log("first");
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
			drawnCards = 0;
			suitToBePlayed = null;

			//foreach (var c in totalCards) {
				//c.transform.localPosition= new Vector3 (0, 0, 0);
				//c.transform.localRotation = new Quaternion (0, 0, 0, 0);
			//}
		}
		else if (drawnCards > playersPlayingGame-1) {
            Debug.Log("second");
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
           // foreach (var c in totalCards)
            //{
              //  c.transform.localPosition = new Vector3(0, 0, 0);
                //c.transform.localRotation = new Quaternion(0, 0, 0, 0);
            //}


        }
		if (turn > 4)
			turn = 1;
		checkForWinners ();


		if (winners == 3) {
			showFinalResult ();
		} else {
            //Debug.Log ("next is: " + turn);
			isCardToBeDrawn = true;
		}
	}

	void botDrawCard(){
		thola = true;
		int cardToBeDrawnByBot = 0;
		if (turn == 2) {
			foreach (var n in player2Cards)
				//Debug.Log ("p2 card: " + n);
			if (suitToBePlayed == null) {
				cardToBeDrawnByBot = player2Cards [0];
				thola = false;
			} else {
				foreach (var card in player2Cards) {
					if (suitToBePlayed == CardsScript.instance.GetCardSuits (card)) {
						cardToBeDrawnByBot = card;
						thola = false;
						break;
					}
				}
			}
			if (thola == true) {
				cardToBeDrawnByBot = player2Cards [Random.Range (0, player2Cards.Count)];
			}

			player2Cards.Remove (cardToBeDrawnByBot);
			player2Cards.Sort ();

		} else if (turn == 3) {
			foreach (var n in player3Cards)
				//Debug.Log ("p3 card: " + n);
			if (suitToBePlayed == null) {
				cardToBeDrawnByBot = player3Cards [0];
				thola = false;
			} else {
				foreach (var card in player3Cards) {
					if (suitToBePlayed == CardsScript.instance.GetCardSuits (card)) {
						cardToBeDrawnByBot = card;
						thola = false;
						break;
					}
				}
			}
			if (thola == true) {
				cardToBeDrawnByBot = player3Cards [0];
			}

			player3Cards.Remove (cardToBeDrawnByBot);
			player3Cards.Sort ();

		}else if(turn==4){
			foreach (var n in player4Cards)
				//Debug.Log ("p4 card: " + n);
			if (suitToBePlayed == null) {
				cardToBeDrawnByBot = player4Cards [0];
				thola = false;
			} else {
				foreach (var card in player4Cards) {
					if (suitToBePlayed == CardsScript.instance.GetCardSuits (card)) {
						cardToBeDrawnByBot = card;
						thola = false;
						break;
					}
				}
			}
			if (thola == true) {
				cardToBeDrawnByBot = player4Cards [Random.Range (0, player4Cards.Count)];
			}

			player4Cards.Remove (cardToBeDrawnByBot);
			player4Cards.Sort ();

		}

		drawCardOnTable (cardToBeDrawnByBot);
	}

	void giveThola(int player)
	{
		centerText.text="Giving Thola to : "+player;
		if (player == 1) {
			for (int i = 0; i < drawnCards; i++) {
				Debug.Log ("giving thola to: " + player+" card: "+cardsOnTable[i]);
				player1Cards.Add (cardsOnTable [i]);
			}
			Player1SetCards ();

		} else if (player == 2) {
			for (int i = 0; i < drawnCards; i++) {
				Debug.Log ("giving thola to: " + player+" card: "+cardsOnTable[i]);
				player2Cards.Add (cardsOnTable [i]);
			}
			player2Cards.Sort ();

		} else if (player == 3) {
			for (int i = 0; i < drawnCards; i++) {
				Debug.Log ("giving thola to: " + player+" card: "+cardsOnTable[i]);
				player3Cards.Add (cardsOnTable [i]);
			}
			player3Cards.Sort ();
		} else if (player == 4) {
			for (int i = 0; i < drawnCards; i++) {
				Debug.Log ("giving thola to: " + player+" card: "+cardsOnTable[i]);
				player4Cards.Add (cardsOnTable [i]);
			}
			player4Cards.Sort ();
		}
	}

	void SetPositionOfCard(int card)
	{
        Debug.Log ("player " + turn + " drawing " + card);
        float pos = Random.Range(-60,60);
        //iTween.RotateTo(totalCards[card].gameObject, new Vector3(0f, 0f, pos), 1f);
  
        if (turn == 1) {
			Debug.Log ("Setting on position 1");
			totalCards [card].transform.position = tablePositions [0].position;
            iTween.MoveTo(totalCards[card].gameObject, playerPositions[0].position, 1f);
            Player1RemoveCard (card);
            

        } else if (turn == 2) {
			Debug.Log ("Setting on position 2");
			totalCards [card].transform.Rotate (new Vector3 (0, 0, 90));
			totalCards [card].transform.position = tablePositions [1].position;
            iTween.MoveTo(totalCards[card].gameObject, playerPositions[1].position, 1f);
            player2Cards.Remove(card);
            player2Cards.Sort();

        } else if (turn == 3) {
			Debug.Log ("Setting on position 3");
			totalCards [card].transform.Rotate (new Vector3 (0, 0, 180));
			totalCards [card].transform.position = tablePositions [2].position;
            iTween.MoveTo(totalCards[card].gameObject, playerPositions[2].position, 1f);
            player3Cards.Remove(card);
            player3Cards.Sort();

        } else if (turn == 4) {
			Debug.Log ("Setting on position 4");
			totalCards [card].transform.Rotate (new Vector3 (0, 0, -90));
			totalCards [card].transform.position = tablePositions [3].position;
            iTween.MoveTo(totalCards[card].gameObject, playerPositions[3].position, 1f);
            player4Cards.Remove(card);
            player4Cards.Sort();
        }

	}

    void setTotalcards()
    {
        foreach (var c in totalCards)
        {
            Debug.Log("cleaning...");
            c.transform.localPosition = new Vector3(0, 0, 0);
            c.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }
	void checkForWinners(){
		if (player1Cards.Count == 0 && didWin [0] == false) {
			didWin [0] = true;
			ranking [winners] = 1;
			winners++;
			//totalPlayerCards[0].text="Player 1 won!";
			playersPlayingGame--;
			Debug.Log ("Total Winner: " + winners);
		}

		if (player2Cards.Count == 0 && didWin [1] == false) {
			didWin [1] = true;
			ranking [winners] = 2;
			winners++;
			//totalPlayerCards[1].text="Player 2 won!";
			playersPlayingGame--;
			Debug.Log ("Total Winner: " + winners);
		}

		if (player3Cards.Count == 0 && didWin [2] == false) {
			didWin [2] = true;
			ranking [winners] = 3;
			winners++;
			//totalPlayerCards[2].text="Player 3 won!";
			playersPlayingGame--;
			Debug.Log ("Total Winner: " + winners);
		}

		if (player4Cards.Count == 0 && didWin [3] == false) {
			didWin [3] = true;
			ranking [winners] = 4;
			winners++;
			//totalPlayerCards[3].text="Player 4 won!";
			playersPlayingGame--;
			Debug.Log ("Total Winner: " + winners);
		}

	}

	void showFinalResult(){
       
        centerText.text="Results: \n";
		centerText.text += "First is player " + ranking [0]+"\n";
		centerText.text += "Second is player " + ranking [1]+"\n";
		centerText.text += "Third is player " + ranking [2]+"\n";
        foreach (var c in totalCards)
        {
            c.transform.localPosition = new Vector3(0, 0, 0);
            c.transform.localRotation = new Quaternion(0, 0, 0, 0);
        }

    }


}
