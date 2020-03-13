using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DesktopFrontend.Models
{
    public class MockServerConnection : IServerConnection
    {
        public bool IsConnected { get; private set; }

        public async Task<bool> Connect()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return IsConnected = true;
        }

        public Task<bool> LogInWithCredentials(string user, string pass)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RequestANewAccount()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> RequestThreadSet(string name)
        {
            throw new NotImplementedException();
        }
    }
}