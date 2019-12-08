using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class SettingsManager : MonoBehaviour
{
    [HideInInspector] public static SettingsManager instance;
    public int Highscore;
    public int Coins;
    public string CharacterSelected;
    public List<string> CharactersPurchased;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Load();
    }


    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerinfo.dat");

        PlayerInfo data = new PlayerInfo();

        data.Highscore = Highscore;
        data.Coins = Coins;
        data.CharacterSelected = string.Copy(CharacterSelected);
        data.CharactersPurchased = new List<string>();
        data.CharactersPurchased = CharactersPurchased.ToList();

        bf.Serialize(file, data);
        file.Close();
    }


    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);

            PlayerInfo data = (PlayerInfo)bf.Deserialize(file);
            Highscore = data.Highscore;
            Coins = data.Coins;
            CharacterSelected = string.Copy(data.CharacterSelected);
            CharactersPurchased = data.CharactersPurchased.ToList();
            file.Close();
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerinfo.dat");

            PlayerInfo data = new PlayerInfo();

            data.Highscore = 0;
            data.Coins = 0;
            data.CharacterSelected = "1";
            data.CharactersPurchased = new List<string>();
            data.CharactersPurchased.Add("1");

            CharacterSelected = string.Copy(data.CharacterSelected);
            CharactersPurchased = data.CharactersPurchased.ToList();

            bf.Serialize(file, data);
            file.Close();
        }
    }
}

[System.Serializable]
class PlayerInfo
{
    public int Highscore;
    public int Coins;
    public string CharacterSelected;
    public List<string> CharactersPurchased;
}