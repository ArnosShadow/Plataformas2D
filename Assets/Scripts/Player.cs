using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementForce;
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
    }
    private void OnEnable()
    {
 
        playerInput.actions["Jump"].started += Jump;
        playerInput.actions["Move"].performed += Move;
        playerInput.actions["Move"].canceled += Move;
        playerInput.actions["Attack"].started += Player_started;
        playerInput.deviceLostEvent.AddListener(OnDeviceLost);
        playerInput.deviceRegainedEvent.AddListener(OnDeviceRegainedEvent);
        playerInput.controlsChangedEvent.AddListener(OnControlsChangedEvent);
    }

    private void Player_started(InputAction.CallbackContext obj)
    {
        anim.SetTrigger("Attack");
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
    private void Move(InputAction.CallbackContext ctx)
    {
        Debug.Log("dffd");
        Debug.Log("dffd");
        //Ya viene normalizado
        input = ctx.ReadValue<Vector2>();
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void UpdateMovement(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();

        if (input.magnitude > 0)
        { // Si mueve el personaje 
            anim.SetBool("running", true);
            spriteRenderer.flipX = input.x <= 0;
        }
        else
        {
            anim.SetBool("running", false);
        }
    }

    // Utilizado para las fisicas, solo para continuas
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(input.x, 0, 0) * movementForce, ForceMode2D.Force);
    }

    private void OnDisable()
    {
        //Antes de que me vaya a desactivar me desuscribo del evento.
        playerInput.actions["Jump"].started -= Jump;
        playerInput.actions["Move"].performed -= Move;
        playerInput.actions["Move"].canceled -= Move;
    }
}
