using System.Collections.Generic;
using UnityEngine;

public interface IGameFactory
{
    public List<ISavedProgressReader> ProgressReaders { get; }
    public List<ISavedProgress> ProgressWriters { get; }

    public void RegisterProgressWatchers(GameObject gameObject);
    
    public void CleanUp();
}