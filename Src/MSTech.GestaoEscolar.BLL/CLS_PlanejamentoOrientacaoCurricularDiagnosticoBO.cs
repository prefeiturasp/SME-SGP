/*
	Classe gerada automaticamente pelo MSTech Code Creator
*/

namespace MSTech.GestaoEscolar.BLL
{
    using System.Linq;

    using MSTech.Business.Common;
	using MSTech.GestaoEscolar.Entities;
	using MSTech.GestaoEscolar.DAL;
    using MSTech.Data.Common;
    using MSTech.Validation.Exceptions;
    using System.Collections.Generic;
    using System.Data;
	
	/// <summary>
	/// Description: CLS_PlanejamentoOrientacaoCurricularDiagnostico Business Object. 
	/// </summary>
	public class CLS_PlanejamentoOrientacaoCurricularDiagnosticoBO : BusinessBase<CLS_PlanejamentoOrientacaoCurricularDiagnosticoDAO, CLS_PlanejamentoOrientacaoCurricularDiagnostico>
    {
        #region Métodos de consulta

        /// <summary>
        /// Busca o planejamento orientação curricular diagnostico atravéz da turma 
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static List<CLS_PlanejamentoOrientacaoCurricularDiagnostico> BuscaPlanejamentoOrientacaoCurricularDiagnostico
        (
           long tur_id
        )
        {
            return new CLS_PlanejamentoOrientacaoCurricularDiagnosticoDAO().BuscaPlanejamentoOrientacaoCurricularDiagnostico
                (
                    tur_id
                );
        }

        /// <summary>
        /// Busca o planejamento orientação curricular diagnostico atravéz da turma 
        /// </summary>
        /// <param name="tud_id"></param>
        /// <returns></returns>
        public static DataTable BuscaPlanejamentoOrientacaoCurricularDiagnosticoDT
        (
           int tud_id, int tdt_posicao
        )
        {
            return new CLS_PlanejamentoOrientacaoCurricularDiagnosticoDAO().BuscaPlanejamentoOrientacaoCurricularDiagnosticoDT
                (
                    tud_id, tdt_posicao
                );
        }

        #endregion

        #region Métodos para salvar / alterar

        /// <summary>
        /// Salva os dados do diagnóstico em lote.
        /// </summary>
        /// <param name="ltDiagnostico">Lista de dados do diagnóstico.</param>
        /// <param name="banco">Transação.</param>
        /// <returns>True em caso de sucesso.</returns>
        public static bool SalvarEmLote(List<CLS_PlanejamentoOrientacaoCurricularDiagnostico> ltDiagnostico, TalkDBTransaction banco = null)
        {
            DataTable dtPlanejamentoOrientacaoCurricularDiagnostico = CLS_PlanejamentoOrientacaoCurricularDiagnostico.TipoTabela_PlanejamentoOrientacaoCurricularDiagnostico();
            if (ltDiagnostico.Any())
            {
                List<DataRow> ltDrPlanejamentoOrientacaoCurricularDiagnostico = (from CLS_PlanejamentoOrientacaoCurricularDiagnostico planejamentoOrientacaoCurricularDiagnostico in ltDiagnostico select PlanejamentoOrientacaoCurricularToDataRow(planejamentoOrientacaoCurricularDiagnostico, dtPlanejamentoOrientacaoCurricularDiagnostico.NewRow())).ToList();

                dtPlanejamentoOrientacaoCurricularDiagnostico = ltDrPlanejamentoOrientacaoCurricularDiagnostico.CopyToDataTable();

                return banco == null ?
                       new CLS_PlanejamentoOrientacaoCurricularDiagnosticoDAO().SalvarEmLote(dtPlanejamentoOrientacaoCurricularDiagnostico) :
                       new CLS_PlanejamentoOrientacaoCurricularDiagnosticoDAO { _Banco = banco }.SalvarEmLote(dtPlanejamentoOrientacaoCurricularDiagnostico);
            }

            return true;
        }

        /// <summary>
        /// O método converte ua registro da CLS_PlanejamentoOrientacaoCurricularDiagnostico em um DataRow.
        /// </summary>
        /// <param name="planejamentoOrientacaoCurricularDiagnostico">Registro da CLS_PlanejamentoOrientacaoCurricularDiagnostico.</param>
        /// <param name="dr">Layout do DataTable.</param>
        /// <returns>DataRow.</returns>
        private static DataRow PlanejamentoOrientacaoCurricularToDataRow(CLS_PlanejamentoOrientacaoCurricularDiagnostico planejamentoOrientacaoCurricularDiagnostico, DataRow dr)
        {
            dr["tud_id"] = planejamentoOrientacaoCurricularDiagnostico.tud_id;
            dr["ocr_id"] = planejamentoOrientacaoCurricularDiagnostico.ocr_id;
            dr["pod_alcancado"] = planejamentoOrientacaoCurricularDiagnostico.pod_alcancado;
            dr["tdt_posicao"] = planejamentoOrientacaoCurricularDiagnostico.tdt_posicao;

            return dr;
        }

        #endregion
    }
}