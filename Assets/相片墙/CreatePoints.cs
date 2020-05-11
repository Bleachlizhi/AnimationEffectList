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
    void Start()
    {
        for (int i = 0; i < _widthCount; i++)
        {
            for (int j = 0; j < _heightCount; j++)
            {
                GameObject go = Instantiate(_prefab) as GameObject;
                go.transform.SetParent(this.transform);
                go.transform.localScale = Vector3.one;
                go.transform.rotation = Quaternion.identity;
                float width = Mathf.Abs(go.GetComponent<SpriteRenderer>().sprite.textureRect.width/Camera.main.transform.position.z);
                float height = Mathf.Abs(go.GetComponent<SpriteRenderer>().sprite.textureRect.height/ Camera.main.transform.position.z);
                go.transform.localPosition = new Vector3(i * (width + _offect.x), j * (height + _offect.y), 0) - new Vector3(_widthCount * (width + _offect.x)/2, _heightCount * (height + _offect.y) /2);
            }
        }
    }
}
