using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueLikeGame.SpriteSheetAnimation
{
    public class SpriteSheetHandler : MonoBehaviour
    {
        public static SpriteSheetHandler Instance { get; private set; }
        [field:SerializeField] public Mesh QuadMesh { get; private set; }
        [field: SerializeField] public List<Material> WalkingSpriteSheetMaterial { get; private set; }

        private void Awake()
        {
            SetAsSingleton();
        }

        private void SetAsSingleton()
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
