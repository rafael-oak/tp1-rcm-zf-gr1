using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Déplacement")]
    public float moveSpeed = 5f;
    public float xLimit = 7f;
    public float fixedZ = -4f; // Position verticale fixe

    [Header("Tir")]
    public GameObject foodPrefab;
    public Transform spawnPoint;

    public ParticleSystem effetTir;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.isGameOver)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("isSad"); // Animation triste
            return;
        }

        // Déplacement horizontal
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0f, 0f);

        transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);

        // Limite de position sur X
        float clampedX = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        transform.position = new Vector3(clampedX, transform.position.y, fixedZ);

        // Animation de course
        bool isMoving = Mathf.Abs(horizontal) > 0.01f;
        animator.SetBool("isRunning", isMoving);

        // Tir de nourriture
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchFood();
        }
    }

    void LaunchFood()
    {
        if (GameManager.isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (foodPrefab != null && spawnPoint != null)
            {
                Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);

                // Déclenche les particules du joueur
                if (effetTir != null)
                {
                    effetTir.Play();
                }

                Debug.Log("🥕 Carotte lancée !");
            }
            else
            {
                Debug.LogWarning("⚠️ prefabNourriture ou pointApparition non assigné !");
            }
        }
        effetTir.Stop();
    }
}
