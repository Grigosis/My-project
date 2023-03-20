using UnityEngine;

namespace SecondCycleGame
{
    /// <summary>
    /// Easy but slow
    /// </summary>
    public class LookAt : MonoBehaviour
    {
        public bool LookAtCamera = true;
        public GameObject LookAtObject;
        
        
        // Update is called once per frame
        void Update()
        {
            
            if (LookAtCamera)
            {
                Look(Camera.main.transform);
                //gameObject.transform.LookAt(Camera.main.transform);
            }
            else
            {
                if (LookAtObject != null)
                {
                    Look(LookAtObject.transform);
                }
            }
            
        }

        private void Look(Transform at)
        {
            var targetPosition = at.position;
            var currentPosition = transform.position;
            targetPosition.x = 0;
            currentPosition.x = 0;
            transform.rotation = Quaternion.LookRotation(-targetPosition + currentPosition, Vector3.up);
        }
    }
}
