using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    private SelectionScript selection;
    private RectTransform rectrans, xRect;
    private Vector3 spherePos, topPos, ballPos;
    private Vector2 mousePos;
    private Camera cam;
    private RaycastHit hit;
    private float r, camZ;
    private GameObject spotted;
    private Dictionary<GameObject, GameObject> circleSpherePairs;
    private Dictionary<GameObject, float> circleRadiusPairs;

    public void startScript()
    {
        cam = Camera.main;
        xRect = GameObject.Find("XHair").GetComponent<RectTransform>();

        selection = GetComponent<SelectionScript>();
        circleSpherePairs = selection.getCircleSpherePairs();
        circleRadiusPairs = selection.getCircleRadiusPairs();
    }

    public void updateScript()
    {
        mousePos = Input.mousePosition;
        bool doEPick = true;

        xRect.position = new Vector3(mousePos.x, mousePos.y, 0.0f);

        if (!Input.GetMouseButton(0))
        {
            if (spotted != null)
            {
                spotted.GetComponent<BallMotion>().spotBall(false);
            }
            spotted = null;
        }

        if (!hasSpottedObj())
        {
            Ray ray = cam.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                spotted = hit.collider.gameObject;

                foreach (GameObject ball in circleSpherePairs.Values)
                {
                    if (ball == spotted)
                    {
                        //spotted.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        spotted.GetComponent<BallMotion>().spot(true);
                    }
                    else
                    {
                        /*
                        if (ball.GetComponent<ProtonScript>())
                        {
                            ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ProtonScript>().getBallColor();
                        }
                        if (ball.GetComponent<ElectronScript>())
                        {
                            ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ElectronScript>().getBallColor();
                        }
                        */
                        ball.GetComponent<BallMotion>().spot(false);
                    }
                }
                doEPick = false;
            }
            else
            {
                foreach (GameObject ball in circleSpherePairs.Values)
                {
                    /*
                    if (ball.GetComponent<ProtonScript>())
                    {
                        ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ProtonScript>().getBallColor();
                    }
                    if (ball.GetComponent<ElectronScript>())
                    {
                        ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ElectronScript>().getBallColor();
                    }
                    */
                    ball.GetComponent<BallMotion>().spot(false);
                }
            }

            /////////////////////////

            if (doEPick)
            {
                float closestDist = -1;
                GameObject found = null;
                foreach (GameObject c in circleSpherePairs.Keys)
                {
                    r = circleRadiusPairs[c];

                    rectrans = c.GetComponent<Image>().rectTransform;
                    rectrans.localScale = new Vector3(r / 50.0f, r / 50.0f, 1.0f);
                    rectrans.position = new Vector3(selection.getScreenPosition(circleSpherePairs[c]).x, selection.getScreenPosition(circleSpherePairs[c]).y, 0.0f);

                    GameObject ball = circleSpherePairs[c];

                    if (selection.magnitudeBetweenPoints(Input.mousePosition, c.transform.position) <= r)
                    {
                        if (found == null || selection.magnitudeBetweenPoints(Input.mousePosition, c.transform.position) < closestDist)
                        {
                            found = ball;
                            closestDist = selection.magnitudeBetweenPoints(Input.mousePosition, c.transform.position);
                        }
                    }
                    else
                    {
                        spotted = null;
                        /*
                        if (ball.GetComponent<ProtonScript>())
                        {
                            ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ProtonScript>().getBallColor();
                        }
                        if (ball.GetComponent<ElectronScript>())
                        {
                            ball.GetComponent<MeshRenderer>().material.color = ball.GetComponent<ElectronScript>().getBallColor();
                        }
                        */
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

    bool hasSpottedObj()
    {
        return spotted != null;
    }
}