using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static VectorXZ[] CheckPoint;
    public static VectorXZ Base;
    [Header("ЧекПоинты")]
    [SerializeField,Range(2,10),Tooltip("Количество точек на карте")] private int CheckPointCount = 2;
    [SerializeField, Range(1, 5), Tooltip("Мин расстояние точек на карте")] private float CheckPointMinDistance = 1f;
    [SerializeField] private GameObject CheckPointObj;
    [SerializeField] private GameObject BaseObj;
    private void Awake()
    {
        //Создаю новую очередь (очередь, потому что в конце нужно убрать координаты Базы из массива с Чекпоинтами)
        Queue<VectorXZ> CheckPointTemp = new Queue<VectorXZ>();
        //Создаю Базу со случайными координатами
        CheckPointTemp.Enqueue(new VectorXZ(8f, 3f));

        //Создаю ЧекПоинты с рандомными координатами, но с условием: не меньше мин. дистанции с любым другим чекПоинтом и Базой
        VectorXZ CheckPointPosTemp;
        int count = 0;
        while (CheckPointTemp.Count < CheckPointCount + 1)
        {
            count++;
            if (count > 1000) { print("Слишком много попыток"); break; } //Если не получается рандомом расставить все объекты, ставим ограничение.
            CheckPointPosTemp = new VectorXZ(8f, 3f);
            //print("Рандомные координаты = " + CheckPointPosTemp);
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
        //Убираем из списка ЧекПоинтов координаты Базы и создаем её на поле
        Base = new VectorXZ(Instantiate(BaseObj, CheckPointTemp.Dequeue().position, Quaternion.identity).transform.position);

        //Заполняем массив с ЧекПоинтами
        CheckPoint = new VectorXZ[CheckPointTemp.Count];
        count = 0;
        foreach (VectorXZ CheckPointXZ in CheckPointTemp)
            CheckPoint[count++] = CheckPointXZ;
    }
}
/// <summary>
/// Класс, который 3D пространство превращает в 2D плоскость по XZ. (Ось y = 4f по дефолту для этого конкретного проекта)
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
