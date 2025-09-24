using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Spawning Configuration")]
    [Tooltip("An array of all the different animal prefabs you want to spawn.")]
    public GameObject[] animalPrefabs;

    [Tooltip("The time in seconds between each spawn.")]
    public float spawnInterval = 2.0f;

    [Header("Spawn Zone")]
    [Tooltip("The center point of the area where animals will be spawned.")]
    public Vector3 spawnZoneCenter;

    [Tooltip("The size (width, height, depth) of the spawn area.")]
    public Vector3 spawnZoneSize = new Vector3(20, 0, 10);

    // A timer to keep track of when to spawn the next animal.
    private float spawnTimer;

    void Start()
    {
        // Initialize the timer.
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        // Count down the timer.
        spawnTimer -= Time.deltaTime;

        // When the timer reaches zero, spawn an animal and reset the timer.
        if (spawnTimer <= 0f)
        {
            SpawnAnimal();
            spawnTimer = spawnInterval;
        }
    }

    /// <summary>
    /// Spawns a random animal at a random position within the defined zone.
    /// </summary>
    private void SpawnAnimal()
    {
        // First, check if there are any prefabs to spawn.
        if (animalPrefabs == null || animalPrefabs.Length == 0)
        {
            Debug.LogWarning("No animal prefabs assigned to the spawner.");
            return;
        }

        // Pick a random animal prefab from the array.
        int randomIndex = Random.Range(0, animalPrefabs.Length);
        GameObject animalToSpawn = animalPrefabs[randomIndex];

        // Calculate a random position within the spawn zone.
        float randomX = Random.Range(-spawnZoneSize.x / 2, spawnZoneSize.x / 2);
        float randomY = Random.Range(-spawnZoneSize.y / 2, spawnZoneSize.y / 2);
        float randomZ = Random.Range(-spawnZoneSize.z / 2, spawnZoneSize.z / 2);
        Vector3 spawnPosition = spawnZoneCenter + new Vector3(randomX, randomY, randomZ);

        // Define the rotation to face the -Z axis (180 degrees on the Y axis).
        Quaternion spawnRotation = Quaternion.Euler(0, 180f, 0);

        // Create an instance of the chosen animal prefab at the calculated position and rotation.
        Instantiate(animalToSpawn, spawnPosition, spawnRotation);
    }

    /// <summary>
    /// This is a special Unity function that draws gizmos in the Scene view.
    /// It helps visualize the spawn zone without needing a visible object.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Set the color for the gizmo.
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Green with 50% transparency

        // Draw a cube representing the spawn zone.
        Gizmos.DrawCube(spawnZoneCenter, spawnZoneSize);
    }
}

