using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void Load(string sceneName, Action onLoaded = null)
    {
        LoadSceneAsync(sceneName, LoadSceneMode.Single, onLoaded).Forget();
    }

    public void LoadAdditive(string sceneName, Action onLoaded = null)
    {
        LoadSceneAsync(sceneName, LoadSceneMode.Additive, onLoaded).Forget();
    }

    private async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode, Action onLoaded = null)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);

        while (!asyncOperation.isDone)
        {
            await UniTask.Yield();
        }

        onLoaded?.Invoke();
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}