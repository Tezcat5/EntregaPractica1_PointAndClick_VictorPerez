using UnityEngine;

public class AnimacionPokemonCombate : MonoBehaviour
{
    public enum TipoAnimacion { RespiracionVertical, FlotarHorizontal }
    public TipoAnimacion tipoMovimiento = TipoAnimacion.RespiracionVertical;

    public float velocidad = 4f;
    public float amplitud = 0.15f; // Lo que se mueve (altura/distancia)
    
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        if (tipoMovimiento == TipoAnimacion.RespiracionVertical)
        {
            // Efecto de respiración o pequeño salto vertical continuo
            float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidad) * amplitud;
            transform.localPosition = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
        }
        else if (tipoMovimiento == TipoAnimacion.FlotarHorizontal)
        {
            // Efecto de vaivén de lado a lado
            float nuevaX = posicionInicial.x + Mathf.Sin(Time.time * velocidad) * amplitud;
            transform.localPosition = new Vector3(nuevaX, posicionInicial.y, posicionInicial.z);
        }
    }
}