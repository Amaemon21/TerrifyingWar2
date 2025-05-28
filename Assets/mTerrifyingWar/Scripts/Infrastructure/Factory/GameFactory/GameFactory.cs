using System.Collections.Generic;
using UnityEngine;

public class GameFactory : IGameFactory
{
    public List<ISavedProgressReader> ProgressReaders { get; } = new();
    public List<IProgressUpdater> ProgressWriters { get; } = new();
    
    public void CleanUp()
    {
        ProgressReaders.Clear();
        ProgressWriters.Clear();
    }
    
    public void RegisterProgressWatchers(GameObject gameObject)
    {
        foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            Register(progressReader);
    }

    private void Register(ISavedProgressReader progressReader)
    {
        if (progressReader is IProgressUpdater progressWriter)
            ProgressWriters.Add(progressWriter);

        ProgressReaders.Add(progressReader);
        
        Debug.Log($"Registered progress reader: {progressReader}");
    }
}