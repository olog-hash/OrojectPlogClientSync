namespace ProjectOlog.Code.Infrastructure.TimeManagement.Interfaces
{
    public interface ILateUpdate
    {
        void OnLateUpdate(float deltaTime);
    }
}