using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the drawing and removal of lines between dots in the game.
/// </summary>
public class LineManager
{
    private readonly List<ConnectorLine> lines;
    private readonly Board board;
    private ConnectorLine currentLine;

    
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
        currentLine.endPos = new Vector2(dot.Column, dot.Row) * Board.offset;

        currentLine = DrawLine(dot);
        lines.Add(currentLine);

    }

    /// <summary>
    /// Updates the appearance of lines.
    /// </summary>
    public void UpdateLines()
    {
        if (lines.Count == 0) return;

        //update the lines' color 
        Color targetColor = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        foreach (ConnectorLine line in lines)
        {
            if (line.color != targetColor)
            {
                line.color = targetColor;
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
        foreach (ConnectorLine line in lines)
        {
            LinePool.Instance.Return(line);
        }
        lines.Clear();
        currentLine = null;
    }

    /// <summary>
    /// Draws a line at the given start position.
    /// </summary>
    /// <param name="startPos">The position of the dot where the line should start.</param>
    private ConnectorLine DrawLine(ConnectableDot dot)
    {

        ConnectorLine line = LinePool.Instance.Get();
        line.transform.parent = board.transform;
        line.color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        line.startPos = new Vector2(dot.Column, dot.Row) * Board.offset;
        line.initialScale = new Vector2(1f, 0.3f);

        //we dont want the line to be active when we have a square
        line.sprite.enabled = !IsSquare();
        return line;
    }
}