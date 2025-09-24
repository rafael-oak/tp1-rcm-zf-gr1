using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false; // Indique si le jeu est terminé

    private Animator animateur; // Référence à l'Animator (si utilisé pour une animation globale)

    void Start()
    {
        animateur = GetComponent<Animator>();
    }

    /// <summary>
    /// Méthode appelée pour gérer la fin du jeu
    /// </summary>
    public void HandleGameOver()
    {
        if (isGameOver) return; // Évite les appels multiples
        isGameOver = true;

        // Stoppe l'animation globale si présente
        if (animateur != null)
        {
            animateur.SetBool("isRunning", false);
            animateur.SetTrigger("isSad");
        }

        // Stoppe toutes les particules actives dans la scène
        ParticleSystem[] toutesLesParticules = FindObjectsOfType<ParticleSystem>();
        foreach (ParticleSystem particule in toutesLesParticules)
        {
            particule.Stop();
        }

        Debug.Log("🎮 Fin du jeu : animations et particules arrêtées.");
    }
}
