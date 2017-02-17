<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="ConfigurarServico.aspx.cs" Inherits="GestaoEscolar.Configuracao.Servico.ConfigurarServico" %>

<%@ Register src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" tagname="UCCamposObrigatorios" tagprefix="uc1" %>

<%@ Register src="~/WebControls/Combos/UCComboServico.ascx" tagname="UCComboServico" tagprefix="uc2" %>

<%@ Register src="../../WebControls/FrequenciaServico/UCFrequenciaServico.ascx" tagname="UCFrequenciaServico" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updServico" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="vsServico" runat="server" ValidationGroup="Servico" />
            <asp:Label ID="lblInformacaoServico" runat="server"></asp:Label>
            <fieldset id="fdsConfigurarServico" runat="server">
            <legend>Configuração de serviços</legend>
                <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" 
                    EnableViewState="False" />
                <uc2:UCComboServico ID="UCComboServico1" runat="server" />
                <div id="divServico" runat="server" visible="false">
                    <asp:CheckBox ID="chkDesativar" runat="server" Text="Desativar serviço" AutoPostBack="true" OnCheckedChanged="chkDesativar_CheckedChanged"/>
                    <div id="divCampos" runat="server">
                        <uc3:UCFrequenciaServico ID="UCFrequenciaServico1" runat="server" ValidationGroupUCFrequenciaServico="Servico"/>
                    </div>
                    <div class="right">
                        <asp:Button ID="btnDispararAgora" runat="server" CausesValidation="false" 
                            Text="Disparar agora" OnClick="btnDispararAgora_Click" />
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar configurações" ValidationGroup="Servico"
                            OnClick="btnSalvar_Click" />
                    </div>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
