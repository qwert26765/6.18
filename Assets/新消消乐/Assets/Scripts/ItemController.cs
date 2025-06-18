using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class ItemManager : MonoBehaviour
{
    public int col; // ����---------------------------------��һ��

    public int row; // ����---------------------------------��һ��

    public int time; // ����ʱ��----------------------�ڶ�ʮ�� 

    private int XiaoChuCount = 0; // ����������  

    private UnityPool itemPool; // Unity����أ����ڹ�����Ϸ����ĸ���--------------------------�ڶ���
    private SingleItem[,] itemList; // �洢������ϷԪ�صĶ�ά����---------------------------------��һ��
    private List<SingleItem> deleteList; // �洢��Ҫ������Ԫ���б�--------------------------�ڶ�ʮ�Ĳ�

    private SingleItem itemFitst; // ��ǰѡ�еĵ�һ��Ԫ��---------------��ʮ����

    // Start is called before the first frame update  
    void Start()
    {
        deleteList = new List<SingleItem>(); // ��ʼ�������б�--------------------------�ڶ�ʮ�Ĳ�
        itemPool = GetComponent<UnityPool>(); // ��ȡ��������-----------------------------�ڶ���
        itemList = new SingleItem[row, col]; // ��ʼ����ϷԪ������---------------------------------��һ��

        // ��ʼ����ϷԪ��-----------------------------------��һ��
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                SingleItem item = NewItem(i, j);
                itemList[i, j] = item;
            }
        }

        // ����Ƿ��п���������Ԫ��------------------�ڶ�ʮ����
        bool canDelete = CheckDelete();
        if (canDelete)
        {
            DeleteItem(); // ִ������  
        }
    }

    // ������Ԫ��------------------------------������
    SingleItem NewItem(int row, int col)
    {
        SingleItem item = itemPool.Get(); // �Ӷ���ػ�ȡ��Ԫ��-------------------------������
        item.transform.SetParent(transform); // ���ø�����---------------------------������
        item.BirthItem(row, col); // ��ʼ��Ԫ��λ��---------------------������

        // ע��������¼�-------------------��ʮ����   
        Action<SingleItem> itemAction = SelectedItem;
        item.RegisterMouseButtonAction(itemAction);
        return item;//-----------------------������
    }

    // ����ѡ��Ԫ�ص��߼�----------------------��ʮ����  
    private void SelectedItem(SingleItem item)
    {
        if (itemFitst == null) // ���û��ѡ�е�һ��Ԫ��--------------------��ʮ����  
        {
            itemFitst = item;  //---------------------��ʮ����
            itemFitst.SetSelect(true); // ����ѡ��״̬---------------------��ʮ����
        }
        else
        {
            itemFitst.SetSelect(false); // ȡ����һ��Ԫ�ص�ѡ��״̬---------------------��ʮ����
            if (item.CompareTypeWith(itemFitst)) // �������Ԫ��������ͬ---------------�ڶ�ʮ����
            {
                StartCoroutine(DelayTimeExchange(item, itemFitst)); // ��ʱ���� ----------------�ڶ�ʮ���� 
            }

            itemFitst = null; // ����ѡ��״̬----------------------��ʮ����  ��ȥ��Ԥ�������ѡ��ͼƬ
        }
    }

    // ��������Ԫ�ص�λ��--------------------------��ʮ�߲� 
    private void Exchange(SingleItem item1, SingleItem item2)
    {
        // ����Ԫ���������е�λ��  
        itemList[item1.row, item1.col] = item2;
        itemList[item2.row, item2.col] = item1;

        // ������������  
        int temp1_col = item1.col, temp1_row = item1.row;
        int temp2_col = item2.col, temp2_row = item2.row;

        item1.SetItemIndex(temp2_row, temp2_col);
        item2.SetItemIndex(temp1_row, temp1_col);

        // ִ���ƶ�����----------------------------�ڶ�ʮ�� 
        item1.MoveTo(item1.row, item1.col, time);
        item2.MoveTo(item2.row, item2.col, time);
    }

    // ��ʱ��������Ԫ��--------------------------�ڶ�ʮ���� 
    IEnumerator DelayTimeExchange(SingleItem item1, SingleItem item2)
    {
        Exchange(item1, item2); // ����Ԫ��  
        yield return new WaitForSeconds(time); // �ȴ������������  

        bool canDelete = CheckDelete(); // ����Ƿ��п���������Ԫ��  -----------------------�ڶ�ʮ�Ĳ� 
        if (canDelete)
        {
            DeleteItem(); // ִ������---------------------�ڶ�ʮ�岽
            XiaoChuCount++; // ������������  
            if (XiaoChuCount >= 3) // ����������������ﵽ3��  
            {
                XiaoChuCount = 0;
                Camera.main.transform.DOShakePosition(0.5f, 0.5f, 10, 90, false, true); // �������Ч��  
            }
        }
        else
        {
            Exchange(item1, item2); // ���û�п���������Ԫ�أ�������ԭλ��  
        }
    }

    // ִ�������߼�---------------------�ڶ�ʮ�岽
    private void DeleteItem()
    {
        int count = deleteList.Count;
        for (int i = 0; i < count; i++)
        {
            SingleItem item = deleteList[i];
            int tmpR = item.row, tmpC = item.col;
            itemPool.Release(item); // �ͷ�Ԫ�ص������  
            itemList[tmpR, tmpC] = null; // ��������е�λ��  

            // ���Ϸ���Ԫ����������  
            for (int j = tmpR - 1; j >= 0; j--)
            {
                SingleItem itemTmp = itemList[j, tmpC];
                itemList[j + 1, itemTmp.col] = itemTmp;
                itemList[j, tmpC] = null;
                itemTmp.SetItemIndex(j + 1, itemTmp.col);
                itemTmp.MoveTo(itemTmp.row, itemTmp.col, time);
            }

            // �ڶ���������Ԫ��  
            SingleItem newItem = itemPool.Get();
            newItem.transform.SetParent(transform);
            newItem.BirthItem(-1, item.col);
            newItem.MoveTo(0, newItem.col, time);
            itemList[0, newItem.col] = newItem;
        }

        deleteList = new List<SingleItem>(); // ��������б�  
        StartCoroutine(CheckDeleteNext()); // �����һ������  
    }

    // �����һ���Ƿ��������  
    IEnumerator CheckDeleteNext()
    {
        yield return new WaitForSeconds(time);
        bool canDelete = CheckDelete();
        if (canDelete)
        {
            DeleteItem();
        }
    }

    // ��Ԫ����ӵ������б�  
    private void AddToDeleteList(SingleItem newItem)
    {
        int index = deleteList.FindIndex(item => item.row == newItem.row && item.col == newItem.col);
        if (index == -1)
        {
            deleteList.Add(newItem);
        }
    }

    // ����Ƿ��п���������Ԫ��  -----------------------�ڶ�ʮ�Ĳ�
    private bool CheckDelete()
    {
        bool canDelete = false;

        for (int c = 0; c < col; c++)
        {
            for (int r = 0; r < row; r++)
            {
                // ��鴹ֱ�����Ƿ���������ͬ��Ԫ��  
                if (r < row - 2 && itemList[r, c] != null && itemList[r + 1, c] != null && itemList[r + 2, c] != null)
                {
                    if (itemList[r, c].type == itemList[r + 1, c].type && itemList[r, c].type == itemList[r + 2, c].type)
                    {
                        AddToDeleteList(itemList[r, c]);
                        AddToDeleteList(itemList[r + 1, c]);
                        AddToDeleteList(itemList[r + 2, c]);
                        canDelete = true;
                    }
                }

                // ���ˮƽ�����Ƿ���������ͬ��Ԫ��  
                if (c < col - 2 && itemList[r, c] != null && itemList[r, c + 1] != null && itemList[r, c + 2] != null)
                {
                    if (itemList[r, c].type == itemList[r, c + 1].type && itemList[r, c].type == itemList[r, c + 2].type)
                    {
                        AddToDeleteList(itemList[r, c]);
                        AddToDeleteList(itemList[r, c + 1]);
                        AddToDeleteList(itemList[r, c + 2]);
                        canDelete = true;
                    }
                }
            }
        }

        Debug.Log(canDelete); // ����Ƿ��������  
        return canDelete;
    }
}
