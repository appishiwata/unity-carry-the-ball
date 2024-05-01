using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] GameObject _ball;
    private Rigidbody _ballRigidbody;
    private Vector3 _lastMousePos;
    private readonly float _power = 5f;
    
    void Start()
    {
        _ballRigidbody = _ball.GetComponent<Rigidbody>();
        bool isMouseDown = false;
    
        // フリック操作で移動
        this.UpdateAsObservable().Subscribe(_ => 
        {
            if (Input.GetMouseButtonDown(0))
            {
                this._lastMousePos = Input.mousePosition;
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
}
