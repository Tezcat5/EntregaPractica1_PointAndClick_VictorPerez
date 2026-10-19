using UnityEngine;

public class PuntoEntradaJugador : MonoBehaviour
{
    public string nombrePunto = "Escaleras"; // Identificador de este punto (ej: "Escaleras", "PuertaEntrada")

    void Start()
    {
        // Si venimos de un cambio de escena que especificó este punto
        if (ObjetoInteractuable.puntoEntradaDestino == nombrePunto)
        {
            GameObject jugador = GameObject.FindWithTag("Player");

            if (jugador == null)
            {
                // Si el jugador no usa Tag, busca por el script de movimiento o nombre
                jugador = GameObject.Find("Prota_Unity_1");
            }

            if (jugador != null)
            {
                jugador.transform.position = transform.position;
            }
        }
    }
}