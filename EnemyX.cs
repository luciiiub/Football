using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed = 150; //Asignamos velocidad (EDITSDO OLD- speed;)
    private Rigidbody enemyRb;
    private GameObject playerGoal;  //Mencionado aqui pero no en el start,dando error desde el inicio !!
    private GameObject player;   // Referencia al jugador (AÑADIDO!!)

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerGoal = GameObject.Find("Player Goal");  // Asignas playerGoal (AÑADIDO!!)
        player = GameObject.Find("Player");           // Asignas jugador (AÑADIDO!!)
        
    }

    // Update is called once per frame
    void Update()
    {
        //AÑADIDO POR MEJORA!!!
        // Se mueve hacia una posicion detras del jugador para empujarle hacia la porteria
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized; //mueve hacia jugador 
        Vector3 playerToGoal = (playerGoal.transform.position - player.transform.position).normalized; //para q intente meter al jugador en la porteria 

        float followDistance = 2.0f;  // Distancia detras del jugador 
        Vector3 targetPosition = player.transform.position - playerToGoal * followDistance;

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        enemyRb.AddForce(moveDirection * speed); //QUITAMOS EL DELTATIME PARA QUE LAS PELOTAS SE MUEVAN,SI NO QUEDAN
    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with Enemy Goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            Destroy(gameObject); // Destruir enemigo si toca porteruia
        }
    }
}
