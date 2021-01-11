using System;
using System.Collections.Generic;
using System.Text;

namespace EasyTCP
{
    public class ChannelRegistrationException : Exception
    {
        public ChannelRegistrationException(string message) : base(message)
        {
        }
    }
}
