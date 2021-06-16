using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; 
    public Joystick joystick;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float moveSpeed = 10f;
    public float jumpHeight = 5f;
    public LayerMask groundMask;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Sphere cast to check if on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            // negative better than zero
            velocity.y = -1f;
        }

        // Apply movement from joystick
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;

        Vector3 move = this.transform.right * moveHorizontal + this.transform.forward * moveVertical;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravity simulation
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Make player jump upward
    public void Jump()
    {
        if (isGrounded)
        {
            // up speed = sqrt(desiredHeight * -2 * g), this is physic, bro
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
