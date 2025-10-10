using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool _canBeChased = true;
    private bool _canBeCollected = true;

    public bool CanBeChased => _canBeChased;
    public bool CanBeCollect => _canBeCollected;

    public void OnChase()
    {
        _canBeChased = false;
    }

    public void OnCollect()
    {
        _canBeCollected = false;
    }

    private void OnDisable()
    {
        _canBeChased = true;
        _canBeCollected = true;
    }
}
