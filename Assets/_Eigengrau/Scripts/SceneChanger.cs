using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class SceneChanger : MonoBehaviour
{
    private static SceneChanger _instance;

    public static SceneChanger Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = Instantiate(Resources.Load("Prefabs/SceneChanger") as GameObject);
                _instance = go.GetComponent<SceneChanger>();
            }

            return _instance;
        }
    }
    
    [SerializeField] private int _drawSceneIndex;
    private int _returnScene;
    private int _drawIndex = -1;
    

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
            Destroy(gameObject);
    }

    public void GoToScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
    
    public void BeginDrawing(int drawIndex)
    {
        _drawIndex = drawIndex;
        GameDataLogger.EnterDrawScene(drawIndex);
        _returnScene = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Opening Artboard");
        SceneManager.LoadScene(_drawSceneIndex);
    }

    // Returns the build index of the current drawing to replace,
    // otherwise -1 if not in a drawing state.
    public int GetActiveDrawing()
    {
        return _drawIndex;
    }

    public void EndDrawing()
    {
        _drawIndex = -1;
        SceneManager.LoadScene(_returnScene);
    }

    public static void ClearDrawings()
    {
        string path = Application.persistentDataPath + "/SaveImages";
        DirectoryInfo di = new DirectoryInfo(path);
        if (System.IO.Directory.Exists(path))
        {
            foreach (FileInfo file in di.GetFiles())
                file.Delete();
            foreach (DirectoryInfo dir in di.GetDirectories())
                dir.Delete();
        }
            
        foreach (HotSwap hs in FindObjectsOfType<HotSwap>())
            hs.Hotswap();
    }

    public static void ExitGame()
    {
        GameDataLogger.Instance.BeforeApplicationQuit();
        Application.Quit();
    }
}
