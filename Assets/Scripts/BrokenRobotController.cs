using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenRobotController : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOver;

    [SerializeField]
    private FireZone[] _fireZones;

    [SerializeField]
    private AnimationCurve _maximumFireZones;

    [SerializeField]
    private AnimationCurve _timeBetweenFireZones;

    [SerializeField]
    private AnimationCurve _fireZonesValue;

    [SerializeField]
    private AnimationCurve _fireZonesGameOverTime;

    private List<FireZone> _activeFireZones;
    private List<FireZone> _creatingFireZones;

    private float _time;

    private Dictionary<FireZone, Coroutine> _gameOverCoroutines;

    // Update is called once per frame

    private void OnEnable()
    {
        Time.timeScale = 1;
        _activeFireZones = new List<FireZone>();
        _creatingFireZones = new List<FireZone>();
        _gameOverCoroutines = new Dictionary<FireZone, Coroutine>();
    }

    void Update()
    {
        _time += Time.deltaTime;

        for (int i = 0; i < _activeFireZones.Count; i++)
        {
            if (_activeFireZones[i].enabled == false)
            {
                StopCoroutine(_gameOverCoroutines[_activeFireZones[i]]);
                _gameOverCoroutines.Remove(_activeFireZones[i]);
                _activeFireZones.Remove(_activeFireZones[i]);
                i--;
            }
        }

        if (_activeFireZones.Count + _creatingFireZones.Count < _maximumFireZones.Evaluate(_time))
        {
            StartCoroutine(CreateFireZoneCoroutine());
        }
    }

    private IEnumerator CreateFireZoneCoroutine()
    {
        FireZone fireZone = null;
        do
        {
            fireZone = _fireZones[Random.Range(0, _fireZones.Length)];
        }
        while (fireZone == null || _activeFireZones.Contains(fireZone) || _creatingFireZones.Contains(fireZone));

        _creatingFireZones.Add(fireZone);

        yield return new WaitForSeconds(_timeBetweenFireZones.Evaluate(_time));

        fireZone.value = _fireZonesValue.Evaluate(_time);

        fireZone.enabled = true;

        _creatingFireZones.Remove(fireZone);
        _activeFireZones.Add(fireZone);
        _gameOverCoroutines.Add(fireZone, StartCoroutine(GameOverCoroutine()));
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(_fireZonesGameOverTime.Evaluate(_time));

        _gameOver.SetActive(true);
        Time.timeScale = 0;
    }
}
