using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnwalkableColliders : MonoBehaviour
{
    public static UnwalkableColliders Instance = null;
    public static List<Collider> unwalkableColliders;

    private void Awake()
    {
        #region Singleton Pattern
        if (Instance == null)
        {
            // doesn't exist yet, this is now our singleton
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
