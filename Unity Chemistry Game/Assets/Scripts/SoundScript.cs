using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour
{
    public AudioClip soundPop, soundSparkles, soundTick, soundSwipe, soundParty, soundButton1, soundButton2, soundButton3, soundDing;
    private AudioSource soundSource;

    /// <summary>
    /// Runs at launch of the application.
    /// </summary>
    
    void Start()
    {
        soundSource = GetComponent<AudioSource>();

        soundPop = Resources.Load("Sounds/pop") as AudioClip;
        soundSparkles = Resources.Load("Sounds/starshine") as AudioClip;
        soundTick = Resources.Load("Sounds/tick") as AudioClip;
        soundSwipe = Resources.Load("Sounds/swoosh") as AudioClip;
        soundParty = Resources.Load("Sounds/party") as AudioClip;
        soundButton1 = Resources.Load("Sounds/button1") as AudioClip;
        soundButton2 = Resources.Load("Sounds/button2") as AudioClip;
        soundButton3 = Resources.Load("Sounds/button3") as AudioClip;
        soundDing = Resources.Load("Sounds/metalDing") as AudioClip;
    }

    /// <summary>
    /// Plays the provided audio clip.
    /// </summary>
    /// <param name="ac"></param>

    public void playSound(AudioClip ac)
    {
        soundSource.clip = ac;

        if (!soundSource.isPlaying)
        {
            soundSource.Play();
        }
    }

    private IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(0.5f);
    }
}