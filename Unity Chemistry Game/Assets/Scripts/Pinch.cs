using UnityEngine;
using Leap.Unity.PinchUtility;
using Leap.Unity;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Pinch : MonoBehaviour
{
    private Camera cam;
    private IHandModel hands;
    private LeapPinchDetector pincher;
    private InfoText info;
    private Timer handsTimer, errorClock;
    private SelectionScript selection;
    private RectTransform rectrans, xfRect;
    private Vector3 spherePos, topPos, ballPos;
    private Vector2 mousePos;
    private RaycastHit hit;
    private float r, camZ;
    private GameObject spotted;
    private Dictionary<GameObject, GameObject> circleSpherePairs;
    private Dictionary<GameObject, float> circleRadiusPairs;
    private bool currentlyPinching = false, handsExist = true;
    private Vector2 pinchLocation2D;
    private Vector3 pinchLocation3D;

    public int noHandsErrorTime = 1500, lookForHandsTime = 5000;

    public void startScript()
    {
        xfRect = GameObject.Find("Finger XHair").GetComponent<RectTransform>();

        cam = Camera.main;
        info = GameObject.Find("Canvas2").GetComponent<InfoText>();
        selection = GetComponent<SelectionScript>();
        circleSpherePairs = selection.getCircleSpherePairs();
        circleRadiusPairs = selection.getCircleRadiusPairs();

        StartCoroutine(WaitForHands());
    }

    public void updateScript()
    {
        checkPincher();

        bool doEPick = true;

        xfRect.position = new Vector3(pinchLocation2D.x, pinchLocation2D.y, 0.0f);

        if (!currentlyPinching)
        {
            if (spotted != null)
            {
                spotted.GetComponent<BallMotion>().spotBall(false);
            }
            spotted = null;
        }

        if (!hasSpottedObj())
        {
            Ray rayPinch = cam.ScreenPointToRay(pinchLocation2D);

            if (Physics.Raycast(rayPinch, out hit, Mathf.Infinity) && currentlyPinching)
            {
                spotted = hit.collider.gameObject;

                foreach (GameObject ball in circleSpherePairs.Values)
                {
                    if (ball == spotted)
                    {
                        ///spotted.GetComponent<MeshRenderer>().material.color = Color.green;
                        spotted.GetComponent<BallMotion>().spot(true);
                    }/*
                    else
                    {
                        if (ball.GetComponent<ProtonScript>())
                        {
                            ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ProtonScript>().getBallColor();
                        }
                        if (ball.GetComponent<ElectronScript>())
                        {
                            ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ElectronScript>().getBallColor();
                        }
                        ball.GetComponent<BallMotion>().spot(false);
                    }*/
                }
                doEPick = false;
            }
            else
            {
                foreach (GameObject ball in circleSpherePairs.Values)
                {/*
                    if (ball.GetComponent<ProtonScript>())
                    {
                        ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ProtonScript>().getBallColor();
                    }
                    if (ball.GetComponent<ElectronScript>())
                    {
                        ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ElectronScript>().getBallColor();
                    }*/
                    ball.GetComponent<BallMotion>().spot(false);
                }
            }

            /////////////////////////

            if (doEPick)
            {
                GameObject found = null;
                foreach (GameObject c in circleSpherePairs.Keys)
                {
                    r = circleRadiusPairs[c];

                    rectrans = c.GetComponent<Image>().rectTransform;
                    rectrans.localScale = new Vector3(r / 50.0f, r / 50.0f, 1.0f);
                    rectrans.position = new Vector3(selection.getScreenPosition(circleSpherePairs[c]).x, selection.getScreenPosition(circleSpherePairs[c]).y, 0.0f);

                    GameObject ball = circleSpherePairs[c];

                    if (selection.magnitudeBetweenPoints(pinchLocation2D, c.transform.position) <= r && currentlyPinching)
                    {
                        spotted = ball;
                        //spotted.GetComponent<MeshRenderer>().material.color = Color.cyan;
                        spotted.GetComponent<BallMotion>().spot(true);
                    }
                    else
                    {
                        spotted = null;/*
                        if (ball.GetComponent<ProtonScript>())
                        {
                            ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ProtonScript>().getBallColor();
                        }
                        if (ball.GetComponent<ElectronScript>())
                        {
                            ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ElectronScript>().getBallColor();
                        }*/
                        ball.GetComponent<BallMotion>().spot(false);
                    }

                }
                if (found != null)
                {
                    spotted = found;
                    //spotted.GetComponent<MeshRenderer>().material.color = Color.red;
                    spotted.GetComponent<BallMotion>().spot(true);
                }
            }
        }
    }

    private IEnumerator WaitForHands()
    {
        yield return new WaitForSeconds(1.0f);

        errorClock = new Timer();
        errorClock.Elapsed += new ElapsedEventHandler(ErrorClockEvent);
        errorClock.Interval = noHandsErrorTime;

        findLeapHands();
    }

    public bool leapPincherExists()
    {
        if (pincher != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool hasSpottedObj()
    {
        return spotted != null;
    }

    void checkPincher()
    {
        if (handsExist && pincher != null)
        {
            if (pincher.IsPinching)
            {
                Debug.Log("PINCH");
                currentlyPinching = true;
                pinchLocation3D = pincher.transform.position;
                pinchLocation2D = cam.WorldToScreenPoint(pinchLocation3D);
                //Debug.Log("Pinch at 3D: " + pincher.transform.position);
                //Debug.Log("Pinch at 2D: " + cam.WorldToScreenPoint(pincher.transform.position));
            }
            else
            {
                Debug.Log("NO PINCH");
                currentlyPinching = false;
            }
        }
    }

    public void findLeapHands()
    {
        try
        {
            pincher = FindObjectOfType<IHandModel>().GetComponent<LeapPinchDetector>();
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Hands not Detected!");
            handsExist = false;
            info.showError = true;

            errorClock.Enabled = true;
        }
    }

    private void ErrorClockEvent(object source, ElapsedEventArgs e)
    {
        info.showError = false;
        errorClock.Enabled = false;
    }

    public Vector2 get2DPinchLocation()
    {
        return pinchLocation2D;
    }

    public Vector3 get3DPinchLocation()
    {
        return pinchLocation3D;
    }

    public bool isCurrentlyPinching()
    {
        return currentlyPinching;
    }

    public bool doHandsExist()
    {
        return handsExist;
    }
}