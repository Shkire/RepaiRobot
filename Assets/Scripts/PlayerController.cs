﻿using System.Collections;
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

    [SerializeField]
    private float _secondsToWasteExtenguiser;

    [SerializeField]
    private GameObject _extinguisherEffect;

    [SerializeField]
    private Transform _particlesParent;

    [SerializeField]
    private float _smoothTime;
    [SerializeField]
    private float _particlesStartSmoothTime;
    [SerializeField]
    private float _particlesEndSmoothTime;

    private float _smoothVelocity;
    private Vector2 _smoothParticlesVelocity;
    private Rigidbody2D _rigidBody;
    private float _horizontalValue;
    private float _verticalValue;
    private bool _usingStairs;
    private Stairs _stairs;
    private float _horizontalExtinguiserValue;
    private float _verticalExtinguisherValue;

    private float _extinguisherLevel = 1f;

    private void OnEnable()
    {
        if (_rigidBody == null)
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        float magnitude = _particlesParent.localScale.magnitude;
        if (magnitude > 0.01)
        {
            _extinguisherLevel -= Mathf.InverseLerp(0, _secondsToWasteExtenguiser, Time.deltaTime) * magnitude;
        }
        if (_extinguisherLevel <= 0)
        {
            _particlesParent.gameObject.SetActive(false);
        }


        _horizontalValue = Input.GetAxis("Horizontal");
        _verticalValue = Input.GetAxis("Vertical");
        _verticalExtinguisherValue = Input.GetAxis("VerticalExtinguiser");
        _horizontalExtinguiserValue = Input.GetAxis("HorizontalExtinguiser");

        Vector2 aux = new Vector2(_horizontalExtinguiserValue, _verticalExtinguisherValue);

        if (aux != Vector2.zero)
        {
            float angle = _extinguisherEffect.transform.rotation.eulerAngles.z;
            float newAngle = Vector2.SignedAngle(Vector2.right, aux);
            Quaternion auxQuat = Quaternion.Euler(0f, 0f, Mathf.SmoothDampAngle(angle, newAngle, ref _smoothVelocity, _smoothTime));
            _extinguisherEffect.transform.rotation = auxQuat;

            aux = _particlesParent.localScale;
            _particlesParent.localScale = Vector2.SmoothDamp(aux, Vector2.one, ref _smoothParticlesVelocity, _particlesStartSmoothTime);
        }
        else
        {
            _smoothVelocity = 0f;
            aux = _particlesParent.localScale;
            _particlesParent.localScale = Vector2.SmoothDamp(aux, Vector2.zero, ref _smoothParticlesVelocity, _particlesEndSmoothTime);
        }
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
                aux = _rigidBody.velocity;
                aux.y = 0f;
                _rigidBody.velocity = aux;
            }
        }

        if (_stairs != null && !_usingStairs)
        {
            if ((Mathf.Abs(transform.position.x - _stairs.transform.position.x) <= _stairsMargin) && ((transform.position.y > _stairs.topPosition.y && _verticalValue < 0) || (transform.position.y < _stairs.bottomPosition.y && _verticalValue > 0)))
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
