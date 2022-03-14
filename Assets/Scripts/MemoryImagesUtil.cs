using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryImagesUtil : MonoBehaviour
{
    public List<Texture2D> allImgs;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //------------

    public List<Texture2D> GetSameSubCategoryTextures(Texture2D tx)
    {
        return GetSameCategoryTextures(tx, allImgs);
    }

    public List<Texture2D> GetSameCategoryTextures(Texture2D tx)
    {
        return GetSameCategoryTextures(tx, allImgs);
    }

    public List<Texture2D> GetSameCategoryGeneralTextures(Texture2D tx)
    {
        return GetSameCategoryGeneralTextures(tx, allImgs);
    }

    public List<Texture2D> GetDifferentCategoryTextures(Texture2D tx)
    {
        return GetDifferentCategoryTextures(tx, allImgs);
    }

    // ------------------ Get Textures based on Category --------------------

    // Texture name format: 1-Fruits-Berries-005blueberry: General-Category-SubCategory-Name
    public List<Texture2D> GetSameSubCategoryTextures(Texture2D tx, List<Texture2D> txList)
    {
        string[] categorizationCorrect = tx.name.Split('-');

        //Add 2 Same Sub-Category Img
        List<Texture2D> selectedTexturesList = new List<Texture2D>();
        foreach (Texture2D txt in txList)
        {
            string[] categorization = txt.name.Split('-');

            Debug.Log(">> Cat 1: " + tx.name);
            Debug.Log(">> Cat 2: " + txt.name);

            if (categorization[0].Equals(categorizationCorrect[0]) && categorization[1].Equals(categorizationCorrect[1]) && categorization[2].Equals(categorizationCorrect[2]))
            {
                {
                    Debug.Log(">> Adding Texture: " + txt.name);
                    selectedTexturesList.Add(txt);
                }
            }
        }

        return selectedTexturesList;
    }

    public List<Texture2D> GetSameCategoryTextures(Texture2D tx, List<Texture2D> txList)
    {
        string[] categorizationCorrect = tx.name.Split('-');

        //Add 2 Same Sub-Category Img
        List<Texture2D> selectedTexturesList = new List<Texture2D>();
        foreach (Texture2D txt in txList)
        {
            string[] categorization = txt.name.Split('-');

            if (categorization[0].Equals(categorizationCorrect[0]) && categorization[1].Equals(categorizationCorrect[1]) &&
                !categorization[2].Equals(categorizationCorrect[2]))
            {
                {
                    selectedTexturesList.Add(txt);
                }
            }
        }

        return selectedTexturesList;
    }

    public List<Texture2D> GetSameCategoryGeneralTextures(Texture2D tx, List<Texture2D> txList)
    {
        string[] categorizationCorrect = tx.name.Split('-');

        //Add 2 Same Sub-Category Img
        List<Texture2D> selectedTexturesList = new List<Texture2D>();
        foreach (Texture2D txt in txList)
        {
            string[] categorization = txt.name.Split('-');

            if (categorization[0].Equals(categorizationCorrect[0]) &&
                !categorization[1].Equals(categorizationCorrect[1]) &&
                !categorization[2].Equals(categorizationCorrect[2]))
            {
                {
                    selectedTexturesList.Add(txt);
                }
            }
        }

        return selectedTexturesList;
    }

    public List<Texture2D> GetDifferentCategoryTextures(Texture2D tx, List<Texture2D> txList)
    {
        string[] categorizationCorrect = tx.name.Split('-');

        //Add 2 Same Sub-Category Img
        List<Texture2D> selectedTexturesList = new List<Texture2D>();
        foreach (Texture2D txt in txList)
        {
            string[] categorization = txt.name.Split('-');

            if (!categorization[0].Equals(categorizationCorrect[0]) &&
                !categorization[1].Equals(categorizationCorrect[1]) &&
                !categorization[2].Equals(categorizationCorrect[2]))
            {
                {
                    selectedTexturesList.Add(txt);
                }
            }
        }

        return selectedTexturesList;
    }

    //-----------

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

    //-----------

    public List<Texture2D> SelectGivenIcons(string[] textureNames)
    {
        List<Texture2D> textureList = new List<Texture2D>();

        foreach (string txName in textureNames)
        {
            foreach (Texture2D tx in allImgs)
            {
                if (tx.name.Equals(txName))
                {
                    textureList.Add(tx);
                    break;
                }
            }
        }
        return textureList;
    }


    public List<Texture2D> GetTestImageTextures()
    {
        string[] textureNames = {
            "3-Ground-Bicycle-003bicycle",
            "5-Landforms-Mountains-035Mining",
            "6-Construction-Small-011pincer",
            "2-Furniture-Eating-015fridge",
            "6-CarTools-External-035gas station",
            "3-Ground-Truck-030tractor",
            "5-Extraterrestrial-Stars-034falling star",
            "4-Birds-Insects-039centipede",
            "4-Sea-Fish-032flying fish",
            "4-Birds-Birds-012hornbill",
            "3-Ground-Truck-014electric train",
            "2-Furniture-Relaxation-034fireplace",
            "6-CarTools-Internal-019car pedals",
            "2-Kitchen-Electronics-031kitchen tool",
            "4-Land-Forest-009beaver",
            "4-Sea-DeepSea-002coral",
            "4-Birds-Insects-017firefly",
            "2-Furniture-Other-032chest of drawers",
            "2-Kitchen-Cleaning-050dishwasher",
            "2-Furniture-Cleaning-006air conditioner",
            "4-Land-Forest-005sloth",
            "4-Birds-Birds-015kiwi",
            "6-CarTools-Internal-024combustion",
            "5-Plants-Leaves-017monstera leaf",
            "4-Land-Savannah-037alpaca",
            "1-Fruits-Weird-003bananas",
            "4-Land-Savannah-017wallaby",
            "2-Furniture-Relaxation-040television",
            "3-Ground-OnFoot-007cable car",
            "6-Construction-Small-026tool"
        };

        return SelectGivenIcons(textureNames);
    }
}
