using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VibrationCloseEnemy
{
    float _standardOfDistance = 1f;
    float _intervaltime = 0;
    float _norm = 0f;
    long _vibSeconds = 60;
    PlayerController _player = null;
    Vector3[] _enemies = null;

    public VibrationCloseEnemy(float standardOfDistance, PlayerController player, EnemyController[] enemies)
    {
        _standardOfDistance = standardOfDistance;
        _player = player;
        _enemies = new Vector3[enemies.Length];
        for (int i = 0;i < enemies.Length;i++)
        {
            _enemies[i] = enemies[i].transform.position;
        }
    }

    public void Vibration()
    {
        Vector3 nereEnemy = Vector3.zero;
        nereEnemy = _enemies.OrderBy(x => Vector3.Distance(_player.transform.position, x)).First();
        _norm = Mathf.Sqrt(Mathf.Pow(nereEnemy.x, 2) + Mathf.Pow(nereEnemy.y, 2));
        if(_norm <= _standardOfDistance)
        {
            
        }
    }
}
