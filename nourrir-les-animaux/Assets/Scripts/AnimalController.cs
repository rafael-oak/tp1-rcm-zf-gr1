using UnityEngine;

public class AnimalController : MonoBehaviour
{
    // French comments from original script are preserved.
    public float eatDuration = 1f;          // Durée de l’animation "Eat"
    public float constantSpeed = 5f;        // The desired constant speed for normal walking.
    [Header("Wandering Behaviour")]
    [Tooltip("How often (in seconds) the animal picks a new random direction to wander in.")]
    public float directionChangeInterval = 2.0f;
    [Tooltip("How much the animal will wander side-to-side. 0 is a straight line, 1 is fully random.")]
    [Range(0f, 1f)]
    public float wanderStrength = 2f;

    private bool isHungry = true;
    private bool isEating = false;

    private Animator animator;
    private AudioSource audioSource;
    private float eatTimer = 0f;

    // --- Variables for Wandering ---
    private float directionChangeTimer;
    private Vector3 wanderDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Start the walking animation immediately.
        animator.SetBool("isWalking", true);

        // Initialize wandering variables
        directionChangeTimer = Random.Range(0, directionChangeInterval); // Start at a random time to desynchronize animals
        ChooseNewWanderDirection();
    }

    void Update()
    {
        transform.position += Vector3.back * constantSpeed * Time.deltaTime;
        // Check for Game Over state first.
        if (GameManager.isGameOver)
        {
            HandleGameOver();
            return; // Stop processing further logic if game is over.
        }

        // Handle the "Eating" state. The animal should not move.
        if (isEating)
        {
            eatTimer += Time.deltaTime;
            if (eatTimer >= eatDuration)
            {
                isEating = false;
                // After eating, the animal is no longer hungry and will simply stop.
                animator.SetBool("isEating", false);
                animator.SetTrigger("isHappy");
            }
            return; // Stop here while eating.
        }

        // Handle the normal "Hungry" / walking state.
        if (isHungry)
        {
            // --- Wandering Logic ---
            directionChangeTimer -= Time.deltaTime;
            if (directionChangeTimer <= 0)
            {
                ChooseNewWanderDirection();
                directionChangeTimer = directionChangeInterval;
            }

            // Combine the constant backward movement with the random wander direction
            Vector3 finalDirection = (wanderDirection * wanderStrength).normalized;

            // --- Movement and Rotation ---
            transform.position += finalDirection  * Time.deltaTime;

            // Make the animal face the direction it's moving in
            if (finalDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(finalDirection);
            }


            // --- Game Over Check ---
            // If a hungry animal gets past the player's line (z = -4), end the game.
            if (transform.position.z < -14f)
            {
                GameManager.isGameOver = true;
                // No need to return, the global game over check will handle it next frame.
            }
        }
    }

    /// <summary>
    /// Picks a new random direction on the XZ plane for wandering.
    /// </summary>
    void ChooseNewWanderDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        wanderDirection = new Vector3(randomX, 0, randomZ).normalized;
    }

    public void Manger()
    {
        if (!isHungry) return;

        isHungry = false;
        isEating = true;
        eatTimer = 0f;

        // Stop the walking animation and start the eating animation.
        animator.SetBool("isWalking", false);
        animator.SetBool("isEating", true);

        if (audioSource != null)
        {
            audioSource.Play(); // Son de joie
        }
    }

    void HandleGameOver()
    {
        // Stop all animations when the game is over.
        animator.SetBool("isWalking", false);
        animator.SetBool("isEating", false);
        animator.SetTrigger("isSad"); // Animation triste ou figée
    }
}

