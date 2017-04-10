using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.CoreSSO.BLL;
using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Entities;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.Entities;
using System.IO;
using System.Web;

namespace AreaAluno.Consulta.BoletimCompleto
{
    public partial class Busca : MotherPageLogado
    {
        #region Estrutura

        /// <summary>
        /// Estrtutura que indica a matrícula do aluno por período.
        /// </summary>
        [Serializable]
        public struct sPeriodoMatricula
        {
            public int tpc_id { get; set; }
            public int mtu_id { get; set; }
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// ID do Aluno
        /// </summary>
        public Int64 VS_Alu_Id
        {
            get
            {
                if (ViewState["VS_Alu_Id"] != null)
                    return (Int64)ViewState["VS_Alu_Id"];
                return -1;
            }
            set
            {
                ViewState["VS_Alu_Id"] = value;
            }
        }

        /// <summary>
        /// ID da MatriculaTurma
        /// </summary>
        public Int32 VS_Mtu_Id
        {
            get
            {
                if (ViewState["VS_Mtu_Id"] != null)
                    return (Int32)ViewState["VS_Mtu_Id"];
                return -1;
            }
            set
            {
                ViewState["VS_Mtu_Id"] = value;
            }
        }

        public string VS_nomeBoletim
        {
            get { return ViewState["VS_nomeBoletim"] as string ?? string.Empty; }
            set { ViewState["VS_nomeBoletim"] = value; }
        }
        #endregion Propriedades

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.CamposData));

                sm.Scripts.Add(new ScriptReference("~/includes/jquery.PrintBoletim.js"));
            }

            try
            {
                if (!IsPostBack)
                {
                    if ((__SessionWEB.__UsuarioWEB ?? new UsuarioWEB()).alu_id <= 0)
                    {
                        throw new ValidationException("Usuário não autorizado a exibir a Área do Aluno.");
                    }

                    IDictionary<string, ICFG_Configuracao> configuracao;
                    MSTech.GestaoEscolar.BLL.CFG_ConfiguracaoBO.Consultar(eConfig.Academico, out configuracao);
                    if (configuracao.ContainsKey("AppURLAreaAlunoInfantil") && configuracao["AppURLAreaAlunoInfantil"].cfg_valor != null)
                    {
                        string url = HttpContext.Current.Request.Url.AbsoluteUri;
                        string configInfantil = configuracao["AppURLAreaAlunoInfantil"].cfg_valor;

                        if (url.Contains(configInfantil))
                            VS_nomeBoletim = (string)GetGlobalResourceObject("AreaAluno.MasterPageAluno", "MenuBoletimInfantil");
                        else
                            VS_nomeBoletim = (string)GetGlobalResourceObject("AreaAluno.MasterPageAluno", "MenuBoletimOnline");
                    }

                    ACA_Aluno entityAluno = ACA_AlunoBO.GetEntity(new ACA_Aluno { alu_id = __SessionWEB.__UsuarioWEB.alu_id });
                    if (entityAluno.alu_possuiInformacaoSigilosa && entityAluno.alu_bloqueioBoletimOnline)
                    {
                        Response.Redirect("~/Index.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }

                    UCComboAnosLetivos.CarregarComboAnosLetivos(__SessionWEB.__UsuarioWEB.alu_id, 0);

                    // Selecionar o mtu_id que está na sessão do aluno.
                    if (UCComboAnosLetivos.Combo.Items.FindByValue(__SessionWEB.__UsuarioWEB.mtu_id.ToString()) != null)
                    {
                        UCComboAnosLetivos.Valor = __SessionWEB.__UsuarioWEB.mtu_id.ToString();
                    }

                    //int mtu_id = 0;
                    //int tpc_id = 0;

                    //if (mtu_id == 0)
                    //{
                    //    mtu_id = (string.IsNullOrEmpty(UCComboAnosLetivos.Valor)
                    //                   ? __SessionWEB.__UsuarioWEB.mtu_id
                    //                   : Convert.ToInt32(UCComboAnosLetivos.Valor));
                    //}

                    //if (tpc_id == 0)
                    //{
                    //    if ((UCComboAnosLetivos.Ano == DateTime.Now.Year) && (__SessionWEB.__UsuarioWEB.tpc_id > 0))
                    //    {
                    //        tpc_id = __SessionWEB.__UsuarioWEB.tpc_id;
                    //    }
                    //}

                    //Session["tpc_id"] = tpc_id;
                    //Session["alu_ids"] = __SessionWEB.__UsuarioWEB.alu_id;
                    //Session["mtu_ids"] = mtu_id;
                    //Session["mostrarPeriodos"] = true;

                    CarregarBoletimPorComboAnoLetivo(0, 0);
                }
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao exibir o " + VS_nomeBoletim + " do aluno.", UtilBO.TipoMensagem.Erro);
            }

            UCComboAnosLetivos.IndexChanged += AnosLetivos_SelectedIndexChanged;

        }

        protected void AnosLetivos_SelectedIndexChanged()
        {
            try
            {
                CarregarBoletimPorComboAnoLetivo(0, 0);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao exibir o " + VS_nomeBoletim + " do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        #endregion Eventos

        #region Métodos

        /// <summary>
        /// Carrega o boletim pelo valor selecionado no combo de ano letivo (mtu_id).
        /// </summary>
        private void CarregarBoletimPorComboAnoLetivo(int mtu_id, int tpc_id)
        {
            if (mtu_id == 0)
            {
                mtu_id = (string.IsNullOrEmpty(UCComboAnosLetivos.Valor)
                               ? __SessionWEB.__UsuarioWEB.mtu_id
                               : Convert.ToInt32(UCComboAnosLetivos.Valor));
            }

            if (tpc_id == 0)
            {
                if ((UCComboAnosLetivos.Ano == DateTime.Now.Year) && (__SessionWEB.__UsuarioWEB.tpc_id > 0))
                {
                    tpc_id = __SessionWEB.__UsuarioWEB.tpc_id;
                }
            }

            ucBoletim.CarregaBoletim(__SessionWEB.__UsuarioWEB.alu_id, mtu_id, tpc_id, true);

        }

        #endregion Métodos
    }
}