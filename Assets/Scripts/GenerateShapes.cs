using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GenerateShapes : MonoBehaviour
{

    private static int questionNum = 1;
    private float timeBetween = 0.0f;
    private int direction;
    private List<int> directions = new List<int>();

    public GameObject totalShape;

    private static System.Random rng = new System.Random();

    public List<GameObject> optionsShapes = new List<GameObject>();
    private List<GameObject> optionsShapesShuffled = new List<GameObject>();
    private List<float> optionsShapesRotations = new List<float>();

    public static int correctScore = 0;
    private string correctButton;
    public static string clickedButton;
    public static Boolean buttonClicked = false;

    public static List<string> visual_data = new List<string>();
    private Vector3 movement = new Vector3(0, 0, 0);

    private List<List<Vector3>> originalLocs = new List<List<Vector3>>();

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
        originalLocs.Add(temp);
        foreach(GameObject obj in optionsShapes)
        {
            List<Vector3> temp2 = new List<Vector3>();
            foreach(Transform child in totalShape.GetComponentInChildren<Transform>())
            {
                temp2.Add(child.position);
                Debug.Log(child.position);
            }
            originalLocs.Add(temp2);
        }
    }

    void generateShape(GameObject totalStructure, List<int> directionList, float rotateAmount, int positionNum)
    {
        //totalStructure.transform.position -= movement;
        totalStructure.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        List<GameObject> listCubes = new List<GameObject>();
        listCubes.Clear();

        

        foreach (Transform child in totalStructure.GetComponentInChildren<Transform>())
        {
            listCubes.Add(child.gameObject);
        }

        Vector3 distanceTraveled = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 1; i < listCubes.Count; i++)
        {
            GameObject cubeCenter = listCubes[0];
            if (i < listCubes.Count - 1)
            {
                if (((int) (directionList[i - 1] / 2)) == ((int) directionList[i] / 2))
                {
                    directionList[i] += 2;
                }
                directionList[i] %= 6;
            }
            switch (directionList[i - 1])
            {
                case 0: //TOP
                    listCubes[i].transform.position = cubeCenter.transform.position + new Vector3(0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.y, 0.0f);
                    distanceTraveled += new Vector3(0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.y, 0.0f);
                    break;
                case 1: //BOTTOM
                    listCubes[i].transform.position = cubeCenter.transform.position - new Vector3(0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.y, 0.0f);
                    distanceTraveled -= new Vector3(0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.y, 0.0f);
                    break;
                case 2: //RIGHT
                    listCubes[i].transform.position = cubeCenter.transform.position + new Vector3(cubeCenter.GetComponent<BoxCollider>().bounds.size.x, 0.0f, 0.0f);
                    distanceTraveled += new Vector3(cubeCenter.GetComponent<BoxCollider>().bounds.size.x, 0.0f, 0.0f);
                    break;
                case 3: //LEFT
                    listCubes[i].transform.position = cubeCenter.transform.position - new Vector3(cubeCenter.GetComponent<BoxCollider>().bounds.size.x, 0.0f, 0.0f);
                    distanceTraveled -= new Vector3(cubeCenter.GetComponent<BoxCollider>().bounds.size.x, 0.0f, 0.0f);
                    break;
                case 4: //FRONT
                    listCubes[i].transform.position = cubeCenter.transform.position - new Vector3(0.0f, 0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.z);
                    distanceTraveled -= new Vector3(0.0f, 0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.z);
                    break;
                case 5: //BEHIND
                    listCubes[i].transform.position = cubeCenter.transform.position + new Vector3(0.0f, 0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.z);
                    distanceTraveled += new Vector3(0.0f, 0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.z);
                    break;
            }
        }

        /*if(distanceTraveled.x > 0 && rotateAmount < 0)
        {
            movement = new Vector3(0.25f, 0.0f, 0.0f);
            totalStructure.transform.position += movement;
        } else if (distanceTraveled.x < 0 && rotateAmount < 0)
        {
            movement = new Vector3(1.0f, 0.0f, 0.0f);
            totalStructure.transform.position += movement;
        }
        else if (distanceTraveled.x > 0 && rotateAmount > 0)
        {
            movement = new Vector3(-1.0f, 0.0f, 0.0f);
            totalStructure.transform.position += movement;
        }
        else if (distanceTraveled.x < 0 && rotateAmount > 0)
        {
            movement = new Vector3(-0.25f, 0.0f, 0.0f);
            totalStructure.transform.position += movement;
        }*/

            totalStructure.transform.Rotate(0.0f, rotateAmount, 0.0f);
    }

    void resetRotations()
    {
        for(int i = 0; i < optionsShapesShuffled.Count; i++)
        {
            optionsShapesShuffled[i].transform.Rotate(0.0f, -1 * optionsShapesRotations[i], 0.0f);
        }
    }

    void generateOptions() {
        List<int> direction1 = new List<int>(directions);
        List<int> direction2 = new List<int>(directions);
        List<int> direction3 = new List<int>(directions);
        List<int> direction4 = new List<int>(directions);

        int temp = direction1[0];
        while(direction1[0] == temp)
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

        int next = rng.Next(1, 4) * 30;
        optionsShapesRotations.Add(next);
        generateShape(options[0], direction1, next, 1);

        next = rng.Next(1, 4) * 30;
        optionsShapesRotations.Add(next);
        generateShape(options[1], direction2, next, 2);

        next = rng.Next(1, 4) * 30;
        optionsShapesRotations.Add(next);
        generateShape(options[2], direction3, next, 3);

        next = rng.Next(1, 4) * 30;
        optionsShapesRotations.Add(next);
        generateShape(options[3], direction4, next, 4);

        correctButton = options[3].name;
    }

    public static float returnScore()
    {
        return correctScore;
    }

    // Update is called once per frame
    void Update()
    {
        if(buttonClicked)
        {
            resetRotations();
            if (clickedButton.Split('-')[1].Equals(correctButton.Split('-')[1]))
            {
                correctScore++;
                visual_data.Add(questionNum + "," + clickedButton.Split('-')[1] + "," + correctButton.Split('-')[1] + "True" + "," + timeBetween);
            }
            else
            {
                visual_data.Add(questionNum + "," + clickedButton.Split('-')[1] + "," + correctButton.Split('-')[1] + "False" + "," + timeBetween);
            }

            questionNum++;
            timeBetween = 0.0f;
            buttonClicked = false;
        }

        
        if (questionNum <= 25)
        {
            if(timeBetween == 0.0f)
            {
                int count = 0;

                System.Random rnd = new System.Random();

                while (count < 6)
                {
                    direction = rnd.Next(0, 5);
                    directions.Add(direction);
                    count++;
                }

                generateShape(totalShape, directions, 0.0f, 0);
                generateOptions();

                timeBetween += Time.deltaTime;
                directions.Clear();
            } else if(timeBetween >= 10.0f)
            {
                visual_data.Add(questionNum + "," + "Null" + "," + correctButton.Split('-')[1] + "False" + "," + timeBetween);
                questionNum++;
                timeBetween = 0.0f;
                buttonClicked = false;
            }
        } else
        {
            DataStorage._visualData = visual_data;
            SceneManager.LoadScene(12);
        }
    }
}
