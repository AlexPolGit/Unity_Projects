using UnityEngine;

public class ElectronScript : MonoBehaviour
{
    public GameObject parentProton = null;
    public GameObject spotted = null;
    private Vector3 centerOfRotation, distanceOfRotation;
    private Vector3[] covalentBoundAtoms;
    private Color ballColor;
    private Camera cam;
    private SoundScript sound;
    private SceneManager scene;
    private bool allowOrbit = true, isHeld = false;
    public bool isCovalentBound = false;
    private float R, theta = -1.0f;
    private Vector3 protonPos;
    private int totalElectrons;
    private byte electronNumber;
    private RaycastHit hit;
    private float haloBrightness;
    private ParticleSystem particles;
    private Texture neutral, happy;

    /// <summary>
    /// Runs at launch of the application.
    /// </summary>

    void Start()
    {
        cam = Camera.main;
        sound = GameObject.Find("Sound Player").GetComponent<SoundScript>();
        particles = GetComponent<ParticleSystem>();
        happy = Resources.Load("Textures/happyFace") as Texture2D;
        neutral = Resources.Load("Textures/neutralFace") as Texture2D;
        GetComponent<MeshRenderer>().material.mainTexture = neutral;
        ballColor = Color.yellow;
        scene = cam.GetComponent<SceneManager>();
        GetComponent<Light>().range = 0.0f;
        ballColor = Color.yellow;
    }

    /// <summary>
    /// Runs every frame of the game.
    /// </summary>

    void Update()
    {
        if (haloBrightness >= (Mathf.PI * 2))
        {
            haloBrightness = 0;
        }
        else
        {
            haloBrightness += (Mathf.PI * 2) / 100.0f;
        }

        GetComponent<Light>().range += Mathf.Sin(haloBrightness) * 2f;

        if (parentProton == null || isHeld)
        {
            allowOrbit = false;
        }
        if (!isCovalentBound)
        {
            centerOfRotation = parentProton.transform.position;
            R = parentProton.transform.localScale.x;
            allowOrbit = true;
        }
        else if (isCovalentBound)
        {
            distanceOfRotation = covalentBoundAtoms[1] - covalentBoundAtoms[0];
            centerOfRotation = distanceOfRotation;
            allowOrbit = true;
        }

        transform.Rotate(0.0f, -5.0f, 0.0f);

        if (isHeld)
        {
            lookForNewParent();
        }
    }

    /*
    
        if (cam.GetComponent<SceneManager>().getProtonsOnScene().ContainsKey(spotted) && spotted != parentProton)
                    {
                        ProtonScript p = cam.GetComponent<SceneManager>().getProtonsOnScene()[spotted];
                        p.addElectron(gameObject);
                        p.checkIfHappy();
                        pps.removeElectron(gameObject);
                        pps.resetOrbits();
                        pps.checkIfHappy();
                        setParentProton(spotted);
                        p.resetOrbits();
                        sound.playSound(sound.soundPop);
                        scene.checkIfLevelIsCompleted();
                        particles.Play();

                        if (p.isAtomHappy())
                        {
                            sound.playSound(sound.soundSparkles);
                        }
                    }

    */

    /// <summary>
    /// Finds a new parent atom ("proton") for this electron, if possible.
    /// </summary>

