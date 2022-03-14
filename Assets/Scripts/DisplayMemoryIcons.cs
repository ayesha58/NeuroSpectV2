using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/**
 * Used by Encoding Scene
 */

public class DisplayMemoryIcons : MonoBehaviour
{
    private List<Texture2D> allImgs;

    public static List<Texture2D> chosenList = new List<Texture2D>();
    private List<int> chosenNums = new List<int>();

    private Sprite mySprite;
    private SpriteRenderer[] sr;
    public List<GameObject> gameObjects;

    public GameObject canvas;

    private const int totalMemoryIcons = Constants.RECALL_TOTAL_ITERATIONS;

    // Memory Images
    private MemoryImagesUtil memoryImagesUtil;

    void Start()
    {
        // Memory Images
        memoryImagesUtil = GameObject.Find("MemoryImages").GetComponent<MemoryImagesUtil>();
        allImgs = memoryImagesUtil.allImgs;

        InitiateImages();
        //InitiateTestImages();
    }

    void Update()
    {
        // FixScale();
    }

    void InitiateImages()
    {
        for (int i = 0; i < allImgs.Count; i++)
        {
            chosenNums.Add(i);
        }

        //Selecting 30 Unique Random Images from set of Images
        for (int num = 0; num < totalMemoryIcons; num++)
        {
            int randNum = Random.Range(0, allImgs.Count);
            if (!chosenNums.Contains(randNum))
            {
                num--;
                continue;
            }
            else
            {
                int sameSubcategoryCount = getSameSubCategoryCount(allImgs[randNum].ToString(), chosenList);

                Debug.Log("Encoding: Same Sabcategory Count: " + sameSubcategoryCount);

                if (sameSubcategoryCount >= 2)
                {
                    num--;
                    continue;
                }
                else
                {
                    chosenList.Add(allImgs[randNum]);
                    chosenNums.Remove(randNum);
                }
            }
        }

        sr = new SpriteRenderer[gameObjects.Count];

        for (int j = 0; j < gameObjects.Count; j++)
        {
            sr[j] = gameObjects[j].AddComponent<SpriteRenderer>() as SpriteRenderer;
        }

        //Changing Image Position Depending on Image Index
        for (int index = 0; index < chosenList.Count; index++)
        {

            Texture2D tx = chosenList[index];

            mySprite = Sprite.Create(
                tx,
                new Rect(0.0f, 0.0f, tx.width, tx.height),
                new Vector2(0.0f, 0.0f));

            sr[index].sprite = mySprite;
        }

        Debug.Log("Encoding: Chosen List of Images: ");
        foreach(Texture2D tx in chosenList)
        {
            Debug.Log("item: " + tx.ToString());
        }

    }

    public int getSameSubCategoryCount(string imgName, List<Texture2D> list)
    {
        int count = 0;
        string[] categ = imgName.Split('-');

        if (list.Count > 0)
        {
            foreach (Texture2D tx in list)
            {
                string[] categ2 = tx.ToString().Split('-');
                if (categ[2].Equals(categ2[2]))
                {
                    count++;
                }
            }
        }

        return count;
    }

    // Note: This code is making the encoding screen images smaller on high resolution
    void FixScale()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;

        Debug.Log(Screen.width + ", " + Screen.height + ", " + aspectRatio);

        // aspect ratio: 2.25
        if (aspectRatio < (1522f / 676f))
        {
            if (Screen.width > 1522f)
            {
                canvas.transform.localScale = new Vector3(1522f / Screen.width, 1522f / Screen.width, 1f);
            }
        }
        else
        {
            canvas.transform.localScale = new Vector3(Screen.width / 1522f, Screen.width / 1522f, 1f);
        }
    }

    void InitiateTestImages()
    {
        chosenList = memoryImagesUtil.GetTestImageTextures();

        sr = new SpriteRenderer[gameObjects.Count];

        for (int index = 0; index < chosenList.Count; index++)
        {
            Texture2D tx = chosenList[index];

            mySprite = Sprite.Create(
                tx,
                new Rect(0.0f, 0.0f, tx.width, tx.height),
                new Vector2(0.0f, 0.0f));

            sr[index] = gameObjects[index].AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr[index].sprite = mySprite;
        }
    }

}
