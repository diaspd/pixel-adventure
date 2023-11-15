using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform point;

    void Start()
    {
        FindObjectOfType<Player>().transform.position = point.position;
    }
}
