<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Configuracao_PeriodoCalendario_Cadastro" Codebehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoPeriodoCalendario/Busca.aspx" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc36" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Cadastro de tipo de período do calendário</legend>
        <uc36:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <asp:Label ID="LabelTipoPeriodoCalendario" runat="server" Text="Tipo de período do calendário *"
            AssociatedControlID="_txtTipoPeriodoCalendario"></asp:Label>
        <asp:TextBox ID="_txtTipoPeriodoCalendario" runat="server" SkinID="text30C" MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvTipoPeriodoCalendario" runat="server" ControlToValidate="_txtTipoPeriodoCalendario"
            ErrorMessage="Tipo de período calendário é obrigatório." Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:Label ID="LabelTipoPeriodoCalendarioAbreviado" runat="server" Text="Nome abreviado"
            AssociatedControlID="_txtTipoPeriodoCalendarioAbreviado"></asp:Label>
        <asp:TextBox ID="_txtTipoPeriodoCalendarioAbreviado" runat="server" SkinID="text30C" MaxLength="50"></asp:TextBox>
        <asp:CheckBox ID="_ckbForaPeriodoLetivo" runat="server" Text="Fora do período letivo" />
        <asp:CheckBox ID="_ckbBloqueado" runat="server" Text="Bloqueado" />
        <div class="right">
            <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" OnClick="_btnSalvar_Click" />
            <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="_btnCancelar_Click" />
        </div>
    </fieldset>
</asp:Content>
