using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] Transform cameraFollowTransform;
    private Camera MainCamera;

    [SerializeField] float RotateSpeed = 20.0f;
    //[SerializeField] [Range(0, 1)] float RotateSpeed = 20.0f;
    //[SerializeField] float RotateSpeed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
    }

    public void UpdateCamera(Vector3 playerPosition, Vector2 moveInput, bool LockCamera)
    {
        transform.position = playerPosition;

        if (!LockCamera)
        {
            transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime * moveInput.x);

        }
        //make the actual camera follow
        MainCamera.transform.position = cameraFollowTransform.position;
        MainCamera.transform.rotation = cameraFollowTransform.rotation;
    }
}
