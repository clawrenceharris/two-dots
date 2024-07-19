using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using Unity.VisualScripting;
using System.Collections;

[System.Serializable]

public class DotAudio : DotsGameObjectAudio
{
    public DotType dotType;
   
}

public class DotsGameObjectAudio
{
    public Audio connectionAudio;
    public Audio[] hitAudio;

    public Audio hitPreviewAudio;
    public Audio spawnAudio;
    public Audio clearAudio;
    public Audio clearPreviewAudio;
}

[System.Serializable]
public class TileAudio : DotsGameObjectAudio
{
    public TileType tileType;
   
}

[System.Serializable]
public class CommandExecutionAudio
{
    public Audio commandExecutionAudio;
    public CommandType commandType;

}


[System.Serializable]
public class Audio
{
    [SerializeField]private AudioClip[] audioClips;
    public int maxPlaysAtOnce = 1;

    public AudioClip GetSound()
    {
        if(audioClips.Length == 0)
        {
            return null;
        }
        int rand = Random.Range(0, audioClips.Length);

        return audioClips[rand];
    }
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] connectionSounds;


    [SerializeField] private DotAudio[] dotAudio;
    [SerializeField] private TileAudio[] tileAudio;
    [SerializeField] private CommandExecutionAudio[] commandExecutionAudio;

    private HashSet<Audio> currentAudio = new();
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
        Board.onObjectSpawned += OnSpawned;
        HittableBase.onCleared += OnCleared;

        HittableBase.onHit += OnHit;
        Connection.onSquareMade += OnSquareMade;
        Command.onCommandExecuting += OnCommandExecuting;

    }

    private void OnSpawned(DotsGameObject dotsGameObject)
    {
        Audio sound = null;
        if (dotsGameObject is Tile tile)
        {
            sound = GetTileSpawnedSound(tile);
        }
        if (dotsGameObject is Dot dot)
        {
            sound = GetDotSpawnedSound(dot);

        }

        PlaySound(sound);
    }

    private Audio GetDotSpawnedSound(Dot dot)
    {
        DotAudio audio = dotAudio
             .FirstOrDefault((audio) => audio.dotType == dot.DotType);

        if (audio != null)
            return audio.spawnAudio;
        return null;
    }

    private Audio GetTileSpawnedSound(Tile tile)
    {
        TileAudio audio = tileAudio
             .FirstOrDefault((audio) => audio.tileType == tile.TileType);

        if (audio != null)
            return audio.spawnAudio;
        return null;
    }

    private Audio GetSpawnSounds(DotsGameObject dotsGameObject)
    {
        throw new System.NotImplementedException();
    }

    private void OnCommandExecuting(Command command)
    {
        Audio sound = GetCommandExecutionSounds(command);
        PlaySound(sound);

    }


    private Audio GetCommandExecutionSounds(Command command)
    {

        //Filter all command execution audio to match this command's command type
        CommandExecutionAudio audio = commandExecutionAudio
             .FirstOrDefault((audio) => audio.commandType == command.CommandType);

        //Return a sound from the execution sound variations

        if(audio != null)
            return audio.commandExecutionAudio;
        return null;
    }



    private Audio GetDotClearedSound(Dot dot)
    {
        //Find the dot audio to match this dot's dot type 
        DotAudio audio = dotAudio.FirstOrDefault((audio) => audio.dotType == dot.DotType);

        //Return a sound from the clear sound variations

        if(audio != null)
            return audio.clearAudio;
        return null;

    }

    private Audio GetDotHitSound(Dot dot)
    {
        //Find the dot audio to match this dot's dot type 
        DotAudio audio = dotAudio.FirstOrDefault((audio) => audio.dotType == dot.DotType);


        //Return a sound from the hit sound variations based on dot hit count
        if(audio != null)
            return audio.hitAudio.Length > 0 ? audio.hitAudio[Mathf.Clamp(dot.HitCount - 1, 0, audio.hitAudio.Length -1)] : null;
        return null;
    }

    
    private Audio GetTileClearedSound(Tile tile)
    {
        //Find all dot audio to match this tile's tile type 
        TileAudio audio = tileAudio.FirstOrDefault((audio) => audio.tileType == tile.TileType);

        //Return a sound from the clear sound variations
        if(audio != null)
            return audio.clearAudio;
        return null;
    }


    private void OnHit(IHittable hittable)
    {
        Audio sound = null;
        if (hittable is Tile tile)
        {
            sound = GetTileHitSound(tile);
            PlaySound(sound);
        }
        if (hittable is Dot dot)
        {
            sound = GetDotHitSound(dot);
            PlaySound(sound);
        }

        PlaySound(sound);

    }

    private Audio GetTileHitSound(Tile tile)
    {
        if(tile is not IHittable hittable)
        {
            return null;
        }
        //Find the dot audio to match this dot's dot type 
        TileAudio audio = tileAudio.FirstOrDefault((audio) => audio.tileType == tile.TileType);


        //Return a sound from the hit sound variations based on dot hit count
        if (audio != null)
            return audio.hitAudio.Length > 0 ? audio.hitAudio[Mathf.Clamp(hittable.HitCount -1, 0, audio.hitAudio.Length -1)] : null;
        return null;
    }

    
    
    private void OnSquareMade(Square square)
    {
        
        audioDistortion.enabled = true;
        audioSource.volume = 0.5f;
        int index1 = GetIndex();

        int index2 = Mathf.Clamp(index1 - 1, 0, connectionSounds.Length - 1);
        int index3 = Mathf.Clamp(index1 + 1, 0, connectionSounds.Length - 1);

        audioSource.PlayOneShot(connectionSounds[index1]);
        audioSource.PlayOneShot(connectionSounds[index2]);
        audioSource.PlayOneShot(connectionSounds[index3]);
        Invoke(nameof(DisableFilters), 0.5f);

    }
    private void DisableFilters()
    {
        audioDistortion.enabled = false;
    }
    private void OnDotSelected(ConnectableDot dot)
    {
        int index = GetIndex();

        audioSource.PlayOneShot(connectionSounds[index]);

    }

    private void OnDotConnected(ConnectableDot dot)
    {
        int index = GetIndex();
        audioSource.PlayOneShot(connectionSounds[index]);
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
        audioSource.PlayOneShot(connectionSounds[index]);
    }
    private void OnCleared(IHittable hittable)
    {
        Audio sound = null;
        if (hittable is Tile tile)
        {
            sound = GetTileClearedSound(tile);
        }
        if (hittable is Dot dot)
        {
            sound = GetDotClearedSound(dot);

        }
        
        PlaySound(sound);

    }


    private void OnCommandBatchCompleted()
    {
        currentAudio.Clear();
    }

    private void PlaySound(Audio audio)
    {
        if(audio == null || audio.GetSound() == null)
        {
            return;
        }

        int currentCount = currentAudio.Count(a => a == audio);


        if (currentCount < audio.maxPlaysAtOnce)
        {
            AudioClip audioClip = audio.GetSound();
            currentAudio.Add(audio);
            audioSource.PlayOneShot(audioClip);
            StartCoroutine(RemoveSoundFromSet(audio, audioClip.length));
        }
    }

    private IEnumerator RemoveSoundFromSet(Audio soundClip, float duration)
    {
        yield return new WaitForSeconds(duration);
        currentAudio.Remove(soundClip);
    }


}