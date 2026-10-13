using UnityEngine;

public class GestorInteracciones : MonoBehaviour
{
    public static GestorInteracciones Instancia;

    public string accionSeleccionada = "MIRAR";

    [HideInInspector]
    public ObjetoInteractuable objetoObjetivo;

    void Awake()
    {
        Instancia = this;
    }

    public void SeleccionarMirar()
    {
        accionSeleccionada = "MIRAR";
        Debug.Log("Acción seleccionada: MIRAR");
    }

    public void SeleccionarHablar()
    {
        accionSeleccionada = "HABLAR";
        Debug.Log("Acción seleccionada: HABLAR");
    }

    public void SeleccionarInteractuar()
    {
        accionSeleccionada = "INTERACTUAR";
        Debug.Log("Acción seleccionada: INTERACTUAR");
    }
}