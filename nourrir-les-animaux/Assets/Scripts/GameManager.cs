using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false; // Indique si le jeu est terminé

    private Animator animateur; // Référence à l'Animator du joueur ou du personnage principal

    void Démarrer()
    {
        animateur = GetComponent<Animator>();
    }

    // Méthode appelée pour gérer la fin du jeu
    public void GererFinDuJeu()
    {
        if (animateur != null)
        {
            animateur.SetBool("estEnCourse", false); // Arrête l'animation de course
            animateur.SetTrigger("estTriste");        // Déclenche l'animation de tristesse
        }

        // Arrête toutes les particules actives dans la scène
        ParticleSystem[] toutesLesParticules = FindObjectsOfType<ParticleSystem>();
        foreach (ParticleSystem particule in toutesLesParticules)
        {
            particule.Stop();
        }

        Debug.Log("🎮 Le jeu est terminé. Toutes les animations et particules ont été arrêtées.");
    }

}