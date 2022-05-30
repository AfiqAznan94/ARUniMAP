
using UnityEngine;
using UnityEngine.SceneManagement;

public class destscene : MonoBehaviour
{
    public void Scenedest(string destroyscene)
    {

        SceneManager.UnloadSceneAsync(destroyscene);

    }
}
