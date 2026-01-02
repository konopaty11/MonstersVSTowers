public interface IUpgradable
{
    public int Level { get; }

    public bool Upgrade();
}
