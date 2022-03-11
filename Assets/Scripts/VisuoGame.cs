using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisuoGame : MonoBehaviour
{

    public GameObject totalShape;

    public List<GameObject> optionsShapes = new List<GameObject>();

    private List<GameObject> optionsShapesShuffled = new List<GameObject>();
    private List<float> optionsShapesRotations = new List<float>();

    // Game Configurations
    private const int totalIterations = Constants.VISUOSPATIAL_TOTAL_ITERATIONS;// 25;

    private static int questionNum = 1;
    private float timeBetween = 0.0f;
    private int direction;
    private List<int> directions = new List<int>();

    public static int correctScore = 0;

    private string correctButton;
    public static string clickedButton;
    public static Boolean buttonClicked = false;

    private static System.Random rng = new System.Random();

    public static List<string> visual_data = new List<string>();


    public static void Shuffle(List<GameObject> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> listCubes = new List<GameObject>();
        listCubes.Clear();

        foreach (Transform child in totalShape.GetComponentInChildren<Transform>())
        {
            listCubes.Add(child.gameObject);
            Debug.Log("Main shape child: " + child.position);
        }

        List<Vector3> temp = new List<Vector3>();
        foreach (Transform child in totalShape.GetComponentInChildren<Transform>())
        {

            temp.Add(child.position);
            Debug.Log("Option shape child: " + child.position);
        }

        foreach (GameObject obj in optionsShapes)
        {
            List<Vector3> temp2 = new List<Vector3>();
            foreach (Transform child in totalShape.GetComponentInChildren<Transform>())
            {
                temp2.Add(child.position);
                Debug.Log(child.position);
            }
        }
    }

    void generateShape(GameObject totalStructure, List<int> directionList, float rotateAmount)
    {
        totalStructure.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        List<GameObject> listCubes = new List<GameObject>();
        

        foreach (Transform child in totalStructure.GetComponentInChildren<Transform>())
        {
            listCubes.Add(child.gameObject);
        }

        for (int i = 1; i < listCubes.Count; i++)
        {
            if (i < listCubes.Count - 1)
            {
                if (((int)(directionList[i - 1] / 2)) == ((int)directionList[i] / 2))
                {
                    directionList[i] += 2;
                }
                directionList[i] %= 6;
            }

            GameObject cubeCenter = listCubes[0];

            Vector3 cubeCenterBoxColliderSize = cubeCenter.GetComponent<BoxCollider>().bounds.size;

            Vector3 offset = Vector3.zero;

            switch (directionList[i - 1])
            {
                case 0: //TOP

                    // offset = new Vector3(0f, cubeCenterBoxColliderSize.y, 0f);
                    
                    offset = new Vector3(0f, 1f, 0f);

                    break;

                case 1: //BOTTOM

                    // offset = new Vector3(0f, -cubeCenterBoxColliderSize.y, 0f);

                    offset = new Vector3(0f, -1f, 0f);

                    break;

                case 2: //RIGHT

                    // offset = new Vector3(cubeCenterBoxColliderSize.x, 0f, 0f);

                    offset = new Vector3(1f, 0f, 0f);

                    break;

                case 3: //LEFT

                    // offset = new Vector3(-cubeCenterBoxColliderSize.x, 0f, 0f);

                    offset = new Vector3(-1f, 0f, 0f);

                    break;

                case 4: //FRONT

                    // offset = new Vector3(0f, 0f, -cubeCenterBoxColliderSize.z);

                    offset = new Vector3(0f, 0f, -1f);

                    break;

                case 5: //BEHIND

                    // offset = new Vector3(0f, 0f, cubeCenterBoxColliderSize.z);

                    offset = new Vector3(0f, 0f, 1f);

                    break;
            }

            listCubes[i].transform.localPosition =
                        cubeCenter.transform.localPosition
                        + offset;
        }

        totalStructure.transform.Rotate(0.0f, rotateAmount, 0.0f);
    }

    void resetRotations()
    {
        for (int i = 0; i < optionsShapesShuffled.Count; i++)
        {
            optionsShapesShuffled[i].transform.Rotate(0.0f, -1 * optionsShapesRotations[i], 0.0f);
        }
    }

    void generateOptions()
    {
        // reset all options cubes to default transform values
        foreach (GameObject sh in optionsShapes) ResetShape(sh);

        List<int> direction1 = new List<int>(directions);
        List<int> direction2 = new List<int>(directions);
        List<int> direction3 = new List<int>(directions);
        List<int> direction4 = new List<int>(directions);

        int temp = direction1[0];
        while (direction1[0] == temp)
        {
            direction1[0] = rng.Next(0, 6);
        }

        temp = direction2[1];
        while (direction2[1] == temp)
        {
            direction2[1] = rng.Next(0, 6);
        }

        temp = direction3[2];
        while (direction3[2] == temp)
        {
            direction3[2] = rng.Next(0, 6);
        }

        List<GameObject> options = optionsShapes;
        Shuffle(options);
        optionsShapesShuffled = options;

        int next = rng.Next(1, 4) * 30; // rotation
        optionsShapesRotations.Add(next);
        generateShape(options[0], direction1, next);

        next = rng.Next(1, 4) * 30;
        optionsShapesRotations.Add(next);
        generateShape(options[1], direction2, next);

        next = rng.Next(1, 4) * 30;
        optionsShapesRotations.Add(next);
        generateShape(options[2], direction3, next);

        next = rng.Next(1, 4) * 30;
        optionsShapesRotations.Add(next);
        generateShape(options[3], direction4, next);

        correctButton = options[3].name;
    }

    public static int returnScore()
    {
        return correctScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonClicked)
        {
            resetRotations();

            if (clickedButton.Split('-')[1].Equals(correctButton.Split('-')[1]))
            {
                correctScore++;
                visual_data.Add(
                    questionNum + ","
                    + clickedButton.Split('-')[1] + ","
                    + correctButton.Split('-')[1]
                    + "True" + ","
                    + timeBetween);
            }
            else
            {
                visual_data.Add(
                    questionNum + ","
                    + clickedButton.Split('-')[1] + ","
                    + correctButton.Split('-')[1]
                    + "False" + ","
                    + timeBetween);
            }

            questionNum++;
            timeBetween = 0.0f;
            buttonClicked = false;
        }


        if (questionNum <= totalIterations)
        {
            if (timeBetween == 0.0f)
            {
                int count = 0;

                System.Random rnd = new System.Random();

                while (count < 6)
                {
                    direction = rnd.Next(0, 5);
                    directions.Add(direction);
                    count++;
                }

                generateShape(totalShape, directions, 0.0f);
                generateOptions();

                timeBetween += Time.deltaTime;
                directions.Clear();
            }
            else if (timeBetween >= 10.0f)
            {
                visual_data.Add(
                    questionNum + ","
                    + "Null" + ","
                    + correctButton.Split('-')[1]
                    + "False" + ","
                    + timeBetween);

                questionNum++;
                timeBetween = 0.0f;
                buttonClicked = false;
            }
        }
        else
        {
            DataStorage._visualData = visual_data;
            SceneManager.LoadScene((int)Constants.SCENES.FINAL_SCORE_VISUO);
        }
    }

    // reset shape made of cubes
    void ResetShape(GameObject totalStructure)
    {
        foreach (Transform child in totalStructure.GetComponentInChildren<Transform>())
        {
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localRotation = Quaternion.identity;

        }
    }
}
