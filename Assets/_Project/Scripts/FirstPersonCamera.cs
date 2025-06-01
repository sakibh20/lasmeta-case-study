using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [HideInInspector] public PlayerMovement target;
    [SerializeField] private float mouseSensitivity = 10f;

    private float _verticalRotation;
    private float _horizontalRotation;
    
    private void LateUpdate()
    {
        if (target == null) return;
        if(!target.IsRoamingEnabled) return;

        transform.position = target.transform.position;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _verticalRotation -= mouseY * mouseSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -70f, 70f);

        _horizontalRotation += mouseX * mouseSensitivity;

        transform.rotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);
    }
}