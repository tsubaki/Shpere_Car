using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] ParticleSystem _particle; // カーブ時のパーティクル
    [SerializeField] GameObject _car; // 車のガワ
    [Range(4, 10)] [SerializeField] float _power = 9f; // エンジン出力
    [Range(5, 20)] [SerializeField] float _handlingPower = 1f; // 曲がる力
    [SerializeField] Vector3 _offset; // 玉と車モデルのギャップを埋めるオフセット

    Rigidbody _rig;
    ParticleSystem.EmissionModule _emissionModule;
    Animator _carAnimator;
    Transform _carTransform;

    bool _controllable = true;
    bool _isGround;
    Quaternion _carRotation;
    Vector3 CarForward { get => _carRotation * Vector3.forward; }

    void Awake()
    {
        _rig = GetComponent<Rigidbody>();
        _emissionModule = _particle.emission;
    }

    void Start()
    {
        _carAnimator = _car.GetComponent<Animator>();
        _carTransform = _car.GetComponent<Transform>();
        _carRotation = _carTransform.rotation;
    }

    void FixedUpdate()
    {
        // 操作可能かつ着地していれば、モデルの方向に前進
        if (_isGround && _controllable)
            _rig.AddForce(CarForward * _power, ForceMode.Force);

        // モデルとボールの向きと座標を同期
        _carTransform.position = transform.position + _offset;
        _carTransform.rotation = _carRotation;
    }

    void Update()
    {
        // モデルの向きと移動方向が一致していない & 接地していればドリフト
        var isDrifting = Vector3.Dot(CarForward.normalized, _rig.velocity.normalized) < 0.866f;
        _isGround = Physics.Raycast(transform.position, Vector3.down, _offset.y * -1.1f);
        _emissionModule.enabled = isDrifting && _isGround;

        // 操作可能ならハンドル操作を受け付ける
        if (_controllable)
        {
            var handle = Input.GetAxis("Horizontal");
            _carAnimator.SetFloat("Handle", handle, 0.1f, Time.deltaTime);
            _carRotation *= Quaternion.AngleAxis(handle * _handlingPower * _power * Time.deltaTime, Vector3.up);
        }
    }

    public void SetControllable(bool value)
    {
        _controllable = value;
    }
}
