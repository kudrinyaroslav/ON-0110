
namespace CameraClient
{
    public enum InformationType
    {
        Request,
        Response,
        Service,
        Error
    }
    public interface IListener
    {
        void WriteLine(string message);
        void WriteLine(string message, InformationType type);
    }
}
