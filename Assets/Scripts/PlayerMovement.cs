using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;

    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private Animator animator;

    private string ultimaDireccion = "Down";

    void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ActualizarAnimacionQuieto();
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

        bool moviendose =
            Vector2.Distance(rb.position, new Vector2(
                targetPosition.x, targetPosition.y)) > 0.05f;

        animator.SetBool("Moviendose", moviendose);

        if (!moviendose)
        {
            ActualizarAnimacionQuieto();
        }
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        Vector2 actual = rb.position;
        Vector2 destino = new Vector2(
            targetPosition.x, targetPosition.y);

        Vector2 diferencia = destino - actual;
        Vector2 nuevo = actual;

        if (Mathf.Abs(diferencia.x) > 0.05f)
        {
            nuevo = Vector2.MoveTowards(
                actual,
                new Vector2(destino.x, actual.y),
                speed * Time.fixedDeltaTime);

            ultimaDireccion = diferencia.x < 0 ? "Left" : "Right";
        }
        else if (Mathf.Abs(diferencia.y) > 0.05f)
        {
            nuevo = Vector2.MoveTowards(
                actual,
                new Vector2(actual.x, destino.y),
                speed * Time.fixedDeltaTime);

            ultimaDireccion = diferencia.y > 0 ? "Up" : "Down";
        }

        rb.MovePosition(nuevo);
    }

    void ActualizarAnimacionQuieto()
    {
        if (animator == null)
            return;

        animator.SetBool("MirandoArriba", ultimaDireccion == "Up");
        animator.SetBool("MirandoIzquierda", ultimaDireccion == "Left");
        animator.SetBool("MirandoDerecha", ultimaDireccion == "Right");
    }
}