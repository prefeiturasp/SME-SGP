using System;
using System.Data;
using MSTech.GestaoEscolar.ObjetosSincronizacao.Entities;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.Util
{
    public class Util
    {
        public static readonly string mascaraData = "dd/MM/yyyy HH:mm:ss";

        public static Protocolo rowToProtocolo(DataRow row, Protocolo protocolo)
        {
            protocolo.Pro_protocolo = (Int64) row["pro_protocolo"];
            protocolo.Pro_status = Convert.ToInt16(row["pro_status"]);
            protocolo.Pro_statusObservacao = row["pro_statusObservacao"].ToString();

            return protocolo;
        }
    }
}