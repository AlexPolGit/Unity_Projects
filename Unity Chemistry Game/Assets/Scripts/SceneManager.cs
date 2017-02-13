using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SceneManager : MonoBehaviour
{
    private Dictionary<GameObject, ProtonScript> protonsOnScene;
    private bool isLevelCompleted = false;
    private int numberOfLevels = 0;
    private int currentLevelNumber = 0;
    private SoundScript sound;
    private FileStream levelFile;
    private Dictionary<GameObject, ElectronScript> electronsOnScene;
    private GameObject baseProton, baseElectron;
    private SelectionScript selection;
    private PauseScript pause;
    private InfoText uiText;
    private string[] levelCodes;
    public static int lvlNumber = 0;

    private enum Elements
    {
        NOELEMENT,
        H, He,
        Li, Be, B, C, N, O, F, Ne, Na, Mg, Al, Si, P, S, Cl, Ar,
        K, Ca, Sc, Ti, V, Cr, Mn, Fe, Co, Ni, Cu, Zn, Ga, Ge, As, Se, Br, Kr,
        Rb, Sr, Y, Zr, Nb, Mo, Tc, Ru, Rh, Pd, Ag, Cd, In, Sn, Sb, Te, I, Xe,
        Cs, Ba, La, Ce, Pr, Nd, Pm, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu, Hf, Ta, W, Re, Os, Ir, Pt, Au, Hg, Tl, Pb, Bi, Po, At, Rn,
        Fr, Ra, Ac, Th, Pa, U, Np, Pu, Am, Cm, Bk, Cf, Es, Fm, Md, No, Lr, Rf, Db, Sg, Bh, Hs, Mt, Ds, Rg, Cn, Nh, Fl, Mc, Lv, Ts, Og
    };

    /// <summary>
    /// Runs at launch of the application.
    /// </summary>

    void Start()
    {
        levelFile = new FileStream("levels.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
        string code = "";

        for (int i = 0; i < levelFile.Length; i++)
        {
            code += (char)levelFile.ReadByte();
        }
        Debug.Log("Level code found: " + code);

        levelCodes = code.Split(';');
        numberOfLevels = levelCodes.Length;

        for (int i = 0; i < levelCodes.Length; i++)
        {
            levelCodes[i] = levelCodes[i].Replace("{", "");
            levelCodes[i] = levelCodes[i].Replace("}", "");
            //Debug.Log(levelCodes[i]);
        }

        startLevel(lvlNumber);
    }

    /// <summary>
    /// Runs every frame of the game.
    /// </summary>

    void Update()
    {
        foreach (GameObject p in protonsOnScene.Keys)
        {
            protonsOnScene[p].updateProton();
        }

        selection.updateScript();
        uiText.updateScript();
        pause.updateScript();
    }

    /// <summary>
    /// Parses the level code string and creates new instances of the base proton/electron.
    /// </summary>

    private void parseLevelCode()
    {
        currentLevelNumber = lvlNumber;
        Debug.Log("Starting Level " + (lvlNumber + 1) + ": " + levelCodes[lvlNumber]);

        string[] atomString = levelCodes[lvlNumber].Split('|');
        int n = 1;

        foreach (string s in atomString)
        {
            int element = int.Parse(s.Split(',')[1]);
            int eCount = int.Parse(s.Split(',')[3]);

            //Debug.Log("element: " + element);
            //Debug.Log("electrons: " + eCount);
            //Debug.Log(getElementNameByNumber(element));

            GameObject proton = Instantiate(baseProton);
            proton.name = "P" + n + " of " + getElementNameByNumber(element);
            proton.tag = "proton";
            proton.GetComponent<ProtonScript>().setUpProton(element);
            proton.transform.position = new Vector3(Random.Range(-500, 500), Random.Range(-200, 200), 200);

            protonsOnScene.Add(proton, proton.GetComponent<ProtonScript>());

            for (int i = 0; i < eCount; i++)
            {
                GameObject electron = Instantiate(baseElectron);
                electron.name = "E" + (i + 1) + " of [" + proton.name + "]";
                electron.tag = "electron";
                electron.GetComponent<ElectronScript>().setParentProton(proton);
                electron.GetComponent<ElectronScript>().setCenterOfRotation(proton.transform.position);

                proton.GetComponent<ProtonScript>().addElectron(electron);
                electronsOnScene.Add(electron, electron.GetComponent<ElectronScript>());
            }

            n++;
        }
        //Debug.Log(JsonUtility.ToJson(this));
    }

    public int getElementNumberByName(string name)
    {
        return (int)System.Enum.Parse(typeof(Elements), name);
    }

    public string getElementNameByNumber(int number)
    {
        return ((Elements)number).ToString();
    }

    /// <summary>
    /// Starts the level, initializes various components and objects.
    /// </summary>
    /// <param name="levelNumber"></param>

    void startLevel(int levelNumber)
    {
        selection = Camera.main.GetComponent<SelectionScript>();
        uiText = GameObject.Find("Canvas2").GetComponent<InfoText>();
        sound = GameObject.Find("Sound Player").GetComponent<SoundScript>();
        pause = GameObject.Find("Pause UI").GetComponent<PauseScript>();

        if(lvlNumber != 0)
        {
            sound.playSound(sound.soundParty);
        }

        baseProton = GameObject.FindGameObjectWithTag("baseProton");
        baseElectron = GameObject.FindGameObjectWithTag("baseElectron");

        protonsOnScene = new Dictionary<GameObject, ProtonScript>();
        electronsOnScene = new Dictionary<GameObject, ElectronScript>();
        parseLevelCode();

        Destroy(baseProton);
        Destroy(baseElectron);

        selection.startScript();
        uiText.startScript();
        pause.startScript();
    }

    /// <summary>
    /// Checks if the current level is completed (all atoms are happy).
    /// </summary>

    public void checkIfLevelIsCompleted()
    {
        foreach (ProtonScript p in protonsOnScene.Values)
        {
            if (p.isAtomHappy())
            {
                isLevelCompleted = true;
                if (lvlNumber != (levelCodes.Length - 1))
                {
                    foreach(GameObject e in electronsOnScene.Keys)
                    {
                        e.GetComponent<MeshRenderer>().material.mainTexture = electronsOnScene[e].getHappyTexture();
                    }
                    lvlNumber++;
                    StartCoroutine(startNextLevel(1.0f));
                }
            }
            else
            {
                isLevelCompleted = false;
                break;
            }
        }
    }

    /// <summary>
    /// Starts the next level after the given amount of seconds.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>

    private IEnumerator startNextLevel(float delay)
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(Application.loadedLevel);
    }

    /// <summary>
    /// Starts the given level after the given amount of seconds.
    /// </summary>
    /// <param name="lNum"></param>
    /// <param name="delay"></param>
    /// <returns></returns>

    private IEnumerator startLevelNumbered(int lNum, float delay)
    {
        lvlNumber = lNum;
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(Application.loadedLevel);
    }

    public Dictionary<GameObject, ProtonScript> getProtonsOnScene()
    {
        return protonsOnScene;
    }

    public int getNumberOfLevels()
    {
        return numberOfLevels;
    }

    public int getCurrentLevelNumber()
    {
        return currentLevelNumber;
    }

    public bool isLevelComplete()
    {
        return isLevelCompleted;
    }
}
