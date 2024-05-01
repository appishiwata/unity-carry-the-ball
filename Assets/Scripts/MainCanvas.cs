using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        Debug.Log(GamesData.Instance.GetFirst().アプリ名_日本語);
    }
}
