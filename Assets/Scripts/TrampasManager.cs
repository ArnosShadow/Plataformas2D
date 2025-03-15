using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampasManager : MonoBehaviour
{
    [Header("Configuración de Trampas")]
    [SerializeField] private GameObject trampaPicosPrefab;
    [SerializeField] private GameObject trampaFlechasPrefab;
    [SerializeField] private Transform[] emptyTrampas; 
    [SerializeField] private float tiempoReaparicion = 5f;

    private List<GameObject> trampasActivas = new List<GameObject>();

    void Start()
    {
        GenerarTrampas();
        StartCoroutine(ReaparecerTrampas());
    }

    void GenerarTrampas()
    {
        foreach (Transform punto in emptyTrampas)
        {
            // Seleccionamos aleatoriamente entre picos o flechas
            GameObject trampaElegida = Random.value > 0.5f ? trampaPicosPrefab : trampaFlechasPrefab;
            GameObject nuevaTrampa = Instantiate(trampaElegida, punto.position, Quaternion.identity);
            trampasActivas.Add(nuevaTrampa);
        }
    }

    IEnumerator ReaparecerTrampas()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoReaparicion);

            foreach (GameObject trampa in trampasActivas)
            {
                Destroy(trampa);
            }
            trampasActivas.Clear();

            GenerarTrampas();
        }
    }
}
