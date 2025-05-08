using R3;

public class ExperienceModel
{
    private readonly IStorageService _storageService;
    
    private readonly ReactiveProperty<int> _experience = new();
    private readonly ReactiveProperty<int> _level = new();
    
    private int _experienceToNextLevel;
    private SaveData _saveData;
    
    public Observable<int> Experience => _experience;
    public Observable<int> Level => _level;
    public int ExperienceToNextLevel => _experienceToNextLevel;
    
    public ExperienceModel(IStorageService storageService)
    {
        _storageService = storageService;
        _storageService.Load(LoadSaveData);
    }

    public void AddExperience(int amount)
    {
        if (amount < 0)
            return;
        
        _experience.Value += amount;
        
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (_experience.Value >= _experienceToNextLevel)
        {
            _level.Value++;
            _experience.Value -= _experienceToNextLevel;
            _experienceToNextLevel = CalculateExperienceToNextLevel();
        }
    }

    private int CalculateExperienceToNextLevel()
    {
        return _level.Value * 100 + 100;
    }

    private void LoadSaveData(SaveData _saveData)
    {
        this._saveData = _saveData;
        
        //_level.Value = _saveData.PlayerData.Level;
        //_experience.Value = _saveData.PlayerData.Experience;
        //_experienceToNextLevel = CalculateExperienceToNextLevel();
    }
}