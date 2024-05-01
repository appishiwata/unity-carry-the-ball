using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] Button[] _buttons;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        _buttons[0].onClick.AddListener(() =>
        {
            LoadSceneFromAddressable("Stage1");
        });
        _buttons[1].onClick.AddListener(() =>
        {
            LoadSceneFromAddressable("Stage2");
        });
        _buttons[2].onClick.AddListener(() =>
        {
            LoadSceneFromAddressable("Stage3");
        });
    }
    
    void LoadSceneFromAddressable(string stageName)
    {
        Addressables.LoadSceneAsync($"Assets/Scenes/Stages/{stageName}.unity");
    }
}
