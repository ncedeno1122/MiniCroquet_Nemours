using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private BallColor m_BallColor;
    public BallColor BallColor { get => m_BallColor; }

    //[Range(0f, 360f)]
    //public float xzSpin = 0f;
    //[Range(0f, 360f)]
    //public float ySpin = 0f;

    // + + + + | Functions | + + + + 

    // + + + + | Gizmos | + + + + 

    //private void OnDrawGizmos()
    //{
    //    // Draw Forward
    //    //Gizmos.color = Color.blue;
    //    //Gizmos.DrawLine(transform.position, transform.position + (transform.TransformDirection(Vector3.forward)));

    //    // Draw Test Points
    //    float radius = transform.localScale.x + (transform.localScale.x / 4);
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawLine(
    //        new Vector3(
    //            transform.position.x + Mathf.Cos(xzSpin * Mathf.Deg2Rad) * radius,
    //            transform.position.y + Mathf.Sin(ySpin * Mathf.Deg2Rad) * radius,
    //            transform.position.z + Mathf.Sin(xzSpin * Mathf.Deg2Rad) * radius),
    //        transform.position);

    //}

}
