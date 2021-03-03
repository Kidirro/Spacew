using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Main_menu_UI : MonoBehaviour
{
    [Tooltip("Спрайты кораблей")]
    public Sprite[] Ships;
    public Text hiscore;
    public Image ship;
    [Tooltip("Ссылка на уведомление")]
    public ShipSpawner shipSpawner;
    public GameObject Desctr;
    public GameObject Guide;
    public GameObject Settings;

    private bool Game_Started = false;

    public void Awake()
    {
        if (PlayerPrefs.GetInt("Unlocked_ships") == 0 || (PlayerPrefs.GetInt("Unlocked_ships") < (int)Mathf.Pow(10, Ships.Length - 1)))
        {
            PlayerPrefs.SetInt("Unlocked_ships", (int)Mathf.Pow(10, Ships.Length - 1) + PlayerPrefs.GetInt("Unlocked_ships"));
        }
        Debug.Log(PlayerPrefs.GetInt("Unlocked_ships"));
        GameManager.first_play =PlayerPrefs.GetInt("First_play")==0;
        if (GameManager.first_play)
        {
            PlayerPrefs.SetInt("Bloom", 1);
            PlayerPrefs.SetInt("ChromaticAberration", 1);
            PlayerPrefs.SetInt("MusicOn", 1);
            PlayerPrefs.SetInt("SoundOn", 1);
        }
        PlayerPrefs.SetInt("First_play",-1);
        GameManager.Unlocked_ship = new bool[Ships.Length];
        GameManager.number_ship = Ships.Length;
        PlayerPrefs.Save();
    }

    public void Startgame()
    {
        if (GameManager.Unlocked_ship[GameManager.chosen_ship])
        {
            StartCoroutine(StartGame_IE());
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Check_unlock()
    {
        int n = PlayerPrefs.GetInt("Unlocked_ships");
        for (int i = Ships.Length - 1; i >= 0; i--)
        {
            GameManager.Unlocked_ship[i] = (n % 10) == 1;
            n = n / 10;
        }
    }

    private void Update()
    { 
        Check_unlock();
        hiscore.text = "Record:" + PlayerPrefs.GetInt("Records");
        GameManager.record = PlayerPrefs.GetInt("Records");
        ship.sprite = Ships[GameManager.chosen_ship];
        if (GameManager.Unlocked_ship[GameManager.chosen_ship])
        {
            ship.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            ship.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
        }
    }

    public void LoadGuide()
    {
        Guide.SetActive(true);
    }

    public void LoadInfo() 
    {
        Desctr.SetActive(true);
    }

    public void LoadSettings()
    {
        Settings.SetActive(true);
    }

    public void SwitchShip()
    {
        if (GameManager.chosen_ship == GameManager.number_ship-1)
        {
            GameManager.chosen_ship = 0;
        }
        else GameManager.chosen_ship++;
    }

    IEnumerator StartGame_IE()
    {
        if (!Game_Started)
        {
            Game_Started = true;
            Animator clips = GetComponent<Animator>();
            clips.Play("Open_Gate", -1, 0);

            yield return new WaitForSeconds(clips.runtimeAnimatorController.animationClips[0].length / clips.GetFloat("Multiplier"));
            shipSpawner.start_game();

            GameManager.gameOver = false;
            Game_Started = false;
        }
        yield break;

    }

}


