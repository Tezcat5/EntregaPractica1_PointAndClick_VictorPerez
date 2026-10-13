using System.Collections;
using TMPro;
using UnityEngine;

public class SistemaDialogo : MonoBehaviour
{
    public static SistemaDialogo Instancia;

    public TMP_Text textoDialogo;
    public float velocidadEscritura = 0.035f;

    private Coroutine escrituraActual;

    void Awake()
    {
        Instancia = this;
    }

    public void MostrarTexto(string mensaje)
    {
        if (textoDialogo == null)
        {
            Debug.LogWarning("Falta asignar Texto_Dialogo.");
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