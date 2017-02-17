using System;
using System.Web.UI;

using MSTech.GestaoEscolar.BLL;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.Validation.Exceptions;
using System.Linq;
using System.Collections.Generic;
using MSTech.CoreSSO.BLL;
using System.Web.UI.WebControls;

namespace GestaoEscolar.WebControls.BoletimCompletoAluno
{
    public partial class UCBoletimCompletoAluno : MotherUserControl
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

        /// <summary>
        /// Lista de relação entre período e a matrícula do aluno.
        /// </summary>
        private List<sPeriodoMatricula> VS_ListaPeriodoMatricula
        {
            get
            {
                return (List<sPeriodoMatricula>)(ViewState["VS_ListaPeriodoMatricula"] ?? new List<sPeriodoMatricula>());
            }

            set
            {
                ViewState["VS_ListaPeriodoMatricula"] = value;
            }
        }

        /// <summary>
        /// Propriedade para indicar se o botão de imprimir deve aparecer ou não.
        /// </summary>
        public bool ImprimirVisible
        {
            get
            {
                if (ViewState["ImprimirVisible"] == null)
                    return false;

                return Convert.ToBoolean(ViewState["ImprimirVisible"]);
            }
            set
            {
                ViewState["ImprimirVisible"] = value;
            }
        }

        /// <summary>
        /// Propriedade para indicar se a Div de botões de boletins anteriores (1, 2, 3, 4) deve aparecer ou não.
        /// </summary>
        public bool DivBoletinsAnterioresVisible
        {
            set
            {
                DivBoletinsAnteriores.Visible = value;

            }
        }

