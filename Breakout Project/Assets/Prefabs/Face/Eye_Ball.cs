using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye_Ball : MonoBehaviour
{
    public Transform target;
    public Transform eyePivot;
    [SerializeField] public float CloseToTarget = 0.3f; // close eyes when close to hit brick or lava
    [SerializeField] public float AwayFromTarget = -0.3f;
    [SerializeField] public float neutral = -0.03f;
    [SerializeField] public float UpdateSpeed = 1.0f;
    [SerializeField] public float happyDistanceThreshold = 7;

    void Update()
    {
        if (target != null)
            eyePivot.transform.up = target.position - eyePivot.position;


        
        Vector3 newScale = transform.localScale;
        if (target == null) newScale.y = AwayFromTarget;
        else
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Ball close to lava.
            if (target.transform.position.y < -2 && (target.transform.position.y - transform.position.y < distance * 0.5f ||
                target.transform.position.y < transform.position.y))
            {
                newScale.y = CloseToTarget;
            }

            // Ball nearby.
            else if (distance < happyDistanceThreshold)
            {
                newScale.y = CloseToTarget;
            }

            // Ball far away.
            else
            {
                newScale.y = neutral;
            }
        }
        transform.localScale = Vector3.Lerp(transform.localScale, newScale, UpdateSpeed * Time.deltaTime);
    }
}
