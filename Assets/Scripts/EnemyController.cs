using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _radius = 2f;
    [SerializeField] float _moveDir = 2f;
    [SerializeField] float _moveInterval = 2f;
    [SerializeField] float _movePos = 2f;
    [SerializeField] Transform _sightCenter = null;
    [SerializeField] MovePaturn _movePaturn;

    GameManager _gameManager;
    Sequence _sequence;
    Sequence _sequence2;

    public bool Discovered => InSight();
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        Move(_moveInterval);
    }




    bool InSight()
    {
        Collider2D[] sightColls = Physics2D.OverlapCircleAll(_sightCenter.position, _radius);
        if (sightColls.Length > 0)
        {
            foreach (Collider2D col in sightColls)
            {
                if (col.GetComponent<PlayerController>() != null)
                {
                    return true;
                }
            }

        }
        return false;
    }

    void Move(float interval)
    {
        Vector3 objPos = this.transform.position;
        switch (_movePaturn)
        {
            case MovePaturn.Horizontal:
                this.transform.position = new Vector2(Mathf.Sin(_moveDir * Mathf.PI * _moveInterval) + objPos.x,  objPos.y);
                break;
            case MovePaturn.Vertical:
                this.transform.position = new Vector2(objPos.x, Mathf.Sin(_moveDir * Mathf.PI * _moveInterval) + objPos.y);
                break;
            case MovePaturn.Random:
                break;
            default:
                break;
        }
    }



    void Rotate(float interval)
    {
        _sequence2 = DOTween.Sequence();
        var tweener = this.transform.DORotate(new Vector3(0f, 0f, this.transform.rotation.z+180f), interval, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .OnStart(() => _sequence.Pause())
            .OnUpdate(() => InSight())
            .OnComplete(() => _sequence.Play());
        _sequence2.Append(tweener);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sightCenter.position, _radius);
        Gizmos.DrawLine(this.transform.position, new Vector3(this.transform.position.x + _movePos, 0f, 0f));
        Gizmos.DrawLine(this.transform.position, new Vector3(0f, this.transform.position.y + _movePos, 0f));
    }

    public enum MovePaturn
    {
        Horizontal,
        Vertical,
        Random
    }
}
