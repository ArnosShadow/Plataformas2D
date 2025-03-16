using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Misiones : MonoBehaviour
{
    public static Misiones instance = null;

    [Header("Objetivos de la misión")]
    [SerializeField] private int monedasObjetivo = 5;
    [SerializeField] private int enemigosObjetivo = 3;

    private float monedasRecolectadas = 0;
    private float enemigosAsesinados = 0;
    private bool bossDerrotado = false;

    [Header("UI de Misiones")]
    [SerializeField] private TextMeshProUGUI misiones;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

    }

    void Start()
    {
        Actualizar();
    }

    public void CollectCoin()
    {
        monedasRecolectadas++;
        Actualizar();
        CheckMissionCompletion();
    }

    public void KillEnemy()
    {
        enemigosAsesinados++;
        Actualizar();
        CheckMissionCompletion();
    }

    public void KillBoss()
    {
        bossDerrotado = true;
        Actualizar();
        CheckMissionCompletion();
    }

    void Actualizar()
    {
       
        misiones.text = $"Recoge monedas: {monedasRecolectadas}/{monedasObjetivo}\n" +
                               $"Mata enemigos: {enemigosAsesinados}/{enemigosObjetivo}\n" +
                               $"Mata al Boss: {(bossDerrotado ? "Si" : "No")}";
        
    }

    void CheckMissionCompletion()
    {
        if (monedasRecolectadas >= monedasObjetivo && enemigosAsesinados >= enemigosObjetivo && bossDerrotado)
        {
            Debug.Log("¡Todas las misiones completadas!");
        }
    }
}
