using UnityEngine;

public class SpawnCubes : MonoBehaviour
{
    [SerializeField] private GameObject body = null;
    [SerializeField] private int numberOfBodiesToSpawn = 100000;
    public int numberOfBodiesToSpawnAtOnce = 1;
    public float timeToSpawnBody = 0f;
    private float timeSinceLastSpawn = 0f;
    private int numberOfSpawnedBodies = 0;

    // Update is called once per frame
    private void Update()
    {
        if (timeSinceLastSpawn > timeToSpawnBody && numberOfSpawnedBodies < numberOfBodiesToSpawn)
        {
            for (int i = 0; i < numberOfBodiesToSpawnAtOnce; i++)
            {
                Instantiate(body, transform.position + Random.insideUnitSphere * 10f, Quaternion.identity);
                numberOfSpawnedBodies++;
                timeSinceLastSpawn = 0f;
            }
        }
        timeSinceLastSpawn += Time.deltaTime;
    }
}