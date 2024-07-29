using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// Manages the drawing and removal of lines between dots in the game.
/// </summary>
public class LineManager
{
    private static List<ConnectorLine> lines;
    private readonly Board board;
    private ConnectorLine currentLine;
    public static Vector2 LineScale { get; private set; } = new Vector2(1f, 0.3f);

    public LineManager(Board board)
    {
        this.board = board;
        lines = new List<ConnectorLine>();
        SubscribeToEvents();
    }

    
    private void SubscribeToEvents()
    {
        ConnectionManager.onDotConnected += OnDotConnected;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        ConnectionManager.onDotSelected += OnDotSelected;
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
    }

    /// <summary>
    /// Checks if the connection currently forms a square.
    /// </summary>
    /// <returns>True if a square is formed, otherwise false.</returns>
    private bool IsSquare()
    {
        return ConnectionManager.IsSquare;
    }

    /// <summary>
    /// Removes the last drawn line.
    /// </summary>
    private void RemoveLine()
    {
        if(lines.Count == 0)
        {
            return;
        }
        if (lines.Count > 1)
            currentLine = lines[^2];

        LinePool.Instance.Return(lines[^1]);
        lines.RemoveAt(lines.Count - 1);
    }

    
    private void OnDotConnected(ConnectableDot dot)
    {
        if (!currentLine)
        {
            return;
        }
        currentLine.endPos = new Vector2(dot.Column, dot.Row) * Board.offset;

        currentLine = DrawLine(dot);
        lines.Add(currentLine);

    }

    /// <summary>
    /// Updates the appearance of lines.
    /// </summary>
    public void UpdateLines()
    {
        if (lines.Count == 0 || ConnectionManager.Connection == null) return;

        //update the lines' color 
        Color targetColor = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        foreach (ConnectorLine line in lines)
        {
            if (line.SpriteRenderer.color != targetColor)
            {
                line.SpriteRenderer.color = targetColor;
            }
        }

        //update the current line's end position
        if (currentLine && !IsSquare())
        {
            currentLine.endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    
    private void OnDotSelected(ConnectableDot dot)
    {
        //draw line when we select a dot
        currentLine = DrawLine(dot);
        lines.Add(currentLine);

    }

    
    private void OnDotDisconnected(Dot dot)
    {
        //remove line when we disconnect a dot
        RemoveLine();
    }

    
    private void OnConnectionEnded(LinkedList<ConnectableDot> dots)
    {
        RemoveAllLines();
        currentLine = null;
    }

   

    

    public static IEnumerator DrawLine(IColorable start, IColorable end)
    {
        DotsGameObject endObject = (DotsGameObject)end;
        DotsGameObject startObject = (DotsGameObject)start;

        ConnectorLine line = LinePool.Instance.Get();
        line.startPos = startObject.transform.position;
        line.initialScale = LineScale;
        line.transform.localScale = line.initialScale;
        line.endPos = endObject.transform.position;
        line.SpriteRenderer.color = ColorSchemeManager.FromDotColor(end.Color);
        line.SpriteRenderer.sortingLayerName = "Line";

        line.update = AutoConnectLine;
        lines.Add(line);

        yield return new WaitForSeconds(0.2f);

    }

    private static void AutoConnectLine(ConnectorLine line, Vector2 startPos, Vector2 endPos)
    {
        float distance = Vector2.Distance(startPos, endPos);
        float scaleXOffset = 1f;
        float newXScale = line.initialScale.x + distance - scaleXOffset;

        float angle = Vector2.SignedAngle(Vector2.right, endPos - startPos);
        line.transform.SetPositionAndRotation(startPos, Quaternion.Euler(0f, 0f, angle));

        line.transform.DOScale(new Vector2(newXScale, line.initialScale.y), 0.2f);
    }

    /// <summary>
    /// Draws a line at the given start position.
    /// </summary>
    /// <param name="startPos">The position of the dot where the line should start.</param>
    private ConnectorLine DrawLine(ConnectableDot dot)
    {

        ConnectorLine line = LinePool.Instance.Get();
        line.transform.parent = board.transform;
        line.SpriteRenderer.color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        line.startPos = new Vector2(dot.Column, dot.Row) * Board.offset;
        line.initialScale = LineScale;

        //we dont want the line to be active when we have a square
        line.SpriteRenderer.enabled = !IsSquare();
        line.update = UpdateLine;
        return line;
    }
    
    public static void RemoveAllLines()
    {
        foreach (ConnectorLine line in lines)
        {
            LinePool.Instance.Return(line);
        }
        lines.Clear();
    }
    private void UpdateLine(ConnectorLine line, Vector2 startPos, Vector2 endPos)
    {

        float distance = Vector2.Distance(startPos, endPos);

        float scaleXOffset = 1;
        // Calculate the new scale value based on the distance
        float newXScale = line.initialScale.x + distance - scaleXOffset;


        float angle = Vector2.SignedAngle(Vector2.right, endPos - startPos);
        
        line.transform.SetPositionAndRotation(startPos, Quaternion.Euler(0f, 0f, angle ));

        // Apply the new scale and position
        line.transform.localScale = new Vector3(newXScale, line.initialScale.y);


    }


}