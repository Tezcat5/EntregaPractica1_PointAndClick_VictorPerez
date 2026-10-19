using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;

    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private Animator animator;

    // Guardamos la última dirección en coordenadas Vector2 (por defecto mirando hacia abajo)
    private Vector2 ultimaDireccion = Vector2.down;

    void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ActualizarAnimacion();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            if (Camera.main == null)
                return;

            Vector3 mousePosition =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePosition.z = transform.position.z;
            targetPosition = mousePosition;
        }

        if (rb == null || animator == null)
            return;

        ActualizarAnimacion();
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        Vector2 actual = rb.position;
        Vector2 destino = new Vector2(targetPosition.x, targetPosition.y);

        Vector2 diferencia = destino - actual;
        Vector2 nuevo = actual;

        if (Mathf.Abs(diferencia.x) > 0.05f)
        {
            nuevo = Vector2.MoveTowards(
                actual,
                new Vector2(destino.x, actual.y),
                speed * Time.fixedDeltaTime);

            // Guardamos la dirección en X (1 para derecha, -1 para izquierda)
            ultimaDireccion = diferencia.x < 0 ? Vector2.left : Vector2.right;
        }
        else if (Mathf.Abs(diferencia.y) > 0.05f)
        {
            nuevo = Vector2.MoveTowards(
                actual,
                new Vector2(actual.x, destino.y),
                speed * Time.fixedDeltaTime);

            // Guardamos la dirección en Y (1 para arriba, -1 para abajo)
            ultimaDireccion = diferencia.y > 0 ? Vector2.up : Vector2.down;
        }

        rb.MovePosition(nuevo);
    }

    void ActualizarAnimacion()
    {
        if (animator == null || rb == null)
            return;

        // Comprobamos si se está moviendo
        bool moviendose = Vector2.Distance(rb.position, new Vector2(targetPosition.x, targetPosition.y)) > 0.05f;

        animator.SetBool("Moviendose", moviendose);

        if (moviendose)
        {
            // Mientras camina, enviamos la dirección hacia la que avanza
            animator.SetFloat("MoveX", ultimaDireccion.x);
            animator.SetFloat("MoveY", ultimaDireccion.y);
        }
        else
        {
            // Al pararse, mantiene la última dirección para el Blend Tree de Idle
            animator.SetFloat("MoveX", ultimaDireccion.x);
            animator.SetFloat("MoveY", ultimaDireccion.y);
        }
    }
}