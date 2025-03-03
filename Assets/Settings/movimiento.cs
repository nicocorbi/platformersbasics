using UnityEngine;

public class movimiento : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    public float vectorLength = 1f; // Longitud deseada del vector
    Vector3 movementInput;   // Entrada del movimiento

    void Start()
    {
        // Ajusta la rotación inicial para que una punta del cuadrado apunte hacia arriba
        transform.up = new Vector3(1, 1, 0).normalized; // Inicialmente apunta en diagonal (45 grados)
    }

    void Update()
    {
        // Reinicia el vector de entrada
        movementInput = Vector3.zero;

        // Captura las teclas de entrada para el movimiento
        if (Input.GetKey(KeyCode.W))
            movementInput.y += 1; // Movimiento hacia arriba
        if (Input.GetKey(KeyCode.S))
            movementInput.y -= 1; // Movimiento hacia abajo
        if (Input.GetKey(KeyCode.D))
            movementInput.x += 1; // Movimiento hacia la derecha
        if (Input.GetKey(KeyCode.A))
            movementInput.x -= 1; // Movimiento hacia la izquierda

        // Si hay movimiento, ajusta la longitud del vector explícitamente
        if (movementInput.magnitude > 0)
        {
            // Normalizar usando tu fórmula personalizada
            float length = Mathf.Sqrt(movementInput.x * movementInput.x + movementInput.y * movementInput.y);
            if (length > 0)
            {
                movementInput = movementInput / length;
            }

            // Escala el vector a la longitud deseada
            movementInput *= vectorLength;

            // Ajusta la dirección del cuadrado utilizando transform.up
            transform.up = movementInput; // Cambia el eje "up" del objeto para apuntar en la dirección del movimiento
        }

        // Mueve al personaje
        transform.position += movementInput * speed * Time.deltaTime;
    }
}

