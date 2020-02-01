using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float Speed = 1f;

    [SerializeField]
    private float _stairsMargin;
    private Rigidbody2D _rigidBody;
    private float _horizontalValue;
    private float _verticalValue;
    private bool _usingStairs;
    private Collider2D _collider2D;

    private void OnEnable()
    {
        if (_rigidBody == null)
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        _horizontalValue = Input.GetAxis("Horizontal");
        _verticalValue = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 aux = Vector3.zero;
        if (_usingStairs)
        {
            if (transform.position.y > _collider2D.bounds.max.y - _collider2D.bounds.extents.y * _stairsMargin * 0.5f || transform.position.y < _collider2D.bounds.min.y + _collider2D.bounds.extents.y * _stairsMargin * 0.5f)
            {
                _rigidBody.isKinematic = false;
                _usingStairs = false;
            }
        }

        if (_collider2D != null && !_usingStairs)
        {
            if (_verticalValue != 0)
            {
                if ((transform.position.y > _collider2D.bounds.max.y - _collider2D.bounds.extents.y * _stairsMargin * 0.5f && _verticalValue < 0) || (transform.position.y < _collider2D.bounds.min.y + _collider2D.bounds.extents.y * _stairsMargin * 0.5f && _verticalValue > 0))
                {
                    _usingStairs = true;
                    _rigidBody.isKinematic = true;
                    aux = transform.position;
                    if (_verticalValue > 0)
                    {
                        aux.y = _collider2D.bounds.min.y + _collider2D.bounds.extents.y * _stairsMargin * 0.5f;
                    }
                    else
                    {
                        aux.y = _collider2D.bounds.max.y - _collider2D.bounds.extents.y * _stairsMargin * 0.5f;
                    }
                }
            }
        }


        if (_usingStairs)
        {
            _rigidBody.velocity = new Vector2(0f, _verticalValue);
            return;
        }

        aux = _rigidBody.velocity;
        aux.x = _horizontalValue * Speed;
        _rigidBody.velocity = aux;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stair")
        {
            _collider2D = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Stair")
        {
            _collider2D = null;
        }
    }
}
