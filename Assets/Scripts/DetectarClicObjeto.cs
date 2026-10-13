using UnityEngine;
using UnityEngine.EventSystems;

public class DetectarClicObjeto : MonoBehaviour
{
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
            return;

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
            objeto = golpe.collider
                .GetComponentInParent<ObjetoInteractuable>();

        if (objeto == null)
            return;

        if (GestorInteracciones.Instancia == null)
            return;

        GestorInteracciones.Instancia.objetoObjetivo = objeto;

        Debug.Log(
            "Objeto seleccionado: " + objeto.nombreObjeto +
            ". Acción: " +
            GestorInteracciones.Instancia.accionSeleccionada
        );
    }
}