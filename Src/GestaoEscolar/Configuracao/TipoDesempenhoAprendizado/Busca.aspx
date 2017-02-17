<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Configuracao.TipoDesempenhoAprendizado.Busca" %>

<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao" TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCCCursoCurriculo" TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCCCurriculoPeriodo" TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCCCalendario" TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoDisciplina.ascx" TagName="UCComboTipoDisciplina" TagPrefix="uc6" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="true"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="TipoAprendizando" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updPesquisa" runat="server">
        <ContentTemplate>
            <fieldset id="fdsTipoAprendizando" runat="server">
                <legend><asp:Label runat="server" ID="lblLegend" Text="<%$ Resources:Configuracao, TipoDesempenhoAprendizado.Busca.lblLegend.Text  %>    "></asp:Label></legend>
                <div id="divPesquisa" runat="server">
                    <uc5:UCCCalendario runat="server" ID="UCCCalendario" MostrarMensagemSelecione="true" PermiteEditar="true" Obrigatorio="true" ValidationGroup="TipoAprendizando" />
                    <uc3:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" MostrarMensagemSelecione="true" PermiteEditar="false" Obrigatorio="true" ValidationGroup="TipoAprendizando" />
                    <uc4:UCCCurriculoPeriodo runat="server" ID="UCCCurriculoPeriodo" MostrarMensagemSelecione="true" PermiteEditar="false" Obrigatorio="false" ValidationGroup="TipoAprendizando" />
                    <uc6:UCComboTipoDisciplina runat="server" ID="UCComboTipoDisciplina" MostrarMensagemSelecione="false" PermiteEditar="false" Obrigatorio="false" ValidationGroup="TipoAprendizando" />
                </div>
                <div class="right">
                    <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click"
                        ValidationGroup="TipoAprendizando" />
                    <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click"
                        CausesValidation="False" />
                    <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Configuracao, TipoDesempenhoAprendizado.Busca.btnNovo.Text %>" OnClick="_btnNovo_Click"
                        CausesValidation="False" />
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updResultado" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <fieldset id="fdsResultados" runat="server" visible="false">
                <legend>Resultados</legend>
                <uc2:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
                <asp:GridView ID="grvTipoAprendizando" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    BorderStyle="None" DataKeyNames="tda_id" DataSourceID="odsTipoAprendizando"
                    EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="_grvTipoAprendizando_RowCommand"
                    OnRowDataBound="_grvTipoAprendizando_RowDataBound" OnDataBound="_grvTipoAprendizando_DataBound" AllowSorting="True" EnableModelValidation="True">
                    <Columns>                        
                        <asp:TemplateField HeaderText="Descrição" SortExpression="tda_descricao">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" PostBackUrl="Cadastro.aspx"
                                    CausesValidation="False" Text='<%# Bind("tda_descricao") %>' CssClass="wrap400px"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("tda_descricao") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Grupamento de ensino" DataField="crp_descricao" SortExpression="crp_descricao" />
                        <asp:BoundField HeaderText="<%$ Resources:Mensagens, MSG_DISCIPLINA %>" DataField="tds_nome" SortExpression="tds_nome" />
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                                    CausesValidation="False" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle CssClass="center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvEscolas" />
                <asp:ObjectDataSource ID="odsTipoAprendizando" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="SELECT_By_Pesquisa" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoDesempenhoAprendizadoBO"></asp:ObjectDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
