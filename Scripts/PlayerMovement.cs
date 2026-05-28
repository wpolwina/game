using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    public float moveSpeed = 5f;
    public Camera mainCamera;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        // Делаем детектива видимым поверх всего
        spriteRenderer.sortingOrder = 10;
    }

    void Update()
    {
        // Ввод с клавиатуры
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Нормализуем диагональное движение
        if (movement.magnitude > 1)
            movement.Normalize();

        // Поворот спрайта в сторону движения
        if (movement.x != 0)
            spriteRenderer.flipX = movement.x < 0;
    }

    void FixedUpdate()
    {
        // Движение
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    // Следим за тем, чтобы камера следовала за детективом
    void LateUpdate()
    {
        if (mainCamera != null)
        {
            Vector3 cameraPos = mainCamera.transform.position;
            cameraPos.x = Mathf.Lerp(cameraPos.x, transform.position.x, 0.1f);
            cameraPos.y = Mathf.Lerp(cameraPos.y, transform.position.y, 0.1f);
            mainCamera.transform.position = cameraPos;
        }
    }
}