using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
	public class RHU_CargoDTO : RHU_Cargo
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public new bool? IsNew { get { return null; } }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public List<RHU_CargaHorariaDTO> CargaHoraria { get; set; }
	}
}
