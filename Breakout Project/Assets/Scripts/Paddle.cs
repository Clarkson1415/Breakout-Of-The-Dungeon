using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float bounds = 5;
    public float speed = 5;

    void Update()
    {
        Vector2 pos = transform.position;
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= Time.deltaTime * speed;
            if (pos.x < -bounds) pos.x = -bounds;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            pos.x += Time.deltaTime * speed;
            if (pos.x > bounds) pos.x = bounds;
        }
        transform.position = pos;
    }

    
}
