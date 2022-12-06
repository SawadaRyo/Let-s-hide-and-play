using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Presenter : MonoBehaviour
{
    GameManager _gameManager = null;
    PlayerController _playerController = null;
    IView _view = null;
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _playerController = _gameManager.Player;
        ItemUI();
    }

    void ItemUI()
    {
        _playerController.ModelData
            .Subscribe(x =>
            _view.EventMethod(x)
            );
    }
}
