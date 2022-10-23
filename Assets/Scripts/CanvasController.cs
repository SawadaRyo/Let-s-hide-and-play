using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] GameObject[] _buttons = null;
    GameButton _gameButtons = null;

    void Start()
    {
        foreach (GameObject button in _buttons)
        {
            Button b = button.GetComponent<Button>();
            Image image = button.GetComponent<Image>();
            _gameButtons = new GameButton(b,image);
        }
    }

    public void ButtonState(GameObject gameButton,bool state)
    {
        //gameButton.button.enabled = true;
        //gameButton.image.enabled = true;
        gameButton.SetActive(state);
    }
}

public class GameButton
{
    public Button button;
    public Image image;

    public GameButton(Button button, Image image)
    {
        this.button = button;
        this.image = image;
    }
}