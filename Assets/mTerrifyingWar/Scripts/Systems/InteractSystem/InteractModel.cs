using R3;
using UnityEngine;

public class InteractModel
{
    private readonly ReactiveProperty<string> _text = new();
    private readonly ReactiveProperty<Sprite> _icon = new();
    private readonly ReactiveProperty<bool> _alpha = new();
    
    public Observable<string> Text => _text;
    public Observable<Sprite> Icon => _icon;
    public Observable<bool> Alpha => _alpha;

    public void SetupText(string text)
    {
        _text.Value = text;
    }

    public void SetupIcon(Sprite icon)
    {
        _icon.Value = icon;
    }

    public void Visible(bool visible)
    {
        _alpha.Value = visible;
    }
}
