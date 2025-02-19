using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public event Action<float> ProgressChanged;
    
    public void Load(string sceneName, Action onLoaded = null)
    { 
        _ = LoadSceneAsync(sceneName, onLoaded);
    }
    
    private async UniTask LoadSceneAsync(string sceneName, Action onLoaded = null)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            ProgressChanged?.Invoke(progress);
            
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            await UniTask.Yield();
        }

        onLoaded?.Invoke();
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}