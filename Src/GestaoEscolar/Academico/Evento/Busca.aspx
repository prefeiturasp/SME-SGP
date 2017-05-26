<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Eventos_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboTipoEvento.ascx" TagName="_UCComboTipoEvento"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="ComboUAEscola"
    TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="_updDadosBasicos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <fieldset id="fdsEventosCalendario" runat="server">
                <legend>Consulta de eventos do calendário</legend>
                <div id="_divPesquisa" runat="server">
                    <uc6:ComboUAEscola ID="ucComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                        ObrigatorioEscola="false" ObrigatorioUA="false" MostrarMessageSelecioneEscola="true"
                        MostrarMessageSelecioneUA="true" FiltroEscolasControladas="true"/>
                    <asp:CheckBox ID="chkPadrao" runat="server" Text="<%$ Resources:Academico, Evento.Busca.chkEventoPadrao.Text %>" 
                        AutoPostBack="True" OnCheckedChanged="chkPadrao_CheckedChanged" />
                    <uc2:_UCComboTipoEvento ID="_UCComboTipoEvento" runat="server" />
                    <asp:Label ID="_lblNome" runat="server" Text="Nome do evento do calendário" AssociatedControlID="_txtNome"></asp:Label>
                    <asp:TextBox ID="_txtNome" runat="server" SkinID="text60C"></asp:TextBox>
                    <uc4:UCComboCalendario ID="_UCComboCalendario" runat="server" />
                </div>
                <div class="right">
                    <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
                    <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
                    <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo evento" OnClick="_btnNovo_Click" />
                </div>
            </fieldset>
            <fieldset id="fdsResultados" runat="server">
                <legend>Resultados</legend>
                <uc5:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                <asp:GridView ID="_dgvEventos" runat="server" DataSourceID="_odsEvento" AutoGenerateColumns="False"
                    AllowPaging="True" DataKeyNames="evt_id, evt_dataInicio, evt_padrao, editar"
                    EmptyDataText="A pesquisa não encontrou resultados." HorizontalAlign="Center"
                    OnRowCommand="_dgvEventos_RowCommand" OnRowDataBound="_dgvEventos_RowDataBound"
                    OnDataBound="_dgvEventos_DataBound" AllowSorting="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Nome do evento do calendário" SortExpression="evt_nome">
                            <ItemTemplate>
                                <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("evt_nome") %>'
                                    PostBackUrl="../Evento/Cadastro.aspx" CssClass="wrap250px"></asp:LinkButton>
                                <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("evt_nome") %>' CssClass="wrap250px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="tev_nome" HeaderText="Tipo de evento" SortExpression="tev_id" />
                        <asp:BoundField DataField="evt_data" HeaderText="Período do evento" SortExpression="evt_dataInicio">
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="evt_semAtividadeDiscente" HeaderText="Sem atividades discentes"
                            SortExpression="evt_semAtividadeDiscente">
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="evt_padrao" HeaderText="<%$ Resources:Academico, Evento.Busca._dgvEventos.EventoPadrao.HeaderText %>" SortExpression="evt_padrao">
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="uni_escolaNome" HeaderText="Escola" SortExpression="uni_escolaNome"></asp:BoundField>
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:GridView>
                <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvEventos" />
            </fieldset>
            <asp:ObjectDataSource ID="_odsEvento" runat="server" SelectMethod="GetSelect_Busca"
                DeleteMethod="Delete" TypeName="MSTech.GestaoEscolar.BLL.ACA_EventoBO" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Evento"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
