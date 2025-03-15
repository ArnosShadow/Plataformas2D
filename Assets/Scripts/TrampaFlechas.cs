using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaPinchos1 : MonoBehaviour
{
    [SerializeField] private GameObject flechaPrefab;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float intervaloDisparo = 2f;

    void Start()
    {
        StartCoroutine(DispararFlechas());
    }

    IEnumerator DispararFlechas()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloDisparo);
            Instantiate(flechaPrefab, puntoDisparo.position, Quaternion.identity);
        }
    }
}
