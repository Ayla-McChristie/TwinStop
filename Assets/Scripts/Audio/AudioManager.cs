using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public enum Sounds { lol, dwd}

    #region Serialized AudioArrays
    [SerializeField]
    BreakableObjSounds[] breakObj;
    [SerializeField]
    ChargerSounds[] charger;
    [SerializeField]
    DoorSounds[] doorSounds;
    [SerializeField]
    DeathSounds[] deathSounds;
    [SerializeField]
    Music[] music;
    [SerializeField]
    PickUpSounds[] pickUp;
    [SerializeField]
    PlayerSounds[] playerSounds;
    [SerializeField]
    ProjectileSounds[] projectileSounds;
    [SerializeField]
    SpawnSounds[] spawnSounds;
    [SerializeField]
    TimeSounds[] timeSounds;
    [SerializeField]
    WallMovingSounds[] wallMovingSounds;
    #endregion
    #region SerialzedFields
    [SerializeField]
    float standardPitch;
    [SerializeField]
    float timeStopPitch;
    #endregion

    static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    List<AudioSource> audioList;
    List<float> oldPitchVal;
    AudioSource overWorldMusic;
    AudioSource battleMusic;

    string sceneName;
    float oldVolume;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        audioList = new List<AudioSource>();
        oldPitchVal = new List<float>();
        SetUpSounds();
        SetSceneMusic();
        SetPitchList();
    }

    void SetPitchList()
    {
        for(int i = 0; i < audioList.Count; i++)
        {
            oldPitchVal.Add(audioList[i].pitch);
        }
    }

    void SetSceneMusic()
    {
        sceneName = "OverWorldMusic";
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            sceneName = "MainMenuMusic";
        }
        if (SceneManager.GetActiveScene().name == "Credits")
        {
            sceneName = "CreditsMusic";
        }
        if (SceneManager.GetActiveScene().name == "Climax")
        {
            sceneName = "BossMusic";
        }
        Debug.Log(SceneManager.GetActiveScene().name);
        PlayMusic(sceneName).Play();
    }

    AudioSource PlayMusic(string name)
    {
        foreach (AudioSource a in audioList)
        {
            if (a.clip.name == name)
                return a;
        }
        return null;
    }

    #region SoundSetUp
    void SetUpSounds()
    {
        SetUpBreakableObj();
        SetUpCharger();
        SetUpDoor();
        SetUpDeath();
        SetUpMusic();
        SetUpPickUps();
        SetUpPlayer();
        SetUpProjectile();
        SetUpSpawn();
        SetUpTimeSounds();
        SetUpWallMoving();
    }

    void SetUpBreakableObj()
    {
        foreach(BreakableObjSounds s in breakObj)
        {
            s.audio.volume = s.Volume;
            s.audio.pitch = s.Pitch;
            audioList.Add(s.audio);
        }
    }

    void SetUpCharger()
    {
        foreach (ChargerSounds c in charger)
        {
            c.audio.volume = c.Volume;
            c.audio.pitch = c.Pitch;
            audioList.Add(c.audio);
        }
    }
    void SetUpDoor()
    {
        foreach (DoorSounds d in doorSounds)
        {
            d.audio.volume = d.Volume;
            d.audio.pitch = d.Pitch;
            audioList.Add(d.audio);
        }
    }
    void SetUpDeath()
    {
        foreach (DeathSounds d in deathSounds)
        {
            d.audio.volume = d.Volume;
            d.audio.pitch = d.Pitch;
            audioList.Add(d.audio);
        }
    }
    void SetUpMusic()
    {
        foreach (Music d in music)
        {
            d.audio.volume = d.Volume;
            d.audio.pitch = d.Pitch;
            audioList.Add(d.audio);
            GetMusic(d.audio);
        }
    }

    void GetMusic(AudioSource a)
    {
        if (a.clip.name == "OverWorldMusic")
            overWorldMusic = a;
        if (a.clip.name == "BattleMusic")
            battleMusic = a;
    }

    void SetUpPickUps()
    {
        foreach (PickUpSounds d in pickUp)
        {
            d.audio.volume = d.Volume;
            d.audio.pitch = d.Pitch;
            audioList.Add(d.audio);
        }
    }
    void SetUpPlayer()
    {
        foreach (PlayerSounds d in playerSounds)
        {
            d.audio.volume = d.Volume;
            d.audio.pitch = d.Pitch;
            audioList.Add(d.audio);
        }
    }

    void SetUpProjectile()
    {
        foreach (ProjectileSounds d in projectileSounds)
        {
            d.audio.volume = d.Volume;
            d.audio.pitch = d.Pitch;
            audioList.Add(d.audio);
        }
    }
    void SetUpSpawn()
    {
        foreach (SpawnSounds d in spawnSounds)
        {
            d.audio.volume = d.Volume;
            d.audio.pitch = d.Pitch;
            audioList.Add(d.audio);
        }
    }

    void SetUpTimeSounds()
    {
        foreach (TimeSounds d in timeSounds)
        {
            d.audio.volume = d.Volume;
            d.audio.pitch = d.Pitch;
            audioList.Add(d.audio);
        }
    }
    void SetUpWallMoving()
    {
        foreach (WallMovingSounds d in wallMovingSounds)
        {
            d.audio.volume = d.Volume;
            d.audio.pitch = d.Pitch;
            audioList.Add(d.audio);
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        PitchSystem();
        MusicTransitionSetUp();
    }

    #region PitchSetUp
    bool CheckTimeIsSlowed()
    {
        if (TimeManager.Instance.isTimeStopped)
            return true;
        return false;
    }

    void RevertPitch()
    {
        for (int i = 0; i < audioList.Count; i++)
            ChangePitch(audioList[i], oldPitchVal[i]);
    }

    void ChangePitch(AudioSource a, float pitchTune)
    {
            a.pitch = pitchTune;
    }

    void SlowDownPitch(float pitchTune)
    {
        foreach (AudioSource a in audioList)
            ChangePitch(a, pitchTune);
    }

    void PitchSystem()
    {
        if (!CheckTimeIsSlowed())
        {
            RevertPitch();
            return;
        }
        SlowDownPitch(timeStopPitch);
    }
    #endregion

    void MusicTransitionSetUp()
    {
        if (EnemyManager.Instance.isInCombat)
        {
            MusicToBattle();
            return;
        }
        MusicToOverWorld();
    }

    void MusicToOverWorld()
    {
        if (sceneName != "BossMusic")
        {
            PlayMusic(overWorldMusic);
            ChangeMusicVol(battleMusic, 0f);
            ChangeMusicVol(overWorldMusic, .4f);
        }
        
    }

    void MusicToBattle()
    {
        if (sceneName != "BossMusic")
        {
            PlayMusic(battleMusic);
            ChangeMusicVol(battleMusic, .4f);
            ChangeMusicVol(overWorldMusic, 0f);
        }
    }

    void PlayMusic(AudioSource a)
    {
        if (a.isPlaying)
            return;
        a.Play();
    }

    void ChangeMusicVol(AudioSource a, float volume)
    {
        if (a.volume == volume)
            return;
        if (a.volume > volume)
        {
            a.volume -= .1f * Time.deltaTime;
            return;
        }
        a.volume += .1f * Time.deltaTime;
    }


    public void PlaySound(string soundName, Vector3 position, bool oneShot)
    { 
        foreach(AudioSource a in audioList)
        {
            if(a.clip.name == soundName)
            { 
                if (!oneShot)
                {
                    a.Play();
                    return;
                }

                a.PlayOneShot(a.clip);

            }
        }
        
    }

    public void AdjustFromInspector()
    {

        foreach (BreakableObjSounds s in breakObj)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
        foreach (DoorSounds s in doorSounds)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
        foreach (ChargerSounds s in charger)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
        foreach (DeathSounds s in deathSounds)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
        foreach (Music s in music)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
            Debug.Log(s.Volume);
        }
        foreach (PickUpSounds s in pickUp)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
        foreach (PlayerSounds s in playerSounds)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
        foreach (ProjectileSounds s in projectileSounds)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
        foreach (SpawnSounds s in spawnSounds)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
        foreach (TimeSounds s in timeSounds)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
        foreach (WallMovingSounds s in wallMovingSounds)
        {
            AdjustInSource(s.Name, s.Volume, s.Pitch);
        }
    }

    void AdjustInSource(string name, float newVolume, float newPitch)
    {
        foreach(AudioSource o in audioList)
        {
            if(o.clip.name == name)
            {
                o.volume = newVolume;
                o.pitch = newPitch;
            }
        }
    }

    #region SerializedSound
    [System.Serializable]
    public class BreakableObjSounds
    {
        public AudioManager.Sounds SoundType;
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }

    [System.Serializable]
    public class ChargerSounds
    {
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    [System.Serializable]
    public class DeathSounds
    {
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    [System.Serializable]
    public class DoorSounds
    {
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    [System.Serializable]
    public class Music
    {
        public AudioManager.Sounds sound = Sounds.dwd;
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    [System.Serializable]
    public class PickUpSounds
    {
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    [System.Serializable]
    public class PlayerSounds
    {
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    [System.Serializable]
    public class ProjectileSounds
    {
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    [System.Serializable]
    public class SpawnSounds
    {
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    [System.Serializable]
    public class TimeSounds
    {
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch = 1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    [System.Serializable]
    public class WallMovingSounds
    {
        public AudioSource audio;
        public string Name;
        [Range(0, 1)]
        public float Pitch =1f;
        [Range(0, 1)]
        public float Volume = .5f;
    }
    #endregion
}
