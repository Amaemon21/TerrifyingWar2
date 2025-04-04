using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SensitivitySettings : MonoBehaviour
{
    [Inject] private readonly PlayerSettingsConfig _playerSettingsConfig;
    
    [SerializeField] private Slider sensitivityXSlider;
    [SerializeField] private TMP_Text _currentXValueText;
    
    [SerializeField] private Slider sensitivityYSlider;
    [SerializeField] private TMP_Text _currentYValueText;

    private void Awake()
    {
        sensitivityXSlider.minValue = 0;
        sensitivityYSlider.minValue = 0;
        
        sensitivityXSlider.maxValue = 5;
        sensitivityYSlider.maxValue = 5;
        
        sensitivityXSlider.value = _playerSettingsConfig.SensitivityX;
        _currentXValueText.text = _playerSettingsConfig.SensitivityX.ToString("F1");
        
        sensitivityYSlider.value = _playerSettingsConfig.SensitivityY;
        _currentYValueText.text = _playerSettingsConfig.SensitivityY.ToString("F1");
    }

    public void ChangedSliderXValue()
    {
        _playerSettingsConfig.SetSensetivityX(sensitivityXSlider.value);
        _currentXValueText.text = _playerSettingsConfig.SensitivityX.ToString("F1");
    }
    
    public void ChangedSliderYValue()
    {
        _playerSettingsConfig.SetSensetivityY(sensitivityYSlider.value);
        _currentYValueText.text = _playerSettingsConfig.SensitivityY.ToString("F1");
    }
}