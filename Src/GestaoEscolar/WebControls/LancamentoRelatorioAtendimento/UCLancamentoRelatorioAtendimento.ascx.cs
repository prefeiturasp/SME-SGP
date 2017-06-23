namespace GestaoEscolar.WebControls.LancamentoRelatorioAtendimento
{
    using MSTech.CoreSSO.BLL;
    using MSTech.CoreSSO.Entities;
    using MSTech.GestaoEscolar.BLL;
    using MSTech.GestaoEscolar.CustomResourceProviders;
    using MSTech.GestaoEscolar.Entities;
    using MSTech.GestaoEscolar.Web.WebProject;
    using MSTech.Validation.Exceptions;
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

        private long VS_tur_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tur_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tur_id"] = value;
            }
        }

        private long VS_tud_id
        {
            get
            {
                return Convert.ToInt64(ViewState["VS_tud_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tud_id"] = value;
            }
        }

        private int VS_tpc_id
        {
            get
            {
                return Convert.ToInt32(ViewState["VS_tpc_id"] ?? -1);
            }

            set
            {
                ViewState["VS_tpc_id"] = value;
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

        private RelatorioPreenchimentoAluno VS_RelatorioPreenchimentoAluno
        {
            get
            {
                if (ViewState["VS_RelatorioPreenchimentoAluno"] == null)
                {
                    ViewState["VS_RelatorioPreenchimentoAluno"] = new RelatorioPreenchimentoAluno();
                }

                return (RelatorioPreenchimentoAluno)ViewState["VS_RelatorioPreenchimentoAluno"];
            }

            set
            {
                ViewState["VS_RelatorioPreenchimentoAluno"] = value;
            }
        }

        public bool PermiteAprovar
        {
            get
            {
                return VS_RelatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoAprovacao) ||
                       VS_RelatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoAprovacao);
            }
        }

        public bool PermiteEditar
        {
            get
            {
                return VS_RelatorioAtendimento.lstCargoPermissao.Any(p => p.rac_permissaoEdicao) ||
                       VS_RelatorioAtendimento.lstGrupoPermissao.Any(p => p.rag_permissaoEdicao);
            }
        }

        public bool PermiteConsultar
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

        public void Carregar(long alu_id, long tur_id, long tud_id, int tpc_id, int rea_id, bool documentoOficial = false)
        {
            VS_alu_id = alu_id;
            VS_tur_id = tur_id;
            VS_tud_id = tud_id;
            VS_tpc_id = tpc_id;
            VS_rea_id = rea_id;
            ACA_Aluno entityAluno = new ACA_Aluno { alu_id = VS_alu_id };
            ACA_AlunoBO.GetEntity(entityAluno);

            PES_Pessoa entityPessoa = new PES_Pessoa { pes_id = entityAluno.pes_id };
            PES_PessoaBO.GetEntity(entityPessoa);

            VS_RelatorioAtendimento = CLS_RelatorioAtendimentoBO.SelecionaRelatorio(rea_id, __SessionWEB.__UsuarioWEB.Usuario.usu_id, ApplicationWEB.AppMinutosCacheLongo);

            VS_RelatorioPreenchimentoAluno = CLS_RelatorioPreenchimentoBO.SelecionaPorRelatorioAlunoTurmaDisciplina(VS_rea_id, VS_alu_id, VS_tur_id, VS_tud_id, VS_tpc_id);

            eExibicaoNomePessoa exibicaoNome = documentoOficial ? eExibicaoNomePessoa.NomeSocial | eExibicaoNomePessoa.NomeRegistro : eExibicaoNomePessoa.NomeSocial;

            string nomeAluno = entityPessoa.NomeFormatado(exibicaoNome);

            //Nome
            lblInformacaoAluno.Text += "<b>Nome do aluno: </b>" + nomeAluno;

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
            
            CarregarHipoteseDiagnostica();
            CarregarQuestionarios();

            updLancamentoRelatorio.Update();
        }

        private void CarregarHipoteseDiagnostica()
        {
            liHipoteseDiagnostica.Visible = fdsHipoteseDiagnostica.Visible = false;
            if (VS_RelatorioAtendimento.rea_tipo == (byte)CLS_RelatorioAtendimentoTipo.AEE)
            {
                rptTipoDeficiencia.DataSource = CLS_AlunoDeficienciaDetalheBO.SelecionaPorAluno(VS_alu_id);
                rptTipoDeficiencia.DataBind();

                liHipoteseDiagnostica.Visible = fdsHipoteseDiagnostica.Visible = rptTipoDeficiencia.Items.Count > 0;

                HabilitaControles(fdsHipoteseDiagnostica.Controls, VS_RelatorioAtendimento.rea_permiteEditarHipoteseDiagnostica && PermiteEditar);
            }
        }

        private void CarregarQuestionarios()
        {
            rptAbaQuestionarios.DataSource = VS_RelatorioAtendimento.lstQuestionario;
            rptAbaQuestionarios.DataBind();

            rptQuestionario.DataSource = VS_RelatorioAtendimento.lstQuestionario;
            rptQuestionario.DataBind();

            HabilitaControles(rptQuestionario.Controls, PermiteEditar);
        }

        private bool RelatorioConcluido()
        {
            var conteudoVazio = (from RepeaterItem itemQuestionario in rptQuestionario.Items
                                 let raq_id = itemQuestionario.FindControl<HiddenField>("hdnRaqId").GetValue().ToInt32()
                                 let rptConteudo = itemQuestionario.FindControl("rptConteudo") as Repeater
                                 where rptConteudo != null && raq_id > 0
                                 from RepeaterItem itemConteudo in rptConteudo.Items
                                 let tipoResposta = itemConteudo.FindControl<HiddenField>("hdnTipoResposta").GetValue().ToByte()
                                 where tipoResposta == (byte)QuestionarioTipoResposta.TextoAberto
                                 let qtc_id = itemConteudo.FindControl<HiddenField>("hdnQtcId").GetValue().ToInt32()
                                 let qcp_textoResposta = itemConteudo.FindControl<TextBox>("txtResposta").GetText()
                                 where qtc_id > 0 && string.IsNullOrEmpty(qcp_textoResposta)
                                 select qtc_id).Union
                                (from RepeaterItem itemQuestionario in rptQuestionario.Items
                                 let raq_id = itemQuestionario.FindControl<HiddenField>("hdnRaqId").GetValue().ToInt32()
                                 let rptConteudo = itemQuestionario.FindControl("rptConteudo") as Repeater
                                 where rptConteudo != null && raq_id > 0
                                 from RepeaterItem itemConteudo in rptConteudo.Items
                                 let tipoResposta = itemConteudo.FindControl<HiddenField>("hdnTipoResposta").GetValue().ToByte()
                                 let qtc_id = itemConteudo.FindControl<HiddenField>("hdnQtcId").GetValue().ToInt32()
                                 let rptResposta = itemConteudo.FindControl("rptResposta") as Repeater
                                 where rptResposta != null && tipoResposta != (byte)QuestionarioTipoResposta.TextoAberto
                                 let qtdeRespondido = (from RepeaterItem itemResposta in rptResposta.Items
                                                       let qtr_id = itemResposta.FindControl<HiddenField>("hdnQtrId").GetValue().ToInt32()
                                                       let txtRespostaTextoAdicional = itemResposta.FindControl<TextBox>("txtRespostaTextoAdicional")
                                                       where (tipoResposta == (byte)QuestionarioTipoResposta.SelecaoUnica ?
                                                                itemResposta.FindControl<RadioButton>("rdbResposta").IsChecked() :
                                                                itemResposta.FindControl<CheckBox>("chkResposta").IsChecked()) &&
                                                              (txtRespostaTextoAdicional != null ?
                                                                (txtRespostaTextoAdicional.Visible && !string.IsNullOrEmpty(txtRespostaTextoAdicional.Text)) || !txtRespostaTextoAdicional.Visible : true)
                                                       select qtr_id).Count()
                                 where qtdeRespondido == 0
                                 select qtc_id);

            return conteudoVazio.Count() == 0;
        }

        public List<CLS_AlunoDeficienciaDetalhe> RetornaListaDeficienciaDetalhe()
        {
            return (from RepeaterItem itemTipoDeficiencia in rptTipoDeficiencia.Items
                    let tde_id = new Guid(itemTipoDeficiencia.FindControl<HiddenField>("hdnTdeId").GetValue())
                    let rptHipoteseDiagnostica = itemTipoDeficiencia.FindControl<Repeater>("rptHipoteseDiagnostica")
                    where rptHipoteseDiagnostica != null
                    from RepeaterItem itemDeficienciaDetalhe in rptHipoteseDiagnostica.Items
                    let dfd_id = itemDeficienciaDetalhe.FindControl<HiddenField>("hdnDfdId").GetValue().ToInt32()
                    let chkDeficienciaDetalhe = itemDeficienciaDetalhe.FindControl<CheckBox>("chkDeficienciaDetalhe")
                    where chkDeficienciaDetalhe.IsChecked()
                    select new CLS_AlunoDeficienciaDetalhe
                    {
                        tde_id = tde_id
                        ,
                        dfd_id = dfd_id
                        ,
                        alu_id = VS_alu_id
                    }).ToList();
        }

        public RelatorioPreenchimentoAluno RetornaQuestionarioPreenchimento(bool aprovar)
        {
            RelatorioPreenchimentoAluno rel = new RelatorioPreenchimentoAluno();
            long reap_id = VS_RelatorioPreenchimentoAluno.entityRelatorioPreenchimento.reap_id;
            bool relatorioConcluido = RelatorioConcluido();

            if (!relatorioConcluido && aprovar)
            {
                throw new ValidationException("Preenchimento do relatório não está finalizado. Verifique se todas as perguntas foram respondidas antes de aprovar o relatório.");
            }

            rel.entityPreenchimentoAlunoTurmaDisciplina = new CLS_RelatorioPreenchimentoAlunoTurmaDisciplina
            {
                alu_id = VS_alu_id
                ,
                tur_id = VS_tur_id
                ,
                tud_id = VS_tud_id
                ,
                tpc_id = VS_tpc_id
                ,
                reap_id = reap_id > 0 ? reap_id : -1
                ,
                ptd_situacao = aprovar ? (byte)RelatorioPreenchimentoAlunoSituacao.Aprovado : 
                                    (relatorioConcluido ? (byte)RelatorioPreenchimentoAlunoSituacao.Finalizado : (byte)RelatorioPreenchimentoAlunoSituacao.Rascunho)
                ,
                IsNew = reap_id <= 0
            };

            rel.entityRelatorioPreenchimento = new CLS_RelatorioPreenchimento
            {
                rea_id = VS_rea_id
                ,
                reap_id = reap_id > 0 ? reap_id : -1
                ,
                IsNew = reap_id <= 0
            };

            rel.lstQuestionarioConteudoPreenchimento = (from RepeaterItem itemQuestionario in rptQuestionario.Items
                                                        let raq_id = itemQuestionario.FindControl<HiddenField>("hdnRaqId").GetValue().ToInt32()
                                                        let rptConteudo = itemQuestionario.FindControl("rptConteudo") as Repeater
                                                        where rptConteudo != null && raq_id > 0
                                                        from RepeaterItem itemConteudo in rptConteudo.Items
                                                        let tipoResposta = itemConteudo.FindControl<HiddenField>("hdnTipoResposta").GetValue().ToByte()
                                                        where tipoResposta == (byte)QuestionarioTipoResposta.TextoAberto
                                                        let qtc_id = itemConteudo.FindControl<HiddenField>("hdnQtcId").GetValue().ToInt32()
                                                        let qcp_textoResposta = itemConteudo.FindControl<TextBox>("txtResposta").GetText()
                                                        where qtc_id > 0 && !string.IsNullOrEmpty(qcp_textoResposta)
                                                        select new CLS_QuestionarioConteudoPreenchimento
                                                        {
                                                            raq_id = raq_id
                                                            ,
                                                            qtc_id = qtc_id
                                                            ,
                                                            qcp_textoResposta = qcp_textoResposta

                                                        }).ToList();

            rel.lstQuestionarioRespostaPreenchimento = (from RepeaterItem itemQuestionario in rptQuestionario.Items
                                                        let raq_id = itemQuestionario.FindControl<HiddenField>("hdnRaqId").GetValue().ToInt32()
                                                        let rptConteudo = itemQuestionario.FindControl("rptConteudo") as Repeater
                                                        where rptConteudo != null && raq_id > 0
                                                        from RepeaterItem itemConteudo in rptConteudo.Items
                                                        let tipoResposta = itemConteudo.FindControl<HiddenField>("hdnTipoResposta").GetValue().ToByte()
                                                        let rptResposta = itemConteudo.FindControl("rptResposta") as Repeater
                                                        where rptResposta != null && tipoResposta != (byte)QuestionarioTipoResposta.TextoAberto
                                                        from RepeaterItem itemResposta in rptResposta.Items
                                                        let qtr_id = itemResposta.FindControl<HiddenField>("hdnQtrId").GetValue().ToInt32()
                                                        let txtRespostaTextoAdicional = itemResposta.FindControl<TextBox>("txtRespostaTextoAdicional")
                                                        where tipoResposta == (byte)QuestionarioTipoResposta.SelecaoUnica ?
                                                                itemResposta.FindControl<RadioButton>("rdbResposta").IsChecked() :
                                                                itemResposta.FindControl<CheckBox>("chkResposta").IsChecked()
                                                        select new CLS_QuestionarioRespostaPreenchimento
                                                        {
                                                            raq_id = raq_id
                                                            ,
                                                            qtr_id = qtr_id
                                                            ,
                                                            qrp_textoAdicional = txtRespostaTextoAdicional != null && txtRespostaTextoAdicional.Visible ? 
                                                                                    txtRespostaTextoAdicional.Text : string.Empty
                                                        }).ToList();
            return rel;
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
                int raq_id = -1;
                Repeater rptConteudo = sender as Repeater;
                if (rptConteudo != null)
                {
                    RepeaterItem itemQuestionario = rptConteudo.NamingContainer as RepeaterItem;
                    if (itemQuestionario != null)
                    {
                        raq_id = itemQuestionario.FindControl<HiddenField>("hdnRaqId").GetValue().ToInt32();
                    }
                }

                TextBox txtResposta = e.Item.FindControl("txtResposta") as TextBox;
                byte qtc_tipoResposta = Convert.ToByte(DataBinder.Eval(e.Item.DataItem, "qtc_tipoResposta"));
                if (txtResposta != null && qtc_tipoResposta == (byte)QuestionarioTipoResposta.TextoAberto)
                {
                    int qtc_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "qtc_id"));
                    txtResposta.Visible = true;

                    if (VS_RelatorioPreenchimentoAluno.lstQuestionarioConteudoPreenchimento.Any(p => p.raq_id == raq_id && p.qtc_id == qtc_id && !string.IsNullOrEmpty(p.qcp_textoResposta)))
                    {
                        txtResposta.Text = VS_RelatorioPreenchimentoAluno.lstQuestionarioConteudoPreenchimento.Find(p => p.raq_id == raq_id && p.qtc_id == qtc_id).qcp_textoResposta;
                    }
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
                int raq_id = -1;
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

                        Repeater rptConteudo = itemConteudo.NamingContainer as Repeater;

                        if (rptConteudo != null)
                        {
                            RepeaterItem itemQuestionario = rptConteudo.NamingContainer as RepeaterItem;

                            if (itemQuestionario != null)
                            {
                                raq_id = itemQuestionario.FindControl<HiddenField>("hdnRaqId").GetValue().ToInt32();
                            }
                        }
                    }
                }

                bool qtr_permiteAdicionarTexto = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "qtr_permiteAdicionarTexto"));
                int qtr_id = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "qtr_id"));

                bool respostaMarcada = VS_RelatorioPreenchimentoAluno.lstQuestionarioRespostaPreenchimento.Any(p => p.raq_id == raq_id && p.qtr_id == qtr_id);

                CheckBox chkResposta = e.Item.FindControl("chkResposta") as CheckBox;
                RadioButton rdbResposta = e.Item.FindControl("rdbResposta") as RadioButton;
                TextBox txtRespostaTextoAdicional = e.Item.FindControl("txtRespostaTextoAdicional") as TextBox;

                if (chkResposta != null && qtc_tipoResposta == (byte)QuestionarioTipoResposta.MultiplaSelecao)
                {
                    chkResposta.Visible = true;
                    chkResposta.Checked = respostaMarcada;
                }
                
                if (rdbResposta != null && qtc_tipoResposta == (byte)QuestionarioTipoResposta.SelecaoUnica)
                {
                    rdbResposta.Visible = true;
                    rdbResposta.GroupName = qtc_id;
                    rdbResposta.Checked = respostaMarcada;
                }

                if (txtRespostaTextoAdicional != null && qtr_permiteAdicionarTexto)
                {
                    txtRespostaTextoAdicional.Visible = true;

                    if (VS_RelatorioPreenchimentoAluno.lstQuestionarioRespostaPreenchimento.Any(p => p.raq_id == raq_id && p.qtr_id == qtr_id && !string.IsNullOrEmpty(p.qrp_textoAdicional)))
                    {
                        txtRespostaTextoAdicional.Text = VS_RelatorioPreenchimentoAluno.lstQuestionarioRespostaPreenchimento.Find(p => p.raq_id == raq_id && p.qtr_id == qtr_id).qrp_textoAdicional;
                    }
                }
            }
        }
    }
}