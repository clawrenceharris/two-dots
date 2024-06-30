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
    private readonly HashSet<AudioClip> hitSounds = new();
    private readonly HashSet<AudioClip> clearSounds = new();


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
        DotsObjectEvents.onCleared += OnCleared;
        DotsObjectEvents.onHit += OnDotHit;
        Connection.onSquareMade += OnSquareMade;
        Command.onCommandExecuted += OnCommandExecuted;
    }


    private void PlayDotHitSound(Dot dot)
    {
        AudioClip sound = GetDotHitSound(dot);
        //if (!hitSounds.Contains(sound))
        //{
            audioSource.PlayOneShot(sound);
            hitSounds.Add(sound); 
        //}
       
        
        

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
        return dot.DotType switch
        {
            DotType.ClockDot => clockHit,
            DotType.Bomb => bombExplode,
            DotType.AnchorDot => anchorDotSink,

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
        //if (!clearSounds.Contains(sound))
        //{
            audioSource.PlayOneShot(sound);
            clearSounds.Add(sound); 
        //}
        
    }


    private void PlayTileCleardSound(Tile tile)
    {
        AudioClip sound = GetTileClearedSound(tile);
        

        //if (!clearSounds.Contains(sound))
        //{
            audioSource.PlayOneShot(sound);
            clearSounds.Add(sound); 
        //}
        
    }


    
    private void OnDotCleared(Dot dot)
    {
        PlayDotCleardSound(dot);

    }

    private void OnDotHit(DotsGameObject dotsObject)
    {
        if (dotsObject is Tile tile)
            PlayTileHitSound(tile);
        if (dotsObject is Dot dot)
            PlayDotHitSound(dot);
    }

    private void PlayTileHitSound(Tile tile)
    {
        
    }

    private void OnCleared(DotsGameObject dotsObject, float clearTime)
    {
        if(dotsObject is Tile tile)
            PlayTileCleardSound(tile);
        if (dotsObject is Dot dot)
            PlayDotCleardSound(dot);
    }
    private void OnCommandExecuted(Command command)
    {
        //if (command is HitCommand)
        //{
        //    hitSounds.Clear();
        //    clearSounds.Clear();
        //}
    }
    //private void OnCommandsEnded()
    //{
    //    hitSounds.Clear();
    //    clearSounds.Clear();
    //}

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
    private void OnDotSelected(ConnectableDot dot)
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
    private void OnDotDisconnected(ConnectableDot dot)
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