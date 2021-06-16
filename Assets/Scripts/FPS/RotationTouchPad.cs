using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTouchPad : MonoBehaviour
{
    public Transform RotatableH;    // player
    public Transform RotatableV;    // camera
    public Joystick joystick;   // Movement joystick
    public float RotationSpeed = .1f;
    public bool InvertedV = false;
    public bool ClampedV = true;

    private Vector2 currentMousePosition;
    private Vector2 mouseDeltaPosition;
    private Vector2 lastMousePosition;

    public static RotationTouchPad rotationTouchPad;
    [HideInInspector]
    public bool istouchpadactive;


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
                    if (Input.touches[i].position.x > Screen.width / 2 && Input.touches[i].position.y > Screen.height / 2.5f)
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
        if (istouchpadactive)
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
                    if (Input.touches[i].position.x > Screen.width / 2 && Input.touches[i].position.y > Screen.height / 2.5f)
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

            // Rotate
            if (RotatableH != null)
                RotatableH.transform.Rotate(0f, mouseDeltaPosition.x * RotationSpeed, 0f);
            if (RotatableV != null)
            {
                if (!InvertedV)
                {
                    RotatableV.transform.Rotate(Mathf.Clamp(mouseDeltaPosition.y * (RotationSpeed * -1), -3, 3), 0f, 0f);
                }
                else
                {
                    RotatableV.transform.Rotate(Mathf.Clamp(mouseDeltaPosition.y * RotationSpeed, -3, 3), 0f, 0f);
                }

                if (ClampedV)
                {
                    float limitedXRot = RotatableV.transform.localEulerAngles.x;
                    if (limitedXRot > 90f && limitedXRot < 280f)
                    {
                        if (limitedXRot < 180f)
                            limitedXRot = 90f;
                        else
                            limitedXRot = 280f;

                    }
                    RotatableV.transform.localEulerAngles = new Vector3(limitedXRot, RotatableV.transform.localEulerAngles.y, RotatableV.transform.localEulerAngles.z);
                }
            }

            lastMousePosition = currentMousePosition;
        }
    }

    public void ActivateTouchpad()
    {
        ResetMousePosition();
        istouchpadactive = true;
    }

    public void DeactivateTouchpad()
    {
        istouchpadactive = false;
    }
}
