using UnityEngine;

public class GestorInteracciones : MonoBehaviour
{
    public static GestorInteracciones Instancia;

    [Header("Detección de objetos cercanos")]
    public Transform jugador;
    public float distanciaBusqueda = 1f;

    [Header("Acción seleccionada")]
    public string accionSeleccionada = "";

    void Awake()
    {
        Instancia = this;
    }

    public void SeleccionarMirar()
    {
        EjecutarAccionCercana("MIRAR");
    }

    public void SeleccionarHablar()
    {
        EjecutarAccionCercana("HABLAR");
    }

    public void SeleccionarInteractuar()
    {
        EjecutarAccionCercana("INTERACTUAR");
    }

    private void EjecutarAccionCercana(string accion)
    {
        if (jugador == null)
        {
            Debug.LogWarning(
                "GestorInteracciones: asigna el jugador en el Inspector."
            );
            return;
        }

        Collider2D[] colliders = FindObjectsByType<Collider2D>(
            FindObjectsSortMode.None
        );

        ObjetoInteractuable objetoCercano = null;
        float distanciaMenor = float.MaxValue;

        foreach (Collider2D col in colliders)
        {
            if (col == null || !col.enabled || !col.gameObject.activeInHierarchy)
                continue;

            ObjetoInteractuable objeto =
                col.GetComponent<ObjetoInteractuable>();

            if (objeto == null)
                objeto = col.GetComponentInParent<ObjetoInteractuable>();

            if (objeto == null)
                continue;

            Vector2 puntoCercano = col.ClosestPoint(jugador.position);

            float distancia = Vector2.Distance(
                jugador.position,
                puntoCercano
            );

            if (distancia < distanciaMenor)
            {
                distanciaMenor = distancia;
                objetoCercano = objeto;
            }
        }

        if (objetoCercano == null || distanciaMenor > distanciaBusqueda)
        {
            Debug.Log("No hay ningún objeto lo bastante cerca para " + accion + ".");
            return;
        }

        accionSeleccionada = accion;

        Debug.Log(
            "Acción " + accion + " sobre " + objetoCercano.nombreObjeto
        );

        objetoCercano.Interactuar(accion);
    }
}