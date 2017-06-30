<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.Alertas.Busca" %>

<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend><asp:Literal ID="litTitulo" runat="server" Text='<%$ Resources:GestaoEscolar.Configuracao.Alertas.Busca, litTitulo.Text %>'></asp:Literal></legend>
        <asp:GridView ID="grvAlertas" runat="server" AllowPaging ="false" AllowSorting="false" DataKeyNames="cfa_id"
            AutoGenerateColumns="false" DataSourceID="odsAlertas" EmptyDataText='<%$ Resources:Padrao, Padrao.SemResultado.Text %>'
            OnRowDataBound="grvAlertas_RowDataBound" OnDataBound="grvAlertas_DataBound">
            <Columns>
                <asp:TemplateField HeaderText='<%$ Resources:GestaoEscolar.Configuracao.Alertas.Busca, grvAlertas.ColunaNome %>' SortExpression="cfa_nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("cfa_nome") %>'
                            PostBackUrl="../Alertas/Cadastro.aspx"></asp:LinkButton>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("cfa_nome") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvAlertas" />
    </fieldset>
    <asp:ObjectDataSource ID="odsAlertas" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.CFG_Alerta"
        SelectMethod="SelecionarAlertas" TypeName="MSTech.GestaoEscolar.BLL.CFG_AlertaBO"></asp:ObjectDataSource>
</asp:Content>
