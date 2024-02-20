using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point 
{
    private Vector3 _position;
    public Vector3 Position { get => _position; private set => _position = value; }

    private bool _isEmpty;
    public bool IsEmpty { get => _isEmpty; set => _isEmpty = value; }

    public Point(Vector3 position)
    {

        _position = position;
        _isEmpty = true;
    }
    
}
