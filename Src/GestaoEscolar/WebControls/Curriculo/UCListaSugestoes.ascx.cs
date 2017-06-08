using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.Curriculo
{
    public partial class UCListaSugestoes : MotherUserControl
    {
        #region Propriedades

        public int QuantidadeSugestoes
        {
            get
            {
                return rptSugestao.Items.Count;
            }
        }

        #endregion Propriedades

        #region Eventos

        protected void rptSugestao_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblDetalhes = (Label)e.Item.FindControl("lblDetalhes");
                if (lblDetalhes != null)
                {
                    string tipo;
                    switch (Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "crs_tipo")))
                    {
                        case 1:
                            tipo = GetGlobalResourceObject("Academico", "Curriculo.Cadastro.ddlTipoSugestao.Sugestao").ToString();
                            break;
                        case 2:
                            tipo = GetGlobalResourceObject("Academico", "Curriculo.Cadastro.ddlTipoSugestao.Exclusao").ToString();
                            break;
                        case 3:
                            tipo = GetGlobalResourceObject("Academico", "Curriculo.Cadastro.ddlTipoSugestao.Inclusao").ToString();
                            break;
                        default:
                            tipo = string.Empty;
                            break;
                    }

                    lblDetalhes.Text = string.Format("{0}, {1} - {2}"
                                                        , DataBinder.Eval(e.Item.DataItem, "pes_nome").ToString()
                                                        , Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "data")).ToString("dd/MM/yyyy")
                                                        , tipo);
                }
            }
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Carrega o repeater com as sugestões.
        /// </summary>
        /// <param name="lista"></param>
        public void Carregar(List<sCurriculoSugestaoCapitulo> lista)
        {
            rptSugestao.DataSource = lista;
            rptSugestao.DataBind();
        }

        /// <summary>
        /// Carrega o repeater com as sugestões.
        /// </summary>
        /// <param name="lista"></param>
        public void Carregar(List<sCurriculoSugestaoObjetivo> lista)
        {
            rptSugestao.DataSource = lista;
            rptSugestao.DataBind();
        }

        #endregion Métodos
    }
}