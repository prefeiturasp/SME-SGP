<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Academico.RecursosHumanos.AtribuicaoEsporadica.Cadastro" %>
<%@ PreviousPageType VirtualPath="~/Academico/RecursosHumanos/AtribuicaoEsporadica/Busca.aspx" %>
<%@ Register Src="~/WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMensagem" runat="server" Text="" EnableViewState="false"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Atribuicao" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Pesquisa" />
    <asp:Panel ID="pnlCadastro" runat="server" GroupingText="Atribuição esporádica">
        <asp:Label ID="lblMensagemInformacao" runat="server" Text="" EnableViewState="false"></asp:Label>
        <asp:Panel ID="pnlSelecaoDocente" runat="server" GroupingText="Seleção de docente">
            <asp:UpdatePanel ID="upnDocente" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblRF" runat="server" Text="RF do docente *" AssociatedControlID="txtRF"></asp:Label>
                    <asp:TextBox ID="txtRF" runat="server" SkinID="text40C" MaxLength="30"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfRF" runat="server" ControlToValidate="txtRF"
                        ErrorMessage="RF do docente é obrigatório."
                        Display="Dynamic"
                        ValidationGroup="Atribuicao">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfRF2" runat="server" ControlToValidate="txtRF"
                        ErrorMessage="RF do docente é obrigatório."
                        Display="Dynamic"
                        ValidationGroup="Pesquisa">*</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="btnPesquisar" runat="server" SkinID="btPesquisar" ToolTip="Pesquisar o RF informado"
                        ValidationGroup="Pesquisa" OnClick="btnPesquisar_Click" />
                    <asp:ImageButton ID="btnRefazerPesquisa" runat="server" SkinID="btDesfazer" ToolTip="Pesquisar outro RF"
                        Visible="false" CausesValidation="false"
                        OnClick="btnRefazerPesquisa_Click" />
                    <asp:Label ID="lblMensagemDocenteNaoEncontrado" runat="server" Text="" EnableViewState="false"></asp:Label>
                    <asp:Label ID="Label1" runat="server" Text="Nome do docente" AssociatedControlID="txtNome"></asp:Label>
                    <asp:TextBox ID="txtNome" runat="server" SkinID="text60C" Enabled="false" ReadOnly="true"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="rvNome" runat="server" ControlToValidate="txtNome"
                        ErrorMessage="Nome do docente é obrigatório."
                        Display="Dynamic"
                        ValidationGroup="Atribuicao">*</asp:RequiredFieldValidator>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <asp:Panel ID="pnlDadosCadastro" runat="server" GroupingText="Dados da atribuição">
            <asp:UpdatePanel ID="upnEscola" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:UCFiltroEscolas ID="UCFiltroEscolas" runat="server" UnidadeAdministrativaCampoObrigatorio="true" EscolaCampoObrigatorio="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="display: block">
                <asp:Label ID="Label3" runat="server" Text="Indique a data de início e término da vigência:"
                    AssociatedControlID="txtDataInicio"></asp:Label>
                <div style="display: inline-table;">
                    <asp:Label ID="Label2" runat="server" Text="Data de início *" AssociatedControlID="txtDataInicio"></asp:Label>
                    <asp:TextBox ID="txtDataInicio" runat="server" SkinID="Data"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfInicio" runat="server" ControlToValidate="txtDataInicio"
                        ErrorMessage="Data de início é obrigatório."
                        Display="Dynamic"
                        ValidationGroup="Atribuicao">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvDataInicio" runat="server" ControlToValidate="txtDataInicio"
                        ValidationGroup="Atribuicao" Display="Dynamic"
                        OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                </div>
                <div style="display: inline-table; margin-left: 5px;">
                    <asp:Label ID="Label4" runat="server" Text="Data de fim *" AssociatedControlID="txtDataFim"></asp:Label>
                    <asp:TextBox ID="txtDataFim" runat="server" SkinID="Data"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDataFim"
                        ErrorMessage="Data de fim é obrigatório."
                        Display="Dynamic"
                        ValidationGroup="Atribuicao">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvDataFim" runat="server" ControlToValidate="txtDataFim"
                        ValidationGroup="Atribuicao" Display="Dynamic"
                        OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                    <asp:CompareValidator ID="cvDataFimMaiorAtual" runat="server" ControlToValidate="txtDataFim"
                        ValidationGroup="Atribuicao" Display="Dynamic"
                        Type="Date" Operator="GreaterThanEqual" ErrorMessage="Data de fim deve ser maior ou igual a data atual.">*</asp:CompareValidator>
                </div>
            </div>
        </asp:Panel>
        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar"
                ValidationGroup="Atribuicao" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
                OnClick="btnCancelar_Click" />
        </div>
    </asp:Panel>
</asp:Content>
