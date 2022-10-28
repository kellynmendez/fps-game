using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // The game object that the pool will hold
    [SerializeField] GameObject _poolObject;
    // How many enemies to start with
    [SerializeField] int _poolSize = 20;
    // Make a list of game objects we want to start with
    List<GameObject> _gameObjects = new List<GameObject>();
    // Spawn times
    [SerializeField] float _spawnTime = 1f;
    [SerializeField] float _spawnDelay = 3f;

    void Start()
    {
        // Create all the objects the pool will use
        for (int i = 0; i < _poolSize; i++)
        {
            // Instantiate object and set it to inactive
            GameObject newObj = Instantiate(_poolObject);
            newObj.transform.SetPositionAndRotation(transform.position, transform.rotation);
            newObj.SetActive(false);
            // Add it to the pool
            _gameObjects.Add(newObj);
        }

        // Repeatedly spawn enemy
        InvokeRepeating(nameof(GetPooledObject), _spawnTime, _spawnDelay);
    }

    public GameObject GetPooledObject()
    {
        // If a game object in the pool is not active, give it out
        foreach (GameObject gameObject in _gameObjects)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.transform.position = transform.position;
                gameObject.transform.rotation = transform.rotation;
                gameObject.SetActive(true);
                return gameObject;
            }
        }
        // If all are active
        return null;
    }
}
