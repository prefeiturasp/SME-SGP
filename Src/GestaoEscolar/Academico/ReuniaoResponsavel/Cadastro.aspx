<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="Cadastro.aspx.cs" Inherits="Academico_ReuniaoResponsavel_Cadastro" %>

<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="_UCLoader"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="_UCCamposObrigatorios"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="_UCComboCursoCurriculo"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="_UCComboCalendario"
    TagPrefix="uc4" %>
<%@ Register src="../../WebControls/Combos/UCComboPeriodoCalendario.ascx" tagname="UCComboPeriodoCalendario" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="_updCadastroReuniao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="_valMessage" runat="server" ValidationGroup="Reuniao"
                EnableViewState="False" />
            <fieldset id="_fdsReuniaoResponsavel" runat="server">
                <legend>Cadastro de reuniões de responsável</legend>
                <uc1:_UCLoader ID="_UCLoader" runat="server" />
                <uc2:_UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
                <uc3:_UCComboCursoCurriculo ID="_UCComboCursoCurriculo" runat="server" MostrarMessageSelecione="true"
                    ValidationGroup="Reuniao" />
                <uc4:_UCComboCalendario ID="_UCComboCalendario" runat="server" Obrigatorio="true"
                    ValidationGroup="Reuniao" MostrarMensagemSelecione="true" />
                <uc5:UCComboPeriodoCalendario ID="UCComboPeriodoCalendario" ValidationGroup="Reuniao" _MostrarMessageSelecione="True" runat="server" />
                <asp:Label ID="_lblQtde" runat="server" Text="Quantidade de reuniões por período do calendário *"
                    AssociatedControlID="_txtQtde" Visible="false"></asp:Label>
                <asp:TextBox ID="_txtQtde" runat="server" Visible="false" SkinID="Numerico" MaxLength="3"></asp:TextBox>
                <asp:RequiredFieldValidator ID="_rfvValidaQde" runat="server" ErrorMessage="Quantidade de reuniões por período do calendário é obrigatório."
                    Text="*" ControlToValidate="_txtQtde" ValidationGroup="Reuniao">*</asp:RequiredFieldValidator>
                <div id="_divBotoes" class="right" runat="server" visible="false">
                    <asp:Button ID="_btnSalvar" runat="server" Text="Salvar" ValidationGroup="Reuniao"
                        OnClick="_btnSalvar_Click" />
                    <asp:Button ID="_btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                        OnClick="_btnCancelar_Click" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
