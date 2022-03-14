using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class RecallGame : MonoBehaviour
{
    
    public List<Button> allButtons = new List<Button>();
    public GameObject timerBar;
    
    private Texture2D correctImg;
    private Texture2D clickedImg;

    private List<Texture2D> allImgs;
    private List<Texture2D> chosenList;
    private List<Texture2D> notChosen = new List<Texture2D>();


    // Game State variables
    private bool gameStarted = false;
    private float timePassed = 0f;

    private int correctButton = 0;
    private bool selected = false;
    private bool timeUp = false;
    private int iteration = 0;
    private bool roundStarted = false;

    private float widthDec = 0.0f;
    
    private static int numCorrect = 0;
    private static int numWrong = 0;
    private static int numTimesUp = 0;

    //private float timeLeft = 5f; // time passed

    // Game play data
    public static List<string> recall_data = new List<string>();

    // Game configurations
    private const int totalIterations = Constants.RECALL_TOTAL_ITERATIONS; //30;
    private const float totalTime = 5f;
    private const float totalTimeToPlay = 4f;

    // Memory Images
    private MemoryImagesUtil memoryImagesUtil;


    public static int[] returnScore()
    {
        int[] finalScore = new int[3];

        finalScore[0] = numCorrect;
        finalScore[1] = numWrong;
        finalScore[2] = numTimesUp;

        return finalScore;
    }


    void Start()
    {
        // Memory Images
        memoryImagesUtil = GameObject.Find("MemoryImages").GetComponent<MemoryImagesUtil>();
        allImgs = memoryImagesUtil.allImgs;

        widthDec = timerBar.transform.localScale.x / 4;
        this.chosenList = DisplayMemoryIcons.chosenList;

        DeActivateButtons();

        foreach (Texture2D img in allImgs)
        {
            if (!chosenList.Contains(img))
            {
                notChosen.Add(img);
            }
        }

        //Iter, Gen_Categ_Correct, Categ_Correct, Sub_Categ_Correct, Img_Correct, Gen_Categ_Clicked, Categ_Clicked, Sub_Categ_Clicked, Img_Clicked, Is_Correct, Time_To_Click

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(5.0f);
        gameStarted = true;
    }

    void onClick(Button clickedButton)
    {
        selected = true;
        clickedImg = clickedButton.GetComponent<Image>().sprite.texture;

        Debug.Log(clickedImg.ToString() + ", " + correctImg.ToString());

        if (clickedButton.GetComponent<Image>().sprite.texture.ToString().Equals(correctImg.ToString()))
        {
            numCorrect++;
        }
        else
        {
            numWrong++;
        }

        Debug.Log(iteration + ": Total Correct:" + numCorrect);
        Debug.Log(iteration + ": Total Wrong:" + numWrong);
    }

    void StartNextRound()
    {
        ActivateButtons();

        List<Texture2D> displayedImgs = new List<Texture2D>();
        List<int> orderOfImgs = new List<int> { 0, 1, 2, 3, 4, 5 };

        Debug.Log(iteration + ": Beginning");

        //Add 1 Correct Img
        correctImg = chosenList[iteration];

        string[] categorizationCorrect = correctImg.ToString().Split('-');

        Debug.Log(iteration + ": Add Correct Img");

        //Add 2 Same Sub-Category Img
        List<Texture2D> sameSubCateg = memoryImagesUtil.GetSameSubCategoryTextures(correctImg, notChosen);

        Debug.Log(iteration + ": Find Same Sub Categories");
        Debug.Log("Correct Image: " + correctImg.ToString());
        Debug.Log("Same Sub Categories Count: " + sameSubCateg.Count);

        int rand1 = Random.Range(0, sameSubCateg.Count);
        int rand2 = Random.Range(0, sameSubCateg.Count);

        if (sameSubCateg.Count >= 2)
        {
            while (rand2 == rand1)
            {
                rand2 = Random.Range(0, sameSubCateg.Count);
            }
        }
        
        displayedImgs.Add(sameSubCateg[rand1]);
        displayedImgs.Add(sameSubCateg[rand2]);
        displayedImgs.Add(correctImg);

        //Add 1 Same Category Img
        List<Texture2D> sameCateg = memoryImagesUtil.GetSameCategoryTextures(correctImg, notChosen);

        Debug.Log(iteration + ": Find Same Categories");
        Debug.Log("Correct Image: " + correctImg.ToString());
        Debug.Log("Same Categories Count: " + sameCateg.Count);

        displayedImgs.Add(sameCateg[Random.Range(0, sameCateg.Count)]);


        //Add 1 Same Category General Img
        List<Texture2D> sameGeneral = memoryImagesUtil.GetSameCategoryGeneralTextures(correctImg, notChosen);

        Debug.Log(iteration + ": Find Same General Categories");
        Debug.Log("Correct Image: " + correctImg.ToString());
        Debug.Log("Same General Categories Count: " + sameGeneral.Count);

        displayedImgs.Add(sameGeneral[Random.Range(0, sameGeneral.Count)]);


        //Add 1 Different Category Img
        List<Texture2D> different = memoryImagesUtil.GetDifferentCategoryTextures(correctImg, notChosen);

        Debug.Log(iteration + ": Find Different Categories");
        Debug.Log("Correct Image: " + correctImg.ToString());
        Debug.Log("Different Categories Count: " + different.Count);

        displayedImgs.Add(different[Random.Range(0, different.Count)]);


        //Assigning Imgs to Buttons
        orderOfImgs = orderOfImgs.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < displayedImgs.Count; i++)
        {
            Texture2D element1 = displayedImgs[i];
            Texture2D element2 = displayedImgs[orderOfImgs[i]];

            displayedImgs[i] = displayedImgs[orderOfImgs[i]];
            displayedImgs[orderOfImgs[i]] = element1;
        }


        for (int i = 0; i < allButtons.Count; i++)
        {
            allButtons[i].GetComponent<Image>().sprite = Sprite.Create(displayedImgs[i], new Rect(0.0f, 0.0f, displayedImgs[i].width, displayedImgs[i].height), new Vector2(0.0f, 0.0f));
        }

        for (int i = 0; i < allButtons.Count; i++)
        {
            if (allButtons[i].GetComponent<Image>().sprite.texture.ToString().Equals(correctImg.ToString()))
            {
                correctButton = i;
                break;
            }
        }

        EnableButtons();

        roundStarted = true;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (gameStarted && iteration < totalIterations)
        {
            // first round
            if (iteration == 0 && !roundStarted)
            {
                HandleFirstRound();
            }

            // waiting for player to select image
            else if (timePassed < totalTimeToPlay
                && iteration < totalIterations
                && !selected)
            {
                HandleWaitState();
            }

            // player selected image within time limit
            else if (timePassed < totalTimeToPlay
                && iteration < totalIterations
                && selected)
            {
                HandleMovePlayed();
            }


            // player failed to select image within time limit
            else if (timePassed >= totalTimeToPlay
                && timePassed < totalTime
                && iteration < totalIterations
                && !timeUp
                && !selected)
            {
                HandleTimeUp();
            }


            // one second pause after timeup or player move
            else if ((timeUp || selected)
                && timePassed < totalTime
                && iteration < totalIterations)
            {
                timePassed += Time.deltaTime;
            }


            // next round
            else if (timePassed >= totalTime
                && iteration < totalIterations)
            {
                HandleCurrentIterationCompleted();
            }

            
        }


        // recall game completed
        else if (iteration >= totalIterations)
        {
            HandleAllIterationsCompleted();
        }
    }

    // --------------- Game States ------------------
    void HandleFirstRound()
    {
        // reset values for next iteration
        selected = false;
        timeUp = false;
        timePassed = 0;
        clickedImg = null;
        timerBar.transform.localScale = new Vector3(widthDec * 4, timerBar.transform.localScale.y, 1.0f);

        StartNextRound();
    }


    void HandleWaitState()
    {
        timePassed += Time.deltaTime;
        timerBar.transform.localScale -= new Vector3(Time.deltaTime * widthDec, 0.0f, 0.0f);
    }

    void HandleMovePlayed()
    {
        // record game play data
        string[] categorizationClicked = clickedImg.ToString().Split('-');
        string[] categorizationCorrect = correctImg.ToString().Split('-');
        bool correct = clickedImg.ToString().Equals(correctImg.ToString());

        recall_data.Add(
            iteration + ","
            + categorizationCorrect[0] + ","
            + categorizationCorrect[1] + ","
            + categorizationCorrect[2] + ","
            + categorizationCorrect[3] + ","
            + categorizationClicked[0] + ","
            + categorizationClicked[1] + ","
            + categorizationClicked[2] + ","
            + categorizationClicked[3] + ","
            + correct.ToString() + ","
            + timePassed.ToString() + "\n");

        // workaround: once player has played move
        timePassed = totalTimeToPlay;

        DisableButtons();
    }

    void HandleTimeUp()
    {
        numTimesUp++;
        timeUp = true;
        DisableButtons();
    }

    void HandleCurrentIterationCompleted()
    {
        if (!selected && iteration > 0)
        {
            // record game play data
            string[] categorizationCorrect = correctImg.ToString().Split('-');

            recall_data.Add(
                iteration + ","
                + categorizationCorrect[0] + ","
                + categorizationCorrect[1] + ","
                + categorizationCorrect[2] + ","
                + categorizationCorrect[3] + ","
                + "Null" + ","
                + "Null" + ","
                + ","
                + "Null" + ","
                + "Null" + ","
                + "False" + ","
                + timePassed.ToString() + "\n");
        }

        // reset values for next iteration
        selected = false;
        timeUp = false;
        timePassed = 0;
        clickedImg = null;
        roundStarted = false;
        timerBar.transform.localScale = new Vector3(widthDec * 4, timerBar.transform.localScale.y, 1.0f);

        DeActivateButtons();

        iteration++;

        if(iteration < totalIterations)
            StartNextRound();
    }

    void HandleAllIterationsCompleted()
    {
        DataStorage._recallData = recall_data;
        MoveToNextScreen();
    }


    // -------------------------------------------------------------

    private void MoveToNextScreen()
    {
        int sceneNumber = (int)Constants.SCENES.VISUO_INSTRUCTIONS;
        if (Constants.SKIP_VISUOSPATIAL_LEVEL == true)
        {
            sceneNumber = (int)Constants.SCENES.FINAL_SCORE;
        } 
        SceneManager.LoadScene(sceneNumber);
        
    }

    // ------------------ Buttons Helper Methods --------------------

    private void EnableButtons()
    {
        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(() => onClick(button));
            button.GetComponent<UIOnHoverEvent>().enabled = true;
            button.GetComponent<UIOnHoverEvent>().ResetScale();
            button.enabled = true;
        }
    }

    private void DisableButtons()
    {
        foreach (Button button in allButtons)
        {
            button.onClick.RemoveAllListeners();
            // button.GetComponent<UIOnHoverEvent>().ResetScale();
            button.GetComponent<UIOnHoverEvent>().enabled = false;
            button.enabled = false;
        }
    }

    void ActivateButtons()
    {
        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    void DeActivateButtons()
    {
        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    // ----------------- Discard Methods --------------------

/*
    void Instantiate()
    {
        foreach (Button button in allButtons)
        {
            button.gameObject.SetActive(true);
        }

        List<Button> buttonList = allButtons;
        List<Texture2D> displayedImgs = new List<Texture2D>();
        List<int> orderOfImgs = new List<int> { 0, 1, 2, 3, 4, 5 };

        Debug.Log(iteration + ": Beginning");

        //Add 1 Correct Img
        correctImg = chosenList[iteration];

        string[] categorizationCorrect = correctImg.ToString().Split('-');

        Debug.Log(iteration + ": Add Correct Img");

        //Add 2 Same Sub-Category Img
        List<Texture2D> sameSubCateg = new List<Texture2D>();
        foreach (Texture2D img in notChosen)
        {
            string[] categorization = img.ToString().Split('-');

            if (categorization[0].Equals(categorizationCorrect[0]) && categorization[1].Equals(categorizationCorrect[1]) && categorization[2].Equals(categorizationCorrect[2]))
            {
                {
                    sameSubCateg.Add(img);
                }
            }
        }

        Debug.Log(iteration + ": Find Sub Categs");
        Debug.Log(correctImg.ToString());
        Debug.Log("Num of sub categs: " + sameSubCateg.Count);

        int rand1 = Random.Range(0, sameSubCateg.Count);
        int rand2 = Random.Range(0, sameSubCateg.Count);

        while (rand2 == rand1)
        {
            rand2 = Random.Range(0, sameSubCateg.Count);
        }

        displayedImgs.Add(sameSubCateg[rand1]);
        displayedImgs.Add(sameSubCateg[rand2]);
        displayedImgs.Add(correctImg);

        Debug.Log(iteration + ": Choose Sub Categs");

        //Add 1 Same Category Img
        List<Texture2D> sameCateg = new List<Texture2D>();
        foreach (Texture2D img in notChosen)
        {
            string[] categorization = img.ToString().Split('-');

            if (categorization[0].Equals(categorizationCorrect[0]) && categorization[1].Equals(categorizationCorrect[1]) && !categorization[2].Equals(categorizationCorrect[2]))
            {
                {
                    sameCateg.Add(img);
                }
            }
        }

        Debug.Log(iteration + ": Find Categs");

        displayedImgs.Add(sameCateg[Random.Range(0, sameCateg.Count)]);

        Debug.Log(iteration + ": Choose Categs");

        //Add 1 Same Category Img
        List<Texture2D> sameGeneral = new List<Texture2D>();
        foreach (Texture2D img in notChosen)
        {
            string[] categorization = img.ToString().Split('-');

            if (categorization[0].Equals(categorizationCorrect[0]) && !categorization[1].Equals(categorizationCorrect[1]) && !categorization[2].Equals(categorizationCorrect[2]))
            {
                {
                    sameGeneral.Add(img);
                }
            }
        }

        Debug.Log(iteration + ": Find Generals");

        displayedImgs.Add(sameGeneral[Random.Range(0, sameGeneral.Count)]);

        Debug.Log(iteration + ": Choose Generals");

        //Add 1 Different Category Img
        List<Texture2D> different = new List<Texture2D>();
        foreach (Texture2D img in notChosen)
        {
            string[] categorization = img.ToString().Split('-');

            if (!categorization[0].Equals(categorizationCorrect[0]) && !categorization[1].Equals(categorizationCorrect[1]) && !categorization[2].Equals(categorizationCorrect[2]))
            {
                {
                    different.Add(img);
                }
            }
        }

        Debug.Log(iteration + ": Find Other");

        displayedImgs.Add(different[Random.Range(0, different.Count)]);

        Debug.Log(iteration + ": Choose Other");



        //Assigning Imgs to Buttons
        orderOfImgs = orderOfImgs.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < displayedImgs.Count; i++)
        {
            Texture2D element1 = displayedImgs[i];
            Texture2D element2 = displayedImgs[orderOfImgs[i]];

            displayedImgs[i] = displayedImgs[orderOfImgs[i]];
            displayedImgs[orderOfImgs[i]] = element1;
        }


        for (int i = 0; i < allButtons.Count; i++)
        {
            allButtons[i].GetComponent<Image>().sprite = Sprite.Create(displayedImgs[i], new Rect(0.0f, 0.0f, displayedImgs[i].width, displayedImgs[i].height), new Vector2(0.0f, 0.0f));
        }

        for (int i = 0; i < allButtons.Count; i++)
        {
            if (allButtons[i].GetComponent<Image>().sprite.texture.ToString().Equals(correctImg.ToString()))
            {
                correctButton = i;
                break;
            }
        }

        EnableButtons();
    }

*/

/*
    // Update is called once per frame
    void Update()
    {
        if (gameStarted && iteration < chosenList.Count) // choseList.Count is same as totalIterations
        {
            // waiting for player to select image
            if (timeLeft < 4f && !selected && iteration < totalIterations)
            {
                timeLeft += Time.deltaTime;
                timerBar.transform.localScale -= new Vector3(Time.deltaTime * widthDec, 0.0f, 0.0f);
            }
            // player selected image within time limit
            else if (timeLeft < 4f && selected && iteration < totalIterations)
            {
                string[] categorizationClicked = clickedImg.ToString().Split('-');
                string[] categorizationCorrect = correctImg.ToString().Split('-');
                bool correct = clickedImg.ToString().Equals(correctImg.ToString());

                recall_data.Add(iteration + "," + categorizationCorrect[0] + "," + categorizationCorrect[1] + "," + categorizationCorrect[2]
                                + "," + categorizationCorrect[3] + "," + categorizationClicked[0] + "," + categorizationClicked[1] + ","
                                + categorizationClicked[2] + "," + categorizationClicked[3] + "," + correct.ToString() + "," + timeLeft.ToString() + "\n");

                timeLeft = 4f;

                DisableButtons();
            }
            // player failed to select image within time limit
            else if (timeLeft >= 4f && timeLeft < 5f && !selected && iteration < totalIterations)
            {
                numTimesUp++;

                selected = true;
            }
            // player selected image after timeup
            else if (timeLeft >= 4f && timeLeft < 5f && selected && iteration < totalIterations)
            {
                timeLeft += Time.deltaTime;
            }
            // next round
            else if (timeLeft >= 5f && iteration < totalIterations)
            {
                if (!selected && iteration > 0)
                {
                    string[] categorizationCorrect = correctImg.ToString().Split('-');
                    recall_data.Add(iteration + "," + categorizationCorrect[0] + "," + categorizationCorrect[1] + "," + categorizationCorrect[2]
                                + "," + categorizationCorrect[3] + "," + "Null" + "," + "Null" + "," +
                                "," + "Null" + "," + "Null" + "," + "False" + "," + timeLeft.ToString() + "\n");
                }

                selected = false;

                foreach (Button button in allButtons)
                {
                    button.gameObject.SetActive(false);
                }

                timeLeft = 0;
                timerBar.transform.localScale = new Vector3(widthDec * 4, timerBar.transform.localScale.y, 1.0f);


                Instantiate();

                //iteration++;

            }
        }
        // recall game completed
        else if (iteration >= totalIterations)
        {
            DataStorage._recallData = recall_data;
            MoveToNextScreen();
        }
    }

*/

}