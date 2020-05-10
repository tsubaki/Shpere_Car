public interface IObjective
{
    bool IsDone { get; }
    bool IsSuccess { get; }
    void SetEnabled(bool value);

}