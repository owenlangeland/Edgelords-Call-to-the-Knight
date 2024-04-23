using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour : MonoBehaviour
{
    public Animator animator;
    public Transform[] routes;

    private int routeToGo;
    private float tParam;
    private Vector2 batPosition;
    private float speedModifier;
    private bool coroutineAllowed;
    private bool movingForward = true;

    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.3f;
        coroutineAllowed = true;
    }

    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        Debug.Log("Route Started");

        animator.SetBool("isFlying", true);
        coroutineAllowed = false;

        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(2).position;
        Vector2 p3 = routes[routeNumber].GetChild(3).position;

        while ((tParam < 1 && movingForward) || (tParam > 0 && !movingForward))
        {
            if (movingForward)
            {
                tParam += Time.deltaTime * speedModifier;
            }
            else
            {
                tParam -= Time.deltaTime * speedModifier;
            }

            batPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = batPosition;
            yield return new WaitForEndOfFrame();

            if (tParam >= 1 && movingForward)
            {
                tParam = 1;
                movingForward = false;
            }
            else if (tParam <= 0 && !movingForward)
            {
                tParam = 0;
                movingForward = true;
                routeToGo += 1;

                if (routeToGo > routes.Length - 1)
                {
                    routeToGo = 0;
                }
            }
        }

        coroutineAllowed = true;
    }
}
