using System;
using System.Collections.Generic;

public interface IUpgradable
{
    public int Level { get; }

    public int Upgrade();

    public int GetUpgradePrice();

    public int CanAffordUpgrade();

    public bool IsCanUpgrade();
}
