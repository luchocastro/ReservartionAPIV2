using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationAPI.Application.DTOs
{
    public class ReservationDTO
    {
        public string Id { get; set; }
        public string ClientName { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string Service { get; set; }
    }
}
