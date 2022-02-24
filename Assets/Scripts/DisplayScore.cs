using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using System.Text;

public class DisplayScore : MonoBehaviour
{

    public Text attentionText;
    public Text recallText;

    public Text code;

    public Button replayButton;

    [DllImport("__Internal")]
    private static extern void InsertData(string tableName, string code, 
        string age, string race, string gender, string attentionData, string recallData, int attentionScore, int recallScore);
        
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

        int attentionScore = 0;
        foreach(int num in attentionScoreTotal[0])
        {
            attentionScore += num;
        }

        int recallScore = recallScoreTotal[0];

        // update labels
        attentionText.text = "Attention: You're Final Score was " + attentionScore + "/100!";
        recallText.text = "Recall: You're Final Score was " + recallScore + "/30!";

        // add click listener to Done button
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            replayButton.onClick.AddListener(Replay);
        }

        // track progress: save scores
        InsertDataIntoDB(recallScore, attentionScore);
        SubmitScoreToServer(recallScore, attentionScore);

    }

    private void InsertDataIntoDB(int recallScore, int attentionScore)
    {
        Debug.Log("Inserting Score to Database");

        System.Random generator = new System.Random();
        string codeStr = generator.Next(100000, 1000000).ToString("D6");

        List<string> attentionData = DataStorage._attentionData;
        List<string> recallData = DataStorage._recallData;
        string age = DataStorage._age;
        string race = DataStorage._race;
        string gender = DataStorage._gender;

        string attention = "";
        string recall = "";

        code.text += " " + codeStr;

        foreach (string a in attentionData)
        {
            attention += (a + "\n");
        }
        foreach (string r in recallData)
        {
            recall += (r + "\n");
        }

        // Dynamo DB insertion
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            InsertData("userdata", codeStr, age, race, gender, attention, recall, attentionScore, recallScore);
        }
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


}
