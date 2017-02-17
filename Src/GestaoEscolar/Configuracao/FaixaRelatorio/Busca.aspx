<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Busca.aspx.cs" Inherits="Configuracao_ParametroFaixaRelatorio" %>

<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="<%=validationGroup %>" />

    <fieldset>
        <legend>Relatórios por faixa</legend>
        <asp:UpdatePanel ID="updFaixaRelatorio" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:UCLoader ID="UCLoader1" runat="server" />
                <asp:GridView ID="grvRelatorioFaixa" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="Não existem faixa por relatórios cadastrados."
                    DataKeyNames="rlt_id"
                    OnRowEditing="grvRelatorioFaixa_RowEditing">  
                    <Columns>                                               
                        <asp:TemplateField HeaderText="Relatórios para cadastro de faixas por relatório.">                           
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("rlt_nome") %>'
                                PostBackUrl="~/Configuracao/FaixaRelatorio/Cadastro.aspx"></asp:LinkButton>
                            </ItemTemplate>                            
                            <HeaderStyle CssClass="Left" Width="300px" />                                                                       
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
