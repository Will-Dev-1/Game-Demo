using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float Speed;
    public float gravity = 20.0f;
    public Transform Cam;
    private Animator anim;
    public float knockbackForce = 10f; // the force to apply to the player when knocked back

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal") * Speed * Time.deltaTime;
        float Vertical = Input.GetAxisRaw("Vertical") * Speed * Time.deltaTime;

        Vector3 Movement = Cam.transform.right * Horizontal + Cam.transform.forward * Vertical;

        Movement.y = 0f;

        if (controller.isGrounded)
        {
            Movement.y = -gravity * Time.deltaTime;
        }
        else
        {
            Movement.y -= gravity * Time.deltaTime;
        }

        // check for enemy collisions
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // apply knockback force to player
                Vector3 knockbackDirection = transform.position - hitCollider.transform.position;
                knockbackDirection.y = 0f;
                knockbackDirection.Normalize();
                controller.Move(knockbackDirection * knockbackForce * Time.deltaTime);
            }
        }

        // move the player
        controller.Move(Movement);

        // rotate the player
        if (Movement.magnitude != 0f)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Cam.GetComponent<cameraController>().sensivity * Time.deltaTime);

            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
        }
    }
}
