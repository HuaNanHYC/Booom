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

    private void Awake()
    {
        if(instance == null)instance = this;
        else Destroy(instance.gameObject);
        DontDestroyOnLoad(gameObject);
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
}
