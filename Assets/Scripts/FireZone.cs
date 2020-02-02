using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float value;

    private List<Collider2D> _colliders;
    private ParticleSystem _particles;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_colliders == null)
        {
            _colliders = new List<Collider2D>();
        }

        if (!_colliders.Contains(collision))
        {
            _colliders.Add(collision);
        }
    }

    private void Update()
    {
        if (_colliders != null)
        {
            foreach (Collider2D collider in _colliders)
            {
                ExtinguiserConfig config = collider.GetComponent<ExtinguiserConfig>();
                if (config != null)
                {
                    value -= Time.deltaTime * config._scaleCurve.Evaluate(new Vector2(config.parent.localScale.x, config.parent.localScale.y).magnitude) * config._distanceCurve.Evaluate(Vector2.Distance(transform.position, collider.transform.position));
                }
            }
            _colliders.Clear();
        }

        if (value <= 0)
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        _particles = GetComponent<ParticleSystem>();
        _particles.Play();
    }

    private void OnDisable()
    {
        _particles.Stop();

    }
}
