using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed;
    private Transform currentPoint;
    private Rigidbody2D rb;
    private Animator animator;
    private Coroutine movementCoroutine;

    public Transform player;
    public bool isChasing;
    public float chaseDistance;
    private bool facingLeft = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return; // Check if the GameObject is active

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) < chaseDistance)
            {
                isChasing = true;
            }

            MoveBetweenPoints();
        }
    }

    private void MoveBetweenPoints()
    {
        rb.velocity = (currentPoint == pointB.transform) ? new Vector2(speed, 0) : new Vector2(-speed, 0);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            currentPoint = (currentPoint == pointA.transform) ? pointB.transform : pointA.transform;
            Flip();
        }
    }

    private void ChasePlayer()
    {
        if (transform.position.x > player.position.x)
        {
            if (!facingLeft) // Check if not already facing left
            {
                Flip();
                facingLeft = true; // Update facing direction
            }
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (transform.position.x < player.position.x)
        {
            if (facingLeft) // Check if not already facing right
            {
                Flip();
                facingLeft = false; // Update facing direction

            }
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }


    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private IEnumerator StopThenGo()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        yield return new WaitForSeconds(2);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = (currentPoint == pointA.transform) ? new Vector2(speed, 0) : new Vector2(-speed, 0);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void StopMovement()
    {
        // Stop the enemy's movement
        rb.velocity = Vector2.zero; // Stop rigidbody's velocity
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine); // Stop any ongoing movement coroutine
            movementCoroutine = null;
        }

        this.enabled = false;
    }

    private void OnDisable()
    {
        // Stop any ongoing movement coroutine when the script is disabled
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
            Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
            Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        }
    }
}