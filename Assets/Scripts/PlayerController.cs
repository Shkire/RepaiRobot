using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _climbSpeed = 1f;

    [SerializeField]
    private float _stairsMargin;
    private Rigidbody2D _rigidBody;
    private float _horizontalValue;
    private float _verticalValue;
    private bool _usingStairs;
    private Stairs _stairs;

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
            if (transform.position.y > _stairs.topPosition.y || transform.position.y < _stairs.bottomPosition.y)
            {
                _rigidBody.isKinematic = false;
                _usingStairs = false;
            }
        }

        if (_stairs != null && !_usingStairs)
        {
            if ((Mathf.Abs(transform.position.x - _stairs.transform.position.x) <= _stairsMargin) && (transform.position.y > _stairs.topPosition.y && _verticalValue < 0) || (transform.position.y < _stairs.bottomPosition.y && _verticalValue > 0))
            {
                _usingStairs = true;
                _rigidBody.isKinematic = true;
                aux = transform.position;
                if (_verticalValue > 0)
                {
                    aux.y = _stairs.bottomPosition.y;
                }
                else
                {
                    aux.y = _stairs.topPosition.y;
                }
                transform.position = aux;
            }
        }


        if (_usingStairs)
        {
            _rigidBody.velocity = new Vector2(0f, _verticalValue * _climbSpeed);
            return;
        }

        aux = _rigidBody.velocity;
        aux.x = _horizontalValue * _speed;
        _rigidBody.velocity = aux;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stair")
        {
            _stairs = collision.GetComponent<Stairs>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Stair")
        {
            _stairs = null;
        }
    }
}
