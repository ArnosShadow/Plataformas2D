using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private Transform jugadorPosicion;
    [SerializeField] private Player jugador;
    [SerializeField] private LayerMask jugadorLayer;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float maxHealth = 1000f;

    private bool isAttacking = false;
    private float currentHealth;
    private bool segundaFase = false; 
    private bool estaActivado = false;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (estaActivado)
        {
            if (jugadorPosicion == null) return;

            float distancia = Vector2.Distance(transform.position, jugadorPosicion.position);

            if (distancia > attackRange)
            {
                MoverHaciaJugador();
            }
            else
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    // 📌 Método para activar al jefe desde el Trigger
    public void Activar()
    {
        estaActivado = true;
        //anim.SetTrigger("Awaken");
        Debug.Log("El jefe ha despertado!");
    }

    void MoverHaciaJugador()
    {
        Vector2 direccion = (jugadorPosicion.position - transform.position).normalized;
        rb.velocity = new Vector2(direccion.x * speed, rb.velocity.y); 
        
        spriteRenderer.flipX = (jugadorPosicion.position.x < transform.position.x); // Girar hacia el jugador
    }

    IEnumerator AttackPlayer()
    {
        if (isAttacking) yield break;
        isAttacking = true;


        spriteRenderer.flipX = jugadorPosicion.position.x < transform.position.x;

        yield return new WaitForSeconds(0.5f);


        Vector2 origenRaycast = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Vector2 direccionRaycast = (jugadorPosicion.position - (Vector3)origenRaycast).normalized;

        Debug.DrawRay(origenRaycast, direccionRaycast * attackRange, Color.red, 1.0f);

        RaycastHit2D golpe = Physics2D.Raycast(origenRaycast, direccionRaycast, attackRange, jugadorLayer);

        if (golpe.collider != null && golpe.collider.CompareTag("Player"))
        {
            jugador.TakeDamage(segundaFase ? 10 : 5); 
        }

        yield return new WaitForSeconds(1.5f); 
        isAttacking = false;
    }



    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Vida del jefe: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth <= maxHealth / 2 && !segundaFase)
        {
            ActivarSegundaFase();
        }
    }

    void ActivarSegundaFase()
    {
        segundaFase = true;
        speed *= 1.5f; // Aumenta la velocidad en la segunda fase
        Debug.Log("¡El Boss ha entrado en la segunda fase!");
        //anim.SetBool("IsEnraged", true);
    }

    void Die()
    {
        Debug.Log("Jefe derrotado");
        //anim.SetTrigger("Die");
        Destroy(gameObject, 2f);

        //Has ganado el nivel
    }
}
