using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] waves;
    [SerializeField] ParticleSystem spawnEffect;
    [SerializeField] UnityEvent onFirstWave, onWavesFinished;
    [SerializeField] string[] tagsToConsider;

    bool _activated = false;
    int _currentWave = 0;
    BoxCollider _spawnerVolume;

    public void Activate()
    {
        if (!_activated)
        {
            _activated = true;
            NextWave();
        }
    }

    public void NextWave()
    {
        // Disable the object carrying the last wave
        if (_currentWave > 0)
            waves[_currentWave - 1].SetActive(false);

        // Enable the object carrying this wave
        // Instantiate a particle system at each child
        if (_currentWave < waves.Length)
        {
            waves[_currentWave].SetActive(true);

            if (spawnEffect)
            {
                foreach (var childTransform in waves[_currentWave].GetComponentsInChildren<Transform>())
                {
                    GameObject.Instantiate(spawnEffect, childTransform);
                }
            }

            // Do something on first wave (lock door for example)
            if (_currentWave == 0)
            {
                _activated = true;
                onFirstWave?.Invoke();
            }

            _currentWave++;
        }
        // If all waves finished, do something (open door for example)
        else
        {
            _activated = false;
            onWavesFinished?.Invoke();
            Destroy(gameObject);
        }
    }

    public void onFirstWaveTest()
    {
        Debug.Log("First wave action");
    }

    public void onWavesFinishedTest()
    {
        Debug.Log("All waves finished action");
    }


    private void Start()
    {
        _spawnerVolume = GetComponent<BoxCollider>();
        if (!_spawnerVolume.isTrigger)
        {
            Debug.LogWarning($"[{gameObject.name}] Spawner volume is not a trigger");
        }

        foreach (var wave in waves)
        {
            wave.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!_activated) return;

        // Using Linq could possibly be allocating in the backgroud
        // So it might be less performant
        Collider[] stillAliveThisWave = Physics
            .OverlapBox(_spawnerVolume.transform.position, _spawnerVolume.size * 0.5f);

        stillAliveThisWave = stillAliveThisWave
            .ToList()
            .Where(coll => tagsToConsider.Contains(coll.tag) && coll.gameObject.activeInHierarchy)
            .ToArray();

        Debug.Log($"{stillAliveThisWave.Length}");
        if (stillAliveThisWave.Length == 0)
            NextWave();
    }


}
