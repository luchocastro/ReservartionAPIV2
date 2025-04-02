using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationAPI.Infrastructure.Context.Model
{
    public class ReadReservation
    {
        public string Id { get; set; }
        public string ClientName { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string Service { get; set; }
    }
}
