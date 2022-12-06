using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController : MonoBehaviour,IModel<int>
{
    [SerializeField, Tooltip("アイテムの所持数")] 
    int _itemIndex = 3;
    [SerializeField, Tooltip("プレイヤーの速度")] 
    float _speed = 5f;
    [SerializeField, Tooltip("フリックのノルム")] 
    float _flickValue = 250f;
    [SerializeField, Tooltip("タップした後もう一度タップするまでの感覚")] 
    float _tapInterval = 0.3f;
    [SerializeField, Tooltip("アイテムのオブジェクト")] 
    GameObject _item = null;

    bool _hiding = false;
    bool _doubleClickflg = false;
    int _clickCount = 0;
    Vector2 _startPos = Vector2.zero;
    Vector2 _endPos = Vector2.zero;
    Rigidbody2D _rb = null;
    Collider2D _coll = null;
    IntReactiveProperty _itemIndexInitial = new IntReactiveProperty(0);
    GameManager _gameManager;

    public bool Hiding => _hiding;
    public IReactiveProperty<int> ModelData => _itemIndexInitial;

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
            _gameManager.GameSet((int)GameState.Clear);
        }
        else if (collision.tag == "Grass")
        {
            IsHide(true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Grass")
        {
            IsHide(false);
        }
    }

    public void Init()
    {
        _itemIndexInitial.Value = _itemIndex;
        _gameManager = FindObjectOfType<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponentInChildren<Collider2D>();
        EnhancedTouchSupport.Enable();
    }
    public void Replay()
    {
        _itemIndexInitial.Value =  _itemIndex;
        _coll.enabled = true;
    }
    public void NotPlay()
    {
        _rb.velocity = Vector2.zero;
        _coll.enabled = false;
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


        if (_itemIndexInitial.Value > 0)
        {
            Instantiate(_item, this.transform.position, Quaternion.identity);
            _itemIndexInitial.Value--;
        }
        _doubleClickflg = false;

    }
    void IsHide(bool hiding)
    {
        _hiding = hiding;
        if(_rb.velocity.magnitude >= 0f && _hiding)
        {
            _hiding = !_hiding;
        }
    }
}

