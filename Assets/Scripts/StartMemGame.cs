using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMemGame : MonoBehaviour
{
    public List<GameObject> gameObjs;
    private bool delayComplete = false;
    private float timePassed = 0;
    private int processNum = 0; // current scale animation count

    private List<GameObject> objsWithoutCurr = new List<GameObject>();

    private Vector3 pos;

    // scale animation values
    private const float scaleAnimDuration = 1.0f; //0.01f; // for testing
    private Vector3 scaleAnimFactor = new Vector3(1f, 1f, 1f);

    private const int totalImagesToEncode = 30;

    // total number of scale up and scale down animations to perform
    private const int totalScaleAnimCount = totalImagesToEncode * 2;


    void Start()
    {
        pos = gameObjs[0].transform.position;
        StartCoroutine(Delay());
    }

    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        delayComplete = true;

        objsWithoutCurr = gameObjs;
    }

    public void Update()
    {

        if (delayComplete && processNum < totalScaleAnimCount)
        {
            int objNum = (int)processNum / 2;

            // scale up
            if (processNum % 2 == 0)
            {
                gameObjs[objNum].transform.position -= pos * Time.deltaTime;
                gameObjs[objNum].transform.position += new Vector3(0, 0, -9f) * Time.deltaTime;
                gameObjs[objNum].transform.localScale += scaleAnimFactor * Time.deltaTime;

                timePassed += Time.deltaTime;
                if (timePassed >= scaleAnimDuration)
                {
                    processNum++;
                    timePassed = 0;
                    delayComplete = false;
                }
            }

            // scale down
            else if (processNum % 2 == 1)
            {
                gameObjs[objNum].transform.position += pos * Time.deltaTime;
                gameObjs[objNum].transform.position -= new Vector3(0, 0, -9f) * Time.deltaTime;
                gameObjs[objNum].transform.localScale -= scaleAnimFactor * Time.deltaTime;

                timePassed += Time.deltaTime;

                if (timePassed >= scaleAnimDuration)
                {
                    processNum++;
                    delayComplete = false;
                    timePassed = 0;
                }
            }
        }
        else if (!delayComplete && processNum >= totalScaleAnimCount)
        {
            MoveToNextScreen();
        }
        else if (!delayComplete && processNum < totalScaleAnimCount)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= scaleAnimDuration)
            {
                if (processNum % 2 == 0)
                {
                    pos = gameObjs[processNum / 2].transform.position;
                }

                timePassed = 0;
                delayComplete = true;
            }
        }

        // skip option
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveToNextScreen();
        }
    }

    private void MoveToNextScreen()
    {
        SceneManager.LoadScene(5);
    }
}
