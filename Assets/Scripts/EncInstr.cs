using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EncInstr : MonoBehaviour
{
    public List<Texture2D> chosenList;

    private GameObject currObj;
    private float timePassed = 0;
    private int objNum = 0;

    private Sprite mySprite;
    private SpriteRenderer[] sr;
    public List<GameObject> gameObjects;

    private const float width = 92.0f;
    private const float height = 34.0f;

    private const float centerX = -9.5f;
    private const float centerY = -8.5f;


    // Start is called before the first frame update
    void Start()
    {       
        for (int i = 0; i < gameObjects.Count; i++)
        {
            SpriteRenderer spriteRenderer = gameObjects[i].AddComponent<SpriteRenderer>();

            Texture2D texture = chosenList[i];

            spriteRenderer.sprite = Sprite.Create(
                texture,
                new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.0f, 0.0f));

        }

        PositionImages();
        StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(AnimateImages());
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(4);
    }

    void PositionImages()
    {
        // Screen bounds in units: (-8, -5), (8, 5)
        Vector2 screenBoundsTopRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenBoundsBottomLeft  = Camera.main.ScreenToWorldPoint(Vector2.zero);

        // Game Object width height in units
        float imageWidth = gameObjects[0].GetComponent<SpriteRenderer>().bounds.size.x; // 1.27
        float imageHeight = gameObjects[0].GetComponent<SpriteRenderer>().bounds.size.y; // 1.27

        // Sprite Texture dimensions
        // float spriteWidth = gameObjects[0].GetComponent<SpriteRenderer>().sprite.rect.width; // 512
        // float spriteHeight = gameObjects[0].GetComponent<SpriteRenderer>().sprite.rect.height; // 512

        //
        float screenWidthNormalized = screenBoundsTopRight.x * 2;
        float spacing = screenWidthNormalized / gameObjects.Count;
        float offsetX = spacing / 2 - imageWidth / 2;

        for (int i = 0; i < gameObjects.Count; i++)
        {
            GameObject img = gameObjects[i];
            float posX = screenBoundsBottomLeft.x + (i * spacing) + offsetX;
            float posY = img.transform.position.y;
            float posZ = img.transform.position.z;
            img.transform.position = new Vector3(posX, posY, posZ);
            
        }

    }

    IEnumerator AnimateImages()
    {
        Vector3 upScale = new Vector3(1.25f, 1.25f, 1.25f);
        float duration = 1f;
        foreach(GameObject img in gameObjects)
        {
            yield return StartCoroutine(ScaleUpAndDownCenter(img.transform, upScale, duration));
        }

    }

    IEnumerator ScaleUpAndDownCenter(Transform objTransform, Vector3 upScale, float duration)
    {
        Vector3 initialScale = objTransform.localScale;
        Vector3 initialPosition = objTransform.position;

        // fixing z index, animating object should be on top
        Vector3 initialPositionAnimate = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z - 2);
        objTransform.position = initialPositionAnimate;
        
        Vector3 centerPoint = new Vector3(-2.5f, -2.5f, initialPositionAnimate.z);

        bool animSwitch = false;
        float lastProgress = 0;
        
        for (float time = 0; time < duration * 2; time += Time.deltaTime)
        {
            float progress = Mathf.PingPong(time, duration) / duration;

            objTransform.localScale = Vector3.Lerp(initialScale, upScale, progress);

            objTransform.position = Vector3.Lerp(initialPositionAnimate, centerPoint, progress);

            // pause before down scaling image
            if (progress < lastProgress && !animSwitch )
            {
                animSwitch = true;
                yield return new WaitForSeconds(1f);
            }

            lastProgress = progress;

            yield return null;
        }

        objTransform.localScale = initialScale;
        objTransform.position = initialPosition;
    }


    // -------------- OLD Code , not in use ---------------

    // Start is called before the first frame update
    void DoStart()
    {
        sr = new SpriteRenderer[gameObjects.Count];

        for (int j = 0; j < gameObjects.Count; j++)
        {
            sr[j] = gameObjects[j].AddComponent<SpriteRenderer>() as SpriteRenderer;
        }

        //Changing Image Position Depending on Image Index 
        for (int index = 0; index < chosenList.Count; index++)
        {
            //Vector3 shift = new Vector3((widthVal / -2.0f) + (widthVal * ((0.4f + (index % 6)) / 6.0f)), (height / 2.0f) - (height * (index / 6) / 4.5f) - 3.0f, 0.0f);
            Vector3 shift = new Vector3(0.0f, 0.0f, 0.0f);
            mySprite = Sprite.Create(chosenList[index], new Rect(0.0f, 0.0f, chosenList[index].width, chosenList[index].height), new Vector2(0.0f, 0.0f));
            sr[index].sprite = mySprite;
            gameObjects[index].transform.position += shift;
        }
       
    }

    void DoUpdate()
    {
        if (timePassed < 1.0f)
        {
            timePassed += Time.deltaTime;
        }

        if (objNum >= 4 && timePassed >= 1.0f)
        {
            SceneManager.LoadScene(4);

        }
        else if (objNum < 4)
        {
            // scale up
            if (timePassed >= 1.0f && timePassed < 2.0f)
            {
                timePassed += Time.deltaTime;

                float valIncrease = 5f - (objNum * 5);

                //Vector3 posTo = new Vector3(valIncrease * Time.deltaTime, -2.5f * Time.deltaTime, -10 * Time.deltaTime);
                Vector3 posTo = new Vector3(valIncrease * Time.deltaTime, -2.5f * Time.deltaTime, 0);

                gameObjects[objNum].transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime);

                gameObjects[objNum].transform.position += posTo;
            }
            // stay
            else if (timePassed >= 2.0f && timePassed < 3.0f)
            {
                timePassed += Time.deltaTime;
            }
            // scale down
            else if (timePassed >= 3.0f && timePassed < 4.0f)
            {
                float valIncrease = 5f - (objNum * 5);

                timePassed += Time.deltaTime;

                //Vector3 posTo = new Vector3(valIncrease * Time.deltaTime, -2.5f * Time.deltaTime, -10 * Time.deltaTime);
                Vector3 posTo = new Vector3(valIncrease * Time.deltaTime, -2.5f * Time.deltaTime, 0);

                gameObjects[objNum].transform.localScale += new Vector3(-1 * Time.deltaTime, -1 * Time.deltaTime, 0.0f);
                gameObjects[objNum].transform.position -= posTo;
            }
            // done
            else if (timePassed >= 4.0f)
            {
                timePassed = 0.0f;
                objNum++;
            }
        }
    }

}

