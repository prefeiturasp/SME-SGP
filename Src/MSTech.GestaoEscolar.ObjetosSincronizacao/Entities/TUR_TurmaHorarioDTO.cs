using MSTech.GestaoEscolar.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Entities
{
    public class TUR_TurmaHorarioDTO : TUR_TurmaHorario
    {
        public bool? IsNew { get { return null; } }

        public TUR_TurmaHorarioDTO()
        {
        }

        public delegate TResult FuncTurmaHorario<T, TResult>(T arg);
    }
}

