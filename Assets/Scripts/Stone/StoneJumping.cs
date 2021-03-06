﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneJumping : MonoBehaviour
{

    private Rigidbody rb;
    private float gravity = 9.82f;
    private float pi = 3.14f;
    private bool isFalling;
    private float speedZ;
    private float speedY;

    void Start()
    {
        Debug.Log("Start Jumping");
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {

        if (isFalling == false)
        {
            isFalling = true;
            Vector3 v = rb.velocity;
            v.y = speedY;
            v.z = speedZ;
            rb.velocity = v;
            print("V(Nuoc) = " + rb.velocity);
        }

    }

    public void move(float Vx, float Vy, float Vz, float angleThrown)
    {
        //rb.velocity = new Vector3(Vx, Vy, Vz);
        transform.Rotate(0, angleThrown, 0);
        Debug.Log("Goc theo Y: " + angleThrown);
        Debug.Log("Tan doan ket: " + Mathf.Tan(angleThrown * 3.14f / 180f));
        Debug.Log("Vx: " + Vz * Mathf.Tan(angleThrown * 3.14f / 180f));
        if (((angleThrown >= 0) && (angleThrown <= 90)) || ((angleThrown >= 270) && (angleThrown <= 360)))
        {
            rb.velocity = new Vector3(Vz * Mathf.Tan(angleThrown * 3.14f / 180f), Vy, Vz);
        }
        else
        {
            rb.velocity = new Vector3(-Vz * Mathf.Tan(angleThrown * 3.14f / 180f), Vy, -Vz);
        }
    }

    public bool conditionToJump(float Vz, float firingAngle)
    {
        float TS = Mathf.Sqrt((16 * (rb.mass / 1000) * gravity) / (pi * 1000));
        float y = 8 * (rb.mass / 1000) * Mathf.Tan(firingAngle * Mathf.Deg2Rad) * Mathf.Tan(firingAngle * Mathf.Deg2Rad);
        float z = pi * 1000 * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float MS = Mathf.Sqrt(1 - y / z);

        float result = 1000 * TS / MS;
        Debug.Log("Vmin = " + result + " - Vz =  " + Vz);
        if (Mathf.Abs(Vz) > result)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float speedZDown(float speedPre)
    {
        float downConstant = -1.4f * 0.13f * gravity * 2f * 10;
        float speedAfter = Mathf.Sqrt(downConstant + speedPre * speedPre);
        return speedAfter;
    }

    public float speedYDown(float speedZAfter, float firingAngle)
    {
        return speedZAfter * Mathf.Tan(firingAngle * Mathf.Deg2Rad);
    }

    void OnTriggerEnter(Collider other)
    {
        print("Collider: " + other.gameObject.name);

        if (other.gameObject.name == "Rain (SplashZone)")
        {
            if (conditionToJump(rb.velocity.z, 10f) == true)
            {
                Debug.Log(other.transform.position);
                if (rb.velocity.z < 0) {
                    speedZ = -(speedZDown(Mathf.Abs(rb.velocity.z)));
                    speedY = speedYDown(-speedZ, 10f);
                }
                else
                {
                    speedZ = speedZDown(rb.velocity.z);
                    speedY = speedYDown(speedZ, 10f);

                }
                isFalling = false;
            }
            else
            {
                isFalling = true;
            }
        }

        if (other.gameObject.name == "Terrain")
        {
            isFalling = true;
            speedY = -speedYDown(speedZ, 10f);
        }

        else if (
            other.gameObject.name == "In Front Of Lake"
            || other.gameObject.name == "Ground 0"
            || other.gameObject.name == "Ground 1"
            || other.gameObject.name == "Ground 2"
            || other.gameObject.name == "Ground 3"
            || other.gameObject.name == "Ground 4"
            || other.gameObject.name == "Ground 5"
            || other.gameObject.name == "Side 1"
            || other.gameObject.name == "Side 2"
            || other.gameObject.name == "Side 3"
            || other.gameObject.name == "Side 4"
            )
        {
            isFalling = false;
            speedZ = -speedZDown(rb.velocity.z);
            speedY = speedYDown(speedZ, 10f);
        }
    }

}
