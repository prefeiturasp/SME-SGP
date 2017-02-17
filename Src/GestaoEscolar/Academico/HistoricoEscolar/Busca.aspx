<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.HistoricoEscolar.Busca" %>

<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="UCComboUAEscola"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurma.ascx" TagName="UCComboTurma"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/BuscaAluno/UCCamposBuscaAluno.ascx" TagName="UCCamposBuscaAluno"
    TagPrefix="uc6" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCurriculoPeriodo.ascx" TagName="UCComboCurriculoPeriodo"
    TagPrefix="uc7" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc8" %>
<%@ Register Src="~/WebControls/Busca/UCAluno.ascx" TagName="UCAluno" TagPrefix="uc9" %>
<%@ Register Src="~/WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="HistoricoEscolar" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="uppPesquisa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <fieldset id="fdsPesquisa" runat="server" style="margin-left: 10px;">
                    <legend>Parâmetros de busca</legend>
                    <uc10:UCCamposObrigatorios ID="UCCamposObrigatorios" runat="server" Visible="false" />
                    <div id="_divPesquisa" class="divPesquisa" runat="server">
                        <asp:Label ID="lblAvisoMensagem" runat="server"></asp:Label>
                        <!-- FiltrosPadrao -->
                        <uc3:UCComboUAEscola ID="UCComboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                            MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" OnIndexChangedUA="UCComboUAEscola_IndexChangedUA"
                            OnIndexChangedUnidadeEscola="UCComboUAEscola_IndexChangedUnidadeEscola"
                            ObrigatorioEscola="true" ObrigatorioUA="true" ValidationGroup="HistoricoEscolar" />
                        <uc2:UCComboCursoCurriculo ID="UCComboCursoCurriculo1" MostrarMensagemSelecione="true" runat="server"
                            Obrigatorio="true" ValidationGroup="HistoricoEscolar" />
                        <uc7:UCComboCurriculoPeriodo ID="UCComboCurriculoPeriodo1" MostrarMensagemSelecione="true" runat="server" 
                            Obrigatorio="true" ValidationGroup="HistoricoEscolar" />
                        <uc5:UCComboCalendario ID="UCComboCalendario1" runat="server" MostrarMensagemSelecione="true"
                            Obrigatorio="true" ValidationGroup="HistoricoEscolar" />
                        <uc4:UCComboTurma ID="UCComboTurma1" runat="server" MostrarMessageSelecione="true" 
                            Obrigatorio="true" ValidationGroup="HistoricoEscolar" />
                        <asp:CheckBox runat="server" ID="chkBuscaAvancada" Text="Busca avançada" OnCheckedChanged="chkBuscaAvancada_CheckedChanged" AutoPostBack="true" />
                        <div id="divBuscaAvancadaAluno" runat="server" class="divBuscaAvancadaAluno" visible="false">
                            <uc6:UCCamposBuscaAluno ID="UCCamposBuscaAluno1" runat="server" VisibleDataNascimento="false" VisibleMatriculaEstadual="false"
                                VisibleNomeMae="false" />
                        </div>
                    </div>
                    <div class="right">
                        <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" ValidationGroup="HistoricoEscolar" CausesValidation="true" />
                    </div>
                </fieldset>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="_btnPesquisar" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="clear">
    </div>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <div id="DivSelecionaTodos" runat="server">
            <div align="right" id="divQtdPaginacao" runat="server">
                <asp:Label ID="_lblPag" runat="server" Text="Itens por página"></asp:Label>
                <asp:DropDownList ID="_ddlQtPaginado" runat="server" AutoPostBack="True" OnSelectedIndexChanged="_ddlQtPaginado_SelectedIndexChanged">
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <asp:GridView ID="_grvDocumentoAluno" runat="server" AllowPaging="True" AllowCustomPaging="true" AutoGenerateColumns="False"
            BorderStyle="None" DataKeyNames="alu_id,tur_id,cal_id,esc_id,mtu_id,EscolaUniDestino,GrupamentoDestino,pes_nome,tur_escolaUnidade"
            EmptyDataText="A pesquisa não encontrou resultados."
            OnRowDataBound="_grvDocumentoAluno_RowDataBound" OnDataBound="_grvDocumentoAluno_DataBound"
            AllowSorting="True" OnPageIndexChanging="grvDocumentoAluno_PageIndexChanging"
            OnRowCommand="_grvDocumentoAluno_RowCommand" OnSorting="_grvDocumentoAluno_Sorting"
            EnableModelValidation="True">
            <Columns>
                <asp:BoundField HeaderText="Número de chamada" DataField="numChamada" />
                <asp:TemplateField HeaderText="Nome" SortExpression="pes_nome">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap150px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap150px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Data de nascimento" DataField="dataNascimento" />
                <asp:TemplateField HeaderText="Editar informações" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" SkinID="btEditar" ID="btnEditar" CommandArgument='<%# Bind("alu_id") %>' CommandName="Editar" CssClass="wrap100px"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Histórico escolar" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" SkinID="btRelatorio" ID="btnRelatorio" CommandArgument='<%# Bind("alu_id") %>' CommandName="HistoricoEscolar" CssClass="wrap100px"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc8:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvDocumentoAluno"
            class="clTotalReg" />
    </fieldset>
</asp:Content>
