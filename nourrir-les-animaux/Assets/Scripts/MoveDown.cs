using UnityEngine;

public class MoveGroundLoop : MonoBehaviour
{
    public float speed = 5f;
    public float teleportZPosition = 50f; // The Z-position to teleport to
    public float destroyZPosition = -50f; // The Z-position at which the ground "destroys" and teleports

    void Update()
    {
        // Move backward along the Z-axis
        transform.position += Vector3.back * speed * Time.deltaTime;

        // Check if the ground has passed the teleportation point
        if (transform.position.z <= destroyZPosition)
        {
            // Teleport the ground to the specified starting Z-position
            transform.position = new Vector3(transform.position.x, transform.position.y, teleportZPosition);

            Debug.Log($"{gameObject.name} teleported to z={teleportZPosition}");
        }
    }
}