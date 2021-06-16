using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTouchPad : MonoBehaviour
{
    public Transform rotatableH;    // player
    public Transform rotatableV;    // camera
    public Joystick joystick;   // Movement joystick
    public float rotationSpeed = 0.1f;
    public bool invertedV = false;
    public bool clampedV = true;

    private Vector2 currentMousePosition;
    private Vector2 mouseDeltaPosition;
    private Vector2 lastMousePosition;

    public static RotationTouchPad rotationTouchPad;
    [HideInInspector]
    public bool isTouchpadActive;


    // Start is called before the first frame update
    void Start()
    {
        rotationTouchPad = this;
        ResetMousePosition();
    }

    public void ResetMousePosition()
    {
        if (Input.touchCount == 1)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].position.x > Screen.width / 2)
                {
                    currentMousePosition = Input.touches[i].position;
                    break;
                }
            }
        }
        else if (Input.touchCount > 1)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (isInRotateArea(Input.touches[i].position))
                    {
                        currentMousePosition = Input.touches[i].position;
                        break;
                    }
                }
            }
        else
        {
            currentMousePosition = Input.mousePosition;
        }

        lastMousePosition = currentMousePosition;
        mouseDeltaPosition = currentMousePosition - lastMousePosition;
    }

    void LateUpdate()
    {
        if (isTouchpadActive)
        {
            if (Input.touchCount == 1)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.touches[i].position.x > Screen.width / 2)
                    {
                        currentMousePosition = Input.touches[i].position;
                        break;
                    }
                }
            }
            else if (Input.touchCount > 1)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (isInRotateArea(Input.touches[i].position))
                    {
                        currentMousePosition = Input.touches[i].position;
                        break;
                    }
                }
            }
            else
            {
                currentMousePosition = Input.mousePosition;
            }
            mouseDeltaPosition = currentMousePosition - lastMousePosition;

            // Rotate player and camera
            if (rotatableH != null)
                rotatableH.transform.Rotate(0f, mouseDeltaPosition.x * rotationSpeed, 0f);
            if (rotatableV != null)
            {
                if (!invertedV)
                {
                    rotatableV.transform.Rotate(Mathf.Clamp(mouseDeltaPosition.y * (rotationSpeed * -1), -3, 3), 0f, 0f);
                }
                else
                {
                    rotatableV.transform.Rotate(Mathf.Clamp(mouseDeltaPosition.y * rotationSpeed, -3, 3), 0f, 0f);
                }

                if (clampedV)
                {
                    // clamp the camera rotation, avoid back flip bro :v
                    float limitedXRot = rotatableV.transform.localEulerAngles.x;
                    if (limitedXRot > 90f && limitedXRot < 280f)
                    {
                        if (limitedXRot < 180f)
                            limitedXRot = 90f;
                        else
                            limitedXRot = 280f;

                    }
                    rotatableV.transform.localEulerAngles = new Vector3(limitedXRot, rotatableV.transform.localEulerAngles.y, rotatableV.transform.localEulerAngles.z);
                }
            }

            lastMousePosition = currentMousePosition;
        }
    }

    // weather touch position is in designated area on the phone's screen
    private bool isInRotateArea(Vector2 position)
    {
        if (position.x > Screen.width * 0.35f && position.x < Screen.width * 0.7f)
        {
            return true;
        }
        else if (position.x >= Screen.width * 0.7f)
        {
            if (position.y > Screen.height / 3)
            {
                return true;
            }
        }
        return false;
    }

    public void ActivateTouchpad()
    {
        ResetMousePosition();
        isTouchpadActive = true;
    }

    public void DeactivateTouchpad()
    {
        isTouchpadActive = false;
    }
}
