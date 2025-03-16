using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemigo : MonoBehaviour
{


    [Header("Movimiento")]
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;
    [SerializeField] private float speed = 2f;

    [Header("Caracteristicas")]
    [SerializeField] private float vida = 100f;
    [SerializeField] private float daño = 10f;
    [SerializeField] private float rangoAtaque = 1.5f;
    [SerializeField] private LayerMask jugadorLayer;


    private Vector3 Objetivo;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator anim;
    private bool movingToB = true;
    private bool isAttacking = false;
    private Rigidbody2D rb;
    [SerializeField] private Misiones misiones;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Objetivo = puntoB.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isAttacking)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, Objetivo, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, Objetivo) < 0.1f)
        {
            if (!movingToB)
            {
                Objetivo = puntoA.position;
                movingToB = true;
                spriteRenderer.flipX = true;
            }
            else
            {
                Objetivo = puntoB.position;
                movingToB = false;
                spriteRenderer.flipX = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(AttackPlayer(collision.collider.GetComponent<Player>()));
        }
    }

    public void TakeDamage(float daño)
    {
        vida -= daño;
        Debug.Log("El enemigo tiene una vida de: " + vida);
        if (vida <= 0)
        {
            Destroy(gameObject);
            misiones.KillEnemy();
        }
    }

    IEnumerator AttackPlayer(Player jugador)
    {
        if (jugador != null && !isAttacking)
        {
            isAttacking = true;

            anim.SetTrigger("Golpear");

            float velocidadAnterior = rb.velocity.x;
            rb.velocity = Vector2.zero; 

            yield return new WaitForSeconds(0.5f); 

            Vector2 direccionAtaque = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            RaycastHit2D golpe = Physics2D.Raycast(transform.position, direccionAtaque, rangoAtaque, jugadorLayer);

            if (golpe.collider != null && golpe.collider.CompareTag("Player"))
            {
                jugador.TakeDamage(daño);
                Debug.Log("El enemigo golpeó al jugador.");
            }

            yield return new WaitForSeconds(1.0f);

            isAttacking = false;
            rb.velocity = new Vector2(velocidadAnterior, rb.velocity.y);
        }
    }

}
