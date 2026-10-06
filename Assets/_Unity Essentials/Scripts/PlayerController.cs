using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Tooltip("Forward/back speed (units/sec).")]
    public float speed = 5.0f;

    [Tooltip("Turn speed (degrees/sec).")]
    public float rotationSpeed = 120.0f;

    [Tooltip("Maximum distance from starting position.")]
    public float movementLimit = 3.0f;

    // Fuerza del salto
    public float jumpForce = 5.0f;

    private Rigidbody rb;
    private Vector3 startPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning("PlayerController needs a Rigidbody.");
            return;
        }

        // Guardamos la posición inicial de la aspiradora
        startPosition = rb.position;
    }

    private void Update()
    {
        if (rb == null || Keyboard.current == null)
            return;

        // Saltar con la barra espaciadora
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        if (rb == null || Keyboard.current == null)
            return;

        float move = 0f;
        float turn = 0f;

        // Adelante
        if (Keyboard.current.wKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed)
        {
            move = 1f;
        }

        // Atrás
        if (Keyboard.current.sKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed)
        {
            move = -1f;
        }

        // Izquierda
        if (Keyboard.current.aKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed)
        {
            turn = -1f;
        }

        // Derecha
        if (Keyboard.current.dKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            turn = 1f;
        }

        // Calcular nueva posición
        Vector3 movement =
            transform.forward * move * speed * Time.fixedDeltaTime;

        Vector3 newPosition = rb.position + movement;

        // Mantener la aspiradora dentro del área
        newPosition.x = Mathf.Clamp(
            newPosition.x,
            startPosition.x - movementLimit,
            startPosition.x + movementLimit
        );

        newPosition.z = Mathf.Clamp(
            newPosition.z,
            startPosition.z - movementLimit,
            startPosition.z + movementLimit
        );

        // Mantener el movimiento vertical del salto
        newPosition.y = rb.position.y;

        rb.MovePosition(newPosition);

        // Rotación
        float rotation =
            turn * rotationSpeed * Time.fixedDeltaTime;

        Quaternion turnRotation =
            Quaternion.Euler(0f, rotation, 0f);

        rb.MoveRotation(rb.rotation * turnRotation);
    }
}