using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class AnimalController : MonoBehaviour
{
    public float eatDuration = 1f;          // Durée de l’animation "Manger"
    public float constantSpeed = 5f;        // Vitesse constante vers l’arrière

    [Header("Comportement de vagabondage / Wandering Behaviour")]
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

    // Variables pour le vagabondage
    private float directionChangeTimer;
    private Vector3 wanderDirection;

    GameOverTrigger gameOverTrigger;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        animator.SetBool("isWalking", true); // Démarre l’animation de marche
        directionChangeTimer = Random.Range(0, directionChangeInterval); // Décalage aléatoire
        ChooseNewWanderDirection();
        gameOverTrigger = GameObject.Find("Player").GetComponent<GameOverTrigger>();
    }

    void Update()
    {
        transform.position += Vector3.back * constantSpeed * Time.deltaTime;
        if (gameOverTrigger.gameOver)
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
                animator.SetBool("isEating", false);
                animator.SetTrigger("isHappy"); // Animation de joie
            }
            return;
        }

        if (isHungry)
        {
            directionChangeTimer -= Time.deltaTime;
            if (directionChangeTimer <= 0)
            {
                ChooseNewWanderDirection();
                directionChangeTimer = directionChangeInterval;
            }

            Vector3 finalDirection = (wanderDirection * wanderStrength).normalized;
            transform.position += finalDirection  * Time.deltaTime;

            if (finalDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(finalDirection);
            }

            if (transform.position.z < -14f)
            {
                gameOverTrigger.gameOver = true;
                Debug.Log("💀 Game Over : un animal affamé est sorti du champ !");
            }
        }
    }

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

        //animator.SetBool("isWalking", false);
        //animator.SetBool("isEating", true);

        if (audioSource != null)
        {
            audioSource.Play(); // Son de nourrissage
        }

        StartCoroutine(FinirAnimal());
    }

    IEnumerator FinirAnimal()
    {
        yield return new WaitForSeconds(eatDuration + 0.5f);
        Destroy(gameObject);
    }

    void HandleGameOver()
    {
        //animator.SetBool("isWalking", false);
        //animator.SetBool("isEating", false);
        //animator.SetTrigger("isSad"); // Animation de tristesse
    }
}
