using System;
using UnityEngine;

namespace mTerrifyingWar.Scripts.Game.Gameplay.Root.View
{
    public class UIGameplayRootBinder : MonoBehaviour
    {
        public event Action GoToMainMenuButtonClick;

        public void HandleGoToMainMenuButtonClick()
        {
            GoToMainMenuButtonClick?.Invoke();
        }
    }
}