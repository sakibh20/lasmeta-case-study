using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : NetworkBehaviour
{
    private Camera _camera;

    private Vector3 _velocity;
    private bool _jumpPressed;

    private CharacterController _controller;

    [FormerlySerializedAs("PlayerSpeed")] [SerializeField] private float playerSpeed = 2f;

    [FormerlySerializedAs("JumpForce")] [SerializeField] private float jumpForce = 5f;
    [FormerlySerializedAs("GravityValue")] [SerializeField] private float gravityValue = -9.81f;

    [SerializeField] private bool isRoamingEnabled;
    public bool IsRoamingEnabled => isRoamingEnabled;
    private CameraController _firstPersonCamera;
    
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(!isRoamingEnabled) return;
        
        if (_controller.isGrounded)
        {
            _velocity = new Vector3(0, -1, 0);
        }

        Quaternion cameraRotationY = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * playerSpeed;

        _velocity.y += gravityValue * Runner.DeltaTime;
        if (_jumpPressed && _controller.isGrounded)
        {
            _velocity.y += jumpForce;
        }
        _controller.Move(move + _velocity * Runner.DeltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        _jumpPressed = false;
    }
    
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _camera = Camera.main;
            if(_camera == null) return;
            _camera.GetComponent<CameraController>().target = this;
            _initialPosition = _camera.transform.localPosition;
            _initialRotation = _camera.transform.localRotation;
        }
    }

    private void UpdateRoamingAbility(bool value)
    {
        isRoamingEnabled = value;
    }

    [ContextMenu("BackToThirdPersonView")]
    public void BackToThirdPersonView()
    {
        isRoamingEnabled = false;
        _camera.transform.SetLocalPositionAndRotation(_initialPosition, _initialRotation);
    }
    
    
    [ContextMenu("BackToFirstPersonView")]
    public void BackToFirstPersonView()
    {
        isRoamingEnabled = true;
    }
}