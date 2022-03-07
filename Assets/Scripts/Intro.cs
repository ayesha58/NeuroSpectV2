using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{

    private bool changeScene = false;

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(7f);//5
        changeScene = true;
    }

    void Start()
    {
        StartCoroutine(Delay());
    }

    void Update()
    {
        if(changeScene)
        {
            SceneManager.LoadScene((int)Constants.SCENES.DEMOGRAPHICS);//2
        }

        // skip option
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            changeScene = true;
        }
    }
}
