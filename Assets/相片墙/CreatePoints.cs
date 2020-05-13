/* ***********************************************
* 记录内容：
* 作者：李智 
* 创建时间：2020-05-09 16:33:59
* 版本：1.0
* ************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePoints : MonoBehaviour
{
    [SerializeField]
    private int _widthCount;
    [SerializeField]
    private int _heightCount;
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Vector3 _offect;
    [SerializeField]
    private float _distance;

    private List<ItemController> _itemControllers = new List<ItemController>();

    Ray ray;
    RaycastHit raycastHit;

    private bool _isDown;
    void Start()
    {
        for (int i = 0; i < _widthCount; i++)
        {
            for (int j = 0; j < _heightCount; j++)
            {
                GameObject go = Instantiate(_prefab) as GameObject;
                go.transform.SetParent(this.transform);
                go.transform.rotation = Quaternion.identity;
                float width = Mathf.Abs(go.GetComponent<SpriteRenderer>().sprite.textureRect.width / 100);
                float height = Mathf.Abs(go.GetComponent<SpriteRenderer>().sprite.textureRect.height / 100);
                Vector3 pos = new Vector3(i * width, j * height , 0);
                go.transform.position = Random.insideUnitCircle * 100;
                go.GetComponent<ItemController>().Init(pos + new Vector3(width / 2, height / 2, 0) + new Vector3(i * _offect.x, j * _offect.y, 0));
                this._itemControllers.Add(go.GetComponent<ItemController>());
            }
        }
        this._isDown = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnMousePointDown();
    }
    void OnMousePointDown()
    {
        this._isDown = !this._isDown;
        if (this._isDown)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                raycastHit.transform.GetComponent<ItemController>()._MoveType = ItemController.MoveType.SELECT; ;
                foreach (var item in this._itemControllers)
                {
                    if (!item.transform.Equals(raycastHit.transform))
                    {
                        float sqrMagnitude = Vector3.Distance(item.transform.position, raycastHit.transform.position);
                        if (sqrMagnitude <= this._distance)
                        {
                            Vector3 diretion = (item.transform.localPosition - raycastHit.transform.localPosition).normalized;
                            float coefficient = sqrMagnitude / this._distance;
                            item.GetPos(raycastHit.transform.localPosition + diretion  * (this._distance  -  (10 * (1 -coefficient))), coefficient,(int) sqrMagnitude - (int)this._distance);
                            item._MoveType = ItemController.MoveType.DISPERSED;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (var item in this._itemControllers)
                item._MoveType = ItemController.MoveType.CREATE;
        }
    }
}
