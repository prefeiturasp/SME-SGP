<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cadastro.aspx.cs" Inherits="GestaoEscolar.Configuracao.Recomendacoes.Cadastro" %>

<%@ PreviousPageType VirtualPath="~/Configuracao/Recomendacoes/Busca.aspx" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Recomendacao" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updCadastroRecomendacao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMessageCadastroRecomendacao" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="vlsHistoricoRecomendacao" runat="server" ValidationGroup="CadastroRecomendacao"
                EnableViewState="False" />
            <asp:Panel ID="pnlCadastroRecomendacao" runat="server" DefaultButton="btnIncluirCadastroRecomendacao">
                <fieldset>
                    <legend>Cadastro de recomendação a alunos e responsáveis</legend>
                    <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios4" runat="server" />
                    <asp:Label ID="lblCadastroRecomendacao" runat="server" Text="Texto da recomendação *"
                        AssociatedControlID="txtDescricao"></asp:Label>
                    <asp:TextBox ID="txtDescricao" runat="server" TextMode="MultiLine" SkinID="limite4000"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCadastroRecomendacao" runat="server" ControlToValidate="txtDescricao"
                        Display="Dynamic" ValidationGroup="Recomendacao" ErrorMessage="Texto da recomendação é obrigatório.">*</asp:RequiredFieldValidator>
                    <asp:Label ID="lblDestino" runat="server" Text="A quem se destina *"
                        AssociatedControlID="rblDestino"></asp:Label>
                    <asp:RadioButtonList ID="rblDestino" runat="server">
                        <asp:ListItem Selected="True" Value="1">Aluno</asp:ListItem>
                        <asp:ListItem Value="2">Pais/Responsável</asp:ListItem>
                        <asp:ListItem Value="3">Ambos</asp:ListItem>
                    </asp:RadioButtonList>
                    <div class="right">
                        <asp:Button ID="btnIncluirCadastroRecomendacao" runat="server" Text="Salvar" OnClick="btnIncluirCadastroRecomendacao_Click"
                            ValidationGroup="Recomendacao" />
                        <asp:Button ID="btnCancelarCadastroRecomendacao" runat="server" Text="Cancelar" CausesValidation="False"
                            OnClick="btnCancelarCadastroRecomendacao_Click" />
                    </div>
                </fieldset>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
