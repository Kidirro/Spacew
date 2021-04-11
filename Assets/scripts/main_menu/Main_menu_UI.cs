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
    public Image ButtonLeft;
    public Image ButtonRight;

    private bool Game_Started = false;

    public void Awake()
    {
        ButtonLeft.transform.position =  (new Vector2(2 + ButtonLeft.rectTransform.sizeDelta.x/2*ButtonLeft.rectTransform.localScale.x ,ButtonLeft.transform.position.y));
        ButtonRight.transform.position =  (new Vector2(Camera.main.pixelWidth - (2 + ButtonRight.rectTransform.sizeDelta.x/2*ButtonLeft.rectTransform.localScale.x) ,ButtonRight.transform.position.y));

        GameManager.first_play =PlayerPrefs.GetInt("First_play")==0;
        if (GameManager.first_play)
        {
            PlayerPrefs.SetInt("Bloom", 1);
            PlayerPrefs.SetInt("ChromaticAberration", 1);
            PlayerPrefs.SetInt("MusicOn", 1);
            PlayerPrefs.SetInt("SoundOn", 1);
            PlayerPrefs.SetInt("PlayerShip0", 1);
        }
        else if (PlayerPrefs.GetInt("PlayerShip1") != 1)
        {
            ClearShip();
        }
        PlayerPrefs.SetInt("First_play",-1);
        GameManager.Unlocked_ship = new bool[Ships.Length];
        ReadShips();
        GameManager.number_ship = Ships.Length;
        PlayerPrefs.Save();
        ship.GetComponent<Image>().color = (GameManager.Unlocked_ship[GameManager.chosen_ship]) ? ship.GetComponent<Image>().color = new Color32(255, 255, 255, 255) : ship.GetComponent<Image>().color = new Color32(0, 0, 0, 100);

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

 
    private void FixedUpdate()
    {
        if (GameManager.record != PlayerPrefs.GetInt("Records") || hiscore.text == "New Text")
        {
            GameManager.record = PlayerPrefs.GetInt("Records");
            hiscore.text = "Record:" + GameManager.record;
            PlayerPrefs.SetInt("Records", GameManager.record);
        }
        if (ship.sprite != Ships[GameManager.chosen_ship])
        {
            ship.sprite = Ships[GameManager.chosen_ship];
            ChangeImageSize();

            ship.GetComponent<Image>().color = (GameManager.Unlocked_ship[GameManager.chosen_ship]) ? new Color32(255, 255, 255, 255) : new Color32(0, 0, 0, 100);
        }
        if (PlayerPrefs.GetInt("New_Ship") != 0)
        {
            Debug.LogError("Writen");
            WriteShips();
            PlayerPrefs.SetInt("New_Ship", 0);
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            for (int i = 0; i < GameManager.Unlocked_ship.Length; i++)
            {
                GameManager.Unlocked_ship[i] = true;
            }
            Debug.Log("Gived");
            WriteShips();
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ClearShip();
        }
    }

    private void ChangeImageSize()
    {
        Sprite Original_texture = Ships[GameManager.chosen_ship];
        float sprite_ratio = Original_texture.rect.width / Original_texture.rect.height;
        ship.rectTransform.sizeDelta = new Vector2(280*sprite_ratio, 280);
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

    void ClearShip()
    {
        if (GameManager.Unlocked_ship != null)
        {
            for (int i = 1; i < GameManager.Unlocked_ship.Length; i++)
            {
                GameManager.Unlocked_ship[i] = false;
                PlayerPrefs.SetInt("Ship" + i + "_prog", 0);
                PlayerPrefs.SetInt("PlayerShip" + i, 0);
            }
            WriteShips();
        }
        Debug.Log("Cleared!");
    }

    IEnumerator StartGame_IE()
    {
        if (!Game_Started)
        {
            Game_Started = true;
            Animator clips = GetComponent<Animator>();
            clips.keepAnimatorControllerStateOnDisable = true;
          

            clips.SetBool("StartAnim", true);
            yield return new WaitForSeconds((float)(clips.runtimeAnimatorController.animationClips[0].length) / clips.GetFloat("Multiplier"));
           
            shipSpawner.start_game();
            clips.SetBool("StartAnim", false);

            GameManager.gameOver = false;
            Game_Started = false;
        }
        yield break;

    }

    public void ReadShips() { 
        for (int i = 0; i < Ships.Length; i++)
        {
            GameManager.Unlocked_ship[i] = PlayerPrefs.GetInt("PlayerShip" + i)==1;
        }
        PlayerPrefs.Save();

    }

    public void WriteShips()
    {
        for (int i = 0; i < Ships.Length; i++)
        {
            PlayerPrefs.SetInt("PlayerShip" + i, ((PlayerPrefs.GetInt("PlayerShip" + i) == 1) || GameManager.Unlocked_ship[i]) ? 1 : 0);
         }
        PlayerPrefs.Save();
    }
}


