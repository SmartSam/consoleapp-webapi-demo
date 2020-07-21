using System;

namespace CodeChallenge.API.Models
{
    
    public class server_reponse_log
    {
        public  DateTime Starttime { get; set; }
        public DateTime Endtime { get; set; }
        public int HttpStatus { get; set; }
        public  string ResponseText { get; set; }
        public int ErrorCode {get; set; }
    }
}
