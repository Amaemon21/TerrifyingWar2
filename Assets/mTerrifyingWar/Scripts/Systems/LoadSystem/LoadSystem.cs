using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Zenject;

public class LoadSystem : MonoBehaviour
{
    [Inject] private readonly DiContainer _container;
    [Inject] private readonly IStorageService _storageService;
    [Inject] private readonly GameStateMachine _gameStateMachine;
    
    [SerializeField] private TMP_InputField _nameSaveInputField;
    [SerializeField] private Transform _saveListParent;
    [SerializeField] private SaveCell _saveCellPrefab;
    
    private List<string> _allSaves = new();
    
    private string SavesDirectoryPath =>
#if UNITY_EDITOR
        Application.dataPath + "/SavedData/";
#else
        Application.persistentDataPath + "/";
#endif

    private void Awake()
    {
        RefreshSaveList();
    }
    
    public void CreateNewGame()
    {
        if (string.IsNullOrWhiteSpace(_nameSaveInputField.text))
        {
            Debug.LogWarning("Save name is empty!");
            return;
        }
        
        string saveName = _nameSaveInputField.text;
        
        _storageService.SetupKey(saveName);
        
        _storageService.CreateNewData(() =>
        {
            LoadGame(saveName);
        });
    }
    
    public void LoadGame(string key)
    {
        _storageService.SetupKey(key, () =>
        {
            _gameStateMachine.Enter<LoadGameplayState, string>(Scenes.Gameplay);
        });
    }
    
    private void RefreshSaveList()
    {
        _allSaves.Clear();

        if (!Directory.Exists(SavesDirectoryPath))
            Directory.CreateDirectory(SavesDirectoryPath);

        string[] files = Directory.GetFiles(SavesDirectoryPath, "*.json");

        foreach (var file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            _allSaves.Add(fileName);
        }

        UpdateSaveListUI();
    }
    
    private void UpdateSaveListUI()
    {
        if (_saveListParent == null)
            return;

        foreach (Transform child in _saveListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var saveName in _allSaves)
        {
            string saveFilePath = Path.Combine(SavesDirectoryPath, saveName + ".json");
            string saveDataJson = File.ReadAllText(saveFilePath);
            
            SaveCell saveCell = _container.InstantiatePrefabForComponent<SaveCell>(_saveCellPrefab, _saveListParent);
            
            SaveData _saveData = JsonConvert.DeserializeObject<SaveData>(saveDataJson);
            
            saveCell.Setup(this, saveName, _saveData.CreationDate);
        }
    }
}