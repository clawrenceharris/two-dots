using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewManager
{
    private Board board;

    public PreviewManager(Board board)
    {
        ConnectionManager.onDotConnected += OnDotConnected;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        ConnectionManager.onConnectionEnded += OnConnectionEnded;

        this.board = board;
    }

    private void OnDotConnected(ConnectableDot dot)
    {
        List<IHittable> dots = ConnectionManager.ToHit;
        
        foreach (IHittable hittable in dots)
        {
            if (hittable is IPreviewable previewable)
            {

                foreach (IHitRule rule in previewable.PreviewHitRules)
                {
                    if (rule.Validate(previewable, board))
                    {
                        CoroutineHandler.StartStaticCoroutine(previewable.StartPreview(PreviewHitType.Connection));

                    }
                }
            }

        }
    }


    private void OnDotDisconnected(ConnectableDot dot)
    {
        List<IHittable> dots = ConnectionManager.ToHit;
        dots.Add(dot);

        foreach (IHittable hittable in dots)
        {
            if (hittable is IPreviewable previewable)
            {

                foreach (IHitRule rule in previewable.PreviewHitRules)
                {
                    if (rule.Validate(previewable, board))
                    {
                        CoroutineHandler.StartStaticCoroutine(previewable.StartPreview(PreviewHitType.Disconnection));

                    }
                    else
                    {
                        previewable.StopPreview();
                    }
                }
            }

        }
    }

    private void OnConnectionEnded(LinkedList<ConnectableDot> dots)
    {

        foreach (IHittable hittable in dots)
        {
            if (hittable is IPreviewable previewable)
            {
                if (previewable.IsPreviewing)
                    previewable.StopPreview();
            }
        }
    }

}
