using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void Load(string sceneName, Action onLoaded = null)
    {
        LoadSceneAsync(sceneName, onLoaded).Forget();
    }
    
    private async UniTask LoadSceneAsync(string sceneName, Action onLoaded = null)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        
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