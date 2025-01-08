using UnityEngine;

namespace mTerrifyingWar.Scripts.Game.GameRoot
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Transform _uiSceneContainer;
        
        public LoadingScreen LoadingScreen => _loadingScreen;
        
        public void Setup(SceneLoader sceneLoader)
        {
            _loadingScreen.Setup(sceneLoader);
        }

        public void AttachSceneUI(GameObject sceneUI)
        {
            ClearSceneUI();
            
            sceneUI.transform.SetParent(_uiSceneContainer, false);
        }

        private void ClearSceneUI()
        {
            int childCount = _uiSceneContainer.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Destroy(_uiSceneContainer.GetChild(i).gameObject);
            }
        }
    }
}