using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    // The game object that the pool will hold
    [SerializeField] GameObject _poolObject;
    // How many enemies to start with
    [SerializeField] int _poolSize = 20;
    // Make a list of game objects we want to start with
    List<GameObject> _gameObjects = new List<GameObject>();

    void Start()
    {
        // Create all the objects the pool will use
        for (int i = 0; i < _poolSize; i++)
        {
            // Instantiate object and set it to inactive
            GameObject newObj = Instantiate(_poolObject);
            newObj.transform.position = transform.position;
            newObj.transform.rotation = transform.rotation;
            newObj.SetActive(false);
            // Add it to the pool
            _gameObjects.Add(newObj);
        }

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
