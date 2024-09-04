using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    public GameObject ball;
    public float happy = 0.3f;
    public float sad = -0.3f;
    public float neutral = -0.03f;
    public float faceUpdateSpeed = 1.0f;
    public float happyDistanceThreshold = 7;

    void Update()
    {
        
        Vector3 newScale = transform.localScale;
        if (ball == null) newScale.y = sad;
        else
        {
            float distance = Vector3.Distance(transform.position, ball.transform.position);

            // Ball close to lava.
            if (ball.transform.position.y < -2 && (ball.transform.position.y - transform.position.y < distance * 0.5f ||
                ball.transform.position.y < transform.position.y))
            {
                newScale.y = sad;
            }

            // Ball nearby.
            else if (distance < happyDistanceThreshold)
            {
                newScale.y = happy;
            }

            // Ball far away.
            else
            {
                newScale.y = neutral;
            }
        }
        transform.localScale = Vector3.Lerp(transform.localScale, newScale, faceUpdateSpeed * Time.deltaTime);
    }
}
