using UnityEngine;
using System.Collections;

public class AnimalController : MonoBehaviour
{
    public float eatDuration = 1f;          // Durée de l’animation "Manger"
    public float constantSpeed = 5f;        // Vitesse constante vers l’arrière

    [Header("Comportement de vagabondage")]
    [Tooltip("Fréquence (en secondes) à laquelle l’animal choisit une nouvelle direction aléatoire.")]
    public float directionChangeInterval = 2.0f;

    [Tooltip("Amplitude du vagabondage latéral. 0 = ligne droite, 1 = très aléatoire.")]
    [Range(0f, 1f)]
    public float wanderStrength = 2f;

    private bool isHungry = true;           // L’animal est affamé au départ
    private bool isEating = false;          // L’animal est en train de manger

    private Animator animator;
    private AudioSource audioSource;
    private float eatTimer = 0f;

    private float directionChangeTimer;
    private Vector3 wanderDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        animator.SetBool("isWalking", true); // Démarre l’animation de marche

        directionChangeTimer = Random.Range(0, directionChangeInterval); // Décalage aléatoire
        ChoisirNouvelleDirection();
    }

    void Update()
    {
        // Si le jeu est terminé, arrêter les animations
        if (GameManager.isGameOver)
        {
            GererFinDuJeu();
            return;
        }

        // Si l’animal est en train de manger, il ne bouge pas
        if (isEating)
        {
            eatTimer += Time.deltaTime;
            if (eatTimer >= eatDuration)
            {
                isEating = false;
                animator.SetBool("isEating", false);
                animator.SetTrigger("isHappy"); // Animation de joie
            }
            return;
        }

        // Si l’animal est affamé, il se déplace
        if (isHungry)
        {
            directionChangeTimer -= Time.deltaTime;
            if (directionChangeTimer <= 0)
            {
                ChoisirNouvelleDirection();
                directionChangeTimer = directionChangeInterval;
            }

            // Combine le mouvement vers l’arrière avec la direction aléatoire
            Vector3 baseDirection = Vector3.back * constantSpeed;
            Vector3 finalDirection = (baseDirection + wanderDirection * wanderStrength).normalized;

            transform.position += finalDirection * constantSpeed * Time.deltaTime;

            // Tourne l’animal vers sa direction
            if (finalDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(finalDirection);
            }

            // Si l’animal affamé dépasse la limite visuelle, Game Over
            if (transform.position.z < -14f)
            {
                GameManager.isGameOver = true;
                Debug.Log("💀 Game Over : un animal affamé est sorti du champ !");
            }
        }
    }

    /// <summary>
    /// Choisit une nouvelle direction aléatoire sur le plan XZ
    /// </summary>
    void ChoisirNouvelleDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        wanderDirection = new Vector3(randomX, 0, randomZ).normalized;
    }

    /// <summary>
    /// Appelée quand l’animal reçoit de la nourriture
    /// </summary>
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
            audioSource.Play(); // Son de nourrissage
        }

        StartCoroutine(FinirAnimal());
    }

    IEnumerator FinirAnimal()
    {
        yield return new WaitForSeconds(eatDuration + 0.5f);
        Destroy(gameObject); // Supprime l’animal après avoir mangé
    }

    /// <summary>
    /// Gère l’état visuel de fin de jeu
    /// </summary>
    void GererFinDuJeu()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isEating", false);
        animator.SetTrigger("isSad"); // Animation de tristesse
    }
}
