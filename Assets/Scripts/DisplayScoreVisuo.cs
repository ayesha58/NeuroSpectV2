using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using System.Text;
using System;

public class DisplayScoreVisuo : MonoBehaviour
{

    public Text attentionText;
    public Text recallText;
    public Text visuoText;

    public Text code;

    public Button replayButton;

    //[DllImport("__Internal")]
    //private static extern void InsertData(string tableName, string code,
    //    string age, string race, string gender, string attentionData, string recallData, int attentionScore, int recallScore);

    [DllImport("__Internal")]
    private static extern void InsertData(string tableName, string code,
        string age, string race, string gender, string attentionData, string recallData, string visualData, float attentionScore, float recallScore, float visualScore, string timestamp);

    [DllImport("__Internal")]
    private static extern string GetToken();

    [DllImport("__Internal")]
    private static extern string GetBaseAPIURL();

    [DllImport("__Internal")]
    private static extern void NextScreen();

    [DllImport("__Internal")]
    private static extern void Replay();


    public void StringCallback(string info)
    {
        Debug.Log(info);
    }


    void Start()
    {
        // calculate attention score and recall score
        List<int>[] attentionScoreTotal = ShapeSpawner.returnScore();
        int[] recallScoreTotal = RecallGame.returnScore();
        int visuoScore = VisuoGame.returnScore(); //GenerateShapes.returnScore();

        int attentionScore = 0;
        
        foreach (int num in attentionScoreTotal[0])
        {
            attentionScore += num;
        }

        int recallScore = recallScoreTotal[0];

        if (recallScore > Constants.RECALL_TOTAL_ITERATIONS)
        {
            recallScore = Constants.RECALL_TOTAL_ITERATIONS;
        }

        SetScoreLabels(attentionScore, recallScore, visuoScore);

        // add click listener to Done button
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            replayButton.onClick.AddListener(Replay);
        }

        // track progress: save scores
        if(Constants.INSERT_SCORE_IN_DATABASE)
            InsertDataIntoDB(recallScore, attentionScore, visuoScore);

        SubmitScoreToServer(recallScore, attentionScore);

    }

    private void InsertDataIntoDB(int recallScore, int attentionScore, float visuoScore)
    {
        Debug.Log("Inserting Score to Database");

        System.Random generator = new System.Random();
        string codeStr = generator.Next(100000, 1000000).ToString("D6");

        List<string> attentionData = DataStorage._attentionData;
        List<string> recallData = DataStorage._recallData;
        List<string> visualData = DataStorage._visualData;
        string age = DataStorage._age;
        string race = DataStorage._race;
        string gender = DataStorage._gender;

        string attention = "";
        string recall = "";
        string visuo = "";

        code.text += " " + codeStr;

        foreach (string a in attentionData)
        {
            attention += (a);//(a + "\n");
        }
        foreach (string r in recallData)
        {
            recall += (r);//(r + "\n");
        }
        foreach (string r in visualData)
        {
            visuo += (r);
        }

        //
        string timestamp = DateTime.Now.ToString(@"MM\/dd\/yyyy");

        // Dynamo DB insertion
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            InsertData("neuro-spect-data", codeStr, age, race, gender, attention, recall, visuo, attentionScore, recallScore, visuoScore, timestamp);
        }


        // Dynamo DB insertion
        //if (Application.platform == RuntimePlatform.WebGLPlayer)
        //{
        //    InsertData("userdata", codeStr, age, race, gender, attention, recall, attentionScore, recallScore);
        //}
    }

    private void SubmitScoreToServer(int recallScore, int attentionScore)
    {
        Debug.Log("Submitting score to server");

        string path = GetBaseAPIURL() + "/assessments/neurospect";

        string paramsJsonStr =
            "{ \"recallScore\": " + recallScore
            + ", \"attentionScore\": " + attentionScore
            + " }";

        StartCoroutine(Post(path, paramsJsonStr));
    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + GetToken());
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }

    // Note: old code, not in use
    private void SendScoreToWeb(int recallScore, int attentionScore)
    {

        WWWForm form = new WWWForm();
        form.AddField("recallScore", recallScore);
        form.AddField("attentionScore", attentionScore);

        string path = GetBaseAPIURL() + "/assessments/neurospect";

        UnityWebRequest www = UnityWebRequest.Post(path, form);
        www.SetRequestHeader("Authorization", "Bearer " + GetToken());

        www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Success");
        }

        // Note: Move this to new method
        // Note: commented out, score screen displays for only a split second with this
        // NextScreen(); 
    }

    void SetScoreLabels(int attentionScore, int recallScore, int visuoScore)
    {
        float attentionScorePercent = attentionScore * 100 / Constants.INTERFERENCE_TOTAL_ITERATIONS;
        float recallScorePercent = recallScore * 100 / Constants.RECALL_TOTAL_ITERATIONS;
        float visuoScorePercent = visuoScore * 100 / Constants.VISUOSPATIAL_TOTAL_ITERATIONS;

        // set score values
        //attentionText.text = "Attention:  " + attentionScore + "/100";
        //recallText.text = "Memory:  " + recallScore + "/30";
        //visuoText.text = "Visuospatial:  " + visuoScore + "/15";

        // set score percent values
        attentionText.text = "Attention:  " + attentionScorePercent + "%";
        recallText.text = "Memory:  " + recallScorePercent + "%";
        visuoText.text = "Visuospatial:  " + visuoScorePercent + "%";
    }


}
