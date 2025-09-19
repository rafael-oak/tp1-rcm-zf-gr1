using UnityEngine;

public class MoveDown : MonoBehaviour
{
    // Vitesse du sol
    public float vitesse = 2f;

    // Direction du mouvement
    public Vector2 direction = new Vector2(1, 0);

    private Vector3 positionDebut;

    void Start()
    {   
        // Enregistre la position initial du sol
        positionDebut = transform.position;

    }

    // Update is called once per frame
    void Update()
    {   
        // Calcule nouvelle position par frame
        transform.position += (Vector3)direction * vitesse * Time.deltaTime;

        if (transform.position.x - positionDebut.x > GetComponent<SpriteRenderer>().bounds.size.x)
        {
            transform.position = positionDebut;
        }
    }
}
