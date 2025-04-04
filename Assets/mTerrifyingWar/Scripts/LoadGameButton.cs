using UnityEngine;
using Zenject;

public class LoadGameButton : MonoBehaviour
{
    [Inject] private GameEntryPoint _gameEntryPoint;
    
    public void HandleLoadGameButton()
    {
        _gameEntryPoint.LoadGameplay();
    }
}