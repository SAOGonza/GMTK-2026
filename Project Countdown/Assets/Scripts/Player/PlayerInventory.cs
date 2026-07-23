using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int PowerCellCount { get; private set; }
    public int AntidoteCount { get; private set; }

    public event Action<int> OnPowerCellCountChanged;

    public event Action<int> OnAntidoteCountChanged;

    public void AddPowerCell()
    {
        PowerCellCount++;
        OnPowerCellCountChanged?.Invoke(PowerCellCount);
    }

    public void AddAntidote()
    {
        AntidoteCount++;
        OnAntidoteCountChanged?.Invoke(AntidoteCount);
    }
}
