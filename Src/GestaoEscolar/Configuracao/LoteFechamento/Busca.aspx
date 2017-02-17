<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.LoteFechamento.Busca" %>

<%@ Register TagPrefix="uc1" Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCCUAEscola" %>
<%@ Register TagPrefix="uc2" Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" %>
<%@ Register TagPrefix="uc3" Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagName="UCCPeriodoCalendario" %>
<%@ Register tagprefix="uc4" src="~/WebControls/Combos/UCComboDocente.ascx" tagname="UCCDocente" %>
<%@ Register TagPrefix="uc5" Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" %>
<%@ Register TagPrefix="uc6" Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="_updBuscaLoteFechamento" runat="server" UpdateMode="Always">
        <Triggers>
            <asp:PostBackTrigger ControlID="gvFechamento" />
        </Triggers>
        <ContentTemplate>
            <asp:ValidationSummary ID="vsLoteFechamento" runat="server" ValidationGroup='<%=validationGroup %>' />
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:Label ID="lblInformacao" runat="server" EnableViewState="false" Visible="false"></asp:Label>
            <fieldset id="fdsPesquisa" runat="server">
                <legend>
                    <asp:Label id="lbllegend" runat="server" Text="<%$ Resources:Configuracao, LoteFechamento.Busca.lbllegend.Text %>" ></asp:Label> 
                </legend>

                <div id="divPesquisa" runat="server">
                    <uc2:UCCCalendario ID="UCCCalendario" runat="server" Obrigatorio="true" SelecionarAnoCorrente="true"
                        PermiteEditar="true" MostrarMensagemSelecione="true" ValidationGroup='<%=validationGroup %>' />
                    <uc3:UCCPeriodoCalendario ID="UCCPeriodoCalendario" runat="server" MostrarMensagemSelecione="true" 
                        Obrigatorio="true" PermiteEditar="false" ValidationGroup='<%=validationGroup %>' />
                    <uc1:UCCUAEscola ID="UCCUAEscola" runat="server" CarregarEscolaAutomatico="false" PermiteAlterarCombos="false"
                            ObrigatorioEscola="true" ObrigatorioUA="true" MostrarMessageSelecioneEscola="true"
                            MostrarMessageSelecioneUA="true" ValidationGroup='<%=validationGroup %>' />
        <%--                <uc4:UCCDocente ID="UCCDocente" runat="server" _MostrarMessageSelecione="true"
                            Obrigatorio="false" PermiteEditar="false"/>--%>
                    <asp:Label ID="lblCodigoTurma" runat="server" Text="Código da turma" AssociatedControlID="txtCodigoTurma"></asp:Label>
                    <asp:TextBox ID="txtCodigoTurma" runat="server" MaxLength="30" SkinID="text30C"></asp:TextBox>
                </div>
                <div class="right">
                    <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" ValidationGroup='<%=validationGroup %>' />
                    <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click" />
                </div>
            </fieldset>
            <fieldset id="fdsResultado" runat="server" visible="false" class="fdsResultado">
                <legend>Resultados</legend>
                    <uc5:UCComboQtdePaginacao ID="UCCQtdePaginacao" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                    <asp:GridView ID="gvFechamento" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="odsFechamento"
                        EmptyDataText="A pesquisa não encontrou resultados." DataKeyNames="esc_id, uni_id, cal_id, tpc_id, tur_id"
                        OnRowCommand="gvFechamento_RowCommand" OnDataBound="gvFechamento_DataBound" AllowSorting="True">
                        <Columns>
                            <asp:BoundField DataField="tur_codigo" HeaderText="Código da turma" SortExpression="tur_codigo" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="DtExportacao" HeaderText="Data de exportação" SortExpression="DtExportacao">
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DtImportacao" HeaderText="Data de importação" SortExpression="DtImportacao" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Exportar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="center" ItemStyle-CssClass="icon-align-center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="_ibtExportar" SkinID="btExportarCSV" CausesValidation="false" runat="server" CommandName="Exportar" ToolTip="Exportar"  CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" CssClass="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtImportar" CausesValidation="false" runat="server" ToolTip="Importar" CommandName="Importar" SkinID="btConvocar"
                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" PostBackUrl="~/Configuracao/LoteFechamento/Cadastro.aspx" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <uc6:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="gvFechamento" />                   
            </fieldset>
            <asp:ObjectDataSource ID="odsFechamento" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.CLS_ArquivoEfetivacao"
                 SelectMethod="SelectBy_Filtros" TypeName="MSTech.GestaoEscolar.BLL.CLS_ArquivoEfetivacaoBO"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divDownload" title="Download do arquivo de fechamento" class="hide">
        <asp:Label ID="lblMsgDownload" runat="server" EnableViewState="False"></asp:Label>
        <asp:Label ID="lblConfirmação" runat="server" Text="Deseja realizar o download do arquivo?" AssociatedControlID="btnDownload"></asp:Label>
        <br />
        <div class="right">
            <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" OnClientClick="$('#divDownload').dialog('close');" />
            <asp:Button ID="btnFechar" runat="server" Text="Fechar" OnClientClick="$('#divDownload').dialog('close'); return false;" />
        </div>
    </div>
</asp:Content>
