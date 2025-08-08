using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    private float spawnRangeX = 10;
    private float spawnZMin = 15; // set min spawn Z
    private float spawnZMax = 25; // set max spawn Z

    public int enemyCount;
    public int waveCount = 1;

    public GameObject player; 

    public float enemySpeed = 1.0f;  // Velocidad base de los enemigos

    // AÑADIDO: para esperar 5 segundos tras recoger el powerup
    private bool powerupRespawnScheduled = false;

    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }

        // Si no hay powerup 
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0 && !powerupRespawnScheduled)
        {
            StartCoroutine(RespawnPowerupAfterDelay(5f)); // esperar 5 segundos antes de reaparecerlo
        }
    }

    // Corrutina para reaparecer el powerup despues de cierto tiempo
    IEnumerator RespawnPowerupAfterDelay(float delay)
    {
        powerupRespawnScheduled = true;
        yield return new WaitForSeconds(delay);

        Vector3 randomOffset = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
Instantiate(powerupPrefab, GenerateSpawnPosition() + randomOffset, powerupPrefab.transform.rotation);



        powerupRespawnScheduled = false;
    }

    Vector3 GenerateSpawnPosition ()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15);

        // Solo instanciar el primer powerup si no hay ninguno en la escena (por ejemplo, al iniciar)
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0 && waveCount == 1)
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        for (int i = 0; i < waveCount; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            newEnemy.GetComponent<EnemyX>().speed = enemySpeed;
        }

        enemySpeed += 0.5f;
        waveCount++;
        ResetPlayerPosition();
    }

    void ResetPlayerPosition ()
    {
        player.transform.position = new Vector3(0, 1, -7);
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
