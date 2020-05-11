using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "GameData/SceneSet")]
public class SceneSet : ScriptableObject
{
    [SerializeField] string[] sceneNames;

    public bool InProcess{get; private set;} = false;

    static Dummy Instance;

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        var obj = new GameObject("SceneSetController");
        Instance = obj.AddComponent<Dummy>();
        DontDestroyOnLoad(obj);
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);
    }

    public void Load()
    {
        Instance.StartCoroutine(LoadSceneCoroutine());
    }

    public void Unload()
    {
        Instance.StartCoroutine(UnloadSceneCoroutine());
    }

#if !UNITY_WEBGL  
    public IEnumerator LoadSceneCoroutine()
    {
        var ops = new AsyncOperation[sceneNames.Length];
        for (int i = 0; i < ops.Length; i++)
        {
            var op = SceneManager.LoadSceneAsync(sceneNames[i], LoadSceneMode.Additive);
            op.allowSceneActivation = false;
            ops[i] = op;
        }

        yield return new WaitUntil(() =>
        {
            var result = true;
            for (int i = 0; i < ops.Length; i++)
            {
                result = result && ops[i].progress >= 0.9f;
            }
            return result;
        });

        for(int i=0; i<ops.Length; i++)
        {
            ops[i].allowSceneActivation = true;
            yield return null;
        }
    }

    public IEnumerator UnloadSceneCoroutine()
    {
        var ops = new AsyncOperation[sceneNames.Length];
        for (int i = 0; i < ops.Length; i++)
        {
            var op = SceneManager.UnloadSceneAsync(sceneNames[i]);
            ops[i] = op;
        }

        yield return new WaitUntil(() =>
        {
            var result = true;
            for (int i = 0; i < ops.Length; i++)
            {
                result = result && ops[i].isDone;
            }
            return result;
        });
    }
#else
 
   public IEnumerator LoadSceneCoroutine()
    {
        var ops = new AsyncOperation[sceneNames.Length];
        for (int i = 0; i < ops.Length; i++)
        {
            SceneManager.LoadScene(sceneNames[i], LoadSceneMode.Additive);
        }
        yield return null;
    }

    public IEnumerator UnloadSceneCoroutine()
    {
        var ops = new AsyncOperation[sceneNames.Length];
        for (int i = 0; i < ops.Length; i++)
        {
            SceneManager.UnloadSceneAsync(sceneNames[i]);
        }

        yield return null;
    }
#endif
}