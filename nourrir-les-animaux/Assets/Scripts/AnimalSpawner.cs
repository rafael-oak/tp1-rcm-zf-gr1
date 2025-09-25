using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Spawning Configuration")]
    [Tooltip("Un tableau de tous les différents prefabs d'animaux que vous souhaitez faire apparaître.")]
    public GameObject[] animalPrefabs;

    [Tooltip("Le temps en secondes entre chaque apparition.")]
    public float spawnInterval = 2.0f;

    [Header("Spawn Zone")]
    [Tooltip("Le point central de la zone où les animaux apparaîtront.")]
    public Vector3 spawnZoneCenter;

    [Tooltip("La taille (largeur, hauteur, profondeur) de la zone d'apparition.")]
    public Vector3 spawnZoneSize = new Vector3(10, 1, 10);

    // Un minuteur pour suivre le moment où faire apparaître le prochain animal.
    private float spawnTimer;

    void Start()
    {
        // Initialiser le minuteur.
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        // Décompter le minuteur.
        spawnTimer -= Time.deltaTime;

        // Lorsque le minuteur atteint zéro, faire apparaître un animal et réinitialiser le minuteur.
        if (spawnTimer <= 0f)
        {
            SpawnAnimal();
            spawnTimer = spawnInterval;
        }
    }

    /// <summary>
    /// Fait apparaître un animal aléatoire à une position aléatoire dans la zone définie.
    /// </summary>
    private void SpawnAnimal()
    {
        // Tout d'abord, vérifier s'il y a des prefabs à faire apparaître.
        if (animalPrefabs == null || animalPrefabs.Length == 0)
        {
            Debug.LogWarning("Aucun prefab d'animal n'est assigné au spawner.");
            return;
        }

        // Choisir un prefab d'animal aléatoire dans le tableau.
        int randomIndex = Random.Range(0, animalPrefabs.Length);
        GameObject animalToSpawn = animalPrefabs[randomIndex];

        // Calculer une position aléatoire dans la zone d'apparition.
        float randomX = Random.Range(-spawnZoneSize.x / 2, spawnZoneSize.x / 2);
        float randomY = Random.Range(-spawnZoneSize.y / 2, spawnZoneSize.y / 2);
        float randomZ = Random.Range(-spawnZoneSize.z / 2, spawnZoneSize.z / 2);
        Vector3 spawnPosition = spawnZoneCenter + new Vector3(randomX, 0, -4);

        // Définir la rotation pour faire face à l'axe -Z (180 degrés sur l'axe Y).
        Quaternion spawnRotation = Quaternion.Euler(0, 90f, 0);

        // Créer une instance du prefab d'animal choisi à la position et à la rotation calculées.
        Instantiate(animalToSpawn, spawnPosition, spawnRotation);
    }

    /// <summary>
    /// Ceci est une fonction Unity spéciale qui dessine des gizmos dans la vue Scène.
    /// Cela aide à visualiser la zone d'apparition sans avoir besoin d'un objet visible.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Définir la couleur du gizmo.
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Vert avec 50% de transparence

        // Dessiner un cube représentant la zone d'apparition.
        Gizmos.DrawCube(spawnZoneCenter, spawnZoneSize);
    }
}