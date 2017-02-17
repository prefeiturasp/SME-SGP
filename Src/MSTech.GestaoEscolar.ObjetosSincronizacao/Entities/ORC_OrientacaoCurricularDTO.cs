using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class ORC_OrientacaoCurricularDTO : ORC_OrientacaoCurricular
    {   
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public new bool? IsNew { get { return null; } }
    }
}
