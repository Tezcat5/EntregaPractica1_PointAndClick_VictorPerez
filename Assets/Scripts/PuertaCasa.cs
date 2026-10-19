using UnityEngine;

public class PuertaCasa : MonoBehaviour
{
    // Esta función la llamará el botón INTERACTUAR o al hacer clic
    public void IntentarSalir()
    {
        // Comprobamos de verdad si la llave está en el inventario
        if (Inventario.Instance.TieneObjeto("Llave de Casa"))
        {
            Debug.Log("YO: Usé la llave para abrir la puerta. ¡Puedo salir!");
            // Aquí pones tu cambio de escena:
            // UnityEngine.SceneManagement.SceneManager.LoadScene("NombreDeTuEscenaExterior");
        }
        else
        {
            Debug.Log("YO: La puerta está cerrada con llave. Debería hablar con Mamá primero.");
        }
    }
}