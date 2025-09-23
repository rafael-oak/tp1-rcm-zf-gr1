using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [Header("Deplacement")]
    public float vitesseAvant = 2f;       // Vitesse vers le bas (Z-)
    public float vitesseLaterale = 1f;    // Vitesse latérale (X-)
    public float vitesseFuite = 4f;       // Vitesse de fuite après avoir mangé

    [Header("Comportement")]
    public float dureeManger = 1f;        // Durée pendant laquelle l'animal mange

    private bool estAffame = true;        // L'animal est affamé au départ
    private bool estEnFuite = false;      // L'animal fuit après avoir mangé
    private bool estEnTrainDeManger = false; // L'animal est en train de manger

    private Animator animateur;           // Composant Animator
    private AudioSource audio;            // Composant AudioSource
    private float minuterieManger = 0f;   // Chrono pour la durée de manger
    private Vector3 directionFuite;       // Direction aléatoire de fuite

    void Start()
    {
        animateur = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        // Choix aléatoire de la direction de fuite (gauche ou droite)
        directionFuite = Random.value < 0.5f ? Vector3.left : Vector3.right;
    }

    void Update()
    {
        if (GameManager.isGameOver)
        {
            GererFinDuJeu();
            return;
        }

        if (estEnTrainDeManger)
        {
            GererManger();
            return;
        }

        if (estEnFuite)
        {
            transform.Translate(directionFuite * vitesseFuite * Time.deltaTime);
            return;
        }

        if (estAffame)
        {
            GererDeplacement();
            VerifierSortieTerrain();
        }
    }

    void GererDeplacement()
    {
        // Mouvement combiné vers le bas et la gauche
        Vector3 mouvement = Vector3.back * vitesseAvant + Vector3.left * vitesseLaterale;
        transform.Translate(mouvement * Time.deltaTime);
        transform.forward = mouvement.normalized;

        animateur.SetBool("marche", true);
    }

    void GererManger()
    {
        minuterieManger += Time.deltaTime;
        if (minuterieManger >= dureeManger)
        {
            estEnTrainDeManger = false;
            estEnFuite = true;

            animateur.SetBool("mange", false);
            animateur.SetTrigger("content");
        }
    }

    void VerifierSortieTerrain()
    {
        if (transform.position.z < -10f) // Ajuste selon ta scène
        {
                GameManager.isGameOver = true;
            animateur.SetBool("marche", false);
            animateur.SetTrigger("triste");
            Debug.Log("💀 Fin du jeu : un animal affamé est sorti du terrain !");
        }
    }

    public void Manger()
    {
        if (!estAffame) return;

        estAffame = false;
        estEnTrainDeManger = true;
        minuterieManger = 0f;

        animateur.SetBool("marche", false);
        animateur.SetBool("mange", true);

        if (audio != null)
        {
            audio.Play(); // Son de joie
        }
    }

    void GererFinDuJeu()
    {
        animateur.SetBool("marche", false);
        animateur.SetBool("mange", false);
        animateur.SetTrigger("triste");
    }
}
