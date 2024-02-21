using System.Collections;
using UnityEngine;
using Cinemachine;
using RogueLikeGame.Player;

namespace RogueLikeGame.Camera
{
    public class PlayerFollowCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera followCamera;
        private void Awake()
        {
            followCamera = GetComponent<CinemachineVirtualCamera>();
            StartCoroutine(SetToPlayerVisualCor());
        }

        private IEnumerator SetToPlayerVisualCor()
        {
            yield return new WaitUntil(() => PlayerVisual.Instance != null);
            followCamera.m_Follow = PlayerVisual.Instance.transform;
        }
    }

}
