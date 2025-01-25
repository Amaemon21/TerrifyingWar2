using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(UniqueId))]
public class UniqueIdEditor : Editor
{
    private void OnEnable()
    {
        UniqueId uniqueId = (UniqueId)target;

        if (IsPrefab(uniqueId))
            return;
        
        if (string.IsNullOrEmpty(uniqueId.Id))
        {
            Generate(uniqueId);
        }
        else
        {
            UniqueId[] ids = FindObjectsByType<UniqueId>(FindObjectsSortMode.None);

            if (ids.Any(other => other != uniqueId && other.Id == uniqueId.Id))
            {
                Generate(uniqueId);
            }
        }
    }

    private void Generate(UniqueId uniqueId)
    {
        string id = $"{uniqueId.gameObject.scene.name}_{Guid.NewGuid().ToString()}";
        
        uniqueId.SetupId(id);

        if (!Application.isPlaying)
        {
            EditorUtility.SetDirty(uniqueId);
            EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
        }
    }

    private bool IsPrefab(UniqueId uniqueId) => uniqueId.gameObject.scene.rootCount == 0;
}