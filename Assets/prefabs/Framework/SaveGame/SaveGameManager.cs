using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameManager
{
    static string SaveDir = Application.persistentDataPath + "/Xenowerk.json";
    static SaveGameData savedData;
   static public void SaveGame()
    {
        CreditManager creditManager = GameObject.FindObjectOfType<CreditManager>();
        Player player = GameObject.FindObjectOfType<Player>();
        if(player == null)
        {
            return;
        }


        SaveGameData data = new SaveGameData();
        data.LevelIndex = SceneManager.GetActiveScene().buildIndex;

        data.SaveTime = System.DateTime.Now.ToString();
        data.PlayerData = player.GenerateSaveData();

        data.PlayerCredits = creditManager.GetComponent<CreditManager>().GetCredits();

        string playerData = JsonUtility.ToJson(data, true);
        File.WriteAllText(SaveDir, playerData);
    }

    static public void LoadGame()
    {
       
        
        string saveData =  File.ReadAllText(SaveDir);
        savedData = JsonUtility.FromJson<SaveGameData>(saveData);
        SceneManager.LoadScene(savedData.LevelIndex);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        ;

    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {

        CreditManager creditManager = GameObject.FindObjectOfType<CreditManager>();
        //apply credits
        int playerCurrentCredits = creditManager.GetComponent<CreditManager>().GetCredits();
        creditManager.GetComponent<CreditManager>().ChangeCredits(savedData.PlayerCredits - playerCurrentCredits);

        Player player = GameObject.FindObjectOfType<Player>();
        if (player == null)
        {
            return;
        }
        player.UpdateFromSaveData(savedData.PlayerData);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

[Serializable]

public struct SaveGameData
{
    public SaveGameData(int levelIndex, PlayerSaveGameData playerSaveData, int playerCredits, string saveTime)
    {
        LevelIndex = levelIndex;
        PlayerData = playerSaveData;
        PlayerCredits = playerCredits;
        SaveTime = saveTime;
    }
    public int LevelIndex;
    public PlayerSaveGameData PlayerData;
    public int PlayerCredits;
    public string SaveTime;
}
