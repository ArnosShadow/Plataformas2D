using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField]private Transform puntoA, puntoB;
    [SerializeField] private float speed = 2f; // Velocidad del enemigo
    private Vector3 Objetivo;
    private SpriteRenderer spriteRenderer;
    private bool movingToB = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Objetivo = puntoB.position;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, Objetivo, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, Objetivo) < 0.1f)
        {
            if (movingToB)
            {
                Objetivo = puntoA.position;
                movingToB = false;
                spriteRenderer.flipX = false; // Para girar el sprite
            }
            else
            {
                Objetivo = puntoB.position;
                movingToB = true;
                spriteRenderer.flipX = true;
            }
            
        }
    }

    private void OnColliderEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            
        }
    }
}
