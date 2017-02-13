using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    private bool gamePaused = false;
    private Camera cam;
    private Button pauseButton;
    private Dropdown levelSelection;
    private Image pauseBG;
    private RaycastHit hit;
    private SoundScript sounds;

    /// <summary>
    /// Runs once (when called) to initialize this script.
    /// </summary>
    
    public void startScript()
    {
        cam = Camera.main;
        //pauseButton = GameObject.Find("Pause Button").GetComponent<Button>();
        levelSelection = GameObject.Find("Level Selector").GetComponent<Dropdown>();
        pauseBG = GameObject.Find("Pause BG").GetComponent<Image>();
        pauseBG.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
        sounds = GameObject.Find("Sound Player").GetComponent<SoundScript>();

        System.Collections.Generic.List<Dropdown.OptionData> lvls = new System.Collections.Generic.List<Dropdown.OptionData>();

        for (int i = 1; i < Camera.main.GetComponent<SceneManager>().getNumberOfLevels() + 1; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = "Level " + i;
            option.image = null;

            lvls.Add(option);
        }

        levelSelection.AddOptions(lvls);
    }

    /// <summary>
    /// Runs every frame (when called) to update this script.
    /// </summary>

    public void updateScript()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused)
        {
            cam.GetComponent<SelectionScript>().enabled = false;
            GameObject.Find("Canvas1").GetComponent<Canvas>().enabled = false;
            GameObject.Find("Canvas2").GetComponent<Canvas>().enabled = false;
            sounds.playSound(sounds.soundTick);
            //Debug.Log("PAUSED");
            gamePaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && gamePaused)
        {
            cam.GetComponent<SelectionScript>().enabled = true;
            GameObject.Find("Canvas1").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Canvas2").GetComponent<Canvas>().enabled = true;
            sounds.playSound(sounds.soundTick);
            //Debug.Log("UNPAUSED");
            gamePaused = false;
        }

        if (gamePaused)
        {
            pauseBG.color = new Color(0.0f, 0.0f, 0.0f, 0.8f);
            levelSelection.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            levelSelection.enabled = true;
        }
        else
        {
            pauseBG.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            levelSelection.GetComponent<RectTransform>().localPosition = new Vector2(0, 1000);
            levelSelection.enabled = false;
        }
    }

    public bool isGamePaused()
    {
        return gamePaused;
    }

    public void pauseGame(bool pause)
    {
        gamePaused = pause;
    }
}