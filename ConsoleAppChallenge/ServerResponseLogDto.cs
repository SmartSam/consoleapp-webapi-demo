using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppChallenge
{
    public class ServerResponseLogDto
    {
        public DateTime Starttime { get; set; }
        public DateTime Endtime { get; set; }
        public int HttpStatus { get; set; }
        public string ResponseText { get; set; }
        public int ErrorCode { get; set; }
    }
}
