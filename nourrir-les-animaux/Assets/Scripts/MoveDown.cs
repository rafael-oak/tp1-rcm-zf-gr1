using UnityEngine;

public class MoveGroundLoop : MonoBehaviour
{
    public float speed = 5f;
    public float resetZ = 50f;
    public float thresholdZ = -50f;

    void Update()
    {
        if (GameManager.isGameOver) return;

        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z <= thresholdZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, resetZ);
        }
    }
}