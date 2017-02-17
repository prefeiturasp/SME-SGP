<%@ Page Language="C#" MasterPageFile="~/MasterPageAluno.Master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="AreaAluno.Cadastro.CompromissoEstudo.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Cadastro/CompromissoEstudo/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoPeriodoCalendario.ascx" TagName="UCComboTipoPeriodoCalendario" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Mensagens/UCConfirmacaoOperacao.ascx" TagName="UCConfirmacaoOperacao" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>    
    <asp:ValidationSummary ID="vsCompromissoEstudo" runat="server" ValidationGroup="CompromissoEstudo" EnableViewState="False" />    
        <fieldset id="fdsCompromissoEstudo" runat="server">
            <legend style="margin-top: 10px;">
                <asp:Label ID="lblLegend" text="<%$ Resources:AreaAluno, Cadastro.CompromissoEstudo.Cadastro.lblLegend.Text %>" runat="server"/>
            </legend>
            <uc2:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
            <br />
            <uc1:UCComboTipoPeriodoCalendario ID="UCComboTipoPeriodoCalendario1" runat="server" />
            <asp:Label ID="lblOqTenhoFeito" runat="server" Text="O que tenho feito? *" AssociatedControlID="txtOqTenhoFeito"></asp:Label>
            <asp:TextBox ID="txtOqTenhoFeito" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvOqTenhoFeito" runat="server" ControlToValidate="txtOqTenhoFeito"
                ErrorMessage="Campo o que tenho feito é obrigatório." ValidationGroup="CompromissoEstudo" 
                Display="Dynamic">*</asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblOqPretendoFazer" runat="server" Text="O que pretendo fazer? *" AssociatedControlID="txtOqPretendoFazer"></asp:Label>
            <asp:TextBox ID="txtOqPretendoFazer" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvOqPretendoFazer" runat="server" ControlToValidate="txtOqPretendoFazer"
                ErrorMessage="Campo o que pretendo fazer é obrigatório." ValidationGroup="CompromissoEstudo" 
                Display="Dynamic">*</asp:RequiredFieldValidator>
        </fieldset>
        <div class="cadastroBts">
            <asp:Button ID="btnVoltar" runat="server" CausesValidation="False" Text="Voltar" PostBackUrl="~/Cadastro/CompromissoEstudo/Busca.aspx"/>
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar"  ValidationGroup="CompromissoEstudo"
                OnClick="btnSalvar_Click" />
        </div>
    <uc3:UCConfirmacaoOperacao ID="UCConfirmacaoOperacao1" runat="server" />
</asp:Content>
