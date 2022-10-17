using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float _intervalTime = 1f;
    float _maxPleasureValue = 0f;
    float _pleasureValue = 0f;

    Interval _interval = null;
    void Start()
    {
        _interval = new Interval(_intervalTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && _interval.IsCountUp())
        {
            AddPleasure(5f);
            Debug.Log(_pleasureValue);
        }
    }
    float AddPleasure(float addVarue)
    {
        _pleasureValue += addVarue;
        _interval.ResetTimer();
        return _pleasureValue;
    }
}
