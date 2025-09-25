using UnityEngine;

public class MoveGroundLoop : MonoBehaviour
{
    public float speed = 5f;
    public float teleportZPosition = 50f; // La position Z à laquelle se téléporter
    public float destroyZPosition = -50f; // La position Z à laquelle le sol se "détruit" et se téléporte

    void Update()
    {
        // Se déplacer vers l'arrière le long de l'axe Z
        transform.position += Vector3.back * speed * Time.deltaTime;

        // Vérifier si le sol a dépassé le point de téléportation
        if (transform.position.z <= destroyZPosition)
        {
            // Téléporter le sol à la position Z de départ spécifiée
            transform.position = new Vector3(transform.position.x, transform.position.y, teleportZPosition);

            Debug.Log($"{gameObject.name} teleported to z={teleportZPosition}");
        }
    }
}