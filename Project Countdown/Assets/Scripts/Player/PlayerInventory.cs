using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int PowerCellCount { get; private set; }

    public event Action<int> OnPowerCellCountChanged;

    public void AddPowerCell()
    {
        PowerCellCount++;
        OnPowerCellCountChanged?.Invoke(PowerCellCount);
    }
}
