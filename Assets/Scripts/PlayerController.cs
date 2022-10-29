using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int _itemIndex = 3;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _flickValue = 250f;
    [SerializeField] float _tapInterval = 0.3f;
    [SerializeField] GameObject _item = null;

    bool _hiding = false;
    bool _doubleClickflg = false;
    int _itemIndexInitial = 0;
    int _clickCount = 0;
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
        EnhancedTouchSupport.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager) return;

        if (_gameManager.PlayGame)
        {
            if (Input.GetMouseButtonDown(0)
             || Input.GetButtonDown("Fire1"))
            {
                _rb.velocity = Vector2.zero;
                _startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0)
                 || Input.GetButtonUp("Fire1"))
            {
                _endPos = Input.mousePosition;
                TapAction(_startPos, _endPos);
            }

            if (Input.GetMouseButtonDown(1) && _itemIndex > 0)
            {
                Vector2 position = Input.mousePosition;
                Vector2 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
                Instantiate(_item, screenToWorldPointPosition, Quaternion.identity);
                _itemIndex--;
            }
        }
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Goal")
        {
            _gameManager.GameSet(2);
        }
        else if (collision.tag == "Grass")
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

    void TapAction(Vector2 startPos, Vector2 endPos)
    {
        if ((endPos - startPos).magnitude >= _flickValue)
        {
            Vector3 vec = (endPos - startPos).normalized;
            _rb.velocity = vec * _speed;
        }
        else if ((endPos - startPos).magnitude < _flickValue)
        {
            _clickCount++;
            Invoke("OnDoubleClick", _tapInterval);
        }

    }
    void OnDoubleClick()
    {
        if (_clickCount != 2) { _clickCount = 0; return; }
        else { _clickCount = 0; }


        if (_itemIndex > 0)
        {
            Vector2 position = Input.mousePosition;
            Vector2 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
            Instantiate(_item, screenToWorldPointPosition, Quaternion.identity);
            _itemIndex--;
        }
        _doubleClickflg = false;

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
