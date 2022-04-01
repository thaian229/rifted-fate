using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;

public class SimpleChase : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 5f;

    void Awake() 
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 chaseDirection = (player.position - this.transform.position).normalized;
            this.transform.Translate(chaseDirection * chaseSpeed * Time.deltaTime);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == "Player")
        {
            LevelManager.instance.GameOver();
        }
    }

    public void OnTriggerEnter(Collider other) 
    {
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == "Player")
        {
            LevelManager.instance.GameOver();
        }
    }
}
