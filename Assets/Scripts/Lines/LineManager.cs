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
        ConnectionManager.onDotConnected += OnDotConnection;
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
        if (lines.Count > 1)
            currentLine = lines[^2];

        LinePool.Instance.Return(lines[^1]);
        lines.RemoveAt(lines.Count - 1);
    }

    
    private void OnDotConnection(Dot dot)
    {
        //set the end position of the current line 
        currentLine.endPos = dot.transform.position;

        //add another line and set it as the current line
        currentLine = DrawLine(dot.transform);
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

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    public void UnsubscribeFromEvents()
    {
        ConnectionManager.onDotConnected -= OnDotConnection;
        ConnectionManager.onDotDisconnected -= OnDotDisconnected;
        ConnectionManager.onDotSelected -= OnDotSelected;
        ConnectionManager.onConnectionEnded -= OnConnectionEnded;
    }

    /// <summary>
    /// Handles the event when a dot is selected.
    /// </summary>
    /// <param name="dot">The dot that is selected.</param>
    private void OnDotSelected(Dot dot)
    {
        //draw line when we select a dot
        currentLine = DrawLine(dot.transform);

    }

    /// <summary>
    /// Handles the event when a dot is disconnected.
    /// </summary>
    /// <param name="dot">The dot that is disconnected.</param>
    private void OnDotDisconnected(Dot dot)
    {
        //remove line when we disconnect a dot
        RemoveLine();
    }

    /// <summary>
    /// Handles the event when a connection between dots is ended.
    /// </summary>
    /// <param name="dots">The dots involved in the connection.</param>
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
    /// Draws a line between dots.
    /// </summary>
    /// <param name="startPos">The position of the dot where the line starts.</param>
    private ConnectorLine DrawLine(Transform start)
    {

        ConnectorLine line = LinePool.Instance.Get();
        line.transform.parent = board.transform;
        line.color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);
        line.startPos = start.position;
        line.initialScale = new Vector2(1f, 0.4f);
        lines.Add(line);

        //we dont want the line to be active when we have a square
        line.gameObject.SetActive(!IsSquare());
        return line;
    }
}