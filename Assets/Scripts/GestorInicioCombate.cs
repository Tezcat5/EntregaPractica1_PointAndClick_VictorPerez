using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GestorInicioCombate : MonoBehaviour
{
    [Header("Configuración de la Transición")]
    public GameObject panelTransicionVS; // Arrastra aquí tu Panel_VS de la jerarquía
    public float duracionTransicion = 3f; // Tiempo que dura la pantalla de VS

    // Este método es el que llamaremos desde tu diálogo con Javier
    public void IniciarSecuenciaTransicion()
    {
        StartCoroutine(RutinaTransicion());
    }

    IEnumerator RutinaTransicion()
    {
        // 1. Activa el panel con tu imagen
        if (panelTransicionVS != null)
        {
            panelTransicionVS.SetActive(true);
        }

        // 2. Espera el tiempo configurado (3 segundos por defecto)
        yield return new WaitForSeconds(duracionTransicion);

        // 3. Carga la escena de combate
        SceneManager.LoadScene("Escena_Combate");
    }
}