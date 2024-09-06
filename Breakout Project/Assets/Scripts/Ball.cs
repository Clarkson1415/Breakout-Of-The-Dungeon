using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    private float ballSpeed = 10;
    private Rigidbody2D rigid;
    private Vector2 direction;
    public GameFeedback surfaceHitFeedback;
    public GameFeedback brickHitFeedback;
    public GameFeedback lavaHitFeedback;
    public GameFeedback paddleHitFeedback;
    public GameFeedback levelWonFeedback;
    public GameFeedback levelLostFeedback;
    public TransitionSettings transition;

    private void OnBallCollision (Collider2D collider)
    {
        surfaceHitFeedback?.ActivateFeedback(gameObject);
        if (collider.transform.tag == "Lava")
        {
            Destroy(gameObject);
            Defeat();
        }
        else if (collider.transform.tag == "Brick")
        {
            Destroy(collider.gameObject);
            brickHitFeedback?.ActivateFeedback(collider.gameObject, null, collider.transform.position);
            GameManager.remainingBricks--;
            if (GameManager.remainingBricks == 0)
            {
                Victory();
            }
        }
        else if (collider.transform.tag == "Paddle")
        {
            paddleHitFeedback?.ActivateFeedback(gameObject);
        }
    }

    /// <summary>
    /// Actions when the player wins.
    /// </summary>
    public void Victory()
    {
        levelWonFeedback?.ActivateFeedback(gameObject);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        TransitionManager.Instance().Transition(SceneManager.GetActiveScene().buildIndex, transition, 2f);
    }

    /// <summary>
    /// Actions when the player loses.
    /// </summary>
    public void Defeat()
    {
        levelLostFeedback?.ActivateFeedback(gameObject);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        TransitionManager.Instance().Transition(SceneManager.GetActiveScene().buildIndex, transition, 2f);
    }
    
    /// <summary>
    /// General logic for handling the ball movement and game collisions.
    /// </summary>
    void Update()
    {
        if (direction.magnitude == 0 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && Time.timeSinceLevelLoad > 1.0f)) 
        {
            direction = new Vector2(1, 1).normalized;
        }

        // Calculate the movement distance for this frame
        float moveDistance = ballSpeed * Time.deltaTime;

        Vector3 newPosition = transform.position;
        int guard = 100;
        while (moveDistance > 0 && guard > 0)
        {
            guard--;
            // Perform the raycast before moving the ball
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, moveDistance);

            if (hit.collider != null)
            {
                // Calculate the distance to the collision point
                float distanceToHit = hit.distance;

                // Move the ball to the collision point
                newPosition = hit.point;

                // Reflect the direction based on the normal of the surface hit
                direction = Vector2.Reflect(direction, hit.normal);

                // Subtract the traveled distance from the remaining move distance
                moveDistance -= distanceToHit;
                transform.up = (newPosition - transform.position);

                // Handle Game Collisions
                OnBallCollision(hit.collider);
                StartCoroutine(DisableCollider(hit.collider));
            }
            else
            {
                // No collision, move the ball the full remaining distance
                newPosition += (Vector3)(direction * moveDistance);
                transform.up = hit.normal;
                break;
            }
        }
        transform.position = newPosition;
    }

    private IEnumerator DisableCollider (Collider2D collider)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        if (collider != null) collider.enabled = true;
    }
}
