using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;
    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    //AÑADIDOS!!
    public ParticleSystem boostParticles; // Efecto de turbo boost   
    public float boostStrength = 10f;     // Fuerza del impulso
    private float boostCooldown = 7f;     // Cooldown 
    private float nextBoostTime = 0f;     // Proxima vez que se puede usar boost

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        // TURBO BOOST: Si se presiona espacio, aplicar impulso y activar particulas!!!
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextBoostTime)
        {
            playerRb.AddForce(focalPoint.transform.forward * boostStrength, ForceMode.Impulse);
            boostParticles.Play(); // Activa el sistema de particulas
            nextBoostTime = Time.time + boostCooldown; // Marcar cuando se puede volver a usar
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;  // Activar el power-up
            powerupIndicator.SetActive(true);  // Mostrar el indicador visual

            Destroy(other.gameObject);  // Eliminar el power-up de la escena
            StartCoroutine(PowerupCooldown());   // CUENTA ATRAS!! 
        }

        if (other.gameObject.name == "Player Goal") // DETECTA TRIGGER DE LA PORTERIA!!
        {
            Debug.Log("¡Game Over!"); // GAME OVER MENSAJE
        }
    }

    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(7); //!!OLD-(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        //AÑADIDO IDEA MEJORA!!
        if (other.gameObject.name == "Player Goal")
    {
        Debug.Log("¡Game Over!");

        // BuUSCA SPANW MANAGER Y RESETEA OLEADA
        GameObject spawnManagerObj = GameObject.Find("Spawn Manager");
        if (spawnManagerObj != null)
        {
            SpawnManagerX spawnManager = spawnManagerObj.GetComponent<SpawnManagerX>();
            if (spawnManager != null)
            {
                spawnManager.waveCount = 1; //TE DEVUELVE A LA OLEADA 1
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position; 

            if (hasPowerup)
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }
}