        /// <summary>
        /// Propriedade para indicar se a Div de Proposta de estudo/compromisso do aluno deve aparecer ou não.
        /// </summary>
        public bool DivPropostaVisible
        {
            set
            {
                DivProposta.Visible = value;
            }
        }
        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {


            const string chaveRodapeBoletim = "MSG_RODAPEBOLETIMCOMPLETO";
            const string nomeReourceRodapeBoletim = "Mensagens";

            rodapeBoletim.Visible = !GetGlobalResourceObject(nomeReourceRodapeBoletim, chaveRodapeBoletim).ToString().Equals(chaveRodapeBoletim);

            lblBoletinsAnteriores.Text = GestaoEscolarUtilBO.nomePadraoPeriodo_Calendario(__SessionWEB.__UsuarioWEB.Usuario.ent_id);

                                //Atribui o caminho do logo do sistema atual, caso ele exista no Sistema Administrativo
            if (string.IsNullOrEmpty(__SessionWEB.UrlLogoGeral))
                logoInstituicao.Visible = false;
            else
            {
                //Carrega logo do sistema atual
                //Carrega logo geral do sistema
                logoInstituicao.ImageUrl = UtilBO.UrlImagemGestao(__SessionWEB.UrlCoreSSO, __SessionWEB.UrlLogoGeral);
                logoInstituicao.ToolTip = __SessionWEB.TituloGeral;
                logoInstituicao.AlternateText = __SessionWEB.TituloGeral;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carrega a parte de notas e foto do aluno no boletim.
        /// </summary>
        /// <param name="dadosBoletimAluno">Dados do boletim do aluno.</param>
        private void CarregarBoletim(ACA_AlunoBO.BoletimDadosAluno dadosBoletimAluno)
        {
            if (dadosBoletimAluno.arq_idFoto > 0)
            {
                imgFotoAluno.ImageUrl = string.Format("~/WebControls/BoletimCompletoAluno/Imagem.ashx?idfoto={0}", dadosBoletimAluno.arq_idFoto);
            }
            else
            {
                imgFotoAluno.ImageUrl = __SessionWEB._AreaAtual._DiretorioImagens + "imgsAreaAluno/fotoAluno.png";
            }

            if (!string.IsNullOrEmpty(dadosBoletimAluno.tci_layout))
            {
                divCiclo.Attributes["class"] = dadosBoletimAluno.tci_layout;
            }
            else
            {
                divCiclo.Attributes["class"] = "cicloLayoutPadrao";
            }
            
            UCDadosBoletim1.ExibeBoletim(dadosBoletimAluno);
        }

        /// <summary>
        /// Carrega todos os dados do boletim, com informações de recomendações, qualidades e notas.
        /// </summary>
        /// <param name="alu_id">Id do aluno.</param>
        /// <param name="periodoAvaliacao">Período da avaliação.</param>
        /// <param name="mtu_id">parâmetro opcional - deve conter o id da MatriculaTurma para buscar o boletim do aluno de acordo com o mtu_id.</param>
        /// <param name="lDadosBoletimAluno">parâmetro opcional - lista com os dados do boletim já carregada.</param>
        public void CarregarDadosBoletim(long alu_id, int periodoAvaliacao, int mtu_id = 0, List<ACA_AlunoBO.BoletimDadosAluno> lDadosBoletimAluno = null)
        {
            try
            {
                UCDadosBoletim1.VS_tpc_id = periodoAvaliacao;

                string mtuId = String.Empty;
                if (mtu_id > 0)
                {
                    mtuId = Convert.ToString(mtu_id);
                }

                List<ACA_AlunoBO.BoletimDadosAluno> lBoletimAluno;
                if (lDadosBoletimAluno == null || lDadosBoletimAluno.Count() == 0)
                {
                    lBoletimAluno = ACA_AlunoBO.BuscaBoletimAlunos(Convert.ToString(alu_id), mtuId, periodoAvaliacao,
                                                                   __SessionWEB.__UsuarioWEB.Usuario.ent_id);
                }
                else
                {
                    lBoletimAluno = lDadosBoletimAluno.FindAll(p => p.alu_id == alu_id);
                }
                CarregarDadosBoletim(lBoletimAluno);

                HabilitaBotoes(periodoAvaliacao);
            }
            catch (ValidationException ex)
            {
                lblMensagem.Text = UtilBO.GetErroMessage(ex.Message, UtilBO.TipoMensagem.Alerta);
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
                lblMensagem.Text = UtilBO.GetErroMessage("Erro ao exibir o boletim do aluno.", UtilBO.TipoMensagem.Erro);
            }
        }

        /// <summary>
        /// Carrega todos os dados do boletim, com informações de recomendações, qualidades e notas.
        /// </summary>
        /// <param name="lBoletimAluno">Lista com os dados do boletim.</param>
        public void CarregarDadosBoletim(List<ACA_AlunoBO.BoletimDadosAluno> lBoletimAluno)
        {
            bool permiteImportacao = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.PERMITIR_IMPORTACAO_DADOS_EFETIVACAO
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            ACA_AlunoBO.BoletimDadosAluno boletimAluno;
            if (lBoletimAluno != null && lBoletimAluno.Any())
            {
                boletimAluno = lBoletimAluno.First();
                VS_Alu_Id = boletimAluno.alu_id;
                VS_Mtu_Id = boletimAluno.mtu_id;

                VS_ListaPeriodoMatricula = boletimAluno.listaNotasEFaltas
                                                       .GroupBy(p => p.tpc_id)
                                                       .Select(p => new sPeriodoMatricula { tpc_id = p.Key, mtu_id = p.First().mtu_id })
                                                       .ToList();

                // Ocultar quando a escolas estiverem marcadas para importar dados da efetivação no período selecionado:
                divRecomendacoes.Visible = DivProposta.Visible = divPerfil.Visible = !permiteImportacao || (permiteImportacao && !boletimAluno.fechamentoPorImportacao);

                DivPropostaVisible = (!boletimAluno.fechamentoPorImportacao && boletimAluno.tci_exibirBoletim);

                lblLegend.Text = @"<h1>BOLETIM - " + boletimAluno.ava_nome + @" - " + boletimAluno.cal_ano + @"</h1>";
                lblLegend.Text += @"<h3> " + GetGlobalResourceObject("WebControls", "BoletimCompletoAluno.UCBoletimCompletoAluno.lblLegend.Titulo").ToString() + " <br/>" + boletimAluno.uad_nome + @"</h3>";
                lblLegend.Text += @"<h2>" + boletimAluno.esc_nome + @"</h2>";
                lblNomeAluno.Text = boletimAluno.pes_nome;
                lblCodigoEOL.Text = boletimAluno.pes_nome_abreviado;
                Label5.Text = boletimAluno.tur_codigo;
                lblCicloAluno.Text = boletimAluno.tci_nome;

                rptQualidades.DataSource = lBoletimAluno.Where(p => !string.IsNullOrEmpty(p.qualidade)).Select(p => new { p.alu_id, tqa_id = 0, p.qualidade, porDisciplina = 0 }).Distinct().ToList();
                rptDesempenho.DataSource = lBoletimAluno.Where(p => !string.IsNullOrEmpty(p.desempenho)).Select(p => new { p.alu_id, tda_id = 0, p.desempenho, porDisciplina = 0 }).Distinct().ToList();
                rptRecomendacoes.DataSource = lBoletimAluno.Where(p => !string.IsNullOrEmpty(p.recomendacaoAluno)).Select(p => new { p.alu_id, rar_id = 0, recomendacao = p.recomendacaoAluno, porDisciplina = 0 }).Distinct().ToList();
                rptRecomendacoesAosResp.DataSource = lBoletimAluno.Where(p => !string.IsNullOrEmpty(p.recomendacaoResponsavel)).Select(p => new { p.alu_id, rar_id = 0, recomendacao = p.recomendacaoResponsavel, porDisciplina = 0 }).Distinct().ToList();

                lblAtividadeFeita.Text = boletimAluno.cpe_atividadeFeita;
                lblAtividadePretendeFazer.Text = boletimAluno.cpe_atividadePretendeFazer;

                rptPeriodo.DataBind();
                if (rptPeriodo.Items.Count == 0)
                {
                    rptPeriodo.DataSource = ACA_AvaliacaoBO.GetSelectBy_FormatoAvaliacao(boletimAluno.fav_id);
                    rptPeriodo.DataBind();
                }               

                Fieldset1.Visible = true;
            }
            else
            {
                Fieldset1.Visible = ImprimirDiv.Visible = false;
                throw new ValidationException("O aluno não possui dados para o boletim.");
            }

            rptQualidades.DataBind();
            rptDesempenho.DataBind();
            rptRecomendacoes.DataBind();
            rptRecomendacoesAosResp.DataBind();

            CarregarBoletim(boletimAluno);
        }

        /// <summary>
        /// Habilita botões de avaliações de acordo com os parâmetros informados.
        /// </summary>
        /// <param name="periodo">Bimestre selecionado</param>
        protected void HabilitaBotoes(int periodo)
        {
            if (periodo > 0)
            {
                foreach (RepeaterItem item in rptPeriodo.Items)
                {
                    Button btnPeriodoRep = (Button)item.FindControl("btnPeriodo");

                    HiddenField hdnPeriodo = (HiddenField)item.FindControl("hdnPeriodo");
                    if (hdnPeriodo != null && !string.IsNullOrEmpty(hdnPeriodo.Value))
                    {
                        if (Convert.ToInt32(hdnPeriodo.Value) == periodo && btnPeriodoRep != null)
                        {
                            btnPeriodoRep.Enabled = false;
                        }
                        else if (btnPeriodoRep != null)
                        {
                            btnPeriodoRep.Enabled = true;
                        }
                    }
                }
            }
        }

        protected void btnPeriodo_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnPeriodo = (Button)sender;
                RepeaterItem itemPeriodo = (RepeaterItem)btnPeriodo.NamingContainer;
                Repeater rptPeriodo = (Repeater)itemPeriodo.NamingContainer;
                HiddenField hdnPeriodo = (HiddenField)itemPeriodo.FindControl("hdnPeriodo");

                if (hdnPeriodo != null && !string.IsNullOrEmpty(hdnPeriodo.Value))
                {
                    int tpc_id = Convert.ToInt32(hdnPeriodo.Value);
                    int mtu_id = VS_ListaPeriodoMatricula.Any(p => p.tpc_id == tpc_id) ?
                        VS_ListaPeriodoMatricula.Find(p => p.tpc_id == tpc_id).mtu_id : VS_Mtu_Id;

                    CarregarDadosBoletim(VS_Alu_Id, tpc_id, mtu_id);
                }
            }
            catch (Exception ex)
            {
                ApplicationWEB._GravaErro(ex);
            }
        }

        #endregion
    }
}