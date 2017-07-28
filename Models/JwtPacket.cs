using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspcorespa.Models
{
    public class JwtPacket
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public DateTime Expiration { get; set; }
    }
}
