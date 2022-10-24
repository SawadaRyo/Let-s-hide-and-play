using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int _itemIndex = 3; 
    [SerializeField] float _speed = 5f;
    [SerializeField] float _flickValue = 250f;
    [SerializeField] GameObject _item = null;

    bool _hiding = false;
    int _itemIndexInitial = 0;
    Vector2 _startPos = Vector2.zero;
    Vector2 _endPos = Vector2.zero;
    Rigidbody2D _rb = null;
    Collider2D _coll = null;
    GameManager _gameManager;

    public bool Hiding => _hiding;

    public void Init()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager) return;

        if (_gameManager.PlayGame)
        {
            if(Input.GetMouseButtonDown(0))
            {
                _rb.velocity = Vector2.zero;
                _startPos = Input.mousePosition;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                _endPos = Input.mousePosition;
                FlickMove(_startPos, _endPos);
            }

            if(Input.GetMouseButtonDown(1) && _itemIndex > 0)
            {
                Vector2 position = Input.mousePosition;
                Vector2 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
                Instantiate(_item,screenToWorldPointPosition, Quaternion.identity);
                _itemIndex--;
            }
        }
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

    void FlickMove(Vector2 startPos, Vector2 endPos)
    {
        if((endPos - startPos).magnitude >= _flickValue)
        {
            Vector3 vec = (endPos - startPos).normalized;
            _rb.velocity = vec * _speed;
        }
        
    }

    public void Replay()
    {
        _itemIndex = _itemIndexInitial;
        _coll.enabled = true;
    }
    public void NotPlay()
    {
        _rb.velocity = Vector2.zero;
        _coll.enabled = false;
    }
}
