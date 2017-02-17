<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.ObservacaoBoletim.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/ObservacaoBoletim/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboObservacaoBoletim.ascx" TagPrefix="uc1" TagName="UCComboObservacaoBoletim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Observacao" />
    <fieldset>
        <legend>Cadastro de observações do boletim</legend>
        <uc1:UCComboObservacaoBoletim runat="server" ID="UCComboObservacaoBoletim" Obrigatorio="true" MostrarMessageSelecione="true" ValidationGroup="Observacao" />

        <asp:Label ID="lblNome" runat="server" Text="Nome *" AssociatedControlID="txtNome"></asp:Label>
        <asp:TextBox runat="server" ID="txtNome" SkinID="text60C" MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvNome" runat="server" ErrorMessage="Nome é obrigatório."
                Display="Dynamic" ControlToValidate="txtNome" ValidationGroup="Observacao">*</asp:RequiredFieldValidator>
      
        
        <asp:Label ID="lblDescricao" runat="server" Text="Descrição *" AssociatedControlID="txtDescricao"></asp:Label>
        <asp:TextBox runat="server" ID="txtDescricao" SkinID="limite2000" TextMode="MultiLine"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvDescricao" runat="server" ErrorMessage="Descrição é obrigatório."
                Display="Dynamic" ControlToValidate="txtDescricao" ValidationGroup="Observacao">*</asp:RequiredFieldValidator>
        
        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                ValidationGroup="Observacao" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                CausesValidation="false" />
        </div>
    </fieldset>
</asp:Content>
