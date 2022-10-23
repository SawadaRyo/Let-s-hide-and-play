using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CanvasController _canvasController;

    bool _playGame = false;
    GameObject[] _stagePrefabs = null;
    Transform _spornTrans = null;
    Vector2 _spornPos = Vector2.zero;
    PlayerController _player;
    EnemyController[] _enemies = null;

    public bool PlayGame => _playGame;

    void Start()
    {
        _stagePrefabs = (GameObject[])Resources.LoadAll("Stage");
    }


    public void GameSet(int state)
    {
        switch ((GameState)state)
        {
            case GameState.Start:
                _player = GameObject.FindObjectOfType<PlayerController>();
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
                GameStop();
                Debug.Log("Clear");
                break;

            case GameState.GameOver:
                _player.transform.position = _spornPos;
                GameStop();
                Debug.Log("Find");
                break;

            case GameState.RePlay:
                foreach (EnemyController enemy in _enemies)
                {
                    enemy.Replay();
                }
                _player.Playing();
                _playGame = true;
                break;
            case GameState.Next:
                break;
        }
    }

    void GameStop()
    {
        foreach (EnemyController enemy in _enemies)
        {
            enemy.Stop();
        }
        _player.NotPlay();
        _playGame = false;
    }
}
public enum GameState
{
    Start = 0,
    Playing = 1,
    Clear = 2,
    GameOver = 3,
    RePlay = 4,
    Next = 5
}
