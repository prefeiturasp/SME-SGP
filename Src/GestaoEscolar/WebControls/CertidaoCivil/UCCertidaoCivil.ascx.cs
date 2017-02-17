using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSTech.GestaoEscolar.Web.WebProject;
using MSTech.CoreSSO.BLL;
using MSTech.CoreSSO.Entities;
using MSTech.GestaoEscolar.BLL;

namespace GestaoEscolar.WebControls.CertidaoCivil
{
    public partial class UCCertidaoCivil : MotherUserControl
    {
        #region Delegates

        public delegate void AbreJanelaCadastroCidade();
        public event AbreJanelaCadastroCidade _AbreJanelaCadastroCidade;

        #endregion

        #region Propriedades

        /// <summary>
        /// ViewState com ctc_id
        /// </summary>
        public Guid _VS_ctc_id
        {
            get
            {
                if (ViewState["_VS_ctc_id"] != null)
                    return (Guid)ViewState["_VS_ctc_id"];
                return Guid.Empty;
            }
            set
            {
                ViewState["_VS_ctc_id"] = value;
            }
        }

        /// <summary>
        /// ViewState com pes_id
        /// </summary>
        public Guid _VS_pes_id
        {
            get
            {
                if (ViewState["_VS_pes_id"] != null)
                    return (Guid)ViewState["_VS_pes_id"];
                return Guid.Empty;
            }
            set
            {
                ViewState["_VS_pes_id"] = value;
            }
        }

        #endregion

        #region Enum

        /// <summary>
        /// Enum do tipo de certidao (Nascimento ou casamento)
        /// </summary>
        public enum TipoCertidao
        {
            Nascimento = 0
            ,
            Casamento = 1
        }

        #endregion

        #region PageLifeCycle

