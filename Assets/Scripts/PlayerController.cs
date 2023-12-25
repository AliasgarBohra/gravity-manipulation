using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float run_speed = 5;
    [SerializeField] private float rotationSpeed = 100;

    [Header("Jump Fields")]
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private float sphereRadius = 0.1f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float jumpForce = 4;

    [Header("Others")]
    [SerializeField] private Animator[] anim;
    [SerializeField] private GravityManipulator gravityManipulator;

    private Rigidbody rb;
    private Transform cameraTransform;

    private const string RUNNING_BOOL_PARAM = "isRunning";
    private const string FALLING_BOOL_PARAM = "isFalling";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        gravityManipulator.onGravityChanged += OnGravityChanged;
    }
    private void Update()
    {
        JumpController();
        MovePlayer();
    }
    private void OnGravityChanged(GravityManipulator.AXIS axis)
    {
        transform.localRotation = gravityManipulator.hologram_rotation;
        transform.localPosition = gravityManipulator.hologram_position;
    }

    #region Player Controller
    private void MovePlayer()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, transform.up) * direction;
        direction.Normalize();

        Vector3 moveDirection = new Vector3(direction.x * run_speed, rb.velocity.y, direction.z * run_speed);

        bool isMoving = direction != Vector3.zero;

        anim[0].SetBool(RUNNING_BOOL_PARAM, isMoving);
        anim[1].SetBool(RUNNING_BOOL_PARAM, isMoving);

        if (isMoving)
            RotateBody(direction);

        transform.localPosition = transform.localPosition + moveDirection * Time.deltaTime;

        if (rb.velocity.magnitude > 30)
        {
            GameManager.Instance.GameOver();
        }
    }
    private void RotateBody(Vector3 direction)
    {
        Vector3 camForward = direction;
        camForward.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(camForward, transform.up);

        float step = rotationSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, step);
    }
    #endregion

    #region Jump Controller
    private void JumpController()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (isOnGround())
        {
            anim[0].SetBool(FALLING_BOOL_PARAM, false);
            anim[1].SetBool(FALLING_BOOL_PARAM, false);
        }
        else
        {
            anim[0].SetBool(FALLING_BOOL_PARAM, true);
            anim[1].SetBool(FALLING_BOOL_PARAM, true);
        }
    }
    private void Jump()
    {
        if (isOnGround())
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    private bool isOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, sphereRadius, layerMask);
    }
    #endregion
}