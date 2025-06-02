using UnityEngine;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public PlayerMovement target;

    [Header("Reference")]
    [SerializeField] private Transform referencePoint;

    [Header("Sensitivity & Ranges")]
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private float orbitSpeed = 50f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private Vector2 zoomRange = new Vector2(1.5f, 6f);

    private float _verticalRotation;
    private float _horizontalRotation;
    private float _orbitAngle = 0f;
    private float _zoomDistance = 2f;

    private Vector3 _initialOffset;
    private Quaternion _initialRotation;

    private void Awake()
    {
        if (referencePoint == null)
        {
            Debug.LogError("Reference Point not assigned.");
            enabled = false;
            return;
        }

        _initialOffset = transform.position - referencePoint.position;
        _initialRotation = Quaternion.LookRotation(referencePoint.position - transform.position);
        _zoomDistance = Mathf.Clamp(_initialOffset.magnitude, zoomRange.x, zoomRange.y);
    }

    private void LateUpdate()
    {
        if (target != null && target.IsRoamingEnabled)
        {
            HandleFirstPerson();
        }
        else
        {
            HandleOrbitZoom();
        }
    }

    private void HandleFirstPerson()
    {
        transform.position = target.transform.position;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _verticalRotation -= mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -70f, 70f);
        _horizontalRotation += mouseX;

        transform.rotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0f);
    }

    private void HandleOrbitZoom()
    {
        if (Input.GetMouseButton(1))
        {
            _orbitAngle += Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _zoomDistance -= scroll * zoomSpeed;
        _zoomDistance = Mathf.Clamp(_zoomDistance, zoomRange.x, zoomRange.y);

        Quaternion orbitRotation = Quaternion.Euler(0f, _orbitAngle, 0f);
        Vector3 offset = orbitRotation * _initialOffset.normalized * _zoomDistance;

        transform.position = referencePoint.position + offset;
        transform.rotation = Quaternion.LookRotation(referencePoint.position - transform.position);
    }
}