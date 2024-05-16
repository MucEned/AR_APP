using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    [SerializeField] private Transform item;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Rotate();
        }
    }
    public void Rotate()
    {
        item.Rotate(Vector3.up * 10f * Time.deltaTime);
    }
}
