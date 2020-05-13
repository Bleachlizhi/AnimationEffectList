using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemController : MonoBehaviour
{
    public enum MoveType
    {
        NORMAL = 0,
        CREATE = 1,
        SELECT = 2,
        DISPERSED = 3
    }
    //移动方式
    [SerializeField]
    private MoveType _moveType;
    public MoveType _MoveType
    {
        get => _moveType;
        set
        {
            _moveType = value;
        }
    }
    private Tween _tween;
    private Vector3 _initPos;
    private Vector3 _targetPos;
    private float _size;
    private int _order;
    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    public void Init(Vector3 initPos)
    {
        this._initPos = initPos;
        _MoveType = MoveType.CREATE;
    }
    public void GetPos(Vector3 target,float size,int order)
    {
        this._targetPos = target;
        this._size = size;
        this._order = order;
    }
    private void  Update()
    {
        SetMoveType(this._moveType);
    }
    /// <summary>
    /// 设置动画模式
    /// </summary>
    /// <param name="moveType"></param>
    void SetMoveType(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.NORMAL:
                break;
            case MoveType.CREATE:
                PlayCreateAnimation();
                break;
            case MoveType.SELECT:
                PlaySelectAnimation();
                break;
            case MoveType.DISPERSED:
                PlayDispersedAnimation(this._size,this._order);
                break;
            default:
                break;
        }
    }
   /// <summary>
   /// 生成动画播放完成
   /// </summary>
    void PlayCreateAnimation()
    {
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, _initPos, Time.deltaTime * 5);
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one, Time.deltaTime * 5);
        this._spriteRenderer.color = Color.Lerp(this._spriteRenderer.color, Color.white, Time.deltaTime * 5);
        this._spriteRenderer.sortingOrder = 0;
    }
    /// <summary>
    /// 播放选中动画
    /// </summary>
    void PlaySelectAnimation()
    {
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one * 3, Time.deltaTime * 10);
    }
    /// <summary>
    /// 播放分散动画
    /// </summary>
    void PlayDispersedAnimation(float size,int order)
    {
        this._spriteRenderer.sortingOrder =  order;
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, this._targetPos, Time.deltaTime * 10);
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one * size ,Time.deltaTime * 10);
        this._spriteRenderer.color = Color.Lerp(this._spriteRenderer.color, new Color(1,1,1,size), Time.deltaTime * 10);
    }
}
