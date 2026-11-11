using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public Vector3 cameraOffset = new Vector3(0, 8, -6);

    private Rigidbody rb;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Application.runInBackground = true;
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (GameManager.Instance != null && !GameManager.Instance.isGameActive.Value)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool jumpPressed = Input.GetButtonDown("Jump");

        MoveServerRpc(moveX, moveZ, jumpPressed);
    }

    [ServerRpc]
    private void MoveServerRpc(float moveX, float moveZ, bool jump)
    {
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        if (jump && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        Camera.main.transform.position = transform.position + cameraOffset;
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
