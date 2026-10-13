using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float distanciaInteraccion = 0.6f;

    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private ObjetoInteractuable objetoPendiente;

    void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
            return;

        if (Camera.main == null)
            return;

        Vector3 mousePosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 punto = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D golpe = Physics2D.Raycast(punto, Vector2.zero);

        if (golpe.collider != null)
        {
            ObjetoInteractuable objeto =
                golpe.collider.GetComponent<ObjetoInteractuable>();

            if (objeto == null)
                objeto = golpe.collider
                    .GetComponentInParent<ObjetoInteractuable>();

            if (objeto != null &&
                GestorInteracciones.Instancia != null)
            {
                objetoPendiente = objeto;
                targetPosition = ObtenerPuntoCercano(objeto);
                return;
            }
        }

        // Clic en el suelo: caminar normalmente.
        objetoPendiente = null;
        mousePosition.z = transform.position.z;
        targetPosition = mousePosition;
    }

    Vector3 ObtenerPuntoCercano(ObjetoInteractuable objeto)
    {
        Bounds bounds = objeto.GetComponent<Collider2D>().bounds;

        Vector3 centro = bounds.center;
        Vector3 jugador = transform.position;

        // Acercarse desde el lado más cercano.
        if (Mathf.Abs(jugador.x - centro.x) >
            Mathf.Abs(jugador.y - centro.y))
        {
            centro.x = jugador.x < bounds.center.x
                ? bounds.min.x - distanciaInteraccion
                : bounds.max.x + distanciaInteraccion;
        }
        else
        {
            centro.y = jugador.y < bounds.center.y
                ? bounds.min.y - distanciaInteraccion
                : bounds.max.y + distanciaInteraccion;
        }

        centro.z = transform.position.z;
        return centro;
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        Vector2 actual = rb.position;
        Vector2 destino = new Vector2(targetPosition.x, targetPosition.y);

        Vector2 nuevo;

        if (Mathf.Abs(destino.x - actual.x) > 0.05f)
        {
            nuevo = Vector2.MoveTowards(
                actual,
                new Vector2(destino.x, actual.y),
                speed * Time.fixedDeltaTime
            );
        }
        else
        {
            nuevo = Vector2.MoveTowards(
                actual,
                new Vector2(actual.x, destino.y),
                speed * Time.fixedDeltaTime
            );
        }

        rb.MovePosition(nuevo);

        if (objetoPendiente != null &&
            Vector2.Distance(rb.position, destino) < 0.12f)
        {
            if (GestorInteracciones.Instancia != null)
            {
                GestorInteracciones.Instancia.objetoObjetivo = null;
                objetoPendiente.Interactuar(
                    GestorInteracciones.Instancia.accionSeleccionada
                );
            }

            objetoPendiente = null;
        }
    }
}