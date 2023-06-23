using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;

public class EnemyKamikazi : MonoBehaviour
{
    public GameObject Player;
    public float rotationSpeed = 100f;

    private bool rotateClockwise;

    public Coroutine moveCrouton;

    private Collider2D collider;

    public bool isDead;

    private Animator animator;

    private void Start()
    {
        rotateClockwise = true;
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Rotate the sprite around the specified object
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.smoothDeltaTime);

        // Set the rotation direction based on the current setting
        rotationSpeed = rotateClockwise ? Mathf.Abs(rotationSpeed) : -Mathf.Abs(rotationSpeed);

        // Move towards the player in an arched trajectory

        if (moveCrouton == null)
        {
            moveCrouton = StartCoroutine(MoveTowardsPlayer());
        }

    }

    private IEnumerator MoveTowardsPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = Player.transform.position;
        float distanceToPlayer = Vector3.Distance(startPosition, targetPosition);

        // Determine the maximum height of the arc
        float maxHeight = Mathf.Min(startPosition.y * 2f, targetPosition.y * 2f);

        float t = 0f;
        float speed = 0.5f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            // Calculate the position on the quadratic Bezier curve
            Vector3 position = QuadraticBezierCurve(startPosition, targetPosition, maxHeight, t);

            // Set the y position of the object based on the calculated position
            position.y = startPosition.y + (targetPosition.y - startPosition.y) * t;

            transform.position = position;
            yield return null;
        }

        // Collide with the player
        // ...
    }


    private Vector3 QuadraticBezierCurve(Vector3 p0, Vector3 p2, float height, float t)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0; // (1-t)^2 * P0
        p += 2f * u * t * (p0 + Vector3.up * height); // 2(1-t)t * P1
        p += tt * p2; // t^2 * P2

        return p;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        collider.enabled = false;
        DestroyEnemyShip();
        if (moveCrouton != null)
        {
            StopCoroutine(moveCrouton);
            moveCrouton = null;
        }

    }


    public void DestroyEnemyShip()
    {
        animator.SetTrigger("shipsplode");

        if (!isDead)
        {
            GameMaster.instance.IncrementScore(20);
            isDead = true;

            // this shouldn't even be allowed, but it's happening, live with it.
            Destroy(gameObject.transform.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

}
