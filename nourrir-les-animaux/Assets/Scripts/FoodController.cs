using UnityEngine;

public class FoodController : MonoBehaviour
{
    public float speed = 7f;

    void Update()
    {
        // Avance vers le haut (Z)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Détruit si hors écran
        if (transform.position.z > 10f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            AnimalController animal = other.GetComponent<AnimalController>();
            if (animal != null)
            {
                animal.Manger();

                // Son de nourrissage
                AudioSource audio = GetComponent<AudioSource>();
                if (audio != null)
                {
                    audio.Play();
                }

                Destroy(gameObject);
            }
        }
    }
}
