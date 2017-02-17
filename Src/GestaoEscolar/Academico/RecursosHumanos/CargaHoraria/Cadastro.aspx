<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_RecursosHumanos_CargaHoraria_Cadastro" Title="Untitled Page" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/RecursosHumanos/CargaHoraria/Busca.aspx" %>
<%@ Register Src="../../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="../../../WebControls/Combos/UCComboCargo.ascx" TagName="UCComboCargo"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Cadastro de carga horária</legend>
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <asp:Label ID="_lblDescricao" runat="server" Text="Descrição da carga horária" AssociatedControlID="_txtDescricao"></asp:Label>
        <asp:TextBox ID="_txtDescricao" runat="server" SkinID="text60C" MaxLength="200" Enabled="false"></asp:TextBox>        
        <asp:CheckBox ID="_chkPadrao" Text="Padrão" runat="server" OnCheckedChanged="_chkPadrao_CheckedChanged" AutoPostBack="true" Enabled="false"/>
        <asp:CheckBox ID="_chkEspecialista" Text="Especialista" runat="server" Visible="false"  Enabled="false"/>
        <uc2:UCComboCargo ID="_UCComboCargo" runat="server" Visible="true" Obrigatorio="true" PermiteEditar="false" />
        <asp:Label ID="_lblCargaHrsSemanais" runat="server" Text="Horas semanais (Minutos) *" AssociatedControlID="_txtCargaHrsSemanais"></asp:Label>
        <asp:TextBox ID="_txtCargaHrsSemanais" runat="server" MaxLength="5" CssClass="numeric" SkinID="Numerico" Enabled="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvCargaHrsSemanais" runat="server" Display="Dynamic"
            ErrorMessage="Horas semanais é obrigatório e deve ser um número inteiro maior que 0 (zero)." ControlToValidate="_txtCargaHrsSemanais">*</asp:RequiredFieldValidator>
        <asp:Label ID="_lblTemposAulas" runat="server" Text="Tempos de aulas" AssociatedControlID="_txtTemposAulas"></asp:Label>
        <asp:TextBox ID="_txtTemposAulas" runat="server" MaxLength="5" CssClass="numeric"
            SkinID="Numerico" Enabled ="false"></asp:TextBox>
        <asp:Label ID="_lblHorasAulas" runat="server" Text="Horas de aulas" AssociatedControlID="_txtHorasAulas"></asp:Label>
        <asp:TextBox ID="_txtHorasAulas" runat="server" MaxLength="5" CssClass="numeric"
            SkinID="Numerico" Enabled="false"></asp:TextBox>
        <asp:Label ID="_lblHorasComplementares" runat="server" Text="Horas complementares" AssociatedControlID="_txtHorasComplementares"></asp:Label>
        <asp:TextBox ID="_txtHorasComplementares" runat="server" MaxLength="5" CssClass="numeric"
            SkinID="Numerico" Enabled="false"></asp:TextBox>
        <asp:CheckBox ID="_ckbBloqueado" Text="Bloqueado" runat="server" Enabled="false" />
        <div class="right">
            <asp:Button ID="_btnCancelar" runat="server" Text="Voltar" OnClick="_btnCancelar_Click"
                CausesValidation="false" /></div>
    </fieldset>
</asp:Content>
