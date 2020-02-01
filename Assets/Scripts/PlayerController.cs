using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float Speed = 1f;
    private Rigidbody2D _rigidBody;
    private float _horizontalValue;
    private float _verticalValue;
    private bool _onStair;
    private bool _usingStairs;

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
        if (_onStair && !_usingStairs)
        {
            if (_verticalValue != 0)
            {
                _usingStairs = true;
            }
        }


        if (_usingStairs)
        {
            _rigidBody.isKinematic = true;
            _rigidBody.velocity = new Vector2(0f, _verticalValue);
            return;
        }

        Vector3 aux = _rigidBody.velocity;
        aux.x = _horizontalValue * Speed;
        _rigidBody.velocity = aux;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stair")
        {
            _onStair = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Stair")
        {
            _onStair = false;
            _usingStairs = false;
            _rigidBody.isKinematic = false;
        }
    }
}
