using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public float Progress { get; private set; }
    public event Action<float> OnProgressUpdated;

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    
    public void Load(string sceneName, Action onLoaded = null)
    {
        LoadSceneAsync(sceneName, onLoaded).Forget();
    }
    
    private async UniTask LoadSceneAsync(string sceneName, Action onLoaded = null)
    {
        if (GetSceneName() == sceneName)
        {
            onLoaded?.Invoke();
            return;
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        float currentProgress = 0f;

        while (!asyncOperation.isDone)
        {
            float targetProgress = asyncOperation.progress < 0.9f ? asyncOperation.progress : 1f;
            
            currentProgress = SmoothProgressUpdate(currentProgress, targetProgress);
            await UniTask.Yield();

            if (Mathf.Approximately(currentProgress, 1f) && asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
        }

        onLoaded?.Invoke();
    }
    
    private float SmoothProgressUpdate(float currentProgress, float targetProgress)
    {
        float updatedProgress = Mathf.MoveTowards(currentProgress, targetProgress, Time.deltaTime * 0.5f);
        Progress = updatedProgress;
        OnProgressUpdated?.Invoke(Progress);
        return updatedProgress;
    }
}