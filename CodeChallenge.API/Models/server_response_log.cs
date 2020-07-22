using System;

namespace CodeChallenge.API.Models
{
    
    public class server_response_log
    {
        public  DateTime Starttime { get; set; }
        public DateTime Endtime { get; set; }
        public int HttpStatus { get; set; }
        public  string ResponseText { get; set; }
        public int ErrorCode {get; set; }
    }

    
    public class recentResponseLog
    {
        public DateTime Starttime { get; set; }
        public string ResponseText { get; set; }
    }

    public class errorCodeLog
    {
        public int ErrorCodeCount { get; set; }
        public Int16 ErrorCode { get; set; }
        public string StartHour  { get; set; }
    }
}
