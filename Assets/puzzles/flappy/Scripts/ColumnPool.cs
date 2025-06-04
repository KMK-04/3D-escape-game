using UnityEngine;

public class ColumnPool : MonoBehaviour
{
    public int columnPoolSize = 5;

    public GameObject columnPrefab;
    public float spawnRate = 4f;
    public float columnMin = -2.5f;
    public float columMax = 1f;

    private GameObject[] columns;
    private Vector2 objectPoolPosition = new Vector2 (-15f, -25f);
    private float timeSinceLastSpawned;
    private float spawnXPosition = 10f;
    private int currentColumn = 0;

    private void Start() {
        columns = new GameObject[columnPoolSize];

        for (int i = 0; i < columnPoolSize; i++) {
            columns[i] = (GameObject)Instantiate(columnPrefab, objectPoolPosition, Quaternion.identity);
        }
    }

    private void Update() {
        timeSinceLastSpawned += Time.deltaTime;
        if(GameControl.instance.isGameover != true && timeSinceLastSpawned >= spawnRate ) {
            timeSinceLastSpawned = 0f;
            float spawnYPosition = Random.Range(columnMin, columMax);
            columns[currentColumn].transform.position = new Vector2(spawnXPosition, spawnYPosition);
            currentColumn++;
            if(currentColumn >= columnPoolSize) {
                currentColumn = 0;
            }
        }
    }
}