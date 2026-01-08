using System;
using System.Collections.Generic;

public interface IUpgradable
{
    public int Level { get; }

    public bool Upgrade();

    public bool IsCanUpgrade();
}
