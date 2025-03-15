using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSystem : MonoBehaviour
{
    public static MissionSystem instance = null;

    [Header("Objetivos de la misión")]
    [SerializedField] private int monedasObjetivo = 5;
    [SerializedField] private int enemigosObjetivo = 3;

    private int monedasRecolectadas = 0;
    private int enemigosAsesinados = 0;
    private bool bossDerrotado = false;

    [Header("UI de Misiones")]
    public TextMesh misiones;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        UpdateMissionText();
    }

    public void CollectCoin()
    {
        monedasRecolectadas++;
        UpdateMissionText();
        CheckMissionCompletion();
    }

    public void KillEnemy()
    {
        enemigosAsesinados++;
        UpdateMissionText();
        CheckMissionCompletion();
    }

    public void KillBoss()
    {
        bossDerrotado = true;
        UpdateMissionText();
        CheckMissionCompletion();
    }

    void UpdateMissionText()
    {
        if (misiones != null)
        {
            misiones.text = $"Recoge monedas: {monedasRecolectadas}/{monedasObjetivo}\n" +
                               $"Mata enemigos: {enemigosAsesinados}/{enemigosObjetivo}\n" +
                               $"Mata al Boss: {(bossDerrotado ? "Si" : "No")}";
        }
    }

    void CheckMissionCompletion()
    {
        if (monedasRecolectadas >= monedasObjetivo && enemigosAsesinados >= enemigosObjetivo && bossDerrotado)
        {
            Debug.Log("¡Todas las misiones completadas!");
            // Aquí puedes añadir una recompensa, pasar de nivel, etc.
        }
    }
}
