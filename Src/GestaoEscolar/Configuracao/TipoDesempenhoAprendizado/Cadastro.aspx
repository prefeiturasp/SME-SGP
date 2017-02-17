<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.TipoDesempenhoAprendizado.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/TipoDesempenhoAprendizado/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagPrefix="uc3" TagName="UCCCursoCurriculo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagPrefix="uc3" TagName="UCCCurriculoPeriodo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagPrefix="uc3" TagName="UCComboTipoDisciplina" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="TipoAprendizando" />
    <fieldset>
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Configuracao, TipoDesempenhoAprendizado.Cadastro.lblLegend.Text %>"></asp:Label></legend>
        <uc3:UCCCalendario runat="server" ID="UCCCalendario" MostrarMensagemSelecione="true" PermiteEditar="true" Obrigatorio="true" ValidationGroup="TipoAprendizando" />
        <uc3:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" MostrarMensagemSelecione="true" PermiteEditar="false" Obrigatorio="true" ValidationGroup="TipoAprendizando" />
        <uc3:UCCCurriculoPeriodo runat="server" ID="UCCCurriculoPeriodo" MostrarMensagemSelecione="true" PermiteEditar="false" Obrigatorio="true" ValidationGroup="TipoAprendizando" />
        <uc3:UCComboTipoDisciplina runat="server" ID="UCComboTipoDisciplina" MostrarMensagemSelecione="false" PermiteEditar="false" Obrigatorio="true" ValidationGroup="TipoAprendizando" />
        <asp:Label ID="lblDescricao" runat="server" Text="<%$ Resources:Configuracao, TipoDesempenhoAprendizado.Cadastro.lblDescricao.Text %>" AssociatedControlID="txtDescricao"></asp:Label>
        <asp:TextBox runat="server" ID="txtDescricao" SkinID="limite4000" TextMode="MultiLine"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvDescricao" runat="server" ErrorMessage="<%$ Resources:Configuracao, TipoDesempenhoAprendizado.Cadastro.rfvDescricao.ErrorMessage %>"
            Display="Dynamic" ControlToValidate="txtDescricao" ValidationGroup="TipoAprendizando">*</asp:RequiredFieldValidator>

        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                ValidationGroup="TipoAprendizando" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                CausesValidation="false" />
        </div>
    </fieldset>
</asp:Content>
