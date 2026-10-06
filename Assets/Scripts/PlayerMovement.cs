using UnityEngine;

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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;

            targetPosition = mousePosition;
        }
    }

    void FixedUpdate()
    {
        Vector2 currentPosition = rb.position;
        Vector2 target = new Vector2(targetPosition.x, targetPosition.y);

        Vector2 newPosition;

        // Primero nos movemos horizontalmente
        if (Mathf.Abs(target.x - currentPosition.x) > 0.05f)
        {
            newPosition = Vector2.MoveTowards(
                currentPosition,
                new Vector2(target.x, currentPosition.y),
                speed * Time.fixedDeltaTime
            );
        }
        // Después nos movemos verticalmente
        else
        {
            newPosition = Vector2.MoveTowards(
                currentPosition,
                new Vector2(currentPosition.x, target.y),
                speed * Time.fixedDeltaTime
            );
        }

        rb.MovePosition(newPosition);
    }
}