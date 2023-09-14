using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ser_ver
{
    internal class Request
    {
        public string command { get; set; }
        public string data { get; set; }

        public Request(string command, string data)
        {
            this.command = command;
            this.data = data;
        }
    }
}
