public interface IObjective
{
    bool IsDone { get; }
    bool IsSuccess { get; }
    void SetEnabled(bool value);

}

public interface IPlayer
{
    void SetControllable(bool value);
}