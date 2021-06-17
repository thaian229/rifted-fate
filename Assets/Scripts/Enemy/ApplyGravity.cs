using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravity : MonoBehaviour
{
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // Sphere cast to check if on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            // negative better than zero
            velocity.y = 0f;
        }

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        this.transform.Translate(velocity * Time.deltaTime);
    }
}
