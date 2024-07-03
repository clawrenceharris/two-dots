using static Type;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using Unity.VisualScripting;

[System.Serializable]

public class DotAudio : DotsGameObjectAudio
{
    public DotType dotType;
   
}

public class DotsGameObjectAudio
{
    public AudioVariations connectionAudio;
    public AudioClip[] hitAudio;

    public AudioVariations hitPreviewAudio;
    public AudioVariations spawnAudio;
    public AudioVariations clearAudio;
    public AudioVariations clearPreviewAudio;
}

[System.Serializable]
public class TileAudio : DotsGameObjectAudio
{
    public TileType tileType;
   
}

[System.Serializable]
public class CommandExecutionAudio
{
    public AudioVariations commandExecutionAudio;
    public CommandType commandType;

}


[System.Serializable]
public class AudioVariations
{
    [SerializeField]private AudioClip[] audioClips;

    public AudioClip GetSound()
    {
        int rand = Random.Range(0, audioClips.Length);

        return audioClips[rand];
    }
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] connectionSounds;


    [SerializeField] private DotAudio[] dotSounds;
    [SerializeField] private TileAudio[] tileSounds;
    [SerializeField] private CommandExecutionAudio[] commandExecutionSounds;


    private AudioSource audioSource;
    private AudioDistortionFilter audioDistortion;


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
        Command.onCommandExecuting += OnCommandExecuting;

    }




    private void OnCommandExecuting(Command command)
    {
        PlayCommandExecutionSound(command);

    }


    private void PlaySounds(List<AudioClip> sounds)
    {
        foreach (AudioClip sound in sounds)
            audioSource.PlayOneShot(sound);
    }
    private void PlayCommandExecutionSound(Command command)
    {
        AudioClip sound = GetCommandExecutionSounds(command);
        PlaySound(sound);
       
    }

    private AudioClip GetCommandExecutionSounds(Command command)
    {

        //Filter all command execution audio to match this command's command type
        CommandExecutionAudio audio = commandExecutionSounds
             .FirstOrDefault((audio) => audio.commandType == command.CommandType);

        //Return a sound from the execution sound variations

        if(audio != null)
            return audio.commandExecutionAudio.GetSound();
        return null;
    }

    private void PlayDotHitSound(Dot dot)
    {
        AudioClip sound = GetDotHitSound(dot);
        audioSource.PlayOneShot(sound);
        
    }
    
    private AudioClip GetDotClearedSound(Dot dot)
    {
        //Find the dot audio to match this dot's dot type 
        DotAudio audio = dotSounds.FirstOrDefault((audio) => audio.dotType == dot.DotType);

        //Return a sound from the clear sound variations

        if(audio != null)
            return audio.clearAudio.GetSound();
        return null;

    }

    private AudioClip GetDotHitSound(Dot dot)
    {
        //Find the dot audio to match this dot's dot type 
        DotAudio audio = dotSounds.FirstOrDefault((audio) => audio.dotType == dot.DotType);


        //Return a sound from the hit sound variations based on dot hit count
        if(audio != null)
            return audio.hitAudio[Mathf.Clamp(dot.HitCount, 0, dot.HitsToClear -2)];
        return null;
    }

    
    private AudioClip GetTileClearedSound(Tile tile)
    {
        //Find all dot audio to match this tile's tile type 
        TileAudio audio = tileSounds.FirstOrDefault((audio) => audio.tileType == tile.TileType);

        //Return a sound from the clear sound variations
        if(audio != null)
            return audio.clearAudio.GetSound();
        return null;
    }


    


    private void PlayDotCleardSound(Dot dot)
    {
        AudioClip sound = GetDotClearedSound(dot);
        audioSource.PlayOneShot(sound);
        
    }


    private void PlayTileCleardSound(Tile tile)
    {
        AudioClip sound = GetTileClearedSound(tile);
        audioSource.PlayOneShot(sound);
        
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

    private void OnCleared(DotsGameObject dotsObject)
    {
        if(dotsObject is Tile tile)
            PlayTileCleardSound(tile);
        if (dotsObject is Dot dot)
            PlayDotCleardSound(dot);
    }
   

    private void OnSquareMade(Square square)
    {
        
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