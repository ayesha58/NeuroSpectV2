using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{

    public static void onButtonClick(Button button)
    {
        GenerateShapes.clickedButton = button.name;
        GenerateShapes.buttonClicked = true;
    }

}
