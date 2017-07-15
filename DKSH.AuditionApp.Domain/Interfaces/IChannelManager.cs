namespace DKSH.AuditionApp.Domain.Interfaces
{
    public interface IChannelManager : ITransmit
    {
        bool CanSend { get; }
    }
}
