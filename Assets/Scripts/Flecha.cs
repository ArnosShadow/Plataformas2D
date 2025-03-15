using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private int da�o = 10;
    void Update()
    {
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(da�o);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Suelo"))
        {
            Destroy(gameObject);
        }
    }
}
