using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]private Transform player;
    [SerializeField]private float speed = 2f;
    [SerializeField]private float attackRange = 1.5f;
    [SerializeField]private int maxHealth = 10;
    private int currentHealth;
    private bool segundaFase = false; // Modo segunda fase
    private bool activarse = false; // Se activa el Boss

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (activarse)
        {
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            spriteRenderer.flipX = (player.position.x < transform.position.x);
        }
        else
        {
            StartCoroutine(AttackPlayer());
        }
    }

    IEnumerator AttackPlayer()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f); // Tiempo del ataque
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            //player.GetComponent<PlayerHealth>()?.TakeDamage(segundaFase ? 2 : 1); // Hace más daño si está en modo furia
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Vida del jefe: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (currentHealth <= maxHealth / 2 && !segundaFase)
        {
            EnrageMode();
        }
    }

    void EnrageMode()
    {
        segundaFase = true;
        speed *= 1.5f; // Se mueve más rápido
        Debug.Log("El jefe ha entrado en modo furia!");
        anim.SetBool("IsEnraged", true);
    }

    void Die()
    {
        Debug.Log("Jefe derrotado");
        anim.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activarse = true; // Activa la pelea cuando el jugador entra en el Trigger
            Debug.Log("El jefe ha despertado!");
        }
    }
}
