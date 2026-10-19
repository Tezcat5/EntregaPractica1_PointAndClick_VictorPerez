using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorPantallaFinal : MonoBehaviour
{
    public GameObject botonVolverInicio; // Arrastra aquí tu botón

    void Start()
    {
        if (botonVolverInicio != null)
        {
            botonVolverInicio.SetActive(false);
            StartCoroutine(MostrarBotoneConRetraso());
        }
    }

    IEnumerator MostrarBotoneConRetraso()
    {
        // Espera 4 segundos antes de mostrar el botón de volver al inicio
        yield return new WaitForSeconds(4f);

        if (botonVolverInicio != null)
        {
            botonVolverInicio.SetActive(true);
        }
    }

    // Función para el OnClick del botón (cámbiala por el nombre de tu escena de menú o pueblo inicial)
    public void VolverAlInicio()
    {
        SceneManager.LoadScene("Menu_Principal"); // Pon aquí el nombre exacto de tu escena inicial
    }
}