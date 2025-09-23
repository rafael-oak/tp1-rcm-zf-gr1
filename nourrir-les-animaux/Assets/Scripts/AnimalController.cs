using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public float moveSpeed = 2f;           // Vitesse vers le bas
    public float sideSpeed = 1f;           // Vitesse latérale
    public float fleeSpeed = 4f;           // Vitesse de fuite après nourrissage
    public float eatDuration = 1f;         // Durée de l’animation "Eat"

    private bool isHungry = true;
    private bool isFleeing = false;
    private bool isEating = false;

    private Animator animator;
    private AudioSource audioSource;
    private float eatTimer = 0f;
    private Vector3 fleeDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Direction de fuite aléatoire (gauche ou droite)
        fleeDirection = Random.value < 0.5f ? Vector3.left : Vector3.right;
    }

    void Update()
    {
        if (GameManager.isGameOver)
        {
            HandleGameOver();
            return;
        }

        if (isEating)
        {
            eatTimer += Time.deltaTime;
            if (eatTimer >= eatDuration)
            {
                isEating = false;
                isFleeing = true;
                animator.SetBool("isEating", false);
                animator.SetTrigger("isHappy");
            }
            return;
        }

        if (isFleeing)
        {
            transform.Translate(fleeDirection * fleeSpeed * Time.deltaTime);
        }
        else if (isHungry)
        {
            // Déplacement vers le bas + latéral
            Vector3 move = Vector3.back * moveSpeed + Vector3.left * sideSpeed;
            transform.Translate(move * Time.deltaTime);

            animator.SetBool("isWalking", true);
        }
    }

    public void Manger()
    {
        if (!isHungry) return;

        isHungry = false;
        isEating = true;
        eatTimer = 0f;

        animator.SetBool("isWalking", false);
        animator.SetBool("isEating", true);

        if (audioSource != null)
        {
            audioSource.Play(); // Son de joie
        }
    }

    void HandleGameOver()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isEating", false);
        animator.SetTrigger("isSad"); // Animation triste ou figée
    }
}
