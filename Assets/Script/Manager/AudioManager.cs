using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }
    private AudioSource audioSource1MainSource;
    [SerializeField]
    private AudioSource audioSource2EffectSource;

    public AudioSource AudioSource1MainSource { get => audioSource1MainSource; set => audioSource1MainSource = value; }
    public AudioSource AudioSource2EffectSource { get => audioSource2EffectSource; set => audioSource2EffectSource = value; }
  
    private AudioClip revolver_Spin;
    private AudioClip revolver_Fire;
    private AudioClip revolver_NoBullet;
    private AudioClip revolver_MissFire;
    private AudioClip hangMusic;
    private AudioClip clickMusic;
    private AudioClip bulletsIn;
    private AudioClip bulletsTurn;
    private AudioClip dialogueMusic;
    private AudioClip fightMusic;
    public string revolver_Spin_Path;
    public string revolver_Fire_Path;
    public string revolver_NoBullet_Path;
    public string revelver_MissFire_Path;
    public string hangMusicPath;
    public string clickMusicPath;
    public string BulletsInPath;
    public string BulletsTurnPath;
    public string DialogueMusicPath;
    public string FightMusicPath;

    public AudioClip Revolver_Spin { get => revolver_Spin; set => revolver_Spin = value; }
    public AudioClip Revolver_Fire { get => revolver_Fire; set => revolver_Fire = value; }
    public AudioClip Revolver_NoBullet { get => revolver_NoBullet; set => revolver_NoBullet = value; }
    public AudioClip Revolver_MissFire { get => revolver_MissFire; set => revolver_MissFire = value; }
    public AudioClip HangMusic { get => hangMusic; set => hangMusic = value; }
    public AudioClip ClickMusic { get => clickMusic; set => clickMusic = value; }
    public AudioClip BulletsIn { get => bulletsIn; set => bulletsIn = value; }
    public AudioClip BulletsTurn { get => bulletsTurn; set => bulletsTurn = value; }
    public AudioClip DialogueMusic { get => dialogueMusic; set => dialogueMusic = value; }
    public AudioClip FightMusic { get => fightMusic; set => fightMusic = value; }

    private void Awake()
    {
        if(instance == null)instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        UpdateAudioClipResource();
    }

    private void Start()
    {
        audioSource1MainSource = GetComponent<AudioSource>();
        audioSource2EffectSource = transform.GetChild(0).GetComponent<AudioSource>();
    }
    private void Update()
    {
        audioSource1MainSource = GetComponent<AudioSource>();
        audioSource2EffectSource = transform.GetChild(0).GetComponent<AudioSource>();
    }
    public void SetVolumn(float volumn)
    {
        audioSource1MainSource.volume = volumn;
        audioSource2EffectSource.volume = volumn;
    }
    public void UpdateAudioClipResource()
    {
        revolver_Spin = Resources.Load<AudioClip>(revolver_Spin_Path);
        revolver_Fire = Resources.Load<AudioClip>(revolver_Fire_Path);
        revolver_NoBullet = Resources.Load<AudioClip>(revolver_NoBullet_Path);
        revolver_MissFire = Resources.Load<AudioClip>(revelver_MissFire_Path);
        hangMusic = Resources.Load<AudioClip>(hangMusicPath);
        clickMusic = Resources.Load<AudioClip>(clickMusicPath);
        bulletsIn = Resources.Load<AudioClip>(BulletsInPath);
        bulletsTurn = Resources.Load<AudioClip>(BulletsTurnPath);
        dialogueMusic = Resources.Load<AudioClip>(DialogueMusicPath);
        fightMusic = Resources.Load<AudioClip>(FightMusicPath);
    }
}
