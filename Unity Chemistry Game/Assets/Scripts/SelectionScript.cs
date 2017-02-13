using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectionScript : MonoBehaviour
{
    private GameObject circle;
    private Image circleTexture;
    private RectTransform rectrans, xRect, xfRect;
    private Vector3 spherePos, topPos, ballPos;
    private Vector2 mousePos;
    private Camera cam;
    private Canvas canv;
    private Ray ray;
    private RaycastHit hit;
    private float r, camZ;
    private PauseScript pause;
    private Dictionary<GameObject, GameObject> circleSpherePairs;
    private Dictionary<GameObject, float> circleRadiusPairs;
    private Pinch pinch;
    private Mouse mouse;
    private bool allowPicking = true;
    private GameObject spotted;

    public bool useMouse;
    public bool useLeap;
    public float selectionError;

    /// <summary>
    /// Runs once (when called) to initialize this script.
    /// </summary>

    public void startScript()
    {
        cam = Camera.main;
        canv = GameObject.Find("Canvas1").GetComponent<Canvas>();
        xRect = GameObject.Find("XHair").GetComponent<RectTransform>();
        xfRect = GameObject.Find("Finger XHair").GetComponent<RectTransform>();
        pause = GameObject.Find("Pause UI").GetComponent<PauseScript>();
        circleSpherePairs = new Dictionary<GameObject, GameObject>();
        circleRadiusPairs = new Dictionary<GameObject, float>();

        pinch = GetComponent<Pinch>();
        mouse = GetComponent<Mouse>();

        List<GameObject> balls = new List<GameObject>();

        foreach (GameObject p in GameObject.FindGameObjectsWithTag("proton"))
        {
            balls.Add(p);
        }

        foreach (GameObject e in GameObject.FindGameObjectsWithTag("electron"))
        {
            balls.Add(e);
        }

        foreach (GameObject ball in balls)
        {
            circle = Instantiate(GameObject.FindGameObjectWithTag("baseCircle"));
            circle.transform.parent = canv.transform;
            circle.name = "Picking Circle of " + ball.name;
            circleTexture = circle.GetComponent<Image>();

            spherePos = getScreenPosition(ball);
            ballPos = ball.transform.position;
            topPos = cam.WorldToScreenPoint(new Vector3(ballPos.x, ballPos.y + 50.0f, ballPos.z));
            r = Mathf.Abs(topPos.y - spherePos.y) + selectionError;

            rectrans = circleTexture.rectTransform;
            rectrans.localScale = new Vector3(r / 50.0f, r / 50.0f, 1.0f);
            rectrans.position = new Vector3(spherePos.x, spherePos.y, 0.0f);

            /*
            Debug.Log("FOUND: " + ball.name + ", MIDDLE: " + spherePos + ", TOP: " + topPos +
                " | WITH: " + circle.name + " (" + rectrans.localPosition.x + ", " +
                rectrans.localPosition.y + ", " + rectrans.localPosition.z + "). R: " + r);
            */

            circleSpherePairs.Add(circle, ball);
            circleRadiusPairs.Add(circle, r);
        }

        Destroy(GameObject.FindGameObjectWithTag("baseCircle"));

        mouse.startScript();
        pinch.startScript();
    }

    /// <summary>
    /// Runs every frame (when called) to update this script.
    /// </summary>

    public void updateScript()
    {
        if (!pause.isGamePaused())
        {
            move();
            rotate();

            if (useMouse)
            {
                mouse.updateScript();
            }
            if (useLeap)
            {
                pinch.updateScript();
            }

            if (!useMouse && !useLeap)
            {
                Debug.Log("NO OBJECT SELECTION DEVICE ENABLED.");
            }

            foreach (GameObject c in circleSpherePairs.Keys)
            {
                circleSpherePairs[c].GetComponent<BallMotion>().updateBall();
            }
            updateCircles();
        }
    }

    /// <summary>
    /// Allows the camera to move up/down/left/right with WASD keys
    /// </summary>

    void move()
    {
        float f = Time.deltaTime * 150.0f;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -f, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, f, 0);
        }
    }

    /// <summary>
    /// Allows the camera to rotate the up/down/left/right arrow keys.
    /// </summary>

    void rotate()
    {
        float f = Time.deltaTime * 150.0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -f, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, f, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(f, 0, 0);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(-f, 0, 0);
        }
    }

    /// <summary>
    /// Updates the locations and sizes of the picking circles ("hitboxes").
    /// </summary>

    public void updateCircles()
    {
        //Debug.Log("CIRCLE UPDATE");
        foreach (GameObject c in circleSpherePairs.Keys)
        {  
            GameObject ball = circleSpherePairs[c];
            spherePos = getScreenPosition(ball);
            ballPos = ball.transform.position;
            topPos = cam.WorldToScreenPoint(new Vector3(ballPos.x, ballPos.y + 50.0f, ballPos.z));
            r = Mathf.Abs(topPos.y - spherePos.y) + selectionError;
            rectrans = circleTexture.rectTransform;
            rectrans.localScale = new Vector3(r / (circleSpherePairs[c].transform.localScale.x), r / (circleSpherePairs[c].transform.localScale.x), 1);
            rectrans.position = new Vector3(spherePos.x, spherePos.y, 0.0f);

            //Debug.Log(c.name + ", World Pos of Ball: " + ballPos + ", Scrren Pos of Ball: " + spherePos + ", Top World Pos of Ball: " + topPos + ", r =  " + r + ", Local Scale: " + sphereCirclePairs[c].transform.localScale.x);
        }
    }

    /// <summary>
    /// The magnitude of the vector between point A and B
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>

    public float magnitudeBetweenPoints(Vector2 p1, Vector2 p2)
    {
        Vector2 distance = new Vector2(Mathf.Abs(p1.x - p2.x), Mathf.Abs(p1.y - p2.y));
        return distance.magnitude;
    }

    /// <summary>
    /// The screen position of a given object.
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>

    public Vector3 getScreenPosition(GameObject g)
    {
        return cam.WorldToScreenPoint(g.transform.position);
    }

    bool hasSpottedObj()
    {
        return spotted != null;
    }

    public ArrayList returnListOfBalls()
    {
        ArrayList temp = new ArrayList();

        foreach (GameObject b in circleSpherePairs.Values)
        {
            temp.Add(b);
        }

        return temp;
    }

    public GameObject getSpottedGameObject()
    {
        return spotted;
    }

    public bool isPichingAllowed()
    {
        return allowPicking;
    }

    public Dictionary<GameObject, GameObject> getCircleSpherePairs()
    {
        return circleSpherePairs;
    }

    public Dictionary<GameObject, float> getCircleRadiusPairs()
    {
        return circleRadiusPairs;
    }

    public void allowMouse(bool b)
    {
        useMouse = b;
    }

    public void allowLeap(bool b)
    {
        useLeap = b;
    }

    public bool canUseMouse()
    {
        return useMouse;
    }

    public bool canUseLeap()
    {
        return useLeap;
    }
}