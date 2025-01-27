using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float speed;
    private Transform player;
    private Vector2 target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, 12);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroySpike();
        }
    }

    void onCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DestroySpike();
        }
    }
    void DestroySpike()
    {
        Destroy(gameObject);
    }
}