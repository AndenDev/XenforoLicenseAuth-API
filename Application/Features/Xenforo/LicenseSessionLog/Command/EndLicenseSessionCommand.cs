
using MediatR;

namespace Application.Features.Xenforo.LicenseSessionLog.Command
{
    public class EndLicenseSessionCommand : IRequest
    {
        public string SessionId { get; }
        public string EndedReason { get; }

        public EndLicenseSessionCommand(string sessionId, string endedReason)
            => (SessionId, EndedReason) = (sessionId, endedReason);
    }
}
