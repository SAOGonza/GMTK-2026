using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int requiredPowerCells = 10;
    public int PowerCellCount { get; private set; }
    public int AntidoteCount { get; private set; }

    public int RequiredPowerCells => requiredPowerCells;

    public bool HasEnoughPowerCells => PowerCellCount >= requiredPowerCells;

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

    public void RemovePowerCells(int amount)
    {
        PowerCellCount = Mathf.Max(0, PowerCellCount - amount);
        OnPowerCellCountChanged?.Invoke(PowerCellCount);
    }
}
