using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Entities;

namespace RogueLikeGame.UI
{
    public class LosePanel : MonoBehaviour
    {
        [SerializeField] private Button PlayAgainButton;
        private void OnEnable()
        {
            PlayAgainButton.onClick.AddListener(PlayAgain);
        }
        private void OnDisable()
        {
            PlayAgainButton.onClick.RemoveListener(PlayAgain);

        }
        private void PlayAgain()
        {
            var defaultWorld = World.DefaultGameObjectInjectionWorld;
            defaultWorld.EntityManager.CompleteAllTrackedJobs();
            foreach (var system in defaultWorld.Systems)
            {
                system.Enabled = false;
            }
            defaultWorld.Dispose();
            DefaultWorldInitialization.Initialize("Default World", false);
            if (!ScriptBehaviourUpdateOrder.IsWorldInCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld))
            {
                ScriptBehaviourUpdateOrder.AppendWorldToCurrentPlayerLoop(World.DefaultGameObjectInjectionWorld);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }


    }

}
