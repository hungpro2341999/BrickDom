using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public Data_Game data = new Data_Game();
    JsonData PlayerJson;
    private void Start()
    {
        data.key = "HUNG";
        //PlayerJson = JsonMapper.ToJson(data);
        //File.WriteAllText(Application.dataPath + "/Player.json", PlayerJson.ToString());
        //File.ReadAllText(Application.dataPath + "/Player.json");

        string json = File.ReadAllText(Application.dataPath + "/Player.json");
        Data_Game game =  JsonUtility.FromJson<Data_Game>(json);

        Debug.Log(game.key);
    }

}

public class Data_Game
{

    public string key;
   
}