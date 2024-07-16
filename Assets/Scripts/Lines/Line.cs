using UnityEngine;
using System.Collections;
using System;
public class ConnectorLine : MonoBehaviour
{
    public Vector2 endPos;
    public Vector2 startPos;
    public Color color;
    public Vector2 initialScale; 
    public delegate void UpdateLine(ConnectorLine line, Vector2 startPos, Vector2 endPos);
    public Quaternion rotation = Quaternion.identity;
    public SpriteRenderer sprite;
    public UpdateLine updateLine;
    public GameObject line;
    public Transform right;
    private void Awake()
    {
        SetUp();
    }
    
    
    
    private void SetUp()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        updateLine?.Invoke(this, startPos, endPos);
    }

    
}
