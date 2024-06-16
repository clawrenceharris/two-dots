using static Type;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] connectionSounds;
    [SerializeField] private AudioClip anchorDotSink;
    [SerializeField] private AudioClip clockHit;

    [SerializeField] private AudioClip bombActive;
    [SerializeField] private AudioClip bombExplode;
    [SerializeField] private AudioClip blockTile;

    private AudioSource audioSource;
    private AudioDistortionFilter audioDistortion;
    private readonly HashSet<AudioClip> playedSounds = new();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioDistortion = GetComponent<AudioDistortionFilter>();
    }
    void Start()
    {

        ConnectionManager.onDotConnected += OnDotConnected;
        ConnectionManager.onDotSelected += OnDotSelected;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
        Dot.onDotCleared += OnDotCleared;
        Tile.onTileCleared += OnTileCleared;
        Dot.onDotHit += OnDotHit;
        Connection.onSquareMade += OnSquareMade;
        CommandInvoker.onCommandsEnded += OnCommandsEnded;

    }

    
    private void PlayDotHitSound(Dot dot)
    {
        AudioClip sound = GetDotHitSound(dot);
        if (!playedSounds.Contains(sound))
        {
            audioSource.PlayOneShot(sound);
            playedSounds.Add(sound); // Add the sound to the HashSet to indicate that it has been played
        }
        

    }
    
    private AudioClip GetDotClearedSound(Dot dot)
    {
        return dot.DotType switch
        {
            DotType.ClockDot => bombActive,
            DotType.NestingDot => bombActive,
            DotType.BeetleDot => bombActive,
            _ => null,
        };


    }
    private AudioClip GetDotHitSound(Dot dot)
    {
        return dot.HitType switch
        {
            HitType.ClockDot => clockHit,
            HitType.BombExplosion => bombExplode,
            HitType.AnchorDot => anchorDotSink,

            _ => null,
        };


    }

    
    private AudioClip GetTileClearedSound(Tile tile)
    {
        return tile.TileType switch
        {
            TileType.BlockTile => blockTile,
            _ => null,
        };
    }


    private AudioClip GetConnectedSound(ConnectableDot dot)
    {
        return dot.DotType switch
        {
            _ => null,
        };
    }



    private void PlayDotCleardSound(Dot dot)
    {
        AudioClip sound = GetDotClearedSound(dot);
        if (!playedSounds.Contains(sound))
        {
            audioSource.PlayOneShot(sound);
            playedSounds.Add(sound); // Add the sound to the HashSet to indicate that it has been played
        }
    }


    private void PlayTileCleardSound(Tile tile)
    {
        AudioClip sound = GetTileClearedSound(tile);
        if (!playedSounds.Contains(sound))
        {
            audioSource.PlayOneShot(sound);
            playedSounds.Add(sound); // Add the sound to the HashSet to indicate that it has been played
        }
    }


    
    private void OnDotCleared(Dot dot)
    {
        PlayDotCleardSound(dot);

    }

    private void OnDotHit(Dot dot)
    {
        PlayDotHitSound(dot);
    }

    private void OnTileCleared(Tile tile)
    {
        PlayTileCleardSound(tile);

    }
    private void OnCommandsEnded()
    {
        playedSounds.Clear();
    }

    private void OnSquareMade(Square square)
    {
        if (square.DotsInSquare.Count > 0)
        {
            audioSource.PlayOneShot(bombActive);
        }
        audioDistortion.enabled = true;
        audioSource.volume = 0.5f;
        int index1 = GetIndex();

        int index2 = Mathf.Clamp(index1 - 1, 0, connectionSounds.Length - 1);
        int index3 = Mathf.Clamp(index1 + 1, 0, connectionSounds.Length - 1);

        PlaySound(connectionSounds[index1]);
        PlaySound(connectionSounds[index2]);
        PlaySound(connectionSounds[index3]);
        Invoke(nameof(DisableFilters), 0.5f);

    }
    private void DisableFilters()
    {
        audioDistortion.enabled = false;
    }
    private void OnDotSelected(Dot dot)
    {
        int index = GetIndex();

        PlaySound(connectionSounds[index]);

    }

    private void OnDotConnected(ConnectableDot dot)
    {
        int index = GetIndex();
        PlaySound(connectionSounds[index]);
    }

    private int GetIndex()
    {
        int connectionCount = ConnectionManager.ConnectedDots.Count;
        int index = Mathf.Clamp(connectionCount, 0, connectionSounds.Length - 1);
        return index;
    }
    private void OnDotDisconnected(Dot dot)
    {
        if (ConnectionManager.ConnectedDots.Count == 0)
        {
            return;
        }

        int index = GetIndex();
        PlaySound(connectionSounds[index]);
    }

    private void PlaySound(AudioClip soundClip)
    {
        if (soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }


}