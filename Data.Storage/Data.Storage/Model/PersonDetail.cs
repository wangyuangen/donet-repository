using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Storage.Model
{
    public class PersonDetail
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    } 
    public class LogModel
    {
        public int Id { get; set; }
        public DateTime LogTime { get; set; }
        public string Host { get; set; }
        public string Source { get; set; }
        public string Logger { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
