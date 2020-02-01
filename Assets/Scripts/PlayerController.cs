using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float Speed = 1f;
    private Rigidbody2D _rigidBody;
    private float _horizontalValue;

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


    }

    private void FixedUpdate()
    {
        Vector3 aux = _rigidBody.velocity;
        aux.x = _horizontalValue * Speed;
        _rigidBody.velocity = aux;
    }
}
