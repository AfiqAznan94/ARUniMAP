
using UnityEngine.SceneManagement;
using UnityEngine;





public class SceneLoad : MonoBehaviour
{
    
    public void Sceneload(string Scenename)
    {
        Debug.Log("hello");
        SceneManager.LoadScene(Scenename);

    }
}
