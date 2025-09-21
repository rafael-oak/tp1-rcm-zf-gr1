using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float xLimit = 7f;
    private Vector2 movement;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Inputs
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Mouvement (X = gauche/droite, Z = avant/arrière)
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        // Déplacement (normalisé pour éviter vitesse plus rapide en diagonale)
        transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);

        // Limite de position sur X
        float clampedX = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        // Animation
        if (animator != null)
        {
            bool isMoving = movement.magnitude > 0.01f;
            animator.SetBool("isRunning", isMoving);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
}
