using UnityEngine;

public class ControleNourriture : MonoBehaviour
{
    public float vitesse = 7f; // Vitesse de d�placement de la nourriture

    void Start()
    {
        // Ne pas jouer les particules si le jeu est termin�
        if (GameManager.isGameOver) return;

        ParticleSystem particules = GetComponentInChildren<ParticleSystem>();
        if (particules != null)
        {
            particules.Play();

            // D�truire les particules apr�s leur dur�e de vie
            Destroy(particules.gameObject, particules.main.duration + particules.main.startLifetime.constantMax);
        }
    }

    void Update()
    {
        // D�placement vers l'avant (axe Z)
        transform.Translate(Vector3.forward * vitesse * Time.deltaTime);

        // D�truire l'objet si hors �cran
        if (transform.position.z > 10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider autre)
    {
        // V�rifie si la collision est avec un animal
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

                // D�truire la nourriture apr�s avoir nourri l'animal
                Destroy(gameObject);
            }
        }
    }
}
