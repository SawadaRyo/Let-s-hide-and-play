using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VibrationController
{
    PlayerController _player = null;
    Vector3[] _enemies = null;
    public void SetValue(PlayerController player, EnemyController[] enemies)
    {
        var i = 0;
        _player = player;
        foreach (EnemyController enemy in enemies)
        {
            _enemies[i] = enemy.transform.position;
            i++;
        }
    }

    public void Vibration()
    {
        float nereEnemy = 0f;
        _enemies.OrderBy(x => nereEnemy = Vector3.Distance(_player.transform.position, x)).First();
        Debug.Log(nereEnemy);
        //if(nereEnemy)
    }
}
