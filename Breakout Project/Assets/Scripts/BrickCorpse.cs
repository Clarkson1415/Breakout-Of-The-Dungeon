using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickCorpse : MonoBehaviour
{
    public GameFeedback brickDestroyFeedback;

    void Start ()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddTorque(100);
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Lava")
        {
            yield return new WaitForSeconds(1.0f);
            brickDestroyFeedback?.ActivateFeedback(gameObject);
            Destroy(gameObject);
        }
    }
}
