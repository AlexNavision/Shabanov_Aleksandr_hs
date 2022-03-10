using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Command;
using UnityEngine.EventSystems;

public class Drone : MonoBehaviour,IDrone
{
    //Можно сделать Scriptable Object для дальнейшего развития игровых объектов.
    [Header("Здоровье, Скорость, Урон по персонажу")]
    [SerializeField] private float health = 10;
    [SerializeField] private int speed = 1;
    [SerializeField,Tooltip("Урон ПО ПЕРСОНАЖУ")] private int ReceiveDamage = 1;
    action CurrCommand = action.gobase;
    private void Awake()
    {
        DroneControler.DronesInput += InputCommand;
    }
    private void Start()
    {
        InputCommand(action.idle);
    }
    public void InputCommand(action command)
    {
        if (CurrCommand == command || health < 1)
            return;
        StopAllCoroutines();
        CurrCommand = command;
        switch (command)
        {
            case action.idle: StartCoroutine(Idle()); break;
            case action.move: StartCoroutine(DoCommand(Map.CheckPoint[Random.Range(0,Map.CheckPoint.Length-1)])); break;
            case action.gobase: StartCoroutine(DoCommand(Map.Base)); break;
        }
    }

    public IEnumerator DoCommand(VectorXZ target)
    {
        Quaternion look = Quaternion.LookRotation(target.position - transform.position + new Vector3(0, 1));
        while (CurrCommand == action.move)
        {
            while (CurrCommand == action.move && target.Distance(transform.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, 1), speed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, look, 3f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            target = Map.CheckPoint[Random.Range(0, Map.CheckPoint.Length-1)];
            look = Quaternion.LookRotation(target.position - transform.position);
        }
        if (CurrCommand == action.gobase)
        {
            while (CurrCommand == action.gobase && target.Distance(transform.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, 1), speed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, look, 3f * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            InputCommand(action.idle);
        }
    }
    [SerializeField] UnityEngine.UI.Text hp;
    private void OnMouseDown()
    {
        if (health < 1)
            return;
        health -= ReceiveDamage;
        hp.text = health.ToString() + "hp";
        if (health < 1)
        {
            StopAllCoroutines();
            hp.text = "Dead";
            DroneControler.DronesInput -= InputCommand;
        }
        Instantiate(DroneControler.instance.Hit,transform.position,Quaternion.identity);
    }
    public IEnumerator Idle()
    {
        Vector3 Scale = transform.localScale;
        Vector3 ScaleChange = new Vector3(0.025f, 0.025f, 0.025f);
        while(CurrCommand == action.idle)
        {
            transform.localScale += ScaleChange * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (transform.localScale.x < 0.015f || transform.localScale.x > 0.04f)
                ScaleChange = -ScaleChange;
        }
        transform.localScale = Scale;
    }
    private void OnDestroy()
    {
        DroneControler.DronesInput -= InputCommand;
    }
}
