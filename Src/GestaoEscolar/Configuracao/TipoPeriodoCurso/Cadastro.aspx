<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
        Inherits="Configuracao_TipoPeriodoCurso_Cadastro" Codebehind="Cadastro.aspx.cs" %>
<%@ PreviousPageType VirtualPath="~/Configuracao/TipoPeriodoCurso/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoModalidadeEnsino.ascx" TagName="UCComboTipoModalidadeEnsino" 
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino" 
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="vgTipoPeriodoCurso" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updCadastroQualidade" runat="server">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
                <fieldset>
                    <legend><asp:Label ID="lblLegendaCadastroPeriodoCurso" runat="server" ></asp:Label></legend>
                     <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
                        <uc3:UCComboTipoNivelEnsino runat="server" ID="UCComboTipoNivelEnsino" Obrigatorio="true" 
                            PermiteEditar="false" ValidationGroup="vgTipoPeriodoCurso" />                            
                        <uc2:UCComboTipoModalidadeEnsino runat="server" ID="UCComboTipoModalidadeEnsino" Obrigatorio="true" 
                            PermiteEditar="false" ValidationGroup="vgTipoPeriodoCurso" />   
                        <asp:Label ID="lblDescricao" runat="server" AssociatedControlID="txtDescricao"></asp:Label>
                        <asp:TextBox ID="txtDescricao" runat="server" MaxLength="100" Width="100px" ValidationGroup ="vgTipoPeriodoCurso" Enabled="false"></asp:TextBox>
                        <asp:Label ID="lblObjetoAprendizagem" runat="server" AssociatedControlID="chkObjetoAprendizagem"></asp:Label>
                        <asp:CheckBox ID="chkObjetoAprendizagem" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvDescricaoPeriodo" runat="server" ControlToValidate="txtDescricao"
                            ValidationGroup="vgTipoPeriodoCurso" Display="Dynamic" ErrorMessage="Tipo de período do curso é obrigatório.">*</asp:RequiredFieldValidator>
                </fieldset>
                <fieldset>
                    <div class="right">
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                            ValidationGroup="vgTipoPeriodoCurso" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                            OnClick="btnCancelarClick" />
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

