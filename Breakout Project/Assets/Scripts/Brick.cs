using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Eye[] eyes = GetComponentsInChildren<Eye>();
        foreach (Eye eye in eyes)
        {
            eye.target = GameObject.FindObjectOfType<Ball>().gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2DABC(Collision2D collision)
    {
        if (collision.collider.GetComponent<Ball>() != null)
        {
            Destroy(gameObject);
        }
    }
}
