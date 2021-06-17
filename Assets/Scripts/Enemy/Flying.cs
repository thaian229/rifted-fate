using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : MonoBehaviour
{
    public LayerMask groundMask;
    public float flyHeight = 6f;
    public float flySpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 50f, groundMask))
        {
            float distanceToGround = Mathf.Abs(this.transform.position.y - hit.point.y);
            
            if (distanceToGround < flyHeight - 0.2f || distanceToGround > flyHeight + 0.2f)
            {
                Vector3 disiredPos = hit.point + new Vector3 (0, flyHeight, 0);
                Vector3 moveDirection = (disiredPos - this.transform.position).normalized;

                this.transform.Translate(moveDirection * flySpeed * Time.deltaTime);
            }
        }
    }
}
