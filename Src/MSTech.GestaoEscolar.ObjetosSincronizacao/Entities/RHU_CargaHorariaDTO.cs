using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
	public class RHU_CargaHorariaDTO : RHU_CargaHoraria
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public new bool? IsNew { get { return null; } }
	}
}
