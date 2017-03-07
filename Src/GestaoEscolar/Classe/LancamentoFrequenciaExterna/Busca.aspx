<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Classe.LancamentoFrequenciaExterna.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCCTurma" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsValidacao" runat="server" EnableViewState="false" ValidationGroup="Busca" />
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlPesquisa" runat="server" GroupingText="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Busca, pnlPesquisa.Text %>">
        <uc1:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" />
        <div id="divPesquisa">
            <asp:UpdatePanel ID="updPesquisa" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:CheckBox ID="chkTurmaExtinta" runat="server" Text="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Busca, chkTurmaExtinta.Text %>" AutoPostBack="true" OnCheckedChanged="chkTurmaExtinta_CheckedChanged" />
                    <uc2:UCFiltroEscolas ID="UCFiltroEscolas" runat="server" EscolaCampoObrigatorio="true" UnidadeAdministrativaCampoObrigatorio="true" />
                    <uc3:UCCCalendario ID="UCCCalendario" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" ValidationGroup="Busca" />
                    <uc4:UCCTurma ID="UCCTurma" runat="server" Obrigatorio="true" MostrarMensagemSelecione="true" ValidationGroup="Busca" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="right area-form">
            <asp:Button ID="btnPesquisar" runat="server" Text="<%$ Resources:Padrao, Padrao.Pesquisar.Text %>" OnClick="btnPesquisar_Click" ValidationGroup="Busca" />
            <asp:Button ID="btnLimparPesquisa" runat="server" CausesValidation="false" Text="<%$ Resources:Padrao, Padrao.LimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click" />
        </div>
    </asp:Panel>

    <asp:UpdatePanel ID="updResultado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="area-form">
                <asp:Panel ID="pnlResultados" runat="server" GroupingText="<%$ Resources:Padrao, Padrao.Resultados.Text %>" Visible="false">
                    <uc5:UCComboQtdePaginacao ID="UCComboQtdePaginacao" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                    <asp:GridView ID="grvResultado" runat="server" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:Padrao, Padrao.SemResultado.Text %>" DataKeyNames="alu_id, mtu_id"
                        AllowSorting="true" AllowPaging="true" OnDataBound="grvResultado_DataBound" OnRowEditing="grvResultado_RowEditing" OnPageIndexChanging="grvResultado_PageIndexChanging"
                        OnSorting="grvResultado_Sorting" SkinID="GridResponsive">
                        <Columns>
                            <asp:BoundField HeaderText="<%$ Resources:GestaoEscolar.Classe.LancamentoFrequenciaExterna.Busca, ctrl_45.HeaderText %>" DataField="mtu_numeroChamada" SortExpression="mtu_numeroChamada" />
                            <asp:TemplateField HeaderText="<%$ Resources:Padrao, Padrao.Nome.Text %>" SortExpression="pes_nome">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbNome" runat="server" Text='<%# Bind("pes_nome") %>' CommandName="Edit" PostBackUrl="~/Classe/LancamentoFrequenciaExterna/Cadastro.aspx"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="grid-responsive-item-inline grid-responsive-no-header" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="pes_dataNascimento" HeaderText="<%$ Resources:Padrao, Padrao.DataNascimento.Text %>" DataFormatString="{0:dd/MM/yyy}"
                                SortExpression="pes_dataNascimento">
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:Padrao, Padrao.NomeMae.Text %>" DataField="pes_nomeMae" SortExpression="pes_nomeMae" />
                            <asp:BoundField HeaderText="<%$ Resources:Mensagens, MSG_NUMEROMATRICULA %>" DataField="alc_matricula" SortExpression="alc_matricula" />
                            <asp:BoundField DataField="mtu_dataMatricula" HeaderText="<%$ Resources:Padrao, Padrao.DataMatricula.Text %>" DataFormatString="{0:dd/MM/yyy}"
                                SortExpression="mtu_dataMatricula">
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <uc6:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="grvResultado" />
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
