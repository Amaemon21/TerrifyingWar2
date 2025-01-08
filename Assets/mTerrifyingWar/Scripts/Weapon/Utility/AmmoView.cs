using DG.Tweening;
using TMPro;
using UnityEngine;

public class AmmoView : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private TMP_Text _ammoNameText;
    [SerializeField] private TMP_Text _weaponNameText;
    
    [SerializeField] private float _scaleFactor = 0.8f;
    [SerializeField] private float _duration = 0.2f;
    
    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = _ammoText.transform.localScale;
    }

    public void DisplayAmmo(int currentAmmo, int availableAmmo, Weapon weapon)
    {
        string AvailableAmmo = null;
        string CurrentAmmo = null;

        if (currentAmmo > 0)
        {
            CurrentAmmo = $"<color=#E78300>{currentAmmo}</color> / ";
        }
        else if (currentAmmo <= 0)
        {
            weapon.ResetCurrentAmmo();
            CurrentAmmo = $"<color=#E78300>{currentAmmo}</color> / ";
        }

        if (availableAmmo > 0)
        {
            AvailableAmmo = $"<color=#E78300>{availableAmmo}</color>";
        }
        else if (availableAmmo <= 0)
        {
            weapon.ResetAvailableAmmo();
            AvailableAmmo = $"<color=#E78300>{availableAmmo}</color>";
        }

        _ammoNameText.text = weapon.WeaponInventoryItemConfig.AmmoID;
        _ammoText.text = CurrentAmmo + AvailableAmmo;
        _weaponNameText.text = weapon.WeaponInventoryItemConfig.ItemName;
    }

    public void PlayShootAnimation()
    {
        _ammoText.transform.DOScale(_originalScale * _scaleFactor, _duration).OnComplete(() =>
            {
                _ammoText.transform.DOScale(_originalScale, _duration);
            });
    }
}