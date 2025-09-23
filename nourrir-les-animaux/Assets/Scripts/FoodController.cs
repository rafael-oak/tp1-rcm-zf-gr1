using UnityEngine;

public class ControleNourriture : MonoBehaviour
{
    public float vitesse = 7f; // Vitesse de déplacement de la nourriture

    void Start()
    {
        // Ne pas jouer les particules si le jeu est terminé
        if (GameManager.isGameOver) return;

        ParticleSystem particules = GetComponentInChildren<ParticleSystem>();
        if (particules != null)
        {
            particules.Play();

            // Détruire les particules après leur durée de vie
            Destroy(particules.gameObject, particules.main.duration + particules.main.startLifetime.constantMax);
        }
    }

    void Update()
    {
        // Déplacement vers l'avant (axe Z)
        transform.Translate(Vector3.forward * vitesse * Time.deltaTime);

        // Détruire l'objet si hors écran
        if (transform.position.z > 10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider autre)
    {
        // Vérifie si la collision est avec un animal
        if (autre.CompareTag("Animal"))
        {
            AnimalController animal = autre.GetComponent<AnimalController>();
            if (animal != null)
            {
                animal.Manger();

                // Jouer le son de nourrissage
                AudioSource audio = GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Play();
                }

                // Détruire la nourriture après avoir nourri l'animal
                Destroy(gameObject);
            }
        }
    }
}
