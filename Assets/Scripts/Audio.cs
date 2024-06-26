using DG.Tweening;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio Instance { get; private set; }
    
    [SerializeField] AudioSource _audioSourceBGM;
    [SerializeField] AudioSource _audioSourceSE;
    
    public enum Clip
    {
        SelectStage,
        ClickMenu,
        ClearStage,
        MoveNext,
    }
    
    [SerializeField] AudioClip[] _clips;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlaySE(Clip clip)
    {
        _audioSourceSE.PlayOneShot(_clips[(int)clip]);
    }
    
    public void PlayBGM()
    {
        _audioSourceBGM.Play();
    }
    
    public void SetBGMVolume(float volume)
    {
        _audioSourceBGM.volume = volume;
    }
    
    public void SetSEVolume(float volume)
    {
        _audioSourceSE.volume = volume;
    }
}
