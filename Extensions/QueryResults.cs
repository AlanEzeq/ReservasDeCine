using System;

namespace ReservasDeCine.Extensions
{
    public class QueryResults
    {
        public Guid Value { get; set; } // change to int if Users.Id has integer value
        public string Text { get; set; }
    }
}
