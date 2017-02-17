<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" 
Inherits="Configuracao_TipoMacroCampoDisciplina_Busca" %>

<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>  
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Configuracao, TipoMacroCampoDisciplina.Busca.lblLegend.Text  %>"></asp:Label> eletivo(a)</legend>
        <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Configuracao, TipoMacroCampoDisciplina.Busca.btnNovo.Text  %>"
            OnClick="btnNovoTipoMacroCampo_Click" />
        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" ComboDefaultValue="true"
            OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="grvTipoMacroCampoDisciplina" runat="server" DataKeyNames="tea_id"
            DataSourceID="OdsTipoMacroCampo" OnRowDataBound="grvTipoMacroCampoDisciplina_RowDataBound"
            AllowPaging="True" AutoGenerateColumns="False" OnDataBound="grvTipoMacroCampoDisciplina_DataBound"
            EmptyDataText="<%$ Resources:Configuracao, TipoMacroCampoDisciplina.Busca.grvTipoMacroCampoDisciplina.EmptyDataText %>" OnRowCommand="grvTipoMacroCampoDisciplina_RowCommand">
            <Columns>
                <asp:BoundField DataField="tea_nome" HeaderText="<%$ Resources:Configuracao, TipoMacroCampoDisciplina.Busca.grvTipoMacroCampoDisciplina.Coluna1 %>"
                    SortExpression="tea_nome" Visible="false" />
                <asp:TemplateField HeaderText="<%$ Resources:Configuracao, TipoMacroCampoDisciplina.Busca.grvTipoMacroCampoDisciplina.Coluna2 %>">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tea_nome") %>'
                            PostBackUrl="~/Configuracao/TipoMacroCampoDisciplina/Cadastro.aspx" CssClass="wrap600px"></asp:LinkButton>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("tea_nome") %>' CssClass="wrap600px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="tea_sigla" HeaderText="Sigla" SortExpression="tea_sigla">
                </asp:BoundField>
                <asp:TemplateField HeaderText="Excluir">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="OdsTipoMacroCampo" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoMacroCampoEletivaAluno"
            SelectMethod="SelecionaTipoMacroCampo" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoMacroCampoEletivaAlunoBO">
        </asp:ObjectDataSource>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvTipoMacroCampoDisciplina" />
    </fieldset>
</asp:Content>
