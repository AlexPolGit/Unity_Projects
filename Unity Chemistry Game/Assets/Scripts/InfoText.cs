using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    private Text pinchText, holdingText, mouseText, fpsText, errorText;
    private SelectionScript selection;
    private Pinch pinch;
    private float deltaTime, fps;
    private GameObject baseLabel;
    private Dictionary<GameObject, GameObject> ballLabelPairs;

    public bool showError = true;

    /// <summary>
    /// Runs once (when called) to initialize this script.
    /// </summary>

	public void startScript()
    {
        pinchText = GameObject.Find("Pinch Text").GetComponent<Text>();
        holdingText = GameObject.Find("Holding Text").GetComponent<Text>();
        mouseText = GameObject.Find("Mouse Text").GetComponent<Text>();
        fpsText = GameObject.Find("FPS Text").GetComponent<Text>();
        errorText = GameObject.Find("Error Text").GetComponent<Text>();
        baseLabel = GameObject.Find("Label");

        ballLabelPairs = new Dictionary<GameObject, GameObject>();

        selection = Camera.main.GetComponent<SelectionScript>();
        pinch = Camera.main.GetComponent<Pinch>();

        foreach (GameObject ball in selection.returnListOfBalls())
        {
            GameObject label = Instantiate(baseLabel);
            label.GetComponent<TextMesh>().text = ball.name;
            label.name = "Label of " + ball.name;
            ballLabelPairs.Add(ball, label);
        }

        Destroy(baseLabel);
    }

    /// <summary>
    /// Runs every frame (when called) to update this script.
    /// </summary>

    public void updateScript()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        fpsText.text = (fps.ToString());

        holdingText.text = "Holding: ";
        mouseText.text = "Mouse: " + Input.mousePosition + ", ";

        foreach (GameObject b in ballLabelPairs.Keys)
        {
            ballLabelPairs[b].transform.position = b.transform.position + (Vector3.right * 25) + (Vector3.up * 25);
        }

        if (pinch.doHandsExist())
        {
            pinchText.text = "Pinching: ";
            if(pinch.leapPincherExists())
            {
                if (pinch.isCurrentlyPinching())
                {
                    pinchText.text += "Yes";
                }
                else
                {
                    pinchText.text += "No";
                }
            }
            else
            {
                pinchText.text += "N/A";
            }
            
        }

        if (selection.getSpottedGameObject() != null)
        {
            holdingText.text += selection.getSpottedGameObject().name + ", " + selection.getSpottedGameObject().transform.position;
        }
        else
        {
            holdingText.text += "Nothing";
        }

        if (Input.GetMouseButton(0))
        {
            mouseText.text += "Down";
        }
        else
        {
            mouseText.text += "Up";
        }

        if (showError)
        {
            errorText.text = "Leap Motion Hands NOT Detected!";
        }
        else
        {
            errorText.text = "";
        }
    }
}