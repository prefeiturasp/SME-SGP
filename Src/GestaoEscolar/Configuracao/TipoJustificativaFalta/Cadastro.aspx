<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoJustificativaFalta_Cadastro" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoJustificativaFalta/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Cadastro de tipo de justificativa de falta</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="lblJustificativaFalta" runat="server" Text="Tipo de justificativa de falta *"
            AssociatedControlID="txtJustificativaFalta"></asp:Label>
        <asp:TextBox ID="txtJustificativaFalta" runat="server" MaxLength="100" SkinID="text60C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvJustificativaFalta" runat="server" ControlToValidate="txtJustificativaFalta"
            Display="Dynamic" ErrorMessage="Tipo de justificativa de falta é obrigatório.">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblCodigo" runat="server" Text="Código" AssociatedControlID="txtCodigo"></asp:Label>
        <asp:TextBox ID="txtCodigo" runat="server" MaxLength="20" SkinID="text30C"></asp:TextBox>
        <asp:CheckBox ID="ckbAbonaFaltas" runat="server" Text="Abona faltas" />
        <asp:CheckBox ID="chkStuacao" runat="server" Text="Inativo" />
        <div class="right">
            <asp:Button ID="bntSalvar" runat="server" Text="Salvar" OnClick="bntSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
