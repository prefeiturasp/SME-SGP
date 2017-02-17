<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.ParametroResultado.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/ParametroResultado/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagPrefix="uc3" TagName="UCCCursoCurriculo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Resultado" />
    <fieldset>
        <legend>Cadastro de resultados</legend>
        <uc3:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" MostrarMensagemSelecione="true" Obrigatorio="true" ValidationGroup="Resultado"/>
        <asp:Label ID="lblConceitoFinal" runat="server" Text="Conceito final *" AssociatedControlID="ddlConceitoFinal"></asp:Label>
        <asp:DropDownList ID="ddlConceitoFinal" runat="server">
            <asp:ListItem Value="-1">-- Selecione um conceito final --</asp:ListItem>
            <asp:ListItem Value="1">Aprovado</asp:ListItem>
            <asp:ListItem Value="2">Reprovado</asp:ListItem>
            <asp:ListItem Value="3">Reprovado por frequência histórico</asp:ListItem>
            <asp:ListItem Value="8">Reprovado por frequência</asp:ListItem>
            <asp:ListItem Value="9">Recuperacao final</asp:ListItem>
            <asp:ListItem Value="10">Aprovado por conselho</asp:ListItem>
        </asp:DropDownList>
        <asp:CompareValidator runat="server" ID="cpvConceitoFinal" ErrorMessage="Conceito final é obrigatório."
            Display="Dynamic" ControlToValidate="ddlConceitoFinal" ValidationGroup="Resultado" ValueToCompare="-1" Operator="NotEqual">*</asp:CompareValidator>
        <asp:Label ID="lblNomenclatura" runat="server" Text="Nomenclatura *" AssociatedControlID="txtNomenclatura"></asp:Label>
        <asp:TextBox runat="server" ID="txtNomenclatura" MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvNomenclatura" runat="server" ErrorMessage="Nomenclatura é obrigatório."
            Display="Dynamic" ControlToValidate="txtNomenclatura" ValidationGroup="Resultado">*</asp:RequiredFieldValidator>
        <asp:Label ID="lblTipoLancamento" runat="server" Text="Tipo de lançamento *" AssociatedControlID="ddlTipoLancamento"></asp:Label>
        <asp:DropDownList ID="ddlTipoLancamento" runat="server"
            AutoPostBack="True" OnSelectedIndexChanged="ddlTipoLancamento_SelectedIndexChanged">
            <asp:ListItem Value="-1" Text="-- Selecione um tipo de lançamento --"/>
            <asp:ListItem Value="1" Text="<%$ Resources:Configuracao, ParametroResultado.Cadastro.ddlTipoLancamento.valor1 %>"/>
            <asp:ListItem Value="2" Text="<%$ Resources:Configuracao, ParametroResultado.Cadastro.ddlTipoLancamento.valor2 %>"/>
            <asp:ListItem Value="3" Text="<%$ Resources:Configuracao, ParametroResultado.Cadastro.ddlTipoLancamento.valor3 %>"/>
        </asp:DropDownList>
        <asp:CompareValidator runat="server" ID="cpvTipoLancamento" ErrorMessage="Tipo de lançamento é obrigatório."
            Display="Dynamic" ControlToValidate="ddlTipoLancamento" ValidationGroup="Resultado" ValueToCompare="-1" Operator="NotEqual">*</asp:CompareValidator>
        <asp:Label ID="lblTipoDisciplina" runat="server" Text="<%$ Resources:Configuracao, ParametroResultado.Cadastro.lblTipoDisciplina.Text %>" AssociatedControlID="ddlTipoDisciplina"></asp:Label>
        <asp:DropDownList ID="ddlTipoDisciplina" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Value="-1" Text="<%$ Resources:Configuracao, ParametroResultado.Cadastro.ddlTipoDisciplina.valor0 %>"/>
        </asp:DropDownList>
        <br />
        <br />
        <asp:Panel runat="server" ID="pnlPeriodos" Visible="false" GroupingText="Séries">
            <asp:CheckBoxList runat="server" ID="cblPeriodos" Visible="true"></asp:CheckBoxList>
        </asp:Panel>
        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                ValidationGroup="Resultado" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                CausesValidation="false" />
        </div>
    </fieldset>
</asp:Content>
