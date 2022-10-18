using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    float _intervalTime = 1f;

    Rigidbody2D _rb = null;

    Interval _interval = null;
    void Start()
    {
        _interval = new Interval(_intervalTime);
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
   
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 vecNor = new Vector2(h, v).normalized;
        _rb.velocity = vecNor * 5;
    }
}
