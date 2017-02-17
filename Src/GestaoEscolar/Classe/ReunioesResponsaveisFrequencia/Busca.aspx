<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Busca.aspx.cs" Inherits="Classe_ReunioesResponsaveisFrequencia_Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="_UCComboCalendario"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="_UCComboCursoCurriculo"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/UCComboTurno.ascx" TagName="_UCComboTurno"
    TagPrefix="uc4" %>
<%@ Register Src="../../WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas"
    TagPrefix="uc6" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
        <fieldset id="fdsPesquisa" runat="server">
            <legend>Consulta de lançamento de frequências em reuniões de responsáveis</legend>
            <div id="_divPesquisa" runat="server">
                <asp:UpdatePanel ID="_updBuscaFrequencia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc6:UCFiltroEscolas ID="UCFiltroEscolas1" runat="server" />
                        <uc3:_UCComboCursoCurriculo ID="_UCComboCursoCurriculo" runat="server" />
                        <uc2:_UCComboCalendario ID="_UCComboCalendario" runat="server" />
                        <uc4:_UCComboTurno ID="_UCComboTurno" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Label ID="_lblCodigoTurma" runat="server" Text="Código da turma" AssociatedControlID="_txtCodigoTurma"></asp:Label>
                <asp:TextBox ID="_txtCodigoTurma" runat="server" MaxLength="30" SkinID="text30C"></asp:TextBox>
            </div>
            <div class="right">
                <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
                <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
            </div>
        </fieldset>
        <fieldset id="fdsResultado" runat="server">
            <legend id="ldgResultado" runat="server">Resultados</legend><legend id="ldgResultadoListagem"
                runat="server">Listagem de lançamento de frequência</legend>
            <uc8:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <asp:GridView ID="_dgvTurma" runat="server" AutoGenerateColumns="False" DataSourceID="_odsTurma"
                DataKeyNames="tur_id,fav_id,cal_id" AllowPaging="True" OnRowDataBound="_dgvTurma_RowDataBound"
                EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="_dgvTurma_RowCommand"
                OnDataBound="_dgvTurma_DataBound" AllowSorting="true">
                <Columns>
                    <asp:TemplateField HeaderText="Código da turma" SortExpression="tur_codigo">
                        <ItemTemplate>
                            <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Selecionar" Text='<%# Bind("tur_codigo") %>'
                                CssClass="wrap100px"></asp:LinkButton>
                            <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("tur_codigo") %>' CssClass="wrap100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Escola" SortExpression="tur_escolaUnidade">
                        <ItemTemplate>
                            <asp:Label ID="lblEscola" runat="server" Text='<%# Bind("tur_escolaUnidade") %>'
                                CssClass="wrap200px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Calendário" SortExpression="tur_calendario">
                        <ItemTemplate>
                            <asp:Label ID="_lblCalendario" runat="server" Text='<%# Bind("tur_calendario") %>'
                                CssClass="wrap100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Curso" SortExpression="tur_curso">
                        <ItemTemplate>
                            <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("tur_curso") %>' CssClass="wrap200px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Turno" SortExpression="tur_turno">
                        <ItemTemplate>
                            <asp:Label ID="lblTurno" runat="server" Text='<%# Bind("tur_turno") %>' CssClass="wrap100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <uc7:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvTurma" />
        </fieldset>
    </div>
    <asp:ObjectDataSource ID="_odsTurma" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.TUR_Turma"
        TypeName="MSTech.GestaoEscolar.BLL.TUR_TurmaBO" SelectMethod="GetSelectBy_Pesquisa_TodosTipos">
    </asp:ObjectDataSource>
    <div id="divAvaliacoes" title="Selecionar avaliação" class="hide">
        <asp:GridView ID="gvAvaliacoes" runat="server" AutoGenerateColumns="false" OnRowCommand="gv_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbSelecionar" runat="server" CommandName="SelecionarAvaliacao"
                            Text='<%#Bind("cap_descricao") %>' CommandArgument='<%#Bind("cap_id") %>' ToolTip="Selecionar avaliação">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
