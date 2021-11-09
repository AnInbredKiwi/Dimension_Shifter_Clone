using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject objSpawned;
    [SerializeField]
    private Sprite Sprite;

    void Start()
    {
        CreateBoxAtPosition(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CreateBoxAtPosition(GetMousePos());
        }
    }

    void CreateBoxAtPosition(Vector3 postion)
    {
        objSpawned = new GameObject("GameObject from ObjectBuilder");
        //Add Components
        var sprite = objSpawned.AddComponent<SpriteRenderer>();
        sprite.sprite = Sprite;
        objSpawned.AddComponent<BoxCollider2D>();
        objSpawned.transform.position = postion;
    }

    Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //works with orthografic cam only because for 3D u need to define depth (z)
        mousePos.z = 0;
        return mousePos;
    }
}
