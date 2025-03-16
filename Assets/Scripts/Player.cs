using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float sprint;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementForce;
    [SerializeField] private Transform checkpoint;
    private int maxSaltos = 2;
    private int saltosRestantes;
    private bool estaCorriendo = false;

    [Header("Estamina")]
    [SerializeField] private float estaminaMaxima = 100f;
    [SerializeField] private float gastoSprintDeEstamina = 10f;
    [SerializeField] private float gastoJumpDeEstamina =15f;
    [SerializeField] private float gastoShieldDeEstamina =5f;
    [SerializeField] private float regeneracionEstamina = 10f;
    private float estaminaActual;
    private bool usandoEscudo = false;

    [Header("Combate")]
    [SerializeField] private float rangoAtaque = 1.5f;
    [SerializeField] private float dañoAtaque = 10f;
    [SerializeField] private LayerMask enemigoLayer;

    [Header("Combos")]
    [SerializeField] private float dañoCombo = 20f;
    [SerializeField] private int golpesParaCompo = 3;


    [Header("Monedas")]
    [SerializeField] private float monedasRecolectadas =0;

    private int contadorCompo = 0;


    private float vida;
    private float vidaMaxima = 100f;

    private PlayerInput playerInput;
    private Vector2 input;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {

        playerInput = GetComponent<PlayerInput>(); 
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        estaminaActual = estaminaMaxima;
        vida = vidaMaxima;
        saltosRestantes = maxSaltos;
    }
    private void OnEnable()
    {

        playerInput.actions["Jump"].started += Jump;

        playerInput.actions["Move"].performed += Move;
        playerInput.actions["Move"].canceled += Move;

        playerInput.actions["Move"].performed += UpdateMovement;
        playerInput.actions["Move"].canceled += UpdateMovement;

        playerInput.actions["Attack"].started += StartAttack;

        playerInput.actions["Correr"].performed += StartSprint;
        playerInput.actions["Correr"].canceled += StopSprint;

        playerInput.actions["Shield"].performed += StartShield;
        playerInput.actions["Shield"].canceled += StopShield;


        playerInput.deviceLostEvent.AddListener(OnDeviceLost);
        playerInput.deviceRegainedEvent.AddListener(OnDeviceRegainedEvent);
        playerInput.controlsChangedEvent.AddListener(OnControlsChangedEvent);

    }


    internal void coleccionarMonedas(float valor)
    {
        monedasRecolectadas += valor;
    }
    private void Update()
    {
        // Gasto de estamina cuando corre
        if (estaCorriendo && estaminaActual > 0)
        {
            estaminaActual -= gastoSprintDeEstamina * Time.deltaTime;
            if (estaminaActual <= 0)
            {
                estaCorriendo = false; // Dejar de correr cuando se agote la estamina
            }
        }

        // Gasto de estamina cuando usa el escudo
        if (usandoEscudo && estaminaActual > 0)
        {
            estaminaActual -= gastoShieldDeEstamina * Time.deltaTime;
            if (estaminaActual <= 0)
            {
                StopShield(new InputAction.CallbackContext()); // Desactiva el escudo cuando se agota la estamina
            }
        }

        // Regeneración de estamina solo si no está usando habilidades
        if (!estaCorriendo && !usandoEscudo && estaminaActual < estaminaMaxima)
        {
            estaminaActual += regeneracionEstamina * Time.deltaTime;
        }
    }

    private void StartSprint(InputAction.CallbackContext ctx)
    {
        if (estaminaActual > 0)
        {
            estaCorriendo = true;
            anim.SetBool("running", true);
        }
    }
    private void StopSprint(InputAction.CallbackContext ctx)
    {
        estaCorriendo = false;
        anim.SetBool("running", false);
    }
    private void StartShield(InputAction.CallbackContext ctx)
    {
        if (!usandoEscudo && estaminaActual >= gastoShieldDeEstamina)
        {
            usandoEscudo = true;
            anim.SetBool("Shield", true);
        }
    }
    private void StopShield(InputAction.CallbackContext ctx)
    {
        usandoEscudo = false;
        anim.SetBool("Shield", false);
    }
    private void StartAttack(InputAction.CallbackContext obj)
    {
        //Realizamos la animacion
        anim.SetTrigger("Attack");
        //Lanzamos un Raycast
        RaycastHit2D golpe = Physics2D.Raycast(transform.position, spriteRenderer.flipX ? Vector2.left : Vector2.right, rangoAtaque, enemigoLayer);
        //si le damos a un enemigo
        
        if (golpe.collider != null)
        {
            contadorCompo++;
            float dañoFinal = dañoAtaque;
            if (contadorCompo >= golpesParaCompo)
            {
                dañoFinal += dañoCombo;
                contadorCompo = 0; 
            }

            if (golpe.collider.GetComponent<Enemigo>())
            {
                Enemigo enemigo = golpe.collider.GetComponent<Enemigo>();
                enemigo.TakeDamage(dañoFinal);
            }
            if (golpe.collider.GetComponent<Boss>())
            {
                Boss boss = golpe.collider.GetComponent<Boss>();
                boss.TakeDamage(dañoFinal);
            }
        }



    }
    private void Move(InputAction.CallbackContext ctx)
    {
        //Ya viene normalizado
        input = ctx.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (saltosRestantes > 0 && estaminaActual >= gastoJumpDeEstamina) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            estaminaActual -= gastoJumpDeEstamina;
            saltosRestantes--;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!usandoEscudo)
        {
            vida -= damage;
            Debug.Log("Vida restante: " + vida);

            if (vida <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        anim.SetTrigger("Muerto");
        Debug.Log("Has Muerto");

        StartCoroutine(EsperarMuerte());
    }

    IEnumerator EsperarMuerte()
    {
        yield return new WaitForSeconds(1f);

        transform.position = checkpoint.position;
        vida = vidaMaxima;
        estaminaActual = estaminaMaxima;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateMovement(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();

        if (input.magnitude > 0)
        { // Si mueve el personaje 
            anim.SetBool("caminar", true);
            spriteRenderer.flipX = input.x <= 0;
        }
        else
        {
            anim.SetBool("caminar", false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            saltosRestantes = maxSaltos;
        }
    }

    // Utilizado para las fisicas, solo para continuas
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x, 0, 0) * movementForce, ForceMode2D.Force);
    }
    private void OnDeviceRegainedEvent(PlayerInput player)
    {
    }
    private void OnDeviceLost(PlayerInput player)
    {
    }
    private void OnControlsChangedEvent(PlayerInput player)
    {
    }
    private void OnDisable()
    {
        //Antes de que me vaya a desactivar me desuscribo del evento.
        playerInput.actions["Jump"].started -= Jump;
        playerInput.actions["Move"].performed -= Move;
        playerInput.actions["Move"].canceled -= Move;
        playerInput.actions["Move"].performed -= UpdateMovement;
        playerInput.actions["Move"].canceled -= UpdateMovement;
        playerInput.actions["Attack"].started -= StartAttack;
        playerInput.actions["Correr"].performed -= StartSprint;
        playerInput.actions["Correr"].canceled -= StopSprint;
        playerInput.actions["Shield"].performed -= StartShield;
        playerInput.actions["Shield"].canceled -= StopShield;

    }

}
