using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class TitleCanvas : MonoBehaviour
{
    [SerializeField] Button _startButton;
    
    void Start()
    {
        _startButton.OnClickAsObservable().Subscribe(_ =>
        {
            var stageName = "Stage1";
            Addressables.LoadSceneAsync($"Assets/Scenes/Stages/{stageName}.unity");
        });
    }
}
