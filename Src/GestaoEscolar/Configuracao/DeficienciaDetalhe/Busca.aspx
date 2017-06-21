<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.DeficienciaDetalhe.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset>
        <legend><asp:Label runat="server" ID="lblLegend" Text="Listagem de detalhamento de deficiência"></asp:Label></legend>
        <asp:Button ID="_btnNovoDetalhamento" runat="server" Text="Cadastrar detalhamento"
            OnClick="_btnNovoDetalhamento_Click" />
        <asp:GridView ID="_dgvDeficienciaDetalhe" runat="server" AutoGenerateColumns="False"
            DataKeyNames="tde_id" DataSourceID="odsDeficienciaDetalhe" EmptyDataText="Nenhum detalhamento de deficiência."
            OnDataBound="_dgvDeficienciaDetalhe_DataBound" OnRowDataBound="_dgvDeficienciaDetalhe_RowDataBound" >
            <Columns>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">        
                    <HeaderTemplate>
                        <div style="text-align:left;">
                            <asp:Label runat="server" Text="Deficiência"></asp:Label>
                        </div>
                    </HeaderTemplate>            
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tde_nome") %>'
                            PostBackUrl="~/Configuracao/DeficienciaDetalhe/Cadastro.aspx"></asp:LinkButton>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("tde_nome") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc4:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvAreaConhecimento" />
        <asp:ObjectDataSource ID="odsDeficienciaDetalhe" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.CFG_DetalheDeficiencia"
            SelectMethod="SelecionaAtivos" TypeName="MSTech.GestaoEscolar.BLL.CFG_DeficienciaDetalheBO"
            OnSelecting="odsDeficienciaDetalhe_Selecting" SelectCountMethod="GetTotalRecords">
        </asp:ObjectDataSource>
    </fieldset>
</asp:Content>
