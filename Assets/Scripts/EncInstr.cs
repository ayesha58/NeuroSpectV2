using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EncInstr : MonoBehaviour
{
    public List<Texture2D> chosenList;
    public List<GameObject> gameObjects;

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
                new Vector2(0.5f, 0.5f));

        }

        PositionImages();

        StartCoroutine(Begin());
    }

    IEnumerator Begin()
    {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(AnimateImages());
        yield return new WaitForSeconds(7);//1
        SceneManager.LoadScene((int)Constants.SCENES.ENCODING);//4
    }

    void PositionImages()
    {
        // Screen bounds in units: (-8, -5), (8, 5)
        Vector2 screenBoundsTopRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenBoundsBottomLeft  = Camera.main.ScreenToWorldPoint(Vector2.zero);

        // calculations
        float screenWidth = screenBoundsTopRight.x * 2;
        float spacing = screenWidth / gameObjects.Count;
        float offsetX = spacing / 2;// - imageWidth / 2;

        for (int i = 0; i < gameObjects.Count; i++)
        {
            GameObject img = gameObjects[i];
            float posX = screenBoundsBottomLeft.x + (i * spacing) + offsetX;
            float posY = img.transform.position.y;
            float posZ = img.transform.position.z;
            img.transform.position = new Vector3(posX, posY, posZ);
            
        }

        // Game Object width height in units
        // float imageWidth = gameObjects[0].GetComponent<SpriteRenderer>().bounds.size.x; // 1.27
        // float imageHeight = gameObjects[0].GetComponent<SpriteRenderer>().bounds.size.y; // 1.27

        // Sprite Texture dimensions
        // float spriteWidth = gameObjects[0].GetComponent<SpriteRenderer>().sprite.rect.width; // 512
        // float spriteHeight = gameObjects[0].GetComponent<SpriteRenderer>().sprite.rect.height; // 512

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
        
        Vector3 centerPoint = new Vector3(0f, -1.5f, initialPositionAnimate.z);

        bool animSwitch = false;
        float lastProgress = 0;
        
        for (float timePassed = 0; timePassed < duration * 2; timePassed += Time.deltaTime)
        {
            float progress = Mathf.PingPong(timePassed, duration) / duration;

            objTransform.localScale = Vector3.Lerp(initialScale, upScale, progress);

            objTransform.position = Vector3.Lerp(initialPositionAnimate, centerPoint, progress);

            // pause before down scaling image
            if ( !animSwitch && progress < lastProgress )
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


    

}

