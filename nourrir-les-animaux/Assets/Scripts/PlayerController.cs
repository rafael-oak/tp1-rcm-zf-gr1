using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float xLimit = 7f;

    public GameObject foodPrefab;   // prefab de la nourriture
    public Transform spawnPoint;    // point d’où sort la nourriture

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Inputs de mouvement
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

        //  Tir nourriture (barre espace)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchFood();
        }
    }

    void LaunchFood()
    {
        if (foodPrefab != null && spawnPoint != null)
        {
            // Instancie la nourriture
            GameObject food = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);

            // Optionnel : effet de particule si attaché au prefab
            ParticleSystem ps = food.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
        }
        else
        {
            Debug.LogWarning("⚠️ foodPrefab ou spawnPoint non assigné dans l'inspecteur !");
        }
    }
}
