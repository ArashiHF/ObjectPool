using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ObjectPoolTest : MonoBehaviour
{
    public Transform bornPoint;//敌人出生位置
    public GameObject enemy;//存放敌人预制体
    public int maxNumber = 30;//敌人最大数量
    public int poolMaxSize = 20;//对象池最大容量

    public int currentNumber;//显示当前存在的敌人数量
    [Header("模拟触发敌人生成与销毁")]
    public bool create;
    public bool destroy;

    //已下用于测试ObjectPool三个属性
    [Header("测试")]
    public int CountActive;//已知每调用一次Release方法就会-1，Get方法+1
    public int CountAll;//已知每调用一次Create方法就会+1
    public int CountInactive;//已知每调用一次Release方法就会+1，Get方法-1,最大值为 poolMaxSize

    ObjectPool<GameObject> enemyPool;//对象池
    int startNumber;//运行时场景中的敌人数量
    int id = 1;//作为敌人命名编号

    private void Awake()
    {
        //获取场景中的敌人数量
        startNumber = GameObject.FindGameObjectsWithTag("Enemy").Length;
        //构建对象池
        enemyPool = new ObjectPool<GameObject>(Create, Get, Release, MyDestroy, true, 10, poolMaxSize);
    }
    public GameObject Create()
    {
        GameObject gameObject = Instantiate(enemy, bornPoint.position, Quaternion.identity);
        gameObject.name = "敌人" + id++ + "号";
        gameObject.tag = "Enemy";
        Debug.Log("池为空,新建对象" + gameObject.name);
        return gameObject;
    }

    void Get(GameObject gameObject)
    {
        gameObject.transform.position = bornPoint.position;
        gameObject.SetActive(true);//显示敌人
        Debug.Log(gameObject.name + "出池");
    }
    void Release(GameObject gameObject)
    {
        gameObject.SetActive(false);//隐藏敌人
        Debug.Log(gameObject.name + "进池");
    }
    void MyDestroy(GameObject gameObject)
    {
        Debug.Log("池已满," + gameObject.name + "被销毁");
        Destroy(gameObject);
    }
    private void Update()
    {
        if (create)
        {
            create = false;
            enemyPool.Get();
        }
        if (destroy)
        {
            destroy = false;
            //获取场景中的所有显示的敌人
            GameObject[] enmeys = GameObject.FindGameObjectsWithTag("Enemy");
            //如果敌人不为空
            if (enmeys.Length > 0)
            {
                //随机选出一个敌人然后删除
                GameObject enemy = enmeys[Random.Range(0, enmeys.Length)];
                enemyPool.Release(enemy);
            }
        }
        //更新数据
        CountAll = enemyPool.CountAll;
        CountActive = enemyPool.CountActive;
        CountInactive = enemyPool.CountInactive;
        currentNumber = startNumber + CountActive;
    }
}