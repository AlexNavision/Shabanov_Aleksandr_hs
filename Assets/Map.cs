using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static VectorXZ[] CheckPoint;
    public static VectorXZ Base;
    [Header("���������")]
    [SerializeField,Range(2,10),Tooltip("���������� ����� �� �����")] private int CheckPointCount = 2;
    [SerializeField, Range(1, 5), Tooltip("��� ���������� ����� �� �����")] private float CheckPointMinDistance = 1f;
    [SerializeField] private GameObject CheckPointObj;
    [SerializeField] private GameObject BaseObj;
    private void Awake()
    {
        //������ ����� ������� (�������, ������ ��� � ����� ����� ������ ���������� ���� �� ������� � �����������)
        Queue<VectorXZ> CheckPointTemp = new Queue<VectorXZ>();
        //������ ���� �� ���������� ������������
        CheckPointTemp.Enqueue(new VectorXZ(8f, 3f));

        //������ ��������� � ���������� ������������, �� � ��������: �� ������ ���. ��������� � ����� ������ ���������� � �����
        VectorXZ CheckPointPosTemp;
        int count = 0;
        while (CheckPointTemp.Count < CheckPointCount + 1)
        {
            count++;
            if (count > 1000) { print("������� ����� �������"); break; } //���� �� ���������� �������� ���������� ��� �������, ������ �����������.
            CheckPointPosTemp = new VectorXZ(8f, 3f);
            //print("��������� ���������� = " + CheckPointPosTemp);
            foreach (VectorXZ CheckPoint in CheckPointTemp)
            {
                //print(CheckPoint.Distance(CheckPointPosTemp));
                if (CheckPoint.Distance(CheckPointPosTemp) < CheckPointMinDistance)
                    goto endWhile;
            }
            Instantiate(CheckPointObj, CheckPointPosTemp.position, Quaternion.identity);
            CheckPointTemp.Enqueue(CheckPointPosTemp);
            endWhile:;
        }
        //print(CheckPointTemp.Count);
        //������� �� ������ ���������� ���������� ���� � ������� � �� ����
        Base = new VectorXZ(Instantiate(BaseObj, CheckPointTemp.Dequeue().position, Quaternion.identity).transform.position);

        //��������� ������ � �����������
        CheckPoint = new VectorXZ[CheckPointTemp.Count];
        count = 0;
        foreach (VectorXZ CheckPointXZ in CheckPointTemp)
            CheckPoint[count++] = CheckPointXZ;
    }
}
/// <summary>
/// �����, ������� 3D ������������ ���������� � 2D ��������� �� XZ. (��� y = 4f �� ������� ��� ����� ����������� �������)
/// </summary>
public class VectorXZ
{
    private float x;
    private float z;
    public Vector3 position => new Vector3(x, -4f, z);
    public VectorXZ(Vector3 position)
    {
        x = position.x;
        z = position.z;
    }
    public VectorXZ(float x,float z)
    {
        this.x = Random.Range(-x, x);
        this.z = Random.Range(-z, z);
    }
    public float Distance(VectorXZ newCheckPoint)
    {
        //Debug.Log(newCheckPoint);
        //Debug.Log(this);
        return Mathf.Abs(Mathf.Sqrt(Mathf.Pow(newCheckPoint.x - x, 2) + Mathf.Pow(newCheckPoint.z - z, 2)));
    }
    public float Distance(Vector3 newCheckPoint)
    {
        return Mathf.Abs(Mathf.Sqrt(Mathf.Pow(newCheckPoint.x - x, 2) + Mathf.Pow(newCheckPoint.z - z, 2)));
    }
    public override string ToString()
    {
        return $"({x},{z})";
    }
}
