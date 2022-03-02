using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour
{
    public void onClickEnc()
    {
        SceneManager.LoadScene((int)Constants.SCENES.INTERFERENCE_INSTRUCTIONS);//5
    }

    public void onClickAttention()
    {
        SceneManager.LoadScene((int) Constants.SCENES.RECALL_INSTRUCTIONS);//8
    }

    public void onClickRecall()
    {
        SceneManager.LoadScene((int)Constants.SCENES.VISUO_INSTRUCTIONS);//10
    }
}
