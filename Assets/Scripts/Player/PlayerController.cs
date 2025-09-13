using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public Vector2 curMovementInput;
    public LayerMask groundLayerMask;


    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    private Rigidbody rb;

    [Header("Jump Stamina")]
    [SerializeField] private float jumpStaminaCost = 10f;

    public Action OnBuildModeInput;
    public BuildingMode buildMode;

    //[SerializeField]
    //private EntityModel condition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if(canLook)
        {
            CameraLook();
        }
    }

    private void Move() //�̵�����
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= 5f;
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private void Jump() //��������
    {
        rb.AddForce(Vector2.up * 10f, ForceMode.Impulse);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            Jump();
        }
    }


    private bool IsGrounded() //���� �ִ��� üũ�ؼ� bool������ ��ȯ�ϴ� �Լ�
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1f, groundLayerMask))
            {
                Debug.DrawRay(rays[i].origin, rays[i].direction, Color.red);
                return true;
            }
        }
        return false;
    }




    public void OnBuildButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            buildMode.DestroyPrevObj();
            buildMode.isBuild = !buildMode.isBuild;
        }
    }

    public void OnBuildTryButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if(buildMode.isBuild) 
                buildMode.TryBuild();
        }
    }


}
