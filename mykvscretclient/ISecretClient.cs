using System;

namespace mykvscretclient
{
    using System.Threading.Tasks;

    public interface ISecretClient
    {
        string GetSecret(string key);
        Task<string> GetSecretAsync(string key);
    }
}
