<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Academico_Curso_Busca" Codebehind="Busca.aspx.cs" %>

<%@ Register Src="../../WebControls/Combos/UCComboTipoNivelEnsino.ascx" TagName="UCComboTipoNivelEnsino"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoModalidadeEnsino.ascx" TagName="UCComboTipoModalidadeEnsino"
    TagPrefix="uc3" %>
<%@ Register Src="../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc4" %>  
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsCurso" runat="server">
        <legend id="lgdConsultaCurso" runat="server" title="Consulta de cursos"></legend>
        <div id="_divPesquisa" runat="server">
            <uc2:UCComboTipoNivelEnsino ID="UCComboTipoNivelEnsino1" runat="server" />
            <uc3:UCComboTipoModalidadeEnsino ID="UCComboTipoModalidadeEnsino1" runat="server" />
            <asp:Label ID="LabelCodigoCurso" runat="server" Text="Código do curso" AssociatedControlID="_txtCodigoCurso"></asp:Label>
            <asp:TextBox ID="_txtCodigoCurso" runat="server" MaxLength="10" SkinID="text10C"></asp:TextBox>
            <asp:Label ID="LabelNomeCurso" runat="server" Text="Nome do curso" AssociatedControlID="_txtNomeCurso"></asp:Label>
            <asp:TextBox ID="_txtNomeCurso" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click"
                CausesValidation="False" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc4:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_grvCursos" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            BorderStyle="None" DataKeyNames="cur_id" DataSourceID="odsCurso" EmptyDataText="A pesquisa não encontrou resultados."
            OnRowDataBound="_grvCursos_RowDataBound" ondatabound="_grvCursos_DataBound" AllowSorting="true">
            <Columns>
                <asp:BoundField DataField="cur_id" HeaderText="cur_id">
                    <HeaderStyle CssClass="hide" />
                    <ItemStyle CssClass="hide" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Nome do curso" SortExpression="cur_nome">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("cur_nome") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("cur_nome") %>' CssClass="wrap400px"></asp:Label>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("cur_nome") %>'
                            PostBackUrl="~/Academico/Curso/Cadastro.aspx" CssClass="wrap400px"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="cur_codigo" HeaderText="Código" SortExpression="cur_codigo"></asp:BoundField>
                <asp:BoundField DataField="tne_nome" HeaderText="Nível de ensino" SortExpression="tne_nome" />
                <asp:BoundField DataField="tme_nome" HeaderText="Modalidade de ensino" SortExpression="tme_nome" />
                <asp:BoundField DataField="cur_situacao" HeaderText="Situação" SortExpression="cur_situacao">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvCursos" />
    </fieldset>
    <asp:ObjectDataSource ID="odsCurso" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_Curso"
        SelectMethod="GetSelect_CadastroCurso"  TypeName="MSTech.GestaoEscolar.BLL.ACA_CursoBO"
        DeleteMethod="Delete"></asp:ObjectDataSource>
</asp:Content>
