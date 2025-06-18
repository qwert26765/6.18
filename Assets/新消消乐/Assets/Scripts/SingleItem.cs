using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//-------------------------第十九步

public class SingleItem : MonoBehaviour
{   
    public Vector3 startPos = new Vector3(0, 0, 0);// 起始位置--------------------第六步
    
    public float width;// 单元格宽度--------------------第六步  

    public float height;// 单元格高度--------------------第六步  

    public List<Sprite> sprites;// 可用的精灵图片列表---------------------------第九步 
    
    public int type;// 当前单元格的类型-----------------------第九步

    public SpriteRenderer itemSprite; // 精灵渲染器，用于显示单元格的图片------------------第九步 

    public event Action<SingleItem> mouseButton;// 鼠标点击事件---------------------第十一步
    
    public int col;// 单元格的列索引--------------------第四步

    public int row;// 单元格的行索引--------------------第四步

    public GameObject block;// 用于显示选中状态的块  ------------------------第十四步  后先赋值图片

    private void Start()
    {
        // 初始化时设置为未选中状态---------------------------第十五步
        SetSelect(false);
    }

    // 初始化单元格----------------------------------第四步
    public void BirthItem(int row, int col)
    {
       
        SetItemIndex(row, col); // 设置单元格的行列索引--------------------------第五步  
        
        SetItemWorldPos(row, col);// 设置单元格在场景中的位置--------------------第七步  
        
        SetItemSpritemRandom();// 随机设置单元格的样式图片-----------------------第八步 
    }

    // 注册鼠标点击事件-----------------------第十一步 
    public void RegisterMouseButtonAction(Action<SingleItem> itemAction)
    {
        mouseButton += itemAction;
    }

    // 设置单元格的选中状态-------------------------第十五步
    public void SetSelect(bool value)
    {
        block.SetActive(value);
    }

    // 比较当前单元格与另一个单元格是否相邻----------------------------第二十一步  
    public bool CompareTypeWith(SingleItem compareItem)
    {
        // 同列且行索引相差1  
        if (compareItem.col == col && Math.Abs(compareItem.row - row) == 1)
        {
            return true;
        }
        // 同行且列索引相差1  
        if (compareItem.row == row && Math.Abs(compareItem.col - col) == 1)
        {
            return true;
        }
        return false;
    }

    // 随机设置单元格的样式图片-------------------------第八步 
    private void SetItemSpritemRandom()
    {
        
        int index = UnityEngine.Random.Range(0, sprites.Count);// 随机选择一个图片索引------------------第九步
        
        type = index;// 设置单元格类型-------------------------------第九步

        itemSprite.sprite = sprites[index];// 设置精灵图片------------------第九步  完事之后先挂载脚本
    }

    // 设置单元格的行列索引------------------------第五步
    public void SetItemIndex(int r, int c)
    {
        col = c;
        row = r;
    }

    // 移动单元格到指定的行列位置-------------------第十八步  后先去添加一个DOTween组件
    public void MoveTo(int r, int c, int t)
    {
        
        SetItemIndex(r, c);// 更新行列索引-------------------------第十九步

        Vector3 pos = startPos + new Vector3(c * width, -r * height, 0);// 计算目标位置-------------------第十九步  

        transform.DOMove(pos, t);// 使用动画移动到目标位置-------------------------第十九步  
    }

    // 设置单元格在场景中的位置------------------------------第七步
    private void SetItemWorldPos(int r, int c)
    {
        // 计算并设置位置  
        transform.position = startPos + new Vector3(+c * width, -r * height, 0);
    }

    // 鼠标点击事件触发 ---------------------------------第十步  后先添加碰撞器组件  
    private void OnMouseDown()
    {
        if (mouseButton != null)
        {
            mouseButton(this);
        }
    }
}
