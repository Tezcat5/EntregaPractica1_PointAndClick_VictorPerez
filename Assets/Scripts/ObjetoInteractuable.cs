using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Información del objeto")]
    public string nombreObjeto = "Objeto";

    [TextArea]
    public string textoMirar = "No ves nada especial.";

    [TextArea]
    public string textoHablar = "No parece que pueda responderte.";

    [TextArea]
    public string textoInteractuar = "No ocurre nada.";

    [Header("Cambio de escena")]
    public string escenaDestino = "";

    public void Interactuar(string accion)
    {
        switch (accion)
        {
            case "MIRAR":
                MostrarTexto(textoMirar);
                break;

            case "HABLAR":
                MostrarTexto(textoHablar);
                break;

            case "INTERACTUAR":
                if (!string.IsNullOrEmpty(escenaDestino))
                {
                    SceneManager.LoadScene(escenaDestino);
                }
                else
                {
                    MostrarTexto(textoInteractuar);
                }
                break;
        }
    }

    void MostrarTexto(string mensaje)
    {
        if (SistemaDialogo.Instancia != null)
        {
            SistemaDialogo.Instancia.MostrarTexto(mensaje);
        }
        else
        {
            Debug.LogWarning("No se encuentra SistemaDialogo en la escena.");
        }
    }
}