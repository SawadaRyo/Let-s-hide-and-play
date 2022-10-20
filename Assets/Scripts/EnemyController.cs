using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _radius = 2f;
    [SerializeField] float _moveInterval = 2f;
    [SerializeField] float _rotInterval = 2f;
    [SerializeField] float _movePos = 2f;
    [SerializeField] Transform _sightCenter = null;
    [SerializeField] MovePaturn _movePaturn;

    GameManager _gameManager;
    Sequence _allMotion = null;
    Sequence _sequence1 = null;
    Sequence _sequence2 = null;

    public bool Discovered => InSight();
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        Move(_movePos, _moveInterval);
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

    void Rotate(float interval)
    {
        _sequence2 = DOTween.Sequence();
        Tween tweener = this.transform.DORotate(new Vector3(0f, 0f, 180f), interval, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .OnComplete(() => _sequence1.Play());
        _sequence2.Append(tweener);
    }
    void Move(float movePos, float interval)
    {
        _sequence1 = DOTween.Sequence();
        Tween tweener1 = null;
        switch (_movePaturn)
        {
            case MovePaturn.Horizontal:
                tweener1 = this.transform.DOLocalMoveX(movePos, interval);
                break;
            case MovePaturn.Vertical:
                tweener1 = this.transform.DOLocalMoveY(movePos, interval);
                break;
            case MovePaturn.Random:
                break;
            default:
                break;
        }

        tweener1.SetEase(Ease.Linear).OnComplete(() => Rotate(_rotInterval));
        _sequence1.Append(tweener1);
    }

    IEnumerator EnemyMotion(float interval)
    {
        yield return new WaitForSeconds(interval);
        _sequence1.Pause();
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
