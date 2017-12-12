using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUps : MonoBehaviour {

    public GameObject AddFriendsPanel;
    public GameObject OnlineFriends;
    public GameObject Settings;
    public GameObject Shop;
    public GameObject coinShop;
    public GameObject giftPanel;
    public GameObject gemShop;
    public GameObject packShop;
    GameObject closingObj;
    GameObject tabs;

    public void openshop(GameObject shopToOpen)
    {
        tabs = shopToOpen;
        tabs.SetActive(true);
        closingObj = Shop;
        iTween.ScaleTo(Shop, new Vector3(1f, 1f, 1f), 0.5f);
    }

    public void AddFriend()
    {
        closingObj = AddFriendsPanel;
        iTween.ScaleTo(AddFriendsPanel, new Vector3(1f, 1f, 1f), 0.5f);
    }

    public void checkOnlineFriends()
    {
        closingObj = OnlineFriends;
        iTween.ScaleTo(OnlineFriends, new Vector3(1f, 1f, 1f), 0.5f);
    }

    public void getSettings()
    {
        closingObj = Settings;
        iTween.ScaleTo(Settings, new Vector3(1f, 1f, 1f), 0.5f);
    }

    public void GetGifts()
    {
        closingObj = giftPanel;
        iTween.ScaleTo(giftPanel, new Vector3(1f, 1f, 1f), 0.5f);
    }

    public void coinTab()
    {
        tabs.gameObject.SetActive(false);
        tabs = coinShop;
        tabs.gameObject.SetActive(true);

    }

    public void gemTab()
    {
        tabs.gameObject.SetActive(false);
        tabs = gemShop;
        tabs.gameObject.SetActive(true);

    }

    public void packsTab()
    {
        tabs.gameObject.SetActive(false);
        tabs = packShop;
        tabs.gameObject.SetActive(true);

    }
    public void cancel()
    {
        if(tabs!=null)
        tabs.SetActive(false);
        //closingObj.transform.localScale = new Vector3(0, 0, 0);
        iTween.ScaleTo(closingObj, new Vector3(0f, 0f, 0f), 0.5f);
    }
}
