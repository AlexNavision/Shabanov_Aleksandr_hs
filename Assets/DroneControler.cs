using System.Collections;
using UnityEngine;
using static Command;

public class DroneControler : MonoBehaviour
{
    public static DroneControler instance { get; private set; }
    [SerializeField] public GameObject Hit; //При клике по персонажу из него будет что-то вылетать.
    public delegate void DroneInput(action command);
    public static event DroneInput DronesInput;
    void Start()
    {
        instance = this;
    }
    public void MenuInput(int click)
    {
        action command = GetCommand(click);
        DronesInput(GetCommand(click));
    }
}

public static class Command
{
    public enum action
    {
        idle,
        move,
        gobase
    }
    public static action GetCommand(int i) =>
        i switch
        {
            1 => action.move,
            2 => action.gobase,
            _ => action.idle
        };
    /*  Хотел сделать через анимацию, но потом решил что в данном ТЗ это не требуется.
    public static string GetAnimationName(action c) =>
        c switch
        {
            action.move => "move",
            action.gobase => "gobase",
            _ => "idle"
        };
    */
}

public interface IDrone
{
    /// <summary> Обработчик команд </summary>
    void InputCommand(action command);
    /// <summary> Куда-то Идем </summary>
    IEnumerator DoCommand(VectorXZ target);
    /// <summary> Идлим </summary>
    IEnumerator Idle();
}