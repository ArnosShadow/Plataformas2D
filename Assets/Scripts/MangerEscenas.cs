using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MangerEscenas : MonoBehaviour
{
    public static MangerEscenas instance = null;
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

    }
    //Método asociado al click del boton de Inicio
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(0);

    }
    public void OnWinButtonClicked()
    {
        SceneManager.LoadScene(1);

    }
    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
