using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GestaoEscolar.Api.Models
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid Entity { get; set; }
    }
}