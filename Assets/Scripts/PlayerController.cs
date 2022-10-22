using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] GameManager _gameManager;

    bool _hiding = false;
    Rigidbody2D _rb = null;
    Collider2D _coll = null;

    public bool Hiding => _hiding;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.PlayGame)
        {
            _speed = 5;
            Move();
        }
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 vecNor = new Vector2(h, v).normalized;
        _rb.velocity = vecNor * _speed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Goal")
        {
            _gameManager.GameSet(2);
        }
        else if(collision.tag == "Grass")
        {
            _hiding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Grass")
        {
            _hiding = false;
        }
    }

    public void Playing()
    {
        _coll.enabled = true;
    }
    public void NotPlay()
    {
        _rb.velocity = Vector2.zero;
        _coll.enabled = false;
    }
}
