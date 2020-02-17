using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.MSMQ
{
    public class MSMQSender
    {
        /// <summary>
        /// Forgets the password message.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="token">The token.</param>
        /// <exception cref="Exception">throws the exception</exception>
        public void ForgetPasswordMessage(string email, string token)
        {
            MessageQueue messageQueue = null;

            if (MessageQueue.Exists(@".\Private$\ForgetQueue"))
            {
                messageQueue = new MessageQueue(@".\Private$\ForgetQueue");
                messageQueue.Label = "Testing Queue";
            }
            else
            {
                MessageQueue.Create(@".\Private$\ForgetQueue");
                messageQueue = new MessageQueue(@".\Private$\ForgetQueue");
                messageQueue.Label = "Newly Created Queue";
            }

            try
            {
                messageQueue.Send(email, token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
