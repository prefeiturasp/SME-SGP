<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.NivelAprendizado.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Academico/NivelAprendizado/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagPrefix="uc1" TagName="UCCamposObrigatorios" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ValidationSummary ID="vsOrientacao" runat="server"/>
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="fdsConsulta" runat="server">
        <legend>Cadastro de nível de aprendizado</legend>
        <div></div>
        <asp:UpdatePanel ID="updNivelAprendizado" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:uccamposobrigatorios runat="server" id="UCCamposObrigatorios" />
                <uc2:uccombocursocurriculo id="UCComboCursoCurriculo1" runat="server" mostrarmessageselecione="true" obrigatorio="true"/>
                <uc3:uccombocurriculoperiodo id="UCComboCurriculoPeriodo1" runat="server" mostrarmessageselecione="true" obrigatorio="true"/>
                
                <asp:Label ID="lblDescricao" runat="server" Text="Descrição *" AssociatedControlID="txtDescricao"></asp:Label>
                <asp:TextBox ID="txtDescricao" runat="server" MaxLength="20" SkinID="text60C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDescricao" runat="server" ControlToValidate="txtDescricao"
                Display="Dynamic" ErrorMessage="Descrição é obrigatório.">*</asp:RequiredFieldValidator>
                <asp:Label ID="lblSigla" runat="server" Text="Sigla *" AssociatedControlID="txtSigla"></asp:Label>
                <asp:TextBox ID="txtSigla" runat="server" MaxLength="2" SkinID="text10C"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSigla" runat="server" ControlToValidate="txtSigla"
                Display="Dynamic" ErrorMessage="Sigla é obrigatório.">*</asp:RequiredFieldValidator>
                <div align="right">
                    <asp:Button ID="bntSalvar" runat="server" Text="Salvar" OnClick="bntSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false" OnClick="btnCancelar_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
