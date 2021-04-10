using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Shop shop;
    private Vector3 target = Vector3.zero;
    private void Start()
    {
        shop.onChangedTarget += Move;
    }
    private void Move(Vector3 target)
    {
        this.target = target;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, transform.position.y, transform.position.z), Time.deltaTime * 4);
    }
}
