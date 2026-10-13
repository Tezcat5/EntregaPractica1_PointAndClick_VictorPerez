using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Información del objeto")]
    public string nombreObjeto = "Objeto";

    [TextArea] public string textoMirar = "No ves nada especial.";
    [TextArea] public string textoHablar = "No parece que pueda responderte.";
    [TextArea] public string textoInteractuar = "No ocurre nada.";

    [Header("Cambio de escena")]
    public string escenaDestino = "";

    [Header("Objeto con dos estados (opcional)")]
    public GameObject objetoEstadoAlternativo;
    public bool estadoAlternativoActivo = false;
    [TextArea] public string textoMirarAlternativo = "";
    [TextArea] public string textoInteractuarAlternativo = "";

    // Solo los botones deberían llamar a este método.
    public void Interactuar(string accion)
    {
        Debug.Log(
            "🔴 Interactuar llamado en " + nombreObjeto +
            ". Acción recibida: " + accion
        );

        switch (accion)
        {
            case "MIRAR":
                MostrarTexto(
                    estadoAlternativoActivo &&
                    !string.IsNullOrEmpty(textoMirarAlternativo)
                    ? textoMirarAlternativo
                    : textoMirar
                );
                break;

            case "HABLAR":
                MostrarTexto(textoHablar);
                break;

            case "INTERACTUAR":
                if (!string.IsNullOrEmpty(escenaDestino))
                {
                    SceneManager.LoadScene(escenaDestino);
                    return;
                }

                if (objetoEstadoAlternativo != null)
                {
                    estadoAlternativoActivo = !estadoAlternativoActivo;

                    objetoEstadoAlternativo.SetActive(
                        estadoAlternativoActivo
                    );

                    MostrarTexto(
                        estadoAlternativoActivo &&
                        !string.IsNullOrEmpty(textoInteractuarAlternativo)
                        ? textoInteractuarAlternativo
                        : textoInteractuar
                    );
                }
                else
                {
                    MostrarTexto(textoInteractuar);
                }
                break;
        }
    }

    private void MostrarTexto(string mensaje)
    {
        if (SistemaDialogo.Instancia != null)
        {
            SistemaDialogo.Instancia.MostrarTexto(mensaje);
        }
        else
        {
            Debug.LogWarning(
                "No se encuentra SistemaDialogo en la escena."
            );
        }
    }
}