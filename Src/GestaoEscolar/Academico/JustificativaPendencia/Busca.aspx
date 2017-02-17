<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="Academico_JustificativaPendencia_Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboUAEscola.ascx" TagName="ComboUAEscola"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCCalendario.ascx" TagName="ComboCalendario"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCTurmaDisciplina.ascx" TagName="ComboTurmaDisciplina"
    TagPrefix="uc3" %>
<%@ Register Src="~/WebControls/Combos/Novos/UCCPeriodoCalendario.ascx" TagName="ComboPeriodoCalendario"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="ComboQtdePaginacao"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="vsConsultaJustificativa" runat="server" ValidationGroup="ConsultaJustificativa" />
    <fieldset id="fdsJustificativas" runat="server">
        <legend><asp:Literal runat="server" ID="litJustificativas" Text="<%$ Resources:Academico, JustificativaPendencia.Busca.litJustificativas.Text %>"></asp:Literal></legend>
        <div id="divPesquisa" runat="server">
            <uc1:ComboUAEscola ID="comboUAEscola" runat="server" CarregarEscolaAutomatico="true"
                ObrigatorioEscola="true" ObrigatorioUA="true" 
                MostrarMessageSelecioneEscola="true" MostrarMessageSelecioneUA="true" 
                ValidationGroupEscola="ConsultaJustificativa" ValidationGroupUA="ConsultaJustificativa" />
            <uc2:ComboCalendario ID="comboCalendario" runat="server" MostrarMessageSelecione="true" Obrigatorio="true" SelecionarAnoCorrente="true" ValidationGroup="ConsultaJustificativa" />
            <uc3:ComboTurmaDisciplina ID="comboTurmaDisciplina" runat="server" MostrarMessageSelecione="true" Obrigatorio="false" VS_TurmaEletiva="true" ValidationGroup="ConsultaJustificativa" />
            <uc4:ComboPeriodoCalendario ID="comboPeriodoCalendario" runat="server" MostrarMensagemSelecione="true" Obrigatorio="false" ValidationGroup="ConsultaJustificativa" />
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="<%$ Resources:Academico, JustificativaPendencia.Busca.btnPesquisar.Text %>" OnClick="btnPesquisar_Click" ValidationGroup="ConsultaJustificativa" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="<%$ Resources:Academico, JustificativaPendencia.Busca.btnLimparPesquisa.Text %>" OnClick="btnLimparPesquisa_Click" CausesValidation="false" />
            <span class="area-botoes-bottom">
                <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Academico, JustificativaPendencia.Busca.btnNovo.Text %>" OnClick="btnNovo_Click" CausesValidation="false" />
            </span> 
       </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server" class="area-form">
        <legend><asp:Literal runat="server" ID="litResultados" Text="<%$ Resources:Academico, JustificativaPendencia.Busca.litResultados.Text %>"></asp:Literal></legend>
        <uc5:ComboQtdePaginacao ID="comboPaginacao" runat="server" OnIndexChanged="comboPaginacao_IndexChanged" />
        <asp:GridView ID="grvJustificativas" runat="server" DataSourceID="odsJustificativas" 
            AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="tud_id,cal_id,tpc_id,fjp_id,fjp_justificativa,uaSuperior,esc_uni_id"
            EmptyDataText="A pesquisa não encontrou resultados." HorizontalAlign="Center"
            OnRowCommand="grvJustificativas_RowCommand" OnRowDataBound="grvJustificativas_RowDataBound"
            OnDataBound="grvJustificativas_DataBound" AllowSorting="true" SkinID="GridResponsive">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:Academico, JustificativaPendencia.Busca.grvJustificativas.colunaTurma %>" SortExpression="tur_codigo">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tur_codigo") %>'
                            PostBackUrl="../JustificativaPendencia/Cadastro.aspx" CssClass="wrap250px"></asp:LinkButton>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("tur_codigo") %>' CssClass="wrap250px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Academico, JustificativaPendencia.Busca.grvJustificativas.colunaCurso %>">
                    <ItemTemplate>
                        <asp:Label ID="lblCurso" runat="server" Text='<%# Bind("cur_nome") %>' CssClass="wrap250px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Academico, JustificativaPendencia.Busca.grvJustificativas.colunaCalendario %>" >
                    <ItemTemplate>
                        <asp:Label ID="lblCalendario" runat="server" Text='<%# Bind("cal_descricao") %>' CssClass="wrap250px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" >
                    <ItemTemplate>
                        <asp:Label ID="lblPeriodoCalendario" runat="server" Text='<%# Bind("tpc_nome") %>' CssClass="wrap100px"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>   
                <asp:TemplateField HeaderText="<%$ Resources:Academico, JustificativaPendencia.Busca.grvJustificativas.colunaJustificativa %>">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnJustificativa" SkinID="btDetalhar" runat="server" CommandName="AbrirJustificativa"
                            ToolTip="<%$ Resources:Academico, JustificativaPendencia.Busca.btnJustificativa.ToolTip %>" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Academico, JustificativaPendencia.Busca.grvJustificativas.colunaExcluir %>">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Excluir" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc6:UCTotalRegistros ID="ucTotalRegistros" runat="server" AssociatedGridViewID="grvJustificativas" />
    </fieldset>
    <asp:ObjectDataSource ID="odsJustificativas" runat="server" SelectMethod="GetSelect_Busca"
        DeleteMethod="Delete" TypeName="MSTech.GestaoEscolar.BLL.CLS_FechamentoJustificativaPendenciaBO" DataObjectTypeName="MSTech.GestaoEscolar.Entities.CLS_FechamentoJustificativaPendencia">
        <SelectParameters>
            <asp:Parameter DbType="Int32" Name="esc_id" />
            <asp:Parameter DbType="Int32" Name="uni_id" />
            <asp:Parameter DbType="Int32" Name="cal_id" />
            <asp:Parameter DbType="Int64" Name="tud_id" />
            <asp:Parameter DbType="Int32" Name="tpc_id" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <div id="divJustificativa" title="" class="hide">
        <asp:UpdatePanel ID="updJustificativa" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblJustificativa" runat="server" Text=""></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
