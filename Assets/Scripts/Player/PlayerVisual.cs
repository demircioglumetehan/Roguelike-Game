using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueLikeGame.Player
{
    public class PlayerVisual : MonoBehaviour
    {
        public static PlayerVisual Instance { get; private set; }
        public void Awake()
        {
            SetAsSingleton();
        }
        public void SetAsSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

}
