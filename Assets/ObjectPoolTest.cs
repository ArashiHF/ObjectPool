using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ObjectPoolTest : MonoBehaviour
{
    public Transform bornPoint;//���˳���λ��
    public GameObject enemy;//��ŵ���Ԥ����
    public int maxNumber = 30;//�����������
    public int poolMaxSize = 20;//������������

    public int currentNumber;//��ʾ��ǰ���ڵĵ�������
    [Header("ģ�ⴥ����������������")]
    public bool create;
    public bool destroy;

    //�������ڲ���ObjectPool��������
    [Header("����")]
    public int CountActive;//��֪ÿ����һ��Release�����ͻ�-1��Get����+1
    public int CountAll;//��֪ÿ����һ��Create�����ͻ�+1
    public int CountInactive;//��֪ÿ����һ��Release�����ͻ�+1��Get����-1,���ֵΪ poolMaxSize

    ObjectPool<GameObject> enemyPool;//�����
    int startNumber;//����ʱ�����еĵ�������
    int id = 1;//��Ϊ�����������

    private void Awake()
    {
        //��ȡ�����еĵ�������
        startNumber = GameObject.FindGameObjectsWithTag("Enemy").Length;
        //���������
        enemyPool = new ObjectPool<GameObject>(Create, Get, Release, MyDestroy, true, 10, poolMaxSize);
    }
    public GameObject Create()
    {
        GameObject gameObject = Instantiate(enemy, bornPoint.position, Quaternion.identity);
        gameObject.name = "����" + id++ + "��";
        gameObject.tag = "Enemy";
        Debug.Log("��Ϊ��,�½�����" + gameObject.name);
        return gameObject;
    }

    void Get(GameObject gameObject)
    {
        gameObject.transform.position = bornPoint.position;
        gameObject.SetActive(true);//��ʾ����
        Debug.Log(gameObject.name + "����");
    }
    void Release(GameObject gameObject)
    {
        gameObject.SetActive(false);//���ص���
        Debug.Log(gameObject.name + "����");
    }
    void MyDestroy(GameObject gameObject)
    {
        Debug.Log("������," + gameObject.name + "������");
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
            //��ȡ�����е�������ʾ�ĵ���
            GameObject[] enmeys = GameObject.FindGameObjectsWithTag("Enemy");
            //������˲�Ϊ��
            if (enmeys.Length > 0)
            {
                //���ѡ��һ������Ȼ��ɾ��
                GameObject enemy = enmeys[Random.Range(0, enmeys.Length)];
                enemyPool.Release(enemy);
            }
        }
        //��������
        CountAll = enemyPool.CountAll;
        CountActive = enemyPool.CountActive;
        CountInactive = enemyPool.CountInactive;
        currentNumber = startNumber + CountActive;
    }
}