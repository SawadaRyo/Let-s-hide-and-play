using System;
using System.Collections;
using System.Linq;
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
    Tween[] _tweens = new Tween[2];

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
        Move(_movePos, _moveInterval);
    }

    IEnumerator MoveInterval(float interval)
    {
        Stop();
        yield return new WaitForSeconds(interval);
        Replay();
    }
    void InSight()
    {
        if (!_gameManager.PlayGame) return;

        Collider2D[] sightColls = Physics2D.OverlapCircleAll(_sightCenter.position, _radius);
        if (sightColls.Length > 0)
        {
            foreach (Collider2D col in sightColls)
            {
                if (col.TryGetComponent<PlayerController>(out PlayerController p))
                {
                    if (p.Hiding) return;
                    _gameManager.GameSet((int)GameState.GameOver);
                }
            }
        }
    }
    void Rotate(float interval)
    {
        _tweens[0] = this.transform.DORotate(new Vector3(0f, 0f, 180f), interval, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .OnUpdate(() => InSight())
            .OnComplete(() => Move(_movePos *= -1, _moveInterval));
    }
    void Move(float movePos, float interval)
    {
        switch (_movePaturn)
        {
            case MovePaturn.Horizontal:
                _tweens[1] = this.transform.DOLocalMoveX(movePos, interval);
                break;
            case MovePaturn.Vertical:
                _tweens[1] = this.transform.DOLocalMoveY(movePos, interval);
                break;
            case MovePaturn.Random:
                break;
            default:
                break;
        }

        _tweens[1]
            .SetEase(Ease.Linear)
            .OnUpdate(()=>InSight())
            .OnComplete(() => Rotate(_rotInterval));
    }
    public void Stop()
    {
        foreach(Tween t in _tweens)
        {
            t.Pause();
        }
    }
    public void Replay()
    {
        foreach (Tween t in _tweens)
        {
            t.Play();
        }
    }

    public enum MovePaturn
    {
        Horizontal,
        Vertical,
        Random
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sightCenter.position, _radius);
        Gizmos.DrawLine(this.transform.position, new Vector3(this.transform.position.x + _movePos, this.transform.position.y, this.transform.position.z));
        Gizmos.DrawLine(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y + _movePos, this.transform.position.z));
    }
#endif
}


