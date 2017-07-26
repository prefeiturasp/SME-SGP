<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.ConfiguracaoServicoPendencia.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoModalidadeEnsino.ascx" TagName="UCComboTipoModalidadeEnsino"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoTurma.ascx" TagName="UCComboTipoTurma"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsPesquisa" runat="server">
        <legend>
            <asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.lblLegend.Text %>" /></legend>
        <div id="divPesquisa" runat="server">
            <uc6:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
            <uc1:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino" runat="server" />
            <uc2:UCComboTipoModalidadeEnsino ID="UCComboTipoModalidadeEnsino" runat="server" />
            <uc3:UCComboTipoTurma ID="UCComboTipoTurma" runat="server" />
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.btnPesquisar.Text %>" OnClick="btnPesquisar_Click" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.btnLimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click" />
            <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.btnIncluirNovo.Text %>" OnClick="btnNovo_Click" />
        </div>
    </fieldset>
    <fieldset id="fdsResultado" runat="server" visible="false">
        <legend>Resultados</legend>
        <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged"
            ComboDefaultValue="true" />
        <asp:GridView ID="grvConfigServPendencia" runat="server" AutoGenerateColumns="False" DataSourceID="odsConfigServPendencia"
            DataKeyNames="csp_id, tne_id, tme_id, tur_tipo, csp_semNota, csp_semParecer, csp_disciplinaSemAula, csp_semResultadoFinal, csp_semPlanejamento, csp_semSintese, csp_semPlanoAula, csp_semRelatorioAtendimento, csp_semObjetoConhecimento"
            AllowPaging="True" EmptyDataText="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.grvConfigServPendencia.EmptyDataText %>" AllowSorting="True"
            OnDataBound="grvConfigServPendencia_DataBound" OnRowDataBound="grvConfigServPendencia_RowDataBound" OnRowCommand="grvConfigServPendencia_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.ColunaTipoNivelEnsino %>" DataField="tne_nome" SortExpression="tne_nome" />
                <asp:BoundField HeaderText="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.ColunaTipoModalidadeEnsino %>" DataField="tme_nome" SortExpression="tme_nome" />
                <asp:BoundField HeaderText="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.ColunaTipoTurma %>" DataField="tur_tipoNome" SortExpression="tur_tipoNome" />
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.ColunaPendencias %>"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPendencias" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <asp:Label runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.btnEditar.Tooltip %>"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEditar" runat="server" ToolTip="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.btnEditar.Tooltip %>"
                            CommandName="Edit" SkinID="btEditar" Style="display: inline-block; vertical-align: middle;" Visible="false"
                            PostBackUrl="~/Academico/ConfiguracaoServicoPendencia/Cadastro.aspx" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle CssClass="center" HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <asp:Label runat="server" Text="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.btnExcluir.Text %>"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" runat="server" ToolTip="<%$ Resources:Academico, ConfiguracaoServicoPendencia.Busca.btnExcluir.Text %>"
                            SkinID="btExcluir" Style="display: inline-block; vertical-align: middle;" Visible="false"
                            CommandName="Deletar"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc5:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="grvConfigServPendencia" />
        <asp:ObjectDataSource ID="odsConfigServPendencia" runat="server" TypeName="MSTech.GestaoEscolar.BLL.ACA_ConfiguracaoServicoPendenciaBO"
            SelectMethod="SelectBy_tne_id_tme_id_tur_tipo" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
            StartRowIndexParameterName="currentPage" EnablePaging="true" OnSelecting="odsConfigServPendencia_Selecting"></asp:ObjectDataSource>
    </fieldset>

</asp:Content>