    private void lookForNewParent()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.gameObject != gameObject)
        {
            spotted = hit.collider.gameObject;

            ProtonScript pps = parentProton.GetComponent<ProtonScript>();
            ProtonScript sps = new ProtonScript();

            if (spotted.GetComponent<ProtonScript>() != null)
            {
                sps = spotted.GetComponent<ProtonScript>();
            }

            if (cam.GetComponent<SceneManager>().getProtonsOnScene().ContainsKey(spotted) && spotted != parentProton)
            {
                sps.addElectron(gameObject);
                sps.checkIfHappy();
                pps.removeElectron(gameObject);
                pps.resetOrbits();
                pps.checkIfHappy();
                setParentProton(spotted);
                sps.resetOrbits();
                sound.playSound(sound.soundPop);
                scene.checkIfLevelIsCompleted();
                particles.Play();

                if (sps.isAtomHappy())
                {
                    sound.playSound(sound.soundSparkles);
                }
            }
        }

        //Debug.DrawRay(cam.ScreenToWorldPoint(transform.position), cam.ScreenToWorldPoint(Input.mousePosition), Color.red, 0.5f);
        /*
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.gameObject != gameObject)
        {
            spotted = hit.collider.gameObject;

            ProtonScript pps = parentProton.GetComponent<ProtonScript>();
            ProtonScript sps = new ProtonScript();

            if (spotted.GetComponent<ProtonScript>() != null)
            {
                sps = spotted.GetComponent<ProtonScript>();
            }
            else if (spotted.GetComponent<ElectronScript>() != null)
            {
                sps = spotted.GetComponent<ElectronScript>().getParentProton().GetComponent<ProtonScript>();

                if (pps.elementType == ProtonScript.ElementTypes.NONMETAL && sps.elementType == ProtonScript.ElementTypes.NONMETAL)
                {
                    if (cam.GetComponent<SceneManager>().getProtonsOnScene().ContainsKey(spotted) && spotted != parentProton)
                    {
                        isCovalentBound = true;
                        sps.addElectron(gameObject);
                        sps.checkIfHappy();
                        sound.playSound(sound.soundPop);
                        scene.checkIfLevelIsCompleted();
                        particles.Play();

                        if (sps.isAtomHappy())
                        {
                            sound.playSound(sound.soundSparkles);
                        }
                    }
                }
            }
        }
        */
    }

    public void resetTheta()
    {
        theta = electronNumber * ((Mathf.PI * 2) / totalElectrons);
    }

    public void setCenterOfRotation(Vector3 vec)
    {
        centerOfRotation = vec;
    }

    public Vector3 getCenterOfRotation()
    {
        return centerOfRotation;
    }

    /// <summary>
    /// Makes the electron orbit around a point.
    /// </summary>

    public void doOrbit()
    {
        if (theta == -1.0f)
        {
            // Debug.Log(name + ", setting theta with eNum: " + electronNumber + ", eTot: " + totalElectrons);
            theta = electronNumber * ((Mathf.PI * 2) / totalElectrons);
        }
        if (allowOrbit)
        {
            /*
            Debug.Log(name + ", theta: " + theta);
            Debug.Log(name + ", x: " + R * Mathf.Cos(theta));
            Debug.Log(name + ", z: " + R * Mathf.Sin(theta));
            */

            transform.position = centerOfRotation + (new Vector3(R * Mathf.Cos(theta), 0, R * Mathf.Sin(theta)));

            if (theta >= (Mathf.PI * 2))
            {
                theta = 0;
            }
            else
            {
                theta += (Mathf.PI / 200.0f);
            }
        }
    }

    /// <summary>
    /// Sets the parent proton of this electron to the given proton.
    /// </summary>
    /// <param name="proton"></param>

    public void setParentProton(GameObject proton)
    {
        parentProton = proton;
        if (proton != null)
        {
            allowOrbit = true;
        }
        else
        {
            allowOrbit = false;
        }
    }

    public void setElectronNumber(byte num)
    {
        electronNumber = num;
    }

    public void setR(float r)
    {
        R = r;
    }

    public void setTotalElectrons(int tot)
    {
        totalElectrons = tot;
    }

    public void setProtonPos(Vector3 pos)
    {
        protonPos = pos;
    }

    /// <summary>
    /// The magnitude of the vector between point A and B
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>

    float magnitudeBetweenPoints(Vector2 p1, Vector2 p2)
    {
        Vector2 distance = new Vector2(Mathf.Abs(p1.x - p2.x), Mathf.Abs(p1.y - p2.y));
        return distance.magnitude;
    }

    public byte getElectronNumber()
    {
        return electronNumber;
    }

    public GameObject getParentProton()
    {
        return parentProton;
    }

    public void isBallHeld(bool held)
    {
        isHeld = held;
    }

    public Color getBallColor()
    {
        return ballColor;
    }

    public Texture getHappyTexture()
    {
        return happy;
    }
    public Texture getNeutralTexture()
    {
        return neutral;
    }

    public void setCovalentBoundAtoms(Vector3 from, Vector3 to)
    {
        covalentBoundAtoms[0] = from;
        covalentBoundAtoms[1] = to;
    }
}