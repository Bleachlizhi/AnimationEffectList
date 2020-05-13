using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhotoWall : MonoBehaviour
{

    public RectTransform prefab;

    public int row = 10;       // 行
     public int column = 15;        // 列

    //第一张照片的位置
    int startXPos = 0;
    int startZPos =0;

    // 照片 X 轴上的间距（Image 目前 （50，50），所以大于 50 照片间即可有间距，Min 和 Max 可以设置不同）
    public float distanceRandomMinX = 150;
    public float distanceRandomMaxX = 150;

    // 照片 Y 轴上的间距（Image 目前 （50，50），所以大于 50 照片间即可有间距，Min 和 Max 可以设置不同）
    public float distanceRandomMinY = 150;
    public float distanceRandomMaxY = 150;

    // 照片移动的距离，根据自己的 Canvas 画布和 Image 大小适配
    public float initMoveDistance = 1000;

    // 选中照片时，放大的倍数
    public float enlargeSize = 5;

    // 选中照片时，周围照片的改变范围
    public float radiateSize = 220;

    // 照片集合列表
    List<List<RectTransform>> goList;

    // 照片和位置字典
    Dictionary<RectTransform, Vector2> itemPosDict;

    // 选中照片，照片变动集合
    List<RectTransform> changedItemList;

    // Use this for initialization
    void Start()
    {

        goList = new List<List<RectTransform>>();
        itemPosDict = new Dictionary<RectTransform, Vector2>();
        changedItemList = new List<RectTransform>();

        CreateGos();
    }


    void CreateGos()
    {
        // 生成所有物体，并添加到字典
        for (int i = 0; i < row; i++)
        {
            List<RectTransform> gos = new List<RectTransform>();
            goList.Add(gos);
            float lastPosX = 0;
            for (int j = 0; j < column; j++)
            {
                // 生成照片，并设置照片名称，以及父物体
                RectTransform item = (Instantiate(prefab.gameObject) as GameObject).GetComponent<RectTransform>();
                item.name = i + " " + j;
                item.transform.SetParent(transform);

                // 设置 照片的初始位置，以及最终运动到的位置
                Vector3 startPos = new Vector3(Random.Range(distanceRandomMinX, distanceRandomMaxX) + lastPosX, startZPos - i * Random.Range(distanceRandomMinY, distanceRandomMaxY));
                item.anchoredPosition = startPos;
                Vector3 endPos = new Vector3(startPos.x - initMoveDistance, startZPos - i * Random.Range(distanceRandomMinY, distanceRandomMaxY));

                // DOTween 开始延迟动画，最后一行先开始运动，依次网上
                Tweener tweener = item.DOAnchorPosX(endPos.x, Random.Range(1.8f, 2f));  // 缓动到目标位置
                tweener.SetDelay(j * 0.1f + (row - i) * 0.1f);      // 延时
                tweener.SetEase(Ease.InOutBounce);           // 缓动效果

                // 适配 Canvas 的 Render Mode 的任何形式，保证显示正常
                item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
                item.transform.localScale = Vector3.one;

                // 修改 Image下的 Text 文本信息
                item.transform.GetChild(0).GetComponent<Text>().text = item.name;

                //添加到集合
                item.gameObject.SetActive(true);
                gos.Add(item);
                itemPosDict.Add(item, endPos);

                // 更新当前 X 轴点信息
                lastPosX = item.anchoredPosition.x;
            }
        }
    }


    /// <summary>
    /// 鼠标进入的事件
    /// </summary>
    /// <param name="item">当前的UI物体</param>
    public void OnMousePointEnter(RectTransform item)
    {

        // 缓动改变中心物体（选中照片）尺寸
        item.DOScale(enlargeSize, 0.5f);

        Vector2 pos = itemPosDict[item];

        // 更新集合
        changedItemList = new List<RectTransform>();

        // 遍历字典，添加扩散物体到集合
        foreach (KeyValuePair<RectTransform, Vector2> i in itemPosDict)
        {
            // 变动范围内的 照片集合
            if (Vector2.Distance(i.Value, pos) < radiateSize)
            {
                changedItemList.Add(i.Key);
            }
        }

        // 缓动来解决扩散物体的动画
        for (int i = 0; i < changedItemList.Count; i++)
        {
            // 方向上变动照片集合
            Vector2 targetPos = itemPosDict[item] + (itemPosDict[changedItemList[i]] - itemPosDict[item]).normalized * radiateSize;
            changedItemList[i].DOAnchorPos(targetPos, 0.8f);
        }
    }

    /// <summary>
    /// 鼠标移开的事件
    /// </summary>
    /// <param name="go"></param>
    public void OnMousePointExit(RectTransform go)
    {
        // 缓动恢复中心物体尺寸
        go.DOScale(1, 1);
        // 缓动将扩散物体恢复到初始位置
        for (int i = 0; i < changedItemList.Count; i++)
        {
            changedItemList[i].DOAnchorPos(itemPosDict[changedItemList[i]], 0.8f);
        }
    }

    /// <summary>
    /// 鼠标按下的事件
    /// </summary>
    /// <param name="go"></param>
    public void OnMousePointDown(RectTransform go)
    {
        // Image 变色
        go.gameObject.GetComponent<Image>().color = Color.red;


    }
}
