using System.Collections;
using TMPro;
using UnityEngine;

public class SistemaDialogo : MonoBehaviour
{
    public static SistemaDialogo Instancia;

    [Header("Texto del diálogo")]
    public TMP_Text textoDialogo;

    [Header("Velocidad de escritura")]
    public float velocidadEscritura = 0.035f;

    private Coroutine escrituraActual;

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        if (textoDialogo == null)
        {
            Debug.LogError(
                "SistemaDialogo: falta asignar Texto_Dialogo en el Inspector."
            );
            return;
        }

        textoDialogo.text = "";
        Debug.Log("SistemaDialogo preparado correctamente.");
    }

    public void MostrarTexto(string mensaje)
    {
        Debug.Log("SistemaDialogo recibe: " + mensaje);

        if (textoDialogo == null)
        {
            Debug.LogError(
                "SistemaDialogo: Texto_Dialogo no está asignado."
            );
            return;
        }

        if (escrituraActual != null)
        {
            StopCoroutine(escrituraActual);
        }

        escrituraActual = StartCoroutine(EscribirTexto(mensaje));
    }

    private IEnumerator EscribirTexto(string mensaje)
    {
        textoDialogo.text = "";

        foreach (char letra in mensaje)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        escrituraActual = null;
    }
}