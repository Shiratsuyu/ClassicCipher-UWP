using System.Threading.Tasks;

namespace ClassicCipher.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}