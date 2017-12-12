using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonsFunctions : MonoBehaviour {
	public GameObject playerInfoPanel;
	public Text infoText;
    public Button btn;
    public Text coinTxt;
    public Text gemTxt;
    public Sprite active, inactive;


    string playerName;
    bool check=false;
	public void pressCard()
	{
		PlayerCardManager.instance.GiveCardToMaster(int.Parse(this.gameObject.name));
        this.gameObject.GetComponent<Button>().interactable = false;
	}

	public void DisplayPlayerInfo(int playerNo)
	{
		playerInfoPanel.SetActive (true);
		infoText.text = "Hi there, I am player " + playerNo.ToString() + " and this is my information.";
	}

	public void CancelInfo(){

		playerInfoPanel.SetActive (false);

	}

	public void PressCardSinglePlayer(){
		SinglePlayerGamePlay.instance.drawCardOnTable (int.Parse(this.gameObject.name));
      //  this.gameObject.GetComponent<ButtonsFunctions>().enabled = false;
	}

	public void setPlayerName(Text pname ){
		PlayerPrefs.SetString("playerName",pname.text);

	}

	public void enableMenu(GameObject mMenu)
	{
		mMenu.SetActive (true);
	}
	public void disableInfo(GameObject infoMenu)
	{
		infoMenu.SetActive (false);
	}

	public void onTextOfInputFieldChange(string changeTxt)
	{
		 
		if (String.IsNullOrEmpty(changeTxt)) {
			btn.interactable = false;
		} else {
			btn.interactable = true;
		}
	}

    public void AddCoins(int coins)
    {
        comunication.InfoControl.coinAmount += coins;
        comunication.InfoControl.Save();
        coinTxt.text = comunication.InfoControl.coinAmount.ToString();
        Debug.Log("Coins Added.");
    }

    public void AddGems(int gems)
    {
        comunication.InfoControl.gemAmount += gems;
        comunication.InfoControl.Save();
        gemTxt.text = comunication.InfoControl.gemAmount.ToString();
        Debug.Log("Gems Added.");
    }

    public void WatchVideoAdd()
    {
        comunication.InfoControl.coinAmount += 500;
        comunication.InfoControl.Save();
        coinTxt.text = comunication.InfoControl.coinAmount.ToString();
        Debug.Log("Video add.");
    }

    public void setSound(GameObject toggleBtn)
    {
        if (check == false)
        {
            toggleBtn.GetComponent<Image>().sprite = inactive;
            check = true;
        }
        else
        {
            toggleBtn.GetComponent<Image>().sprite = active;
            check = false;
        }
        //this.gameObject.SetActive(false);
    }
}
