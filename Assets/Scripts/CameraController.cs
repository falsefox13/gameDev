using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    [Range(1.0f, 10.0f)]
    public float speed = 3.0f;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position + offset, Time.deltaTime * speed);
    }
}
