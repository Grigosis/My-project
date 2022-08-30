using UnityEngine;

namespace ROR.Core
{
    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _i;
        public static GameAssets i
        {
            get
            {
                //if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
                return _i;
            }
        }

        public GameAssets()
        {
            _i = this;
        }
        
       [SerializeField] public GameObject pfDamagePopup;
       [SerializeField] public Animation floatingAnimation;
       [SerializeField] public GameObject pfShip;
       [SerializeField] public GameObject mapCell;
    }
}