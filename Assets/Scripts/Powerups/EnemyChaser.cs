using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField]
    private float _senseDistance;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _detectionDistance;

    [SerializeField]
    private GameObject[] _enemy = new GameObject[3];

    [SerializeField]
    private bool[] _chasing = new bool[3];

    private GameObject _currentTarget = null;

    void Start()
    {
        // Initialization if needed
    }

    void Update()
    {
        if (_currentTarget == null)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            CalculateEnemyChase();
        }
        else
        {
            ChaseCurrentTarget();
        }

        CheckDestroyCondition();
    }

    private void CalculateEnemyChase()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _senseDistance, Vector2.up);

        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            if (!_chasing[0] && !_chasing[1] && !_chasing[2])
            {
                _enemy[0] = hit.collider.gameObject;
                _currentTarget = _enemy[0];
                _chasing[0] = true;
            }
            else if (!_chasing[0] && !_chasing[1])
            {
                _enemy[1] = hit.collider.gameObject;
                _currentTarget = _enemy[1];
                _chasing[1] = true;
            }
            else if (!_chasing[0] && !_chasing[2])
            {
                _enemy[2] = hit.collider.gameObject;
                _currentTarget = _enemy[2];
                _chasing[2] = true;
            }
        }
    }

    private void ChaseCurrentTarget()
    {
        if (_currentTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.transform.position, _speed * Time.deltaTime);

            // Optionally, stop chasing if reached close enough to the target
            if (Vector3.Distance(transform.position, _currentTarget.transform.position) < _detectionDistance)
            {
                // Handle what happens when close to the target, e.g., stop chasing or perform an action
            }
        }
    }

    private void CheckDestroyCondition()
    {
        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}