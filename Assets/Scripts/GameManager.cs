using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController _player;
    bool _playGame = false;
    Transform _spornTrans = null;
    Vector2 _spornPos = Vector2.zero;
    EnemyController[] _enemies = null;

    public bool PlayGame => _playGame;

    public void ContinueGame()
    {
        _player.transform.position = _spornTrans.position;
    }
    public void GameSet(int state)
    {
        switch ((GameState)state)
        {
            case GameState.Start:
                _enemies = GameObject.FindObjectsOfType<EnemyController>();
                foreach (EnemyController enemy in _enemies)
                {
                    enemy.Init(this);
                }
                _spornTrans = _player.transform;
                _spornPos = _spornTrans.position;
                _playGame = true;
                break;

            case GameState.Playing:
                break;
            case GameState.Clear:
                foreach (EnemyController enemy in _enemies)
                {
                    enemy.Stop();
                }
                _player.NotPlay();
                _playGame = false;
                Debug.Log("Clear");
                break;

            case GameState.GameOver:
                foreach (EnemyController enemy in _enemies)
                {
                    enemy.Stop();
                }
                _player.NotPlay();
                _playGame = false;
                Debug.Log("Find");
                break;

            case GameState.RePlay:
                _player.transform.position = _spornPos;
                foreach (EnemyController enemy in _enemies)
                {
                    enemy.Replay();
                }
                _player.Playing();
                _playGame = true;
                break;
        }
    }

}
public enum GameState
{
    Start = 0,
    Playing = 1,
    Clear = 2,
    GameOver = 3,
    RePlay = 4
}
