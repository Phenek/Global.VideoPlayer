using System.Threading.Tasks;

namespace Global.VideoPlayer
{
    public interface IVideoPicker
    {
        Task<string> GetVideoFileAsync();
    }
}