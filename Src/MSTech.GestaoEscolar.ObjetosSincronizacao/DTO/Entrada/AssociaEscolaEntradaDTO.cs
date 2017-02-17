using System;

namespace MSTech.GestaoEscolar.ObjetosSincronizacao.DTO.Entrada
{
    /// <summary>
    /// utilizado na associacao da escola ao tablet (ApiAssociaEscolaController) e 
    /// para atualizar os dados do tablet no SGP (ApiBuscaDataHoraController).
    /// quando for atualizar os dados do tablet, uad_codigo vai vir apenas com 1
    /// codigo de unidade administrativa... caso contrario pode vir com varios, separando por ; (ponto e virgula).
    /// </summary>
    public class AssociaEscolaEntradaDTO
    {
        public string K4 { get; set; }
        public string K1 { get; set; }
        public string Uad_codigo { get; set; }
        public string appVersion { get; set; }
        public string soVersion { get; set; }
        public int sisId { get; set; }
    }
}