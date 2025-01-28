using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 20f;
    private Vector2 entradaMovimiento;
    private Rigidbody rb;

    [Header("Camera")]
    public float mouseSensitivity = 100f;
    private float mouseX;
    public Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue valor)
    {
        entradaMovimiento = valor.Get<Vector2>();
    }

    public void OnLook(InputValue valor)
    {
        mouseX = valor.Get<Vector2>().x;
    }

    void Update()
    {
        float rotationY = mouseX * mouseSensitivity * Time.deltaTime;
        transform.Rotate(0, rotationY, 0);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 movimiento = forward * entradaMovimiento.y + right * entradaMovimiento.x;
        rb.MovePosition(transform.position + movimiento * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemigo"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}