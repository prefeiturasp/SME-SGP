<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.NivelOrientacaoCurricular.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina" 
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboMatrizHabilidades.ascx" TagName="UCComboMatrizHabilidades" 
    TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upnPesquisa" runat="server">
        <ContentTemplate>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="ValidationNivelOrientacao" />
    <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend>Consulta de níveis de orientação curricular</legend>
        <div id="divPesquisa" runat="server">
            <uc4:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
            <uc1:UCComboCalendario ID="UCComboCalendario1" runat="server" Obrigatorio="true" 
                        MostrarMensagemSelecione="true" ValidationGroup="ValidationNivelOrientacao"
                        OnIndexChanged="UCComboCalendario1_IndexChanged" />
            <uc2:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" runat="server" Obrigatorio="true" 
                        MostrarMessageSelecione="true" ValidationGroup="ValidationNivelOrientacao"
                        PermiteEditar="false" OnIndexChanged="UCComboCursoCurriculo1_IndexChanged"/>
            <uc3:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" runat="server" Obrigatorio="true" 
                        _MostrarMessageSelecione="true" ValidationGroup="ValidationNivelOrientacao"
                        PermiteEditar="false" On_OnSelectedIndexChange="UCComboCurriculoPeriodo1_OnSelectedIndexChange" />
            <uc5:UCComboTipoDisciplina ID="UCComboTipoDisciplina" runat="server" Obrigatorio="true"
                        _MostrarMessageSelecione="true" ValidationGroup="ValidationNivelOrientacao"
                        PermiteEditar="false" />
                    <uc6:UCComboMatrizHabilidades ID="UCComboMatrizHabilidades" runat="server" 
                        mostrarmessageselecione="true" Obrigatorio="true" ValidationGroup="ValidationNivelOrientacao" />
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" 
                PostBackUrl="~/Configuracao/NivelOrientacaoCurricular/Cadastro.aspx"  
                ValidationGroup="ValidationNivelOrientacao" />
        </div>
    </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>