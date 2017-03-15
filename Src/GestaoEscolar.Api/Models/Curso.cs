using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GestaoEscolar.Api.Models
{
    public class Curso : jsonObject
    {
        public string curriculoId { get; set; }
    }
}