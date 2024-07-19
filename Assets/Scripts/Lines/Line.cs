using UnityEngine;
using System.Collections;
using System;
public class ConnectorLine : MonoBehaviour
{
    public Vector2 endPos;
    public Vector2 startPos;
    public Vector2 initialScale; 
    public delegate void UpdateLine(ConnectorLine line, Vector2 startPos, Vector2 endPos);
    public Quaternion rotation = Quaternion.identity;
    public SpriteRenderer SpriteRenderer { get; private set; }
    public UpdateLine updateLine;
    public GameObject line;
    public Transform right;

    private void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }
    private void Update()
    {
        updateLine?.Invoke(this, startPos, endPos);
    }

    
}
