using System.Collections;
using UnityEngine;
using static Command;

public class DroneControler : MonoBehaviour
{
    public static DroneControler instance { get; private set; }
    [SerializeField] public GameObject Hit; //��� ����� �� ��������� �� ���� ����� ���-�� ��������.
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
    /*  ����� ������� ����� ��������, �� ����� ����� ��� � ������ �� ��� �� ���������.
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
    /// <summary> ���������� ������ </summary>
    void InputCommand(action command);
    /// <summary> ����-�� ���� </summary>
    IEnumerator DoCommand(VectorXZ target);
    /// <summary> ����� </summary>
    IEnumerator Idle();
}