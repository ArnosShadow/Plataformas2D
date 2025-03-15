using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaPinchos : MonoBehaviour
{
    [SerializeField] private int da�o = 10;
    [SerializeField] private Player jugador;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugador.TakeDamage(da�o);
        }
    }
}
