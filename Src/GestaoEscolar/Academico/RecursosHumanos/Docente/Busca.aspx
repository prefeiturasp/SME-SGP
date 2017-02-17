<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_RecursosHumanos_Docente_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="../../../WebControls/Combos/UCComboCargo.ascx" TagName="UCComboCargo"
    TagPrefix="uc2" %>
<%@ Register Src="../../../WebControls/Combos/UCComboFuncao.ascx" TagName="UCComboFuncao"
    TagPrefix="uc4" %>
<%@ Register Src="../../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc5" %>
<%@ Register Src="../../../WebControls/Busca/UCUA.ascx" TagName="UCUA" TagPrefix="uc3" %>
<%@ Register Src="../../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader"
    TagPrefix="uc6" %>
<%@ Register src="../../../WebControls/Combos/UCComboUAEscola.ascx" tagname="UCComboUAEscola" tagprefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var idchkTodosDocentes = '#<%=chkTodosDocentes.ClientID %>';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsConsulta" runat="server">
        <legend>Consulta de docentes</legend>
        <asp:UpdatePanel ID="_updBuscaDocente" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc6:UCLoader ID="UCLoader2" runat="server" AssociatedUpdatePanelID="_updBuscaDocente" />
                <div id="_divPesquisa" runat="server">
                    <asp:Label ID="LabelNome" runat="server" Text="Nome do docente" AssociatedControlID="_txtNome"></asp:Label>
                    <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                    <asp:Label ID="LabelMatricula" runat="server" Text="Matrícula" AssociatedControlID="_txtMatricula"></asp:Label>
                    <asp:TextBox ID="_txtMatricula" runat="server" MaxLength="30" SkinID="text20C"></asp:TextBox>
                    <asp:Label ID="_lblCPF" runat="server" Text="Label" AssociatedControlID="_txtCPF"></asp:Label>
                    <asp:TextBox ID="_txtCPF" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
                    <asp:Label ID="_lblRG" runat="server" Text="Label" AssociatedControlID="_txtRG"></asp:Label>
                    <asp:TextBox ID="_txtRG" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
                    <asp:CheckBox ID="chkTodosDocentes" runat="server"
                        Text="Selecionar docentes de toda a rede" />
                    <div id="divFiltroEscolas">
                        <uc7:UCComboUAEscola ID="uccFiltroEscolas" runat="server"
                            MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                            CarregarEscolaAutomatico="true" ObrigatorioEscola="false" ObrigatorioUA="false" />
                    </div>
                    <uc2:UCComboCargo ID="UCComboCargo1" runat="server" />
                    <uc4:UCComboFuncao ID="UCComboFuncao1" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
            <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo docente" OnClick="_btnNovo_Click" />
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc5:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_grvDocente" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            BorderStyle="None" DataSourceID="odsDocente" DataKeyNames="col_id,doc_id" EmptyDataText="A pesquisa não encontrou resultados."
            OnRowCommand="_grvDocente_RowCommand" OnRowDataBound="_grvDocente_RowDataBound"
            OnDataBound="_grvDocente_DataBound" AllowSorting="True">
            <Columns>
                <asp:TemplateField HeaderText="Nome do docente" SortExpression="pes_nome">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("pes_nome") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CommandName="Edit"
                            PostBackUrl="~/Academico/RecursosHumanos/Docente/Cadastro.aspx"></asp:LinkButton>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("pes_nome") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Doc Padrao CPF" DataField="tipo_documentacao_cpf" SortExpression="tipo_documentacao_cpf" />
                <asp:BoundField HeaderText="Doc Padrao RG" DataField="tipo_documentacao_rg" SortExpression="tipo_documentacao_rg" />
                <asp:BoundField HeaderText="Escola" DataField="escolaunidade" SortExpression="escolaunidade" />
                <asp:BoundField HeaderText="Matrícula" DataField="coc_matricula" SortExpression="coc_matricula" />
                <asp:BoundField HeaderText="Cargo" DataField="cargofuncao" SortExpression="cargofuncao" />
                <asp:TemplateField HeaderText="Excluir">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="_btnExcluir" runat="server" CausesValidation="False" SkinID="btExcluir"
                            CommandName="Deletar" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvDocente" />
    </fieldset>
    <asp:ObjectDataSource ID="odsDocente" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Docente"
        SelectMethod="SelecionaDocentesNaoPaginadoComPermissaoTotal" TypeName="MSTech.GestaoEscolar.BLL.ACA_DocenteBO"
        DeleteMethod="Delete"></asp:ObjectDataSource>
</asp:Content>
