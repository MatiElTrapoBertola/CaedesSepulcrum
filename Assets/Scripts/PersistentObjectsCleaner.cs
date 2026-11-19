using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObjectsCleaner : MonoBehaviour
{
    private void Start()
    {
        GameObject temp = new GameObject("Temp");
        DontDestroyOnLoad(temp);

        Scene dontDestroyScene = temp.scene;

        Destroy(temp);

        foreach (GameObject obj in dontDestroyScene.GetRootGameObjects())
        {
            Destroy(obj);
        }
    }
}
