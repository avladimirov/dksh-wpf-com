namespace DKSH.AuditionApp.Domain.Interfaces
{
    public interface IChannel : ITransmit
    {
        bool IsActive { get; }
    }
}
