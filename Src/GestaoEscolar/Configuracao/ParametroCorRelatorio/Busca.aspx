<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Busca.aspx.cs" Inherits="Configuracao_ParametroCorRelatorio" %>

<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="<%=validationGroup %>" />
    <fieldset>
        <legend>Relatórios</legend>
        <asp:UpdatePanel ID="updCor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:UCLoader ID="UCLoader1" runat="server" />
                <asp:GridView ID="grvCor" runat="server" AutoGenerateColumns="False"
                    EmptyDataText="Não existem cores para relatórios cadastrados."
                    DataKeyNames="rlt_id"
                    OnRowEditing="grvCor_RowEditing">  
                    <Columns>                                               
                        <asp:TemplateField HeaderText="Relatórios para cadastro de cores">                           
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("rlt_nome") %>'
                                PostBackUrl="~/Configuracao/ParametroCorRelatorio/Cadastro.aspx"></asp:LinkButton>
                            </ItemTemplate>                            
                            <HeaderStyle CssClass="Left" Width="300px" />                                                                       
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
