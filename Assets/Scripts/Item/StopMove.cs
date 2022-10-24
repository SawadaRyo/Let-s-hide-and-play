using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMove : MonoBehaviour
{
    [SerializeField] float _radius = 3f;
    [SerializeField] float _remainTime = 1f;
    [SerializeField] float _stopTime = 1f;

    float _time = 0f;

    private void OnEnable()
    {
        StartCoroutine(Enumerator(_remainTime));
    }

    IEnumerator Enumerator(float remainTime)
    {
        while (true)
        {
            _time += Time.deltaTime;
            InRenge();
            if (_time >= _remainTime)
            {
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }
    void InRenge()
    {
        Collider2D[] sightColls = Physics2D.OverlapCircleAll(transform.position, _radius);
        if (sightColls.Length > 0)
        {
            foreach (Collider2D col in sightColls)
            {
                if (col.TryGetComponent<EnemyController>(out EnemyController e))
                {
                    e.StartCoroutine(e.MoveInterval(_stopTime));
                    Destroy(this.gameObject, _remainTime - _time);
                }
            }
        }
    }
}
