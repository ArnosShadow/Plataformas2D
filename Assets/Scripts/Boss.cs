using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform jugadorPosicion;
    [SerializeField] private Player jugador;
    [SerializeField] private LayerMask jugadorLayer;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float maxHealth = 1000f;

    [SerializeField] private Canvas canvasVictoria;

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
        anim = GetComponent<Animator>();
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

    public void Activar()
    {
        estaActivado = true;
        anim.SetBool("Andar", true);
        Debug.Log("El jefe ha despertado!");
    }

    void MoverHaciaJugador()
    {
        Vector2 direccion = (jugadorPosicion.position - transform.position).normalized;
        rb.velocity = new Vector2(direccion.x * speed, rb.velocity.y); 
        
        spriteRenderer.flipX = (jugadorPosicion.position.x < transform.position.x); 
    }

    IEnumerator AttackPlayer()
    {
        if (isAttacking) yield break;
        isAttacking = true;

        anim.SetBool("Andar", false);
        anim.SetTrigger("Golpear");

        spriteRenderer.flipX = jugadorPosicion.position.x < transform.position.x;

        float velocidadAnterior = rb.velocity.x;
        rb.velocity = Vector2.zero;

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

        anim.SetBool("Andar", true);
        rb.velocity = new Vector2(velocidadAnterior, rb.velocity.y);
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
        //anim.SetBool("segundaFase", true);
    }

    void Die()
    {
        Debug.Log("Jefe derrotado");
        //anim.SetTrigger("Die");
        Destroy(gameObject, 1f);
        //Has ganado el nivel
        StartCoroutine(CambiarDeEscena());
    }
    IEnumerator CambiarDeEscena()
    {
        yield return new WaitForSeconds(2f);

        canvasVictoria.enabled = true;
    }
}
