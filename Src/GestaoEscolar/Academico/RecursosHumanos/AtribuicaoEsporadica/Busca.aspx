<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.RecursosHumanos.AtribuicaoEsporadica.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMensagem" runat="server" Text="" EnableViewState="false"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Atribuicao" />
    <asp:Panel ID="pnlBusca" runat="server" GroupingText="Atribuição esporádica de docentes">
        <asp:UpdatePanel ID="upnEscola" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCComboUAEscola ID="UCComboUAEscola1" runat="server" ObrigatorioUA="true" ObrigatorioEscola="true"
                    CarregarEscolaAutomatico="true" ValidationGroup="Atribuicao"
                    MostrarMessageSelecioneUA="true" MostrarMessageSelecioneEscola="true" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" ValidationGroup="Atribuicao"
                OnClick="btnPesquisar_Click" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" CausesValidation="false"
                OnClick="btnLimparPesquisa_Click" />
            <asp:Button ID="btnIncluir" runat="server" Text="Incluir nova atribuição"
                CausesValidation="false" OnClick="btnIncluir_Click" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlResultados" runat="server" GroupingText="Resultados" Visible="false">
        <uc5:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao1_IndexChanged" />
        <asp:GridView ID="grvResultados" runat="server" AutoGenerateColumns="False"
            DataSourceID="odsResultado"
            AllowPaging="True" DataKeyNames="col_id,crg_id,coc_id,pes_nome,usu_id,doc_id,esc_id"
            EmptyDataText="A pesquisa não encontrou resultados." HorizontalAlign="Center"
            OnRowEditing="grvResultados_RowEditing" OnRowCommand="grvResultados_RowCommand"
            OnRowDataBound="grvResultados_RowDataBound"
            OnSorted="grvResultados_Sorted" OnDataBound="grvResultados_DataBound" AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderText="Nome do docente" SortExpression="pes_nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("pes_nome") %>'
                            PostBackUrl="Cadastro.aspx" CssClass="wrap250px"></asp:LinkButton>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap250px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="coc_matricula" HeaderText="RF" SortExpression="coc_matricula" />
                <asp:BoundField DataField="Vigencia" HeaderText="Vigência" SortExpression="coc_vigenciaInicio" />
                <asp:TemplateField HeaderText="Excluir">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <asp:ObjectDataSource ID="odsResultado" runat="server" SelectMethod="PesquisaAtribuicoesEsporadicas_PorFiltros"
            TypeName="MSTech.GestaoEscolar.BLL.RHU_ColaboradorBO" OnSelecting="odsResultado_Selecting"></asp:ObjectDataSource>
        <uc2:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="grvResultados" />
    </asp:Panel>
</asp:Content>
