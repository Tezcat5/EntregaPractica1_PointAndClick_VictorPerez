using UnityEngine;
using UnityEngine.EventSystems;

public class DetectarClicObjeto : MonoBehaviour
{
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        // Ignorar los clics sobre los botones y la interfaz.
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Camera.main == null)
            return;

        Vector2 posicionClic =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D golpe =
            Physics2D.Raycast(posicionClic, Vector2.zero);

        if (golpe.collider == null)
            return;

        ObjetoInteractuable objeto =
            golpe.collider.GetComponent<ObjetoInteractuable>();

        if (objeto == null)
            objeto = golpe.collider.GetComponentInParent<ObjetoInteractuable>();

        if (objeto == null)
            return;

        if (GestorInteracciones.Instancia == null)
            return;

        string accion =
            GestorInteracciones.Instancia.accionSeleccionada;

        objeto.Interactuar(accion);
    }
}