using mTerrifyingWar.Scripts.Game.Gameplay.Root.View;
using mTerrifyingWar.Scripts.Game.GameRoot;
using UnityEngine;

namespace mTerrifyingWar.Scripts.Game.Gameplay.Root
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;

        public void Run(UIRootView uiRoot)
        {
            UIGameplayRootBinder uiScene= Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);
        }
    }
}