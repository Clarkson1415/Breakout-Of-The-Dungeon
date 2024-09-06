using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    public Transform target;
    public Transform eyePivot;

    void Update()
    {
        if (target != null)
            eyePivot.transform.up = target.position - eyePivot.position;
            
    }
}
