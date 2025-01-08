using UnityEngine;
using Zenject;

public class MainMenuLoadButton : MonoBehaviour
{
    //[Inject] private readonly Game _game;
    
    public void OnButtonClick()
    {
        //_game.StateMachine.Enter<LoadMainMenuState, string>(Constans.MainMenu);
    }
}