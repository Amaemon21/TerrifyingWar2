using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class InternetAccess : MonoBehaviour
{
    [SerializeField] private string[] _uris;

    public IEnumerator TestConnection(Action<bool> callback)
    {
        foreach (var uri in _uris)
        {
            using UnityWebRequest request = UnityWebRequest.Get(uri);
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                callback?.Invoke(true);
                yield break;
            }
        }

        callback?.Invoke(false);
    }
}