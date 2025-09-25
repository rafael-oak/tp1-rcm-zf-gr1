using UnityEngine;

public class MoveGroundLoop : MonoBehaviour
{
    public float speed = 5f;
    public float teleportZPosition = 50f; // La position Z � laquelle se t�l�porter
    public float destroyZPosition = -50f; // La position Z � laquelle le sol se "d�truit" et se t�l�porte

    void Update()
    {
        // Se d�placer vers l'arri�re le long de l'axe Z
        transform.position += Vector3.back * speed * Time.deltaTime;

        // V�rifier si le sol a d�pass� le point de t�l�portation
        if (transform.position.z <= destroyZPosition)
        {
            // T�l�porter le sol � la position Z de d�part sp�cifi�e
            transform.position = new Vector3(transform.position.x, transform.position.y, teleportZPosition);

            Debug.Log($"{gameObject.name} teleported to z={teleportZPosition}");
        }
    }
}