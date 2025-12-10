using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    // Character stats
    public float MouseSensitivity = 10f;
    public float WalkSpeed = 10f;
    public float JumpPower = 7f;
    public Rigidbody RB;

    private Transform cam;

    // Ground check via collision list
    public List<GameObject> Floors = new List<GameObject>();

    // cached input & jump flag
    private Vector3 input;
    private bool jumpPressed = false;

    void Awake()
    {
        if (RB == null)
            RB = GetComponent<Rigidbody>();

        // Attempt to auto-grab main camera transform
        cam = Camera.main != null ? Camera.main.transform : null;
    }

    void Update()
    {
        if (cam == null || RB == null)
            return;

        // Get input each frame
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        input = new Vector3(inputX, 0f, inputZ);

        // Jump input (Space)
        if (Input.GetKeyDown(KeyCode.Space) && OnGround())
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        if (cam == null || RB == null)
            return;

        // --- CAMERA-RELATIVE MOVE DIRECTION ---

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * input.z + camRight * input.x;

        if (moveDir.sqrMagnitude > 1f)
            moveDir.Normalize();

        // --- BUILD VELOCITY ---

        // Start from current velocity so gravity keeps working
        Vector3 velocity = RB.linearVelocity;

        // Horizontal (XZ) from camera-relative direction
        velocity.x = moveDir.x * WalkSpeed;
        velocity.z = moveDir.z * WalkSpeed;

        // Jump (Y)
        if (jumpPressed && OnGround())
        {
            // Classic �set Y to JumpPower�
            velocity.y = JumpPower;
        }

        RB.linearVelocity = velocity;
        jumpPressed = false; // consume jump

        // --- ROTATION TO FACE MOVE DIRECTION ---
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                MouseSensitivity * Time.fixedDeltaTime
            );
        }
    }

    public bool OnGround()
    {
        return Floors.Count > 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Floors.Contains(other.gameObject))
            Floors.Add(other.gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        if (Floors.Contains(other.gameObject))
            Floors.Remove(other.gameObject);
    }
}
