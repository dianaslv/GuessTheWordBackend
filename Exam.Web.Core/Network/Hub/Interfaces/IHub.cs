using System.Threading.Tasks;

namespace Exam.Web.Core.Network.Hub.Interfaces
{
    public interface ICommunicationHub
    {
        Task SendMessageAsync(string user, string message);
    }
}