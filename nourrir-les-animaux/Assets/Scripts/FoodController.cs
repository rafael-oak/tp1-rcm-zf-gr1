using UnityEngine;

public class FoodController : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        Destroy(gameObject, 3f); // auto-destruction après 3s
    }

    void Update()
    {
        // Avancer vers l’avant (Z local)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            other.GetComponent<AnimalController>().Manger();
            Destroy(gameObject);
        }
    }
}
