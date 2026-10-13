using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;

    private Vector3 targetPosition;
    private Rigidbody2D rb;

    void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // El ratón solo sirve para caminar.
        if (!Input.GetMouseButtonDown(0))
            return;

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

    void FixedUpdate()
    {
        if (rb == null)
            return;

        Vector2 actual = rb.position;
        Vector2 destino = new Vector2(
            targetPosition.x,
            targetPosition.y
        );

        Vector2 nuevo;

        // Primero horizontal y después vertical.
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
    }
}