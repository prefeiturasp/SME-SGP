<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Classe.CompensacaoAusencia.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagPrefix="uc1" TagName="UCComboUAEscola" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCursoCurriculo.ascx" TagPrefix="uc2" TagName="UCCCursoCurriculo" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagPrefix="uc3" TagName="UCCPeriodoCalendario" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagPrefix="uc5" TagName="UCComboQtdePaginacao" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagPrefix="uc6" TagName="UCTotalRegistros" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx"  TagPrefix="uc7" TagName="UCComboCalendario"%>

<%@ Register src="../../WebControls/Combos/Novos/UCCTurmaDisciplina.ascx" tagname="UCCTurmaDisciplina" tagprefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="CompAusencia"/>
        </ContentTemplate>
    </asp:UpdatePanel>
    <fieldset id="fdsEscola" runat="server">
        <legend>Consulta de compensação de ausência</legend>

        <asp:UpdatePanel ID="uppPesquisa" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divPesquisa" runat="server">
                    <uc1:UCComboUAEscola runat="server" ID="UCComboUAEscola" AsteriscoObg="true" ObrigatorioEscola="true" ObrigatorioUA="true"
                        CarregarEscolaAutomatico="true" MostraApenasAtivas="true" MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true"
                        ValidationGroupEscola="CompAusencia" ValidationGroupUa="CompAusencia" />
                    <uc7:UCComboCalendario ID="UCComboCalendario" runat="server" MostrarMensagemSelecione="true" ValidationGroup="CompAusencia"
                            Obrigatorio="true" PermiteEditar="false" />
                    <uc2:UCCCursoCurriculo runat="server" ID="UCCCursoCurriculo" Obrigatorio="true" MostrarMensagemSelecione="true" PermiteEditar="false"
                        ValidationGroup="CompAusencia" />
                    <asp:Label ID="lblTurma" Text="Turma *" runat="server" AssociatedControlID="ddlTurma"></asp:Label>
                    <asp:DropDownList ID="ddlTurma" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="true" DataTextField="tur_esc_nome" DataValueField="tur_id"
                        SkinID="text60C" OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="cpvTurma" runat="server" ErrorMessage="Turma é obrigatório."
                        ControlToValidate="ddlTurma" Operator="NotEqual" ValueToCompare="-1" Display="Dynamic" ValidationGroup="CompAusencia"
                        Visible="True">*</asp:CompareValidator>
                    <uc4:UCCTurmaDisciplina ID="UCCTurmaDisciplina1" runat="server" Obrigatorio="true" PermiteEditar="false" MostrarMensagemSelecione="true" 
                        ValidationGroup="CompAusencia" VS_MostraFilhosRegencia="false" VS_MostraExperiencia="true" VS_MostraTerritorio="false" />
                    <uc3:UCCPeriodoCalendario runat="server" ID="UCCPeriodoCalendario" MostrarMensagemSelecione="true" Obrigatorio="false" PermiteEditar="false"
                        ValidationGroup="CompAusencia" SelecionaPeriodoAtualAoCarregar="true" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="right area-form">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CausesValidation="true" ValidationGroup="CompAusencia" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click"
                CausesValidation="False" />
            <div class="area-botoes-bottom" style="display:inline">
                <asp:Button ID="btnNovo" runat="server" Text="Incluir nova compensação" OnClick="btnNovo_Click"
                    CausesValidation="False" />
            </div>
        </div>
    </fieldset>
    <%--Resultados da pesquisa--%>
    <fieldset id="fdsResultados" runat="server" visible="false">
        <legend>Resultados</legend>
        <uc5:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="gvCompAusencia" runat="server" AutoGenerateColumns="False" DataSourceID="odsCompAusencia"
            DataKeyNames="cpa_id, tud_id, tpc_id, cap_dataInicio, cap_dataFim" AllowPaging="True" OnRowDataBound="gvCompAusencia_RowDataBound"
            EmptyDataText="A pesquisa não encontrou resultados." OnRowCommand="gvCompAusencia_RowCommand"
            OnDataBound="gvCompAusencia_DataBound" AllowSorting="true">
            <Columns>
                <asp:BoundField DataField="cap_descricao" HeaderText="Bimestre" SortExpression="cap_descricao">
                    <HeaderStyle CssClass="thLeft" Width="100px" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Atividades Desenvolvidas" SortExpression="atividadesDesenv">
                    <ItemTemplate>
                        <asp:Label ID="lblAtividadesDesenv" runat="server"
                            Text='<%# Eval("cpa_atividadesDesenvolvidas").ToString() %>'  
                            ToolTip='<%# Bind("cpa_atividadesDesenvolvidas") %>'> 
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                                                              
                <asp:TemplateField HeaderText="Alunos">
                    <ItemTemplate>
                        <asp:Label ID="lblAluno" runat="server" Text='<%# Eval("numeroAlunos").ToString() + (Convert.ToInt32(Eval("numeroAlunos").ToString()) > 1 ? " Alunos - " : " Aluno  - ")  %>'
                            ToolTip="Quantidade de alunos com compensação de ausências" 
                            Visible='<%# Convert.ToBoolean(Convert.ToInt32(Eval("numeroAlunos")) > 0) %>'>
                        </asp:Label>
                        <asp:ImageButton ID="btnDetalharCompensacao" runat="server" SkinID="btDetalhar" CommandName="DetalharCompensacao"
                            ToolTip="Detalhar compensação de ausência" CssClass="CompensacaoAusenciaCompensacao" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Alterar">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnAlterar" runat="server" CommandName="Edit" SkinID="btEditar" PostBackUrl="Cadastro.aspx"
                            CausesValidation="False" CssClass="CompensacaoAusenciaAlterar" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Excluir">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" runat="server" CommandName="Deletar" SkinID="btExcluir"
                            CausesValidation="False" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc6:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="gvCompAusencia" />
    </fieldset>
    <asp:ObjectDataSource ID="odsCompAusencia" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="SelectByPesquisa" TypeName="MSTech.GestaoEscolar.BLL.CLS_CompensacaoAusenciaBO"></asp:ObjectDataSource>
    <%-- Detalhes dos alunos com compensação de ausência [POP UP]  --%>
    <div id="divCompensacaoDetalhes" title="Compensação de ausência" class="hide">
        <asp:UpdatePanel ID="upnCompensacao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <fieldset id="fdsCompensacaoAusencia" runat="server" visible="false">
                    <asp:GridView ID="grvCompensacaoAulas" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        DataSourceID="odsQtdeAlunosCompensados" EmptyDataText="A pesquisa não encontrou resultados.">
                        <Columns>
                            <asp:BoundField DataField="mtd_numeroChamada" HeaderText="Número da chamada">
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="pes_nome" HeaderText="Nome do aluno">
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="cpa_quantidadeAulasCompensadas" HeaderText="Qtde. de ausências compensadas">
                                <HeaderStyle CssClass="center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
                <div class="right">
                    <asp:Button ID="btnFecharConsultaCompensacao" runat="server" Text="Fechar" OnClientClick="$('#divCompensacaoDetalhes').dialog('close'); return false;" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:ObjectDataSource ID="odsQtdeAlunosCompensados" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="SelecionaQtdeAlunosAusenciaCompensadas" TypeName="MSTech.GestaoEscolar.BLL.MTR_MatriculaTurmaDisciplinaBO"></asp:ObjectDataSource>
</asp:Content>
