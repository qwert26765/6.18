using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//-------------------------��ʮ�Ų�

public class SingleItem : MonoBehaviour
{   
    public Vector3 startPos = new Vector3(0, 0, 0);// ��ʼλ��--------------------������
    
    public float width;// ��Ԫ����--------------------������  

    public float height;// ��Ԫ��߶�--------------------������  

    public List<Sprite> sprites;// ���õľ���ͼƬ�б�---------------------------�ھŲ� 
    
    public int type;// ��ǰ��Ԫ�������-----------------------�ھŲ�

    public SpriteRenderer itemSprite; // ������Ⱦ����������ʾ��Ԫ���ͼƬ------------------�ھŲ� 

    public event Action<SingleItem> mouseButton;// ������¼�---------------------��ʮһ��
    
    public int col;// ��Ԫ���������--------------------���Ĳ�

    public int row;// ��Ԫ���������--------------------���Ĳ�

    public GameObject block;// ������ʾѡ��״̬�Ŀ�  ------------------------��ʮ�Ĳ�  ���ȸ�ֵͼƬ

    private void Start()
    {
        // ��ʼ��ʱ����Ϊδѡ��״̬---------------------------��ʮ�岽
        SetSelect(false);
    }

    // ��ʼ����Ԫ��----------------------------------���Ĳ�
    public void BirthItem(int row, int col)
    {
       
        SetItemIndex(row, col); // ���õ�Ԫ�����������--------------------------���岽  
        
        SetItemWorldPos(row, col);// ���õ�Ԫ���ڳ����е�λ��--------------------���߲�  
        
        SetItemSpritemRandom();// ������õ�Ԫ�����ʽͼƬ-----------------------�ڰ˲� 
    }

    // ע��������¼�-----------------------��ʮһ�� 
    public void RegisterMouseButtonAction(Action<SingleItem> itemAction)
    {
        mouseButton += itemAction;
    }

    // ���õ�Ԫ���ѡ��״̬-------------------------��ʮ�岽
    public void SetSelect(bool value)
    {
        block.SetActive(value);
    }

    // �Ƚϵ�ǰ��Ԫ������һ����Ԫ���Ƿ�����----------------------------�ڶ�ʮһ��  
    public bool CompareTypeWith(SingleItem compareItem)
    {
        // ͬ�������������1  
        if (compareItem.col == col && Math.Abs(compareItem.row - row) == 1)
        {
            return true;
        }
        // ͬ�������������1  
        if (compareItem.row == row && Math.Abs(compareItem.col - col) == 1)
        {
            return true;
        }
        return false;
    }

    // ������õ�Ԫ�����ʽͼƬ-------------------------�ڰ˲� 
    private void SetItemSpritemRandom()
    {
        
        int index = UnityEngine.Random.Range(0, sprites.Count);// ���ѡ��һ��ͼƬ����------------------�ھŲ�
        
        type = index;// ���õ�Ԫ������-------------------------------�ھŲ�

        itemSprite.sprite = sprites[index];// ���þ���ͼƬ------------------�ھŲ�  ����֮���ȹ��ؽű�
    }

    // ���õ�Ԫ�����������------------------------���岽
    public void SetItemIndex(int r, int c)
    {
        col = c;
        row = r;
    }

    // �ƶ���Ԫ��ָ��������λ��-------------------��ʮ�˲�  ����ȥ���һ��DOTween���
    public void MoveTo(int r, int c, int t)
    {
        
        SetItemIndex(r, c);// ������������-------------------------��ʮ�Ų�

        Vector3 pos = startPos + new Vector3(c * width, -r * height, 0);// ����Ŀ��λ��-------------------��ʮ�Ų�  

        transform.DOMove(pos, t);// ʹ�ö����ƶ���Ŀ��λ��-------------------------��ʮ�Ų�  
    }

    // ���õ�Ԫ���ڳ����е�λ��------------------------------���߲�
    private void SetItemWorldPos(int r, int c)
    {
        // ���㲢����λ��  
        transform.position = startPos + new Vector3(+c * width, -r * height, 0);
    }

    // ������¼����� ---------------------------------��ʮ��  ���������ײ�����  
    private void OnMouseDown()
    {
        if (mouseButton != null)
        {
            mouseButton(this);
        }
    }
}
