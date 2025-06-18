using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
/// <summary>
/// 第二步
/// </summary>
public class UnityPool : MonoBehaviour
{
    public SingleItem itemModel; // 用于生成对象池中对象的模板  

    private ObjectPool<SingleItem> pool; // 对象池实例

    int poolSize = 50; // 对象池的初始大小  

    int poolMaxSize = 100; // 对象池的最大大小  

    private void Awake()
    {
        // 初始化对象池，指定创建、获取、释放和销毁对象的回调方法  
        pool = new ObjectPool<SingleItem>(OnCreate, OnGet, OnRelease, OnDes, true, poolSize, poolMaxSize);
    }

    // 从对象池中获取一个对象  
    public SingleItem Get()
    {
        return pool.Get();
    }

    // 将对象释放回对象池  
    public void Release(SingleItem obj)
    {
        pool.Release(obj);
    }

    // 创建对象池中的新对象  
    private SingleItem OnCreate()
    {
        SingleItem go = Instantiate(itemModel); // 实例化一个新的对象  
        return go;
    }

    // 当从对象池中获取对象时调用  
    private void OnGet(SingleItem obj)
    {
        obj.gameObject.SetActive(true); // 激活对象  
    }

    // 当将对象释放回对象池时调用  
    private void OnRelease(SingleItem obj)
    {
        obj.gameObject.SetActive(false); // 禁用对象  
    }

    // 当对象池销毁对象时调用  
    private void OnDes(SingleItem obj)
    {
        Destroy(obj.gameObject); // 销毁对象  
    }
}
