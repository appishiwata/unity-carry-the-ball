using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody _ballRigidbody;
    private Vector3 _lastMousePos;
    private readonly float _power = 5f;
    
    [SerializeField] Transform _startTransform;
    [SerializeField] Transform _goalTransform;
    private Vector3 _startPosition;
    private Vector3 _goalPosition;
    
    private bool _hasReachedGoal;
    
    [SerializeField] AdMobInterstitial _adMobInterstitial;

    void Start()
    {
        _ballRigidbody = GetComponent<Rigidbody>();
        bool isMouseDown = false;
        
        _startPosition = _startTransform.position;
        _goalPosition = _goalTransform.position;
        
        // スタート位置から始める
        transform.position = _startPosition;
    
        // フリック操作で移動
        this.UpdateAsObservable().Subscribe(_ => 
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePos = Input.mousePosition;
                isMouseDown = true;
            }

            if (isMouseDown && Input.GetMouseButton(0))
            {
                var mousePos = Input.mousePosition;
                Vector3 direction = (mousePos - _lastMousePos).normalized;
                direction = Camera.main!.transform.rotation * direction;
                _ballRigidbody.AddForce(new Vector3(direction.x, 0, direction.y) * _power);
                _lastMousePos = mousePos;
            }
            if(Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
            }
        }).AddTo(this);
    }
    
    private async void OnTriggerEnter(Collider other)
    {
        // ステージ外に出たらスタート地点に戻す
        if (other.CompareTag("OutOfBounds"))
        {
            transform.position = _startPosition;
            
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        // ゴール範囲に入ったらゴール地点に移動
        if (!_hasReachedGoal && other.CompareTag("Goal"))
        {
            _hasReachedGoal = true;

            SaveManager.Instance.StageCleared++;
            Audio.Instance.PlaySE(Audio.Clip.ClearStage);
            
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            
            var sequence = DOTween.Sequence();
            await sequence.AppendInterval(0.1f)
                .AppendCallback(() =>
                {
                    transform.DOMove(_goalPosition, 0.8f);
                })
                .AppendInterval(0.8f)
                .AppendCallback(() =>
                {
                    // TODO 別スクリプト使う処理をシングルトンで共通化
                    IngamePanel ingamePanel = FindObjectOfType<IngamePanel>();
                    if (ingamePanel != null)
                    {
                        ingamePanel.ShowClearPanel();
                    }
                });
            
            if (SaveManager.Instance.StageCleared % 3 == 0)
            {
                _adMobInterstitial.ShowAdMobInterstitial();
            }
        }
    }
    
    public void ResetBall()
    {
        transform.position = _startPosition;
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false;
        }
    }
}