        protected void Page_Init(object sender, EventArgs e)
        {
            cvDtEmissao.ErrorMessage = GestaoEscolarUtilBO.RetornaMsgValidacaoData("Data emissão");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm != null)
            {
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JQueryValidation));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.JqueryMask));
                sm.Scripts.Add(new ScriptReference(ArquivoJS.MascarasCampos));
            }
        }

        #endregion

        #region Eventos

        protected void rblTipoCertidao_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCertidao.Text = rblTipoCertidao.SelectedItem.Text;
        }

        protected void btnCadastraCidade_Click(object sender, ImageClickEventArgs e)
        {
            if (_AbreJanelaCadastroCidade != null)
                _AbreJanelaCadastroCidade();
        }

        protected void btnLimparCertidao_Click(object sender, ImageClickEventArgs e)
        {
            LimpaCampos();
        }

        protected void ValidarDatas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool flag = true;

            DateTime dtEmissao;
            DateTime.TryParse(args.Value, out dtEmissao);

            if ((dtEmissao != new DateTime()) && (dtEmissao > DateTime.Now))
                flag = false;

            args.IsValid = flag;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Método para carregar as unidades federativas no combo
        /// </summary>
        private void CarregaComboUnidadeFederativa()
        {
            ddlUF.Items.Clear();
            ddlUF.DataSource = END_UnidadeFederativaBO.GetSelect();
            ddlUF.Items.Insert(0, new ListItem("-- Selecione uma unidade federativa --", "00000000-0000-0000-0000-000000000000", true));
            ddlUF.DataBind();
        }

        /// <summary>
        /// Método para limpar os dados da certidao civil
        /// </summary>
        private void LimpaCampos()
        {
            txtMatricula.Text = string.Empty;
            tbNumTerm.Text = string.Empty;
            tbFolha.Text = string.Empty;
            tbLivro.Text = string.Empty;
            tbDtEmissao.Text = string.Empty;
            tbNomeCart.Text = string.Empty;
            tbDistritoCart.Text = string.Empty;
            tbCidadeCart.Text = string.Empty;
            ddlUF.SelectedValue = Guid.Empty.ToString();
            chbGemeos.Checked = false;
        }

        /// <summary>
        /// Método para validar os campos preenchidos na certidao civil
        /// </summary>
        /// <returns>Retorna true se todos os campos em validação foram preenchidos corretamente, caso contrário retorna false</returns>
        private bool ValidaCampos()
        {
            if (!string.IsNullOrEmpty(tbNumTerm.Text) || !string.IsNullOrEmpty(tbFolha.Text) || !string.IsNullOrEmpty(tbLivro.Text) ||
                !string.IsNullOrEmpty(tbDtEmissao.Text) || !string.IsNullOrEmpty(tbNomeCart.Text) || !string.IsNullOrEmpty(tbDistritoCart.Text) ||
                !string.IsNullOrEmpty(tbCidadeCart.Text) || !string.IsNullOrEmpty(txtMatricula.Text) || chbGemeos.Checked == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Método para validar os campos preenchidos na certidao civil, onde o campo de 'matricula' ou todos os outros campos menos 'data de emissão' são obrigatórios
        /// </summary>
        /// <returns>Retorna true se os campos foram preenchidos corretamente</returns>
        private bool ValidaCamposMatriculaCertidao()
        {
            if (!string.IsNullOrEmpty(txtMatricula.Text))
            {
                return true;
            }
            else if (!string.IsNullOrEmpty(tbNumTerm.Text) && !string.IsNullOrEmpty(tbFolha.Text) && !string.IsNullOrEmpty(tbLivro.Text)
                && !string.IsNullOrEmpty(tbNomeCart.Text) && !string.IsNullOrEmpty(tbDistritoCart.Text) && !string.IsNullOrEmpty(tbCidadeCart.Text))
            {
                return true;
            }

            return false;
        }

        //<summary>
        //Método para inicializar o usercontrol, carregando o combo de unidade federativa e limpando os demais campos
        //</summary>
        public void Inicializa(string validationGroup)
        {
            cvDtEmissao.ValidationGroup = validationGroup;
            LimpaCampos();
            CarregaComboUnidadeFederativa();

            chbGemeos.Visible = !ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.EXIBIR_CAMPO_ALUNO_GEMEO, __SessionWEB.__UsuarioWEB.Usuario.ent_id);
        }

        /// <summary>
        /// Método para carregar os dados da certidao civil
        /// apartir da entidade de certidão cívil
        /// </summary>
        public void CarregaCertidaoCivil(PES_CertidaoCivil certidao)
        {
            _VS_ctc_id = certidao.ctc_id;
            _VS_pes_id = certidao.pes_id;

            txtMatricula.Text = certidao.ctc_matricula;
            tbNumTerm.Text = certidao.ctc_numeroTermo;
            tbFolha.Text = certidao.ctc_folha;
            tbLivro.Text = certidao.ctc_livro;
            tbDtEmissao.Text = certidao.ctc_dataEmissao != new DateTime() ? certidao.ctc_dataEmissao.ToString("dd/MM/yyyy") : string.Empty;
            tbNomeCart.Text = certidao.ctc_nomeCartorio;
            tbDistritoCart.Text = certidao.ctc_distritoCartorio;

            _txtCid_id.Value = certidao.cid_idCartorio.ToString();
            ////Carrega a cidade
            END_Cidade cidade = new END_Cidade { cid_id = certidao.cid_idCartorio };
            END_CidadeBO.GetEntity(cidade);
            tbCidadeCart.Text = cidade.cid_nome;

            if (rblTipoCertidao.Items.FindByValue(certidao.ctc_tipo.ToString()) != null)
                rblTipoCertidao.SelectedValue = certidao.ctc_tipo.ToString();
            else if (rblTipoCertidao.Items.Count > 0)
                rblTipoCertidao.SelectedValue = rblTipoCertidao.Items[0].Value;

            lblCertidao.Text = rblTipoCertidao.SelectedItem.Text;

            ddlUF.SelectedValue = certidao.unf_idCartorio.ToString();

            chbGemeos.Checked = certidao.ctc_gemeo;
        }

        /// <summary>
        /// Método para carregar os dados da certidão civil
        /// apartir do Id da pessoa
        /// </summary>
        /// <param name="pes_id">Id da pessoa</param>
        public void CarregaCertidaoCivil(Guid pes_id)
        {
            List<PES_CertidaoCivil> lt = PES_CertidaoCivilBO.SelecionaPorPessoa(pes_id);

            PES_CertidaoCivil entityCertidaoCivil = new PES_CertidaoCivil();
            if (lt.Count > 0)
            {
                entityCertidaoCivil = (lt.Select(p => p.ctc_tipo).Distinct().Count() > 1
                                           ? lt.Find(
                                               p =>
                                               p.ctc_tipo ==
                                               Convert.ToByte(PES_CertidaoCivilBO.TipoCertidaoCivil.Casamento))
                                           : lt.FirstOrDefault());
            }

            CarregaCertidaoCivil(entityCertidaoCivil);
        }

        /// <summary>
        /// Método para retornar dados da certidão civil com a validaçao ja efetuada
        /// </summary>
        public bool RetornaCertidaoCivil(out PES_CertidaoCivil entityCertidaoCivil)
        {
            bool paramCertidaoObrigatorio = ACA_ParametroAcademicoBO.ParametroValorBooleanoPorEntidade(eChaveAcademico.CAMPO_CERTIDAO_NASCIMENTO_OBRIGATORIO
                , __SessionWEB.__UsuarioWEB.Usuario.ent_id);

            entityCertidaoCivil = new PES_CertidaoCivil();

            if (paramCertidaoObrigatorio && !ValidaCamposMatriculaCertidao())
            {
                return false;
            }
            else if (!ValidaCampos())
            {
                return false;
            }

            entityCertidaoCivil.ctc_id = _VS_ctc_id;
            entityCertidaoCivil.pes_id = _VS_pes_id;
            entityCertidaoCivil.ctc_tipo = Convert.ToByte(rblTipoCertidao.SelectedValue);
            entityCertidaoCivil.ctc_numeroTermo = tbNumTerm.Text;
            entityCertidaoCivil.ctc_folha = tbFolha.Text;
            entityCertidaoCivil.ctc_livro = tbLivro.Text;
            entityCertidaoCivil.ctc_dataEmissao = string.IsNullOrEmpty(tbDtEmissao.Text) ? new DateTime() : Convert.ToDateTime(tbDtEmissao.Text);
            entityCertidaoCivil.ctc_nomeCartorio = tbNomeCart.Text;
            entityCertidaoCivil.ctc_distritoCartorio = tbDistritoCart.Text;
            entityCertidaoCivil.cid_idCartorio = string.IsNullOrEmpty(_txtCid_id.Value) ? Guid.Empty : new Guid(_txtCid_id.Value);
            entityCertidaoCivil.unf_idCartorio = new Guid(ddlUF.SelectedValue);
            entityCertidaoCivil.ctc_situacao = 1;
            entityCertidaoCivil.ctc_matricula = txtMatricula.Text;
            entityCertidaoCivil.IsNew = _VS_ctc_id == Guid.Empty ? true : false;
            entityCertidaoCivil.ctc_gemeo = chbGemeos.Checked;

            return true;            
        }

        #endregion
    }
}