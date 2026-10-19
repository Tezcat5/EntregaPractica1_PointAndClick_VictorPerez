using UnityEngine;
using TMPro; // Si usas TextMeshPro

public class UIMochila : MonoBehaviour
{
    public GameObject panelMochila;
    public TextMeshProUGUI textoListaObjetos; // O 'Text' de UnityEngine.UI si no usas TextMeshPro

    public void ToggleMochila()
    {
        if (panelMochila == null) return;

        // Invierte la visibilidad del panel (abre si está cerrado, cierra si está abierto)
        bool estado = !panelMochila.activeSelf;
        panelMochila.SetActive(estado);

        if (estado)
        {
            ActualizarLista();
        }
    }

    public void ActualizarLista()
    {
        if (textoListaObjetos == null) return;

        textoListaObjetos.text = "";

        if (Inventario.Instance == null || Inventario.Instance.objetos.Count == 0)
        {
            textoListaObjetos.text = "- Mochila vacía";
        }
        else
        {
            foreach (string obj in Inventario.Instance.objetos)
            {
                textoListaObjetos.text += "• " + obj + "\n";
            }
        }
    }
}