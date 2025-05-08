using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveCell : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _dateCreatedText;

    [SerializeField] private Button _button;
    
    private string _key;
    private LoadSystem _loadSystem;
    
    public void Setup(LoadSystem loadSystem, string key, string dateCreated)
    {
        _loadSystem = loadSystem;
        
        _key = key;
        _nameText.text = _key;

        _dateCreatedText.text = dateCreated;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadGame);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadGame);
    }

    private void LoadGame()
    {
        _loadSystem.LoadGame(_key);
    }
}