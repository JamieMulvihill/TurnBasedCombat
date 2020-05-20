using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    public float speedStat;
    float minSpeed = 10f;
    float maxSpeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        speedStat = Random.Range( minSpeed, maxSpeed);
    }
}
