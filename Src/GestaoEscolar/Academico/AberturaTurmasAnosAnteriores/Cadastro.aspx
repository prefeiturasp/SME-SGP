<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.AberturaTurmasAnosAnteriores.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Academico/AberturaTurmasAnosAnteriores/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="ComboUAEscola"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="vsAberturaAnosAnteriores" runat="server" ValidationGroup="vgAberturaAnosAnteriores"/>
    <fieldset>
        <legend>Cadastro de agendamentos de abertura de anos letivos anteriores</legend>
        <div id="divPesquisa" runat="server">
            <asp:UpdatePanel ID="updPesquisa" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblAno" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.lblAno.Text %>" AssociatedControlID="txtAno"></asp:Label>
                    <asp:TextBox ID="txtAno" runat="server" SkinID="Numerico" MaxLength="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAno" ControlToValidate="txtAno" ValidationGroup="vgAberturaAnosAnteriores"
                        runat="server" ErrorMessage="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.rfvAno.ErrorMessage %>">*</asp:RequiredFieldValidator>
                    <uc1:ComboUAEscola ID="ucComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                        ObrigatorioEscola="false" ObrigatorioUA="false" MostrarMessageSelecioneEscola="true"
                        MostrarMessageSelecioneUA="true" ValidationGroup="vgAberturaAnosAnteriores"/>
                    <asp:Label ID="lblDataInicial" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.lblDataInicial.Text %>" AssociatedControlID="txtDataInicial"></asp:Label>
                    <asp:TextBox ID="txtDataInicial" runat="server" SkinID="Data"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDataInicial" ControlToValidate="txtDataInicial" ValidationGroup="vgAberturaAnosAnteriores"
                        runat="server" ErrorMessage="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.rfvDataInicial.ErrorMessage %>">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvDataInicial" runat="server" ControlToValidate="txtDataInicial" ErrorMessage="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.cvDataInicial.ErrorMessage %>" Display="Dynamic" OnServerValidate="ValidarData_ServerValidate"
                        ValidationGroup="vgAberturaAnosAnteriores">*</asp:CustomValidator>
                    <asp:Label ID="lblDataFinal" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.lblDataFinal.Text %>" AssociatedControlID="txtDataFinal"></asp:Label>
                    <asp:TextBox ID="txtDataFinal" runat="server" SkinID="Data"></asp:TextBox>
                    <asp:CustomValidator ID="cvDataFinal" runat="server" ControlToValidate="txtDataFinal" ErrorMessage="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.cvDataFinal.ErrorMessage %>" Display="Dynamic" OnServerValidate="ValidarData_ServerValidate"
                        ValidationGroup="vgAberturaAnosAnteriores">*</asp:CustomValidator>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.btnSalvar.Text %>" OnClick="btnSalvar_Click" 
                ToolTip="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.btnSalvar.ToolTip %>" ValidationGroup="vgAberturaAnosAnteriores"/>
            <asp:Button ID="btnCancelar" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.btnCancelar.ToolTip %>" OnClick="btnCancelar_Click"
                ToolTip="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Cadastro.btnCancelar.ToolTip %>" CausesValidation="false" />
        </div>
    </fieldset>
</asp:Content>
