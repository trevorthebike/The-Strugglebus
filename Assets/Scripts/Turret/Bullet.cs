using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 5.0f;
    public Transform enemy;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = enemy.position - transform.position;
        rb.velocity = direction.normalized * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D Col){
        if (Col.gameObject.transform.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}