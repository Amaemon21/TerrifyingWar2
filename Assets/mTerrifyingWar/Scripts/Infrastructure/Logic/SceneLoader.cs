using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private readonly Coroutines _coroutines;

    public SceneLoader(Coroutines coroutines)
    {
        _coroutines = coroutines;
    }

    public void Load(string sceneName, Action onLoaded = null)
    {
        _coroutines.StartCoroutine(LoadScene(sceneName, onLoaded));
    }

    private IEnumerator LoadScene(string sceneName, Action onLoaded = null)
    {
        if (SceneManager.GetActiveScene().name == sceneName) 
        {
            onLoaded?.Invoke();
            yield break; 
        }

        AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(sceneName);

        while (!waitNextScene.isDone)
        {
            yield return null;
        }

        onLoaded?.Invoke();
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}