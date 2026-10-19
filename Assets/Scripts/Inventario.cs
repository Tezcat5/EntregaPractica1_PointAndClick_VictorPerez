using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    // Permitir acceder al inventario desde cualquier script con Inventario.Instance
    public static Inventario Instance;

    // Lista donde se guardan los nombres de los objetos conseguidos
    [Header("Lista de Objetos")]
    public List<string> objetos = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para que no se borren los objetos al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Añadir un objeto a la mochila
    public void AgregarObjeto(string nombreObjeto)
    {
        if (!objetos.Contains(nombreObjeto))
        {
            objetos.Add(nombreObjeto);
            Debug.Log("Objeto guardado en la mochila: " + nombreObjeto);
        }
    }

    // Comprobar si tenemos un objeto ("Llaves", "Carta", "TarjetaBus")
    public bool TieneObjeto(string nombreObjeto)
    {
        return objetos.Contains(nombreObjeto);
    }
}