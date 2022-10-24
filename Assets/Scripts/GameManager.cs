using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CanvasController _canvasController;

    bool _playGame = false;
    int _stageIndex = 0;
    GameObject[] _stagePrefabs = null;
    Transform _spornTrans = null;
    Vector2 _spornPos = Vector2.zero;
    PlayerController _player;
    EnemyController[] _enemies = null;

    public bool PlayGame => _playGame;

    void Start()
    {
        _stagePrefabs = (GameObject[])Resources.LoadAll("Stage");
        Instantiate(_stagePrefabs[0]);
        _canvasController.ButtonState(_canvasController.Buttons[0],true);   
        _canvasController.ButtonState(_canvasController.Buttons[1],false);   
        _canvasController.ButtonState(_canvasController.Buttons[2],false);   
    }


    public void GameSet(int state)
    {
        switch ((GameState)state)
        {
            case GameState.Start:
                _canvasController.ButtonState(_canvasController.Buttons[0], false);
                _player = GameObject.FindObjectOfType<PlayerController>();
                _enemies = GameObject.FindObjectsOfType<EnemyController>();
                foreach (EnemyController enemy in _enemies)
                {
                    enemy.Init(this);
                }
                _player.Init();
                _spornTrans = _player.transform;
                _spornPos = _spornTrans.position;
                _playGame = true;
                break;

            case GameState.Playing:
                break;

            case GameState.Clear:
                _canvasController.ButtonState(_canvasController.Buttons[2], true);
                GameStop();
                Debug.Log("Clear");
                break;

            case GameState.GameOver:
                _canvasController.ButtonState(_canvasController.Buttons[1], true);
                _player.transform.position = _spornPos;
                GameStop();
                Debug.Log("Find");
                break;

            case GameState.RePlay:
                _canvasController.ButtonState(_canvasController.Buttons[1], false);
                foreach (EnemyController enemy in _enemies)
                {
                    enemy.Replay();
                }
                _player.Replay();
                _playGame = true;
                break;
            case GameState.Next:
                _canvasController.ButtonState(_canvasController.Buttons[2], false);
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
