using UnityEngine;

public class ControleJoueur : MonoBehaviour
{
    [Header("Deplacement")]
    public float vitesseDeplacement = 5f;     // Vitesse du joueur
    public float limiteX = 7f;                // Limite horizontale du terrain
    public float positionZFixe = -4f;         // Position verticale fixe du joueur

    [Header("Tir")]
    public GameObject prefabNourriture;       // Prefab de la carotte ou nourriture
    public Transform pointApparition;         // Position d'apparition de la nourriture

    private Animator animateur;               // Composant Animator du joueur

    void Start()
    {
        animateur = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.isGameOver)
        {
            GererFinDuJeu();
            return;
        }

        GererDeplacement();
        GererTir();
        VerifierSortieTerrain();
    }

    void GererDeplacement()
    {
        float mouvementHorizontal = Input.GetAxis("Horizontal");
        Vector3 mouvement = new Vector3(mouvementHorizontal, 0f, 0f);

        // Déplacement horizontal indépendant du temps
        transform.Translate(mouvement.normalized * vitesseDeplacement * Time.deltaTime, Space.World);

        // Limiter la position X pour rester dans le terrain
        float positionXLimitee = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(positionXLimitee, transform.position.y, positionZFixe);

        // Animation de course
        bool estEnMouvement = Mathf.Abs(mouvementHorizontal) > 0.01f;
        animateur.SetBool("enCourse", estEnMouvement);
    }

    void GererTir()
    {
        // Ne pas tirer si le jeu est terminé
        if (GameManager.isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (prefabNourriture != null && pointApparition != null)
            {
                Instantiate(prefabNourriture, pointApparition.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("⚠️ prefabNourriture ou pointApparition non assigné !");
            }
        }
    }

    void VerifierSortieTerrain()
    {
        if (Mathf.Abs(transform.position.x) > limiteX + 1f)
        {
            GameManager.isGameOver = true;
            animateur.SetBool("enCourse", false);
            animateur.SetTrigger("triste");
            Debug.Log("💀 Fin du jeu : le joueur est sorti du terrain !");
        }
    }

    void GererFinDuJeu()
    {
        animateur.SetBool("enCourse", false);
        animateur.SetTrigger("triste");
    }
}
