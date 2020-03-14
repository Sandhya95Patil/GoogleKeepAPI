//-----------------------------------------------------------------------
// <copyright file="AccountRL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
using System;

namespace RepositoryLayer.Service
{
    internal class FCMSender : IDisposable
    {
        private string serverKey;
        private string firstName;

        public FCMSender(string serverKey, string firstName)
        {
            this.serverKey = serverKey;
            this.firstName = firstName;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        internal void SendAsync(string email, object p)
        {
            email = "";
            p = "";
        }
    }
}