using UnityEngine;

namespace SecondCycleGame.Assets._Project.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.1f;

        public void Update()
        {
            if(Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0,0, _speed));
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -_speed));
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3( -_speed, 0, 0));
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(_speed, 0, 0));
            }
        }
    }
}
