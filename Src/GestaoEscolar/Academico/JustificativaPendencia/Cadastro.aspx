<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="Academico_JustificativaPendencia_Cadastro" %>
<%@ PreviousPageType VirtualPath="~/Academico/JustificativaPendencia/Busca.aspx" %>

<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="ComboUAEscola" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="ComboCalendario" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurmaDisciplina.ascx" TagName="ComboTurmaDisciplina" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updJustificativaPendencia" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="vsCadastroJustificativa" runat="server" ValidationGroup="CadastroJustificativa" />
            <fieldset>
                <legend><asp:Literal runat="server" ID="litJustificativas" Text="<%$ Resources:Academico, JustificativaPendencia.Cadastro.litJustificativas.Text %>"></asp:Literal></legend>
                
                <div class="area-form">
                    <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
                    <uc3:ComboUAEscola ID="comboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                        ObrigatorioEscola="true" ObrigatorioUA="true" 
                        MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" 
                        ValidationGroupEscola="CadastroJustificativa" ValidationGroupUA="ConsultaJustificativa" />
                    <uc4:ComboCalendario ID="comboCalendario" runat="server" MostrarMessageSelecione="true" Obrigatorio="true" SelecionarAnoCorrente="true" ValidationGroup="CadastroJustificativa" />
                    <uc5:ComboTurmaDisciplina ID="comboTurmaDisciplina" runat="server" MostrarMessageSelecione="true" VS_TurmaEletiva="true" ValidationGroup="CadastroJustificativa" />
                
                    <asp:Label ID="lblJustificativa" runat="server" Text="<%$ Resources:Academico, JustificativaPendencia.Cadastro.lblJustificativa.Text %>" AssociatedControlID="txtJustificativa"></asp:Label>
                    <asp:TextBox ID="txtJustificativa" runat="server" TextMode="MultiLine" SkinID="text60C"
                        CssClass="wrap250px" onkeypress="LimitarCaracter(this,'contadesc3','4000');"
                        onkeyup="LimitarCaracter(this,'contadesc3','4000');"></asp:TextBox>
                    <span id="contadesc3" style="display: inline; font-size: 85%; position: relative; top: -8px;">0/4000</span>
                    <asp:RequiredFieldValidator ID="rfvJustificativa" runat="server" ErrorMessage="<%$ Resources:Academico, JustificativaPendencia.Cadastro.rfvJustificativa.ErrorMessage %>"
                        Display="Dynamic" ControlToValidate="txtJustificativa" ValidationGroup="CadastroJustificativa">*</asp:RequiredFieldValidator>
                
                    <fieldset id="fdsPeriodoCalendario" runat="server" visible="false">
                        <legend><asp:Literal runat="server" ID="litPeriodoCalendario" Text="<%$ Resources:Mensagens, MSG_CALENDARIOPERIODO_PLURAL %>"></asp:Literal></legend>
                        <div></div>
                        <asp:Repeater ID="rptCampos" runat="server" OnItemDataBound="rptCampos_ItemDataBound">
                            <HeaderTemplate>
                                <div class="checkboxlist-columns four">
                            </HeaderTemplate>

                            <ItemTemplate>
                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("tpc_id") %>' />
                                <asp:CheckBox ID="ckbCampo" runat="server" Text='<%# Eval("cap_descricao") %>' />
                            </ItemTemplate>

                            <FooterTemplate>
                                </div> 
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Label ID="lblNenhumPeriodo" runat="server" CssClass="summary" Text="<%$ Resources:Academico, JustificativaPendencia.Cadastro.lblNenhumPeriodo.Text %>"></asp:Label>
                    </fieldset>

                    <asp:HiddenField ID="hdnFjpId" runat="server" Value="-1" />
                    <asp:HiddenField ID="hdnTpcId" runat="server" Value="-1" />
                </div>
                <div class="right area-botoes-bottom">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                        ValidationGroup="CadastroJustificativa" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                        CausesValidation="false" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
