using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class DeathBarFall : MonoBehaviour
{
    public float MoveSpeed = 1f;
    private bool isActive;
    Vector3 StartPos;
    private void Awake()
    {
        StartPos = transform.position;
        isActive = true;        
    }
    void Update()
    {
        if (PlayerScript.Instance.SimilationStarted)
        {
            float newposition = transform.position.y - MoveSpeed * Time.deltaTime;
            Vector3 newPos = new Vector3(transform.position.x, newposition, transform.position.z);
            transform.position = newPos;
        }
    }
}
