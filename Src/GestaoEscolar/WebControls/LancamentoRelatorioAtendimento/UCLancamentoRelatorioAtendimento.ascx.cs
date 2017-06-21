namespace GestaoEscolar.WebControls.LancamentoRelatorioAtendimento
{
    using MSTech.CoreSSO.BLL;
    using MSTech.CoreSSO.Entities;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.CustomResourceProviders;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class UCLancamentoRelatorioAtendimento : MotherUserControl
    {

        private long VS_alu_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_alu_id"] ?? -1);
            }

            set
            {
                ViewState["VS_alu_id"] = value;
            }
        }

        private int VS_rea_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_rea_id"] ?? -1);
            }

            set
            {
                ViewState["VS_rea_id"] = value;
            }
        }

        private RelatorioAtendimento VS_RelatorioAtendimento
        {
            get
            {
                if (ViewState["VS_RelatorioAtendimento"] == null)
                {
                    ViewState["VS_RelatorioAtendimento"] = new RelatorioAtendimento();
                }

                return (RelatorioAtendimento)ViewState["VS_RelatorioAtendimento"];
            }

            set
            {
                ViewState["VS_RelatorioAtendimento"] = value;
            }
        }

        private bool PermiteAprovar
        {
            get
            {
                return VS_RelatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoAprovacao) ||
                       VS_RelatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoAprovacao);
            }
        }

        private bool PermiteEditar
        {
            get
            {
                return VS_RelatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoEdicao) ||
                       VS_RelatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoEdicao);
            }
        }

        private bool PermiteConsultar
        {
            get
            {
                return VS_RelatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoConsulta) ||
                       VS_RelatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoConsulta) || PermiteAprovar || PermiteEditar;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference("~/Includes/jsUCLancamentoRelatorioAtendimento.js"));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.UiAriaTabs));
                sm.Scripts.Add(new ScriptReference("~/Includes/jsTabs.js"));
            }
        }

        public string RetornaClasseQuestionarioConteudo(byte qtc_tipo)
        {
            switch (qtc_tipo)
            {
                case (byte)QuestionarioTipoConteudo.Texto:
                    return "questionario-conteudo-texto";
                case (byte)QuestionarioTipoConteudo.Titulo1:
                    return "questionario-conteudo-titulo1";
                case (byte)QuestionarioTipoConteudo.Titulo2:
                    return "questionario-conteudo-titulo2";
                case (byte)QuestionarioTipoConteudo.Pergunta:
                    return "questionario-conteudo-pergunta";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Retorna o id da div que possui a aba.
        /// </summary>
        /// <param name="tpc_id"></param>
        /// <returns></returns>
        public string RetornaTabID(int qst_id)
        {
            return "divTabs-" + qst_id.ToString();
        }

        public void Carregar(long alu_id, int rea_id, bool documentoOficial = false)
        {
            VS_alu_id = alu_id;
            VS_rea_id = rea_id;
            ACA_Aluno entityAluno = new ACA_Aluno { alu_id = VS_alu_id };
            ACA_AlunoBO.GetEntity(entityAluno);

            PES_Pessoa entityPessoa = new PES_Pessoa { pes_id = entityAluno.pes_id };
            PES_PessoaBO.GetEntity(entityPessoa);

            VS_RelatorioAtendimento = CLS_RelatorioAtendimentoBO.SelecionaRelatorio(rea_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, ApplicationWEB.AppMinutosCacheLongo);

            eExibicaoNomePessoa exibicaoNome = documentoOficial ? eExibicaoNomePessoa.NomeSocial | eExibicaoNomePessoa.NomeRegistro : eExibicaoNomePessoa.NomeSocial;

            string nomeAluno = entityPessoa.NomeFormatado(exibicaoNome);

            //Nome
            lblInformacaoAluno.Text += "<b>Nome do aluno: </b>" + nomeAluno + "<br/>";

            //Idade
            if (entityPessoa.pes_dataNascimento != new DateTime() && entityPessoa.pes_dataNascimento < DateTime.Today)
            {
                string dataExtenso = GestaoEscolarUtilBO.DiferencaDataExtenso(entityPessoa.pes_dataNascimento, DateTime.Today);
                if (!string.IsNullOrEmpty(dataExtenso))
                    lblInformacaoAluno.Text += "<b>Idade: </b>" + dataExtenso;
            }

            string sexo = entityPessoa.SexoFormatado();

            if (!string.IsNullOrEmpty(sexo))
            {
                lblInformacaoAluno.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Sexo: </b>" + sexo;
            }

            if (!VS_RelatorioAtendimento.rea_permiteEditarRecaCor)
            {
                UCCRacaCor.Visible = false;

                string racaCor = entityPessoa.RacaCorFormatado();

                if (!string.IsNullOrEmpty(racaCor))
                {
                    lblInformacaoAluno.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Raça/cor: </b>" + racaCor;
                }
            }
            else
            {
                UCCRacaCor.Visible = true;
                if (UCCRacaCor._Combo.Items.FindByValue(entityPessoa.pes_racaCor.ToString()) != null)
                {
                    UCCRacaCor._Combo.SelectedValue = entityPessoa.pes_racaCor.ToString();
                }
            }

            if (VS_RelatorioAtendimento.arq_idAnexo > 0 && PermiteEditar)
            {
                divDownloadAnexo.Visible = true;
                hplDownloadAnexo.Text = VS_RelatorioAtendimento.rea_tituloAnexo;
                hplDownloadAnexo.NavigateUrl = String.Format("~/FileHandler.ashx?file={0}", VS_RelatorioAtendimento.arq_idAnexo);
            }

            CarregarSituacao();
            CarregarHipoteseDiagnostica(VS_RelatorioAtendimento.rea_permiteEditarHipoteseDiagnostica);
            CarregarQuestionarios();

            updLancamentoRelatorio.Update();
        }

        private void CarregarHipoteseDiagnostica(bool permiteEditar)
        {
            rptTipoDeficiencia.DataSource = CLS_AlunoDeficienciaDetalheBO.SelecionaPorAluno(VS_alu_id);
            rptTipoDeficiencia.DataBind();

            liHipoteseDiagnostica.Visible = fdsHipoteseDiagnostica.Visible = rptTipoDeficiencia.Items.Count > 0;

            HabilitaControles(fdsHipoteseDiagnostica.Controls, permiteEditar);
        }

        private void CarregarQuestionarios()
        {
            rptAbaQuestionarios.DataSource = VS_RelatorioAtendimento.lstQuestionario;
            rptAbaQuestionarios.DataBind();

            rptQuestionario.DataSource = VS_RelatorioAtendimento.lstQuestionario;
            rptQuestionario.DataBind();
        }

        private void CarregarSituacao()
        {
            ddlSituacao.Items.Clear();

            Type objType = typeof(RelatorioPreenchimentoAlunoSituacao);
            FieldInfo[] propriedades = objType.GetFields();
            foreach (FieldInfo objField in propriedades)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])objField.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    RelatorioPreenchimentoAlunoSituacao situacao = (RelatorioPreenchimentoAlunoSituacao)Enum.Parse(typeof(RelatorioPreenchimentoAlunoSituacao), objField.GetRawConstantValue().ToString());
                    if (situacao.In(RelatorioPreenchimentoAlunoSituacao.Rascunho, RelatorioPreenchimentoAlunoSituacao.Finalizado) ||
                        (situacao == RelatorioPreenchimentoAlunoSituacao.Aprovado && PermiteAprovar))
                    {
                        ddlSituacao.Items.Add(new ListItem(CustomResource.GetGlobalResourceObject("Enumerador", attributes[0].Description), ((byte)situacao).ToString()));
                    }
                }
            }
        }

        public void RetornaLancamentoRelatorio()
        {

        }

        protected void rptTipoDeficiencia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType.In(ListItemType.Item, ListItemType.AlternatingItem))
            {
                Repeater rptHipoteseDiagnostica = e.Item.FindControl("rptHipoteseDiagnostica") as Repeater;
                if (rptHipoteseDiagnostica != null)
                {
                    List<sAlunoDeficienciaDetalhe> lstDeficienciaDetalhe = DataBinder.Eval(e.Item.DataItem, "lstDeficienciaDetalhe") as List<sAlunoDeficienciaDetalhe>;
                    if (lstDeficienciaDetalhe != null)
                    {
                        rptHipoteseDiagnostica.DataSource = lstDeficienciaDetalhe;
                        rptHipoteseDiagnostica.DataBind();
                    }
                }
            }
        }

        protected void rptQuestionario_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType.In(ListItemType.Item, ListItemType.AlternatingItem))
            {
                Repeater rptConteudo = e.Item.FindControl("rptConteudo") as Repeater;
                if (rptConteudo != null)
                {
                    List<QuestionarioConteudo> lstConteudo = DataBinder.Eval(e.Item.DataItem, "lstConteudo") as List<QuestionarioConteudo>;
                    if (lstConteudo != null)
                    {
                        rptConteudo.DataSource = lstConteudo;
                        rptConteudo.DataBind();

                        rptConteudo.Visible = lstConteudo.Any();
                    }
                }
            }
        }

        protected void rptConteudo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType.In(ListItemType.Item, ListItemType.AlternatingItem))
            {
                TextBox txtResposta = e.Item.FindControl("txtResposta") as TextBox;
                byte qtc_tipoResposta = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "qtc_tipoResposta"));
                if (txtResposta != null && qtc_tipoResposta == (byte)QuestionarioTipoResposta.TextoAberto)
                {
                    txtResposta.Visible = true;
                }
                else
                {
                    Repeater rptResposta = e.Item.FindControl("rptResposta") as Repeater;
                    if (rptResposta != null)
                    {
                        List<CLS_QuestionarioResposta> lstRepostas = DataBinder.Eval(e.Item.DataItem, "lstRepostas") as List<CLS_QuestionarioResposta>;
                        if (lstRepostas != null)
                        {
                            rptResposta.DataSource = lstRepostas;
                            rptResposta.DataBind();

                            rptResposta.Visible = lstRepostas.Any();
                        }
                    }
                }
            }
        }

        protected void rptResposta_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType.In(ListItemType.Item, ListItemType.AlternatingItem))
            {
                byte qtc_tipoResposta = 0;
                string qtc_id = string.Empty;
                Repeater rptResposta = sender as Repeater;
                if (rptResposta != null)
                {
                    RepeaterItem itemConteudo = rptResposta.NamingContainer as RepeaterItem;
                    if (itemConteudo != null)
                    {
                        HiddenField hdnTipoResposta = itemConteudo.FindControl("hdnTipoResposta") as HiddenField;
                        if (hdnTipoResposta != null)
                        {
                            byte.TryParse(hdnTipoResposta.Value, out qtc_tipoResposta);
                        }

                        HiddenField hdnQtcId = itemConteudo.FindControl("hdnQtcId") as HiddenField;
                        if (hdnQtcId != null)
                        {
                            qtc_id = hdnQtcId.Value;
                        }
                    }
                }

                bool qtr_permiteAdicionarTexto = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "qtr_permiteAdicionarTexto"));

                CheckBox chkResposta = e.Item.FindControl("chkResposta") as CheckBox;
                RadioButton rdbResposta = e.Item.FindControl("rdbResposta") as RadioButton;
                TextBox txtRespostaTextoAdicional = e.Item.FindControl("txtRespostaTextoAdicional") as TextBox;

                if (chkResposta != null && qtc_tipoResposta == (byte)QuestionarioTipoResposta.MultiplaSelecao)
                {
                    chkResposta.Visible = true;
                }
                
                if (rdbResposta != null && qtc_tipoResposta == (byte)QuestionarioTipoResposta.SelecaoUnica)
                {
                    rdbResposta.Visible = true;
                    rdbResposta.GroupName = qtc_id;
                }

                if (txtRespostaTextoAdicional != null && qtr_permiteAdicionarTexto)
                {
                    txtRespostaTextoAdicional.Visible = true;
                }
            }
        }
    }
}