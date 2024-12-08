using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void Load(string sceneName, Action onLoaded = null)
    {
        LoadScene(sceneName, onLoaded);
    }

    public async UniTask LoadScene(string sceneName, Action onLoaded = null)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            onLoaded?.Invoke();
            return;
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            Debug.Log($"Loading progress: {asyncOperation.progress * 100}%");
            await UniTask.Yield();
        }

        // Invoke the callback after the scene is loaded
        onLoaded?.Invoke();
    }
}