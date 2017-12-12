using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsScript : MonoBehaviour 
{
	string[] suits = { "spades", "diamonds", "clubs", "hearts" };
	int[] ranks = {14,2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
	int[] sequencedCardsRanks=new int[52];
	string[] sequencedCardsSuits=new string[52];

	public static CardsScript instance;

	void Awake()
	{
		if (instance == null)
			instance = this;

		int k = 0;
		for (int i = 0; i < 4; i++) 
		{

			for (int j = 0; j < 13; j++)
			{
				sequencedCardsRanks [k] = ranks [j];
				sequencedCardsSuits [k] = suits [i];
				k++;
			}
		}

	}
		

	public int GetCardRank(int index)
	{
		return (sequencedCardsRanks [index]);
	}


	public string GetCardSuits(int index)
	{
		return (sequencedCardsSuits [index]);
	}


}
