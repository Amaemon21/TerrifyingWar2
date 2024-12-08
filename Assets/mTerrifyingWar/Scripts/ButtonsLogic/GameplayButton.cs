using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class GameplayButton : MonoBehaviour, IPointerDownHandler
{
    [Inject] private readonly Game _game;

    public void OnPointerDown(PointerEventData eventData)
    {
        _game.StateMachine.Enter<LoadGameplayState>();
    }
}