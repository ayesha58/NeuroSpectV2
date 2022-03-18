using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RecallInstructions : MonoBehaviour
{

    public Text text;

    private bool start = false;

    public void onCorrectClick()
    {
        text.text = "Great! Get Ready to Start!";
        PlayAudio();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            SceneManager.LoadScene((int)Constants.SCENES.RECALL);//
        }
    }

    void PlayAudio()
    {
        AudioSource[] audios = GameObject.Find("AudioObject")?.GetComponents<AudioSource>();
        audios[0]?.Stop();
        audios[1]?.Play();
    }
}
