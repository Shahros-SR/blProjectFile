using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardManager : Photon.MonoBehaviour {

	List<int> player1Cards=new List<int>();
	List<int> player2Cards=new List<int>();
	List<int> player3Cards=new List<int>();
	List<int> player4Cards=new List<int>();


	public Button[] inHandCards;
	public Sprite[] cardsSprites;
	public RectTransform panelRectTransform;
	public static PlayerCardManager instance;

	PhotonView pv;
	bool thola ;

	void Start()
	{
		instance = this;
		pv = GetComponent<PhotonView> ();
	}

	[PunRPC]
	void SetCards()
	{
		foreach (var c in inHandCards) {
			c.interactable = false;
		}
		foreach (var btn in inHandCards) {
			btn.gameObject.SetActive (false);
		}

		int x = 0;
		if (PhotonNetwork.player.ID == 1) {
			player1Cards.Sort ();
			panelRectTransform.sizeDelta = new Vector2 (0, 0);
			foreach (var sprite in inHandCards) {
				sprite.GetComponent<Image> ().sprite = null;
			}
			foreach (var card in player1Cards) {
				panelRectTransform.sizeDelta = new Vector2 (panelRectTransform.sizeDelta.x + 250, panelRectTransform.sizeDelta.y);
				inHandCards [x].gameObject.SetActive (true);
				inHandCards [x].name = card.ToString ();
				inHandCards [x].GetComponent<Image> ().sprite = cardsSprites [card];
				x++;

			}

			panelRectTransform.position = new Vector3 (panelRectTransform.sizeDelta.x / 2, panelRectTransform.position.y, 0);
		} 
		else if (PhotonNetwork.player.ID == 2) {
			player2Cards.Sort ();
			panelRectTransform.sizeDelta = new Vector2 (0, 0);
			foreach (var sprite in inHandCards) {
				sprite.GetComponent<Image> ().sprite = null;
			}
			foreach (var card in player2Cards) {
				panelRectTransform.sizeDelta = new Vector2 (panelRectTransform.sizeDelta.x + 250, panelRectTransform.sizeDelta.y);
				inHandCards [x].gameObject.SetActive (true);
				inHandCards [x].name = card.ToString ();
				inHandCards [x].GetComponent<Image> ().sprite = cardsSprites [card];
				x++;

			}

			panelRectTransform.position = new Vector3 (panelRectTransform.sizeDelta.x / 2, panelRectTransform.position.y, 0);
		} 
		else if (PhotonNetwork.player.ID == 3) {
			player3Cards.Sort ();
			panelRectTransform.sizeDelta = new Vector2 (0, 0);
			foreach (var sprite in inHandCards) {
				sprite.GetComponent<Image> ().sprite = null;
			}
			foreach (var card in player3Cards) {
				panelRectTransform.sizeDelta = new Vector2 (panelRectTransform.sizeDelta.x + 250, panelRectTransform.sizeDelta.y);
				inHandCards [x].gameObject.SetActive (true);
				inHandCards [x].name = card.ToString ();
				inHandCards [x].GetComponent<Image> ().sprite = cardsSprites [card];
				x++;

			}

			panelRectTransform.position = new Vector3 (panelRectTransform.sizeDelta.x / 2, panelRectTransform.position.y, 0);

		} 
		else if (PhotonNetwork.player.ID == 4) {
			player4Cards.Sort ();
			panelRectTransform.sizeDelta = new Vector2 (0, 0);
			foreach (var sprite in inHandCards) {
				sprite.GetComponent<Image> ().sprite = null;
			}
			foreach (var card in player4Cards) {
				panelRectTransform.sizeDelta = new Vector2 (panelRectTransform.sizeDelta.x + 250, panelRectTransform.sizeDelta.y);
				inHandCards [x].gameObject.SetActive (true);
				inHandCards [x].name = card.ToString ();
				inHandCards [x].GetComponent<Image> ().sprite = cardsSprites [card];
				x++;

			}

			panelRectTransform.position = new Vector3 (panelRectTransform.sizeDelta.x / 2, panelRectTransform.position.y, 0);
		}
	}

	[PunRPC]
	void RecieveCards(int card, int player){
		if (player == 1) {
			player1Cards.Add (card);
			player1Cards.Sort ();
		} else if (player == 2) {
			player2Cards.Add (card);
			player2Cards.Sort ();
		} else if (player == 3) {
			player3Cards.Add (card);
			player3Cards.Sort ();
		} else if (player == 4) {
			player4Cards.Add (card);
			player4Cards.Sort ();
		}
	}

	[PunRPC]
	void GiveTurn(string suitToBePlayed)
	{
		thola = true;
		int numberOfCardsToActive = 0;

		Debug.Log ("suit to be played: " + suitToBePlayed);
		if (PhotonNetwork.player.ID == 1) {
			if (suitToBePlayed == null) {
				foreach (var btn in inHandCards) {

					if (numberOfCardsToActive < player1Cards.Count) {
						Debug.Log ("Enabling1: " + btn.name);
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
							Debug.Log ("Enabling2: " + btn.name);
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
						Debug.Log ("Enabling3: " + btn.name);
						btn.interactable = true;
					} else
						break;
				}

			}
		}

		if (PhotonNetwork.player.ID == 2) {
			if (suitToBePlayed == null) {
				foreach (var btn in inHandCards) {

					if (numberOfCardsToActive < player2Cards.Count) {
						Debug.Log ("Enabling1: " + btn.name);
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
					if (numberOfCardsToActive < player2Cards.Count) {
						if (CardsScript.instance.GetCardSuits (int.Parse (btn.name)) == suitToBePlayed) {
							Debug.Log ("Enabling2: " + btn.name);
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
					if (numberOfCardsToActive < player2Cards.Count) {
						Debug.Log ("Enabling3: " + btn.name);
						btn.interactable = true;
					} else
						break;
				}

			}
			
		}

		if (PhotonNetwork.player.ID == 3) {
			if (suitToBePlayed == null) {
				foreach (var btn in inHandCards) {

					if (numberOfCardsToActive < player3Cards.Count) {
						Debug.Log ("Enabling1: " + btn.name);
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
					if (numberOfCardsToActive < player3Cards.Count) {
						if (CardsScript.instance.GetCardSuits (int.Parse (btn.name)) == suitToBePlayed) {
							Debug.Log ("Enabling2: " + btn.name);
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
					if (numberOfCardsToActive < player3Cards.Count) {
						Debug.Log ("Enabling3: " + btn.name);
						btn.interactable = true;
					} else
						break;
				}

			}
		}

		if (PhotonNetwork.player.ID == 4) {
			if (suitToBePlayed == null) {
				foreach (var btn in inHandCards) {

					if (numberOfCardsToActive < player4Cards.Count) {
						Debug.Log ("Enabling1: " + btn.name);
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
					if (numberOfCardsToActive < player4Cards.Count) {
						if (CardsScript.instance.GetCardSuits (int.Parse (btn.name)) == suitToBePlayed) {
							Debug.Log ("Enabling2: " + btn.name);
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
					if (numberOfCardsToActive < player4Cards.Count) {
						Debug.Log ("Enabling3: " + btn.name);
						btn.interactable = true;
					} else
						break;
				}

			}
		}
	}

	[PunRPC]
	void RemoveCard(int player,int card)
	{
		Debug.Log ("I am player: " + PhotonNetwork.player.ID + " and removing  card: " + card);
		if (player == 1) {
			player1Cards.Remove (card);
			player1Cards.Sort ();
		} else if (player == 2) {
			player2Cards.Remove (card);
			player2Cards.Sort ();
		} else if (player == 3) {
			player3Cards.Remove (card);
			player3Cards.Sort ();
		} else if (player == 4) {
			player4Cards.Remove (card);
			player4Cards.Sort ();
		}
	
		MultiplayerManager.instance.SetPositionOfCardOnTable (card,player);
	}

	[PunRPC]
	void botDrawCard(int turn, string suitToBePlayed){
		thola = true;
		int cardToBeDrawnByBot = 0;

		if (turn == 1) {
			foreach (var n in player2Cards)
				Debug.Log ("p1 card: " + n);
			if (suitToBePlayed == null) {
				cardToBeDrawnByBot = player1Cards [0];
				thola = false;
			} else {
				foreach (var card in player1Cards) {
					if (suitToBePlayed == CardsScript.instance.GetCardSuits (card)) {
						cardToBeDrawnByBot = card;
						thola = false;
						break;
					}
				}
			}
			if (thola == true) {
				cardToBeDrawnByBot = player1Cards [Random.Range (0, player2Cards.Count)];
			}

			player1Cards.Remove (cardToBeDrawnByBot);
			player1Cards.Sort ();

		} else if (turn == 2) {
			foreach (var n in player2Cards)
				Debug.Log ("p2 card: " + n);
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
				Debug.Log ("p3 card: " + n);
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
				Debug.Log ("p4 card: " + n);
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
		pv.RPC ("getCardFromBot",PhotonTargets.MasterClient,cardToBeDrawnByBot,turn,thola);

	}

	public void GiveCardToMaster(int card){
		pv.RPC ("GetCardFromClient",PhotonTargets.MasterClient,card,thola,PhotonNetwork.player);

	}
		
	public bool CheckForWinners(int player){
		if (player+1 == 1 ) {
			if (player1Cards.Count == 0) {
				Debug.Log ("player 1 wins.");
				return true;
			}
		}
		else if (player+1 == 2 ) {
			if (player2Cards.Count == 0) {
				Debug.Log ("player 2 wins.");
				return true;
			}
	
		}
		else if (player+1 == 3 ) {
			if (player3Cards.Count == 0) {
				Debug.Log ("player 3 wins.");
				return true;
			}
		
		}
		else if (player+1 == 4 ) {
			if (player4Cards.Count == 0) {
				Debug.Log ("player 4 wins.");
				return true;
			}
		}

		return false;
	}
}
