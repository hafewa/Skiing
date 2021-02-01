using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class qqq : MonoBehaviour
{
    public Rigidbody rg;

    public Collider collider;

    // Use this for initialization
    void Start()
    {
        rg = transform.GetComponent<Rigidbody>();
        collider = transform.GetComponent<Collider>();
    }


    public int force = 3; //力的变化大小，可以自己设置

    public int jumpForce = 3; //力的变化大小，可以自己设置

    public float dynamicFriction = 0.0312f;

    // Update is called once per frame
    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");

        float h = Input.GetAxis("Horizontal"); //得到键盘左右控制

        if (rg != null)
        {
            float y = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                y = jumpForce;
                Debug.Log("Jump!");
            }

            Debug.Log("Horizontal: " + h);
            
            if (collider != null)
            {
                collider.material.dynamicFriction = (h != 0 ? 0.1f : dynamicFriction);
            }

            rg.AddForce(h * force, y, 0);
        }
    }
}