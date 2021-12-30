using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] public Transform playerPos;
    private Vector3 cameraOffset;

    [Range(0.01f, 1.0f)]
    [SerializeField] float smoothness = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - playerPos.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Follow's the player position
        Vector3 newPos = playerPos.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothness);
    }
}
