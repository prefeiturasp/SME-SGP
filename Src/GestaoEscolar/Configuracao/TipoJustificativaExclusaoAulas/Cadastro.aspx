<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoJustificativaExclusaoAulas_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoJustificativaExclusaoAulas/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Cadastro de tipo de justificativa para exclusão de aulas</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="lblJustificativaExclusaoAulas" runat="server" Text="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Cadastro.lblJustificativaExclusaoAulas.Text %>"
            AssociatedControlID="txtJustificativaExclusaoAulas"></asp:Label>
        <asp:TextBox ID="txtJustificativaExclusaoAulas" runat="server" MaxLength="100" SkinID="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvJustificativaExclusaoAulas" runat="server" ControlToValidate="txtJustificativaExclusaoAulas"
            Display="Dynamic" ErrorMessage="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Cadastro.rfvJustificativaExclusaoAulas.ErrorMessage %>">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblCodigo" runat="server" Text="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Cadastro.lblCodigo.Text %>" AssociatedControlID="txtCodigo"></asp:Label>
        <asp:TextBox ID="txtCodigo" runat="server" MaxLength="20" SkinID="text30C"></asp:TextBox>
        <asp:CheckBox ID="chkStuacao" runat="server" Text="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Cadastro.chkStuacao.Text %>" />
        <div class="right">
            <asp:Button ID="bntSalvar" runat="server" Text="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Cadastro.bntSalvar.Text %>" OnClick="bntSalvar_Click" 
                ToolTip="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Cadastro.bntSalvar.ToolTip %>"/>
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Cadastro.btnCancelar.Text %>" CausesValidation="false"
                OnClick="btnCancelar_Click" ToolTip="<%$ Resources:Configuracao, TipoJustificativaExclusaoAulas.Cadastro.btnCancelar.ToolTip %>"/>
        </div>
    </fieldset>
</asp:Content>
