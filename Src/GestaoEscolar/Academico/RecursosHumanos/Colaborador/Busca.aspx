<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_RecursosHumanos_Colaborador_Busca" Title="Untitled Page"
    CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="../../../WebControls/Combos/UCComboCargo.ascx" TagName="UCComboCargo"
    TagPrefix="uc2" %>
<%@ Register Src="../../../WebControls/Combos/UCComboFuncao.ascx" TagName="UCComboFuncao"
    TagPrefix="uc3" %>
<%@ Register Src="../../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader"
    TagPrefix="uc6" %>
<%@ Register Src="../../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc5" %>
<%@ Register src="../../../WebControls/Combos/UCComboUAEscola.ascx" tagname="UCComboUAEscola" tagprefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var idchkTodosColaboradores = '#<%=chkTodosColaboradores.ClientID %>';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divBuscaUA" title="Busca de unidades administrativas" class="hide">
        <asp:UpdatePanel ID="_updBuscaUA" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc6:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="_updBuscaUA" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:Panel ID="pConsultaColaboradores" runat="server" DefaultButton="_btnPesquisar">
        <fieldset id="fdsConsulta" runat="server">
            <legend>Consulta de colaboradores</legend>
            <asp:UpdatePanel ID="_updBuscaColaborador" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc6:UCLoader ID="UCLoader2" runat="server" AssociatedUpdatePanelID="_updBuscaColaborador" />
                    <div id="_divPesquisa" runat="server">
                        <asp:Label ID="LabelNome" runat="server" Text="Nome do colaborador" AssociatedControlID="_txtNome"></asp:Label>
                        <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
                        <asp:Label ID="LabelMatricula" runat="server" Text="Matrícula" AssociatedControlID="_txtMatricula"></asp:Label>
                        <asp:TextBox ID="_txtMatricula" runat="server" MaxLength="30" SkinID="text20C"></asp:TextBox>
                        <asp:Label ID="_lblCPF" runat="server" Text="Label" AssociatedControlID="_txtCPF"></asp:Label>
                        <asp:TextBox ID="_txtCPF" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
                        <asp:Label ID="_lblRG" runat="server" Text="Label" AssociatedControlID="_txtRG"></asp:Label>
                        <asp:TextBox ID="_txtRG" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
                        <asp:CheckBox ID="chkTodosColaboradores" runat="server" Text="Selecionar colaboradores de toda a rede" />
                        <div id="divFiltroEscolas">
                            <uc7:UCComboUAEscola ID="ucComboUAEscola" runat="server"
                            MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                            CarregarEscolaAutomatico="true" ObrigatorioEscola="false" ObrigatorioUA="false" />
                        </div>
                        <uc2:UCComboCargo ID="UCComboCargo1" runat="server" />
                        <uc3:UCComboFuncao ID="UCComboFuncao1" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="right">
                <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click"
                    CausesValidation="False" />
                <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
                <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo colaborador" OnClick="_btnNovo_Click"
                    CausesValidation="False" />
            </div>
        </fieldset>
    </asp:Panel>
    <fieldset id="fdsResultado" runat="server">
        <legend>Resultados</legend>
        <uc5:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_grvColaborador" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            AllowSorting="true" BorderStyle="None" DataSourceID="odsColaborador" DataKeyNames="col_id"
            EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="_grvColaborador_RowCommand"
            OnRowDataBound="_grvColaborador_RowDataBound" OnDataBound="_grvColaborador_DataBound">
            <Columns>
                <asp:TemplateField HeaderText="Nome do colaborador" SortExpression="pes_nome">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("pes_nome") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CommandName="Edit"
                            PostBackUrl="~/Academico/RecursosHumanos/Colaborador/Cadastro.aspx" CausesValidation="False"
                            CssClass="wrap400px"></asp:LinkButton>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("pes_nome") %>' CssClass="wrap400px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Doc padrao CPF" DataField="tipo_documentacao_cpf" SortExpression="tipo_documentacao_cpf" />
                <asp:BoundField HeaderText="Doc padrao RG" DataField="tipo_documentacao_rg" SortExpression="tipo_documentacao_rg" />
                <asp:BoundField HeaderText="Unidade administrativa" DataField="uad_nome" SortExpression="uad_nome" />
                <asp:BoundField HeaderText="Matrícula" DataField="coc_matricula" SortExpression="coc_matricula" />
                <asp:BoundField HeaderText="Cargo / função" DataField="cargofuncao" SortExpression="cargofuncao" />
                <asp:TemplateField HeaderText="Criar docente">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnCriaDocente" runat="server" CausesValidation="False" SkinID="btNovo"
                            CommandName="CriaDocente" Visible="false" ToolTip="Cria o docente a partir do colaborador" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
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
        <uc4:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvColaborador" />
    </fieldset>
    <asp:ObjectDataSource ID="odsColaborador" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.RHU_Colaborador"
        SelectMethod="SelecionaColaboradorNaoPaginadoComPermissaoTotal" TypeName="MSTech.GestaoEscolar.BLL.RHU_ColaboradorBO">
    </asp:ObjectDataSource>
</asp:Content>
