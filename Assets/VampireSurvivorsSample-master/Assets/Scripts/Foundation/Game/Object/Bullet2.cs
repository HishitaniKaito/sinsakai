using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bullet2 : MonoBehaviour, IObjectPool
{
    [SerializeField] float _speed = 255;
    SpriteRenderer _image;
    Enemy _target;
    Vector3 _shootVec;

    [SerializeField] Exp exp = null;
    Vector3 exppop = new Vector3(0, 0, 0);
    ObjectPool<Exp> _expPool = new ObjectPool<Exp>();

    float _cRad = 0.0f;
    Vector3 _popPos ;

    float _timer = 0.0f;

    void Awake()
    {
        _image = GetComponent<SpriteRenderer>();
    }

    public void Shoot(Enemy target)
    {
        _target = target;
        if (_target == null) return;

        _shootVec = _target.transform.position - GameManager.Player.transform.position;
        _shootVec.Normalize();
    }

    void Update()
    {
        if (Player.hp < 0) return;
        if (!_isActrive) return;

        _popPos.x = GameManager.Player.transform.position.x + 5 * Mathf.Cos(_cRad);
        _popPos.y = GameManager.Player.transform.position.y + 5 * Mathf.Sin(_cRad);
        _cRad += 0.2f;
        this.transform.position = _popPos;
        //transform.position += _popPos * _speed * Time.deltaTime;
        //Debug.Log(this.transform.position);
        
        var list = GameManager.EnemyList;
        _target = null;
        Vector3 vec;
        foreach (var e in list)
        {
            if (e.IsActive)
            {
                vec = e.transform.position - this.transform.position;
                if (vec.magnitude < 1.5f)
                {
                    e.Damage();
                    Destroy();
                    break;
                }
                
            }
        }

        _timer += Time.deltaTime;
        if (_timer > 3.0f)
        {
            Destroy();
        }
    }

    //ObjectPool
    bool _isActrive = false;
    public bool IsActive => _isActrive;
    public void DisactiveForInstantiate()
    {
        _image.enabled = false;
        _isActrive = false;
    }
    public void Create()
    {
        _timer = 0.0f;
        _image.enabled = true;
        _isActrive = true;
    }
    public void Destroy()
    {
        _image.enabled = false;
        _isActrive = false;
    }
}
