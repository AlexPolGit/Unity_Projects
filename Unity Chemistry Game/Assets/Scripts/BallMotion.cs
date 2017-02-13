using UnityEngine;
using UnityEngine.UI;

public class BallMotion : MonoBehaviour
{
    private Vector2 easePoint;
    private Camera cam;
    private Pinch pinch;
    private float zDistance;
    private Text mouseToText;
    private bool allowMotion = false;
    private Vector2 currentPoint;
    private Vector2 offset;
    private SoundScript sounds;
    private bool isSpotted = false;

    public float motionSpeed = 10.0f;

    /// <summary>
    /// Runs at creation of this object.
    /// </summary>

    void Start()
    {
        cam = Camera.main;
        sounds = GameObject.Find("Sound Player").GetComponent<SoundScript>();
        pinch = cam.GetComponent<Pinch>();
        //mouseToText = GameObject.Find("Mouseto Text").GetComponent<Text>();
        zDistance = cam.transform.position.z + Mathf.Abs(transform.position.z);
    }

    /// <summary>
    /// Runs every frame (when called) to update this script.
    /// </summary>

    public void updateBall()
    {
        if (GetComponent<ElectronScript>() != null)
        {
            if (isSpotted && Input.GetMouseButton(0))
            {
                GetComponent<ElectronScript>().isBallHeld(true);
            }
            else if (isSpotted && !Input.GetMouseButton(0))
            {
                GetComponent<ElectronScript>().isBallHeld(false);
            }
            else if (!isSpotted)
            {
                GetComponent<ElectronScript>().isBallHeld(false);
            }
        }

        if (isSpotted && !Input.GetMouseButton(0))
        {
            allowMotion = true;
        }
        else if (!isSpotted && Input.GetMouseButton(0))
        {
            allowMotion = false;
        }
        else if (!isSpotted && !Input.GetMouseButton(0))
        {
            allowMotion = false;
        }

        if (allowMotion && Input.GetMouseButton(0))
        {
            mouseMotion();
        }

        /////////////////

        if (isSpotted && !pinch.isCurrentlyPinching())
        {
            allowMotion = true;
        }
        else if (!isSpotted && pinch.isCurrentlyPinching())
        {
            allowMotion = false;
        }
        else if (!isSpotted && !pinch.isCurrentlyPinching())
        {
            allowMotion = false;
        }

        if (allowMotion && pinch.isCurrentlyPinching())
        {
            leapMotion();
        }
    }

    /// <summary>
    /// Spots this object.
    /// </summary>
    /// <param name="b"></param>

    public void spot (bool b)
    {
        isSpotted = b;

        if (isSpotted)
        {
            Vector2 start = cam.WorldToScreenPoint(transform.position);
            currentPoint = Input.mousePosition;
            offset = start - currentPoint;
        }
    }

    /// <summary>
    /// Allows the user to move this object with the mouse.
    /// </summary>

    void mouseMotion()
    {
        Vector2 mp = Input.mousePosition;
        Vector2 delta = mp - currentPoint;
        currentPoint = new Vector2(currentPoint.x + delta.x / 10, currentPoint.y + delta.y / 10);
        Vector3 finalPoint = new Vector3(currentPoint.x + offset.x, currentPoint.y + offset.y, zDistance);
        offset.x *= 0.9f;
        offset.y *= 0.9f;

        transform.position = cam.ScreenToWorldPoint(finalPoint);
    }

    /// <summary>
    /// Allows the user to move this object with a Leap Motion sensor.
    /// </summary>
    
    void leapMotion()
    {
        Vector2 lp = pinch.get2DPinchLocation();
        Vector2 delta = lp - currentPoint;
        currentPoint = new Vector2(currentPoint.x + delta.x / 10, currentPoint.y + delta.y / 10);
        Vector3 finalPoint = new Vector3(currentPoint.x + offset.x, currentPoint.y + offset.y, zDistance);
        offset.x *= 0.9f;
        offset.y *= 0.9f;

        transform.position = cam.ScreenToWorldPoint(finalPoint);
    }

    public void spotBall(bool spot)
    {
        isSpotted = spot;
    }

    public bool isBallSpotted()
    {
        return isSpotted;
    }
}