using UnityEngine;

[RequireComponent(typeof(BaseResourceScaner))]
public class Base : MonoBehaviour
{
    private BaseResourceScaner _resourceScaner;
    private Robot _robot;

    public void Initialize(Robot robot, Scanner scaner)
    {
        _robot = robot;
        _resourceScaner.Initialize(scaner);
    }

    private void Awake()
    {
        _resourceScaner = GetComponent<BaseResourceScaner>();
    }

    private void Start()
    {
        _resourceScaner.StartDetect();
    }

    private void OnEnable()
    {
        _resourceScaner.NearestDetected += SendRobotCollect;
    }

    private void OnDisable()
    {
        _resourceScaner.NearestDetected -= SendRobotCollect;
    }

    private void SendRobotCollect(Resource target)
    {
        if (_robot.IsWork)
            return;

        target.OnChase();
        _robot.MoveTo(target.transform.position);
    }
}
