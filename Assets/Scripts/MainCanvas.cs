using UnityEngine;
using UnityEngine.SceneManagement;
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
            SceneManager.LoadScene("Stage1");
        });
        _buttons[1].onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Stage2");
        });
        _buttons[2].onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Stage3");
        });
    }
}
