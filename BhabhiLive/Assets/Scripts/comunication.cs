using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class comunication : MonoBehaviour {

	public static comunication InfoControl;
	public int coinAmount;
	public int gemAmount;
    public int bet;

	void Awake () 
	{
		// retain the aspect ratio of devices
		//		Camera.main.aspect = 1920 / 1080;
		//Screen.sleepTimeout = SleepTimeout.NeverSleep;
		//		Screen.SetResolution (1920, 1080, true);
		// refernceing i.e if there is no GameInformation Controller then make this the ONE.
		// if we don't do this DontDestroyOnLoad create one gameobject every time the scene switches.
		if(InfoControl == null)
		{
			DontDestroyOnLoad(gameObject);
			InfoControl = this;
			Load ();
		}
		else
			if(InfoControl != this)
			{
				Destroy(gameObject);
				Debug.Log("Destroyed other Infocontrol");
			}
	}

    private void Start()
    {
        comunication.InfoControl.Load();
    }

    public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/BhabhiLive.bl");
		Debug.Log (Application.persistentDataPath);
		// Save data here from local to orignal data path storage i.e /BhabhiLive.bl
		GameData data = new GameData();
		data.coinAmount_SR = coinAmount;
		data.gemAmount_SR = gemAmount;

		// this take the "data" class data and write it to our "BhabhiLive.bl" file
		bf.Serialize(file , data);
		file.Close();
		Debug.Log (coinAmount + " " + gemAmount);
	}

	public void Load()
	{
		// First we have to check that the file BhabhiLive.bl is exixting or not 
		if(File.Exists (Application.persistentDataPath + "/BhabhiLive.bl"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/BhabhiLive.bl", FileMode.Open);
			GameData data = (GameData)bf.Deserialize(file);
			file.Close();

			coinAmount = data.coinAmount_SR;
			gemAmount = data.gemAmount_SR;
		}

		// if not the give an exaption
		else
		{
            comunication.InfoControl.coinAmount = 1000;
            comunication.InfoControl.gemAmount = 100;
            comunication.InfoControl.Save();
            comunication.InfoControl.Load();
        }
    }
}

// Class GameData can save data to file located in data 
[Serializable]
public class GameData
{
	public int coinAmount_SR;
	public int gemAmount_SR;

}
	