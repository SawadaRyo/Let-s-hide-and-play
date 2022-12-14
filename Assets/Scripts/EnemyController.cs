using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _radius = 0.75f;
    [SerializeField] float _moveInterval = 2f;
    [SerializeField] float _rotInterval = 2f;
    [SerializeField] float _movePos = 2f;
    [SerializeField] GameObject _sightCenter = null;
    [SerializeField] Transform[] _sightPoints = new Transform[3];
    [SerializeField] MovePaturn _movePaturn;

    GameManager _gameManager;
    PlayerController _playerController = null;
    Tween[] _tweens = new Tween[2];
    

    public void Init(GameManager gameManager,PlayerController playerController)
    {
        _gameManager = gameManager;
        _playerController = playerController;
        Move(_movePos, _moveInterval);
    }

    public IEnumerator MoveInterval(float interval)
    {
        _sightCenter.SetActive(false);
        Stop();
        yield return new WaitForSeconds(interval);
        _sightCenter.SetActive(true);
        Replay();
    }

    void PlayerCompornet(PlayerController playerTrans)
    {
        InBady(playerTrans);
        InSight(playerTrans);
    }

    void InSight(PlayerController playerTrans)
    {
        Vector3 vec1 = _sightPoints[1].position - _sightPoints[0].position;
        Vector3 vecP1 = playerTrans.transform.position - _sightPoints[1].position;
        Vector3 vec2 = _sightPoints[2].position - _sightPoints[1].position;
        Vector3 vecP2 = playerTrans.transform.position - _sightPoints[2].position;
        Vector3 vec3 = _sightPoints[0].position - _sightPoints[2].position;
        Vector3 vecP3 = playerTrans.transform.position - _sightPoints[0].position;

        float c1 = vec1.x * vecP1.y - vec1.y * vecP1.x;
        float c2 = vec2.x * vecP2.y - vec2.y * vecP2.x;
        float c3 = vec3.x * vecP3.y - vec3.y * vecP3.x;

        if(!playerTrans.Hiding
        && c1 > 0 && c2 > 0 && c3 > 0
        || c1 < 0 && c2 < 0 && c3 < 0)
        {
            _gameManager.GameSet((int)GameState.GameOver);
        }
    }
    void InBady(PlayerController playerTrans)
    {
        if(!playerTrans.Hiding
        && (playerTrans.transform.position - transform.position).magnitude < _radius)
        {
            _gameManager.GameSet((int)GameState.GameOver);
        }
    }
    void Rotate(float interval)
    {
        _tweens[0] = this.transform.DORotate(new Vector3(0f, 0f, 180f), interval, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .OnUpdate(() => PlayerCompornet(_playerController))
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
            .OnUpdate(()=> PlayerCompornet(_playerController))
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
        Gizmos.DrawLine(_sightPoints[0].position, _sightPoints[1].position);
        Gizmos.DrawLine(_sightPoints[1].position, _sightPoints[2].position);
        Gizmos.DrawLine(_sightPoints[2].position, _sightPoints[0].position);
    }
#endif
}


