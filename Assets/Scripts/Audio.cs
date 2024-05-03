using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Audio : MonoBehaviour
{
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
    
    public void PlaySE(Clip clip)
    {
        _audioSourceSE.PlayOneShot(_clips[(int)clip]);
    }
    
    public void PlayBGM()
    {
        _audioSourceBGM.DOFade(0f, 0);
        _audioSourceBGM.Play();
        _audioSourceBGM.DOFade(1f, 2f);
    }
    
    public void FadeOutBGM()
    {
        _audioSourceBGM.DOFade(0f, 2f);
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
