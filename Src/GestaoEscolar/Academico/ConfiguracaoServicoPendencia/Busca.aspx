<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.ConfiguracaoServicoPendencia.Busca" %>

<%@ Register Src="~/WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoModalidadeEnsino.ascx" TagName="UCComboTipoModalidadeEnsino"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoTurma.ascx" TagName="UCComboTipoTurma"
    TagPrefix="uc3" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc4" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsPesquisa" runat="server">
        <legend>Busca</legend>
        <div id="divPesquisa" runat="server">
            <uc1:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino" runat="server" />
            <uc2:UCComboTipoModalidadeEnsino ID="UCComboTipoModalidadeEnsino" runat="server" />
            <uc3:UCComboTipoTurma ID="UCComboTipoTurma" runat="server"/>
        </div>
        <div class="right">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CausesValidation="False" />
            <asp:Button ID="btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="btnLimparPesquisa_Click"
                CausesValidation="False" />
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc5:UCComboQtdePaginacao ID="UCComboQtdePaginacao" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="dgvConfiguracaoServicoPendencia" runat="server" AllowPaging="True" 
            DataKeyNames="csp_semNota, csp_semParecer, csp_disciplinaSemAula, csp_semResultadoFinal, csp_semPlanejamento"
            AutoGenerateColumns="False" DataSourceID="odsConfiguracaoServicoPendencia" EmptyDataText="A pesquisa não encontrou resultados."
            HeaderStyle-HorizontalAlign="Center" OnRowDataBound="dgvConfiguracaoServicoPendencia_RowDataBound"
            OnDataBound="dgvConfiguracaoServicoPendencia_DataBound" AllowSorting="true">
            <Columns>
                <%--<asp:TemplateField HeaderText="Nome" SortExpression="nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("nome") %>'
                            PostBackUrl="../EscalaAvaliacao/Cadastro.aspx"></asp:LinkButton>
                        <asp:Label ID="lblAlterar" runat="server" Text='<%# Bind("nome") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:BoundField DataField="tne_descricao" HeaderText="Tipo de nível de ensino" SortExpression="tne_descricao">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>     
                <asp:BoundField DataField="tme_descricao" HeaderText="Tipo de modalidade de ensino" SortExpression="tme_descricao">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>  
                <asp:BoundField DataField="tur_tipoDescricao" HeaderText="Tipo de turma" SortExpression="tur_tipoDescricao">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>  
                <asp:TemplateField HeaderText="Sem nota">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSemNota" runat="server" CssClass="wrap150px"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>       
                <asp:TemplateField HeaderText="Sem parecer">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSemParecer" runat="server" CssClass="wrap150px"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>       
                <asp:TemplateField HeaderText="Disciplina sem aula">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDisciplinaSemAula" runat="server" CssClass="wrap150px"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>       
                <asp:TemplateField HeaderText="Sem resultado final">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSemResultadoFinal" runat="server" CssClass="wrap150px"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>       
                <asp:TemplateField HeaderText="Sem planejamento">
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSemPlanejamento" runat="server" CssClass="wrap150px"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField> 
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc4:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="dgv" />
        <div class="right">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" 
                ToolTip="Salvar"/>
        </div>
    </fieldset>
    <asp:ObjectDataSource ID="odsConfiguracaoServicoPendencia" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_ConfiguracaoServicoPendencia"
        DeleteMethod="Delete" SelectMethod="SelectBy_tne_id_tme_id_tur_tipo" TypeName="MSTech.GestaoEscolar.BLL.ACA_ConfiguracaoServicoPendencia"></asp:ObjectDataSource>
</asp:Content>
