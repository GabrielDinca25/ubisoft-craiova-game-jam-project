using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [HideInInspector] public static MenuManager instance;

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
    }

    public GameObject mainMenu;
    public GameObject skinsMenu;

    public void GoToSkinMenu()
    {
        PrepareMenu();
        GetComponent<AudioSource>().Play();
        mainMenu.GetComponent<Animator>().SetTrigger("slide");
        skinsMenu.GetComponent<Animator>().SetTrigger("slide");
    }

    public void GoToMainMenu()
    {
        GetComponent<AudioSource>().Play();
        mainMenu.GetComponent<Animator>().SetTrigger("slide");
        skinsMenu.GetComponent<Animator>().SetTrigger("slide");
    }

    public void StartGame()
    {
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Main");
    }

    public void LoadMenu()
    {
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Menu");
    }


    public TMP_Text currentGoldText;
    public GameObject selectedImage;
    public GameObject[] characters;

    public void PrepareMenu()
    {
        foreach(GameObject character in characters)
        {
            if(SettingsManager.instance.CharactersPurchased.Contains(character.name))
            {
                character.GetComponentInChildren<TMP_Text>().text = "";
            }
            if (SettingsManager.instance.CharacterSelected.Equals(character.name))
            {
                selectedImage.GetComponent<RectTransform>().position = character.GetComponent<RectTransform>().position;
                selectedImage.transform.SetParent(character.GetComponent<Button>().transform);
            }
        }
        currentGoldText.text = SettingsManager.instance.Coins + "";
    }


    public void SelectCharacter(string name)
    {
        GameObject item = GameObject.Find(EventSystem.current.currentSelectedGameObject.name);

        if (item.transform.GetChild(0).GetComponent<TMP_Text>().text.Equals("") && selectedImage.GetComponent<RectTransform>().position == item.transform.position)
        {
            Debug.Log("aici");
            return;
        }
        else if (item.transform.GetChild(0).GetComponent<TMP_Text>().text.Equals(""))
        {
            SettingsManager.instance.CharacterSelected = string.Copy(item.name);
            PrepareMenu();
            SettingsManager.instance.Save();
            return;
        }

        string price = item.transform.GetChild(0).GetComponent<TMP_Text>().text;
        int itemPrice = int.Parse(price);

        if (SettingsManager.instance.Coins >= itemPrice)
        {
            SettingsManager.instance.Coins -= itemPrice;
            SettingsManager.instance.CharactersPurchased.Add(item.name);
            SettingsManager.instance.CharacterSelected = string.Copy(item.name);
            SettingsManager.instance.Save();
            PrepareMenu();
        }
    }
}
