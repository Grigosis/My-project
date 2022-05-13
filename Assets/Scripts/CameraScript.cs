using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject choosenCamera;
    private Vector3 cameraStartPosition;

    private void Start()
    {
        choosenCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraStartPosition = choosenCamera.transform.position;
    }

    void Update()
    {
        choosenCamera.transform.position = cameraStartPosition+transform.position;
        ZoomCam();
    }
    
    private void ZoomCam()
    {
        Camera localCam = choosenCamera.GetComponent<Camera>();
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && localCam.fieldOfView<100)
        {
            choosenCamera.GetComponent<Camera>().fieldOfView += 10f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && localCam.fieldOfView > 10)
        {
            choosenCamera.GetComponent<Camera>().fieldOfView -= 10f;
        }
    }
}
