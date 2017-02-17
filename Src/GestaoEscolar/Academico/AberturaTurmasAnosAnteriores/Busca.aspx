<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.AberturaTurmasAnosAnteriores.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="ComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset class="msgInfo">
        <legend>Consulta de agendamentos de abertura de anos letivos anteriores</legend>
        <div id="divPesquisa" runat="server">
            <asp:UpdatePanel ID="updPesquisa" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblAno" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.lblAno.Text %>" AssociatedControlID="txtAno"></asp:Label>
                    <asp:TextBox ID="txtAno" runat="server" SkinID="Numerico" MaxLength="4"></asp:TextBox>
                    <uc1:ComboUAEscola ID="ucComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                        ObrigatorioEscola="false" ObrigatorioUA="false" MostrarMessageSelecioneEscola="true"
                        MostrarMessageSelecioneUA="true" />
                    <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.lblStatus.Text %>" AssociatedControlID="ddlStatus" EnableViewState="False"></asp:Label>
                    <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="True" SkinID="text30C">
                    </asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.btnPesquisar.Text %>"
                ToolTip="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.btnPesquisar.ToolTip %>" OnClick="btnPesquisar_Click" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.btnLimparPesquisa.Text %>"
                ToolTip="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.btnLimparPesquisa.ToolTip %>" OnClick="btnLimparPesquisa_Click" />
            <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.btnNovo.Text %>"
                ToolTip="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.btnNovo.ToolTip %>" OnClick="btnNovo_Click" />
        </div>
    </fieldset>
    <!-- Resultado -->
    <fieldset id="fdsResultado" runat="server" visible="false">
        <legend>Resultados</legend>
        <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged"
            ComboDefaultValue="true" />
        <asp:GridView ID="gdvAberturaAnosAnteriores" runat="server" AutoGenerateColumns="False" AllowCustomPaging="true"
            DataKeyNames="tab_id, tab_status, tab_dataFim" AllowPaging="True" EmptyDataText="A pesquisa não encontrou resultados." DataSourceID="odsAberturaAnosAnteriores"
            OnRowCommand="gdvAberturaAnosAnteriores_RowCommand" OnRowDataBound="gdvAberturaAnosAnteriores_RowDataBound" OnDataBound="gdvAberturaAnosAnteriores_DataBound"
            AllowSorting="True" >
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.Ano.HeaderText %>" SortExpression="tab_ano">
                    <ItemTemplate>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("tab_ano") %>' CssClass="wrap400px"></asp:Label>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tab_ano") %>'
                            PostBackUrl="~/Academico/AberturaTurmasAnosAnteriores/Cadastro.aspx" CssClass="wrap400px"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="uad_nomeSuperior" HeaderText="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.DRE.HeaderText %>" SortExpression="uad_nomeSuperior"></asp:BoundField>
                <asp:BoundField DataField="esc_nome" HeaderText="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.Escola.HeaderText %>" SortExpression="esc_nome" />
                <asp:BoundField DataField="tab_dataInicio" HeaderText="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.DataInicial.HeaderText %>" SortExpression="tab_dataInicio" DataFormatString="{0:dd/MM/yyy}">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="tab_dataFim" HeaderText="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.DataFinal.HeaderText %>" SortExpression="tab_dataFim" DataFormatString="{0:dd/MM/yyy}">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="tab_statusTexto" HeaderText="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.Status.HeaderText %>" SortExpression="tab_statusTexto">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="<%$ Resources:Academico, AberturaTurmasAnosAnteriores.Busca.gdvAberturaAnosAnteriores.Excluir.HeaderText %>">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <asp:ObjectDataSource ID="odsAberturaAnosAnteriores" runat="server" SelectMethod="SelecionaAberturasAnosLetivosBy_AnoDreEscStatus"
            TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaAberturaAnosAnterioresBO" EnablePaging="True" MaximumRowsParameterName="pageSize"
            SelectCountMethod="GetTotalRecords" OnSelecting="odsAberturaAnosAnteriores_Selecting"
            StartRowIndexParameterName="currentPage"></asp:ObjectDataSource>
        <uc3:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="gdvAberturaAnosAnteriores" />
    </fieldset>
</asp:Content>
