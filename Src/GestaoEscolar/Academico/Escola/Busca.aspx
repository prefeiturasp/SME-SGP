<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Academico_Escola_Busca" Title="Untitled Page" Codebehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboCursoCurriculo.ascx" TagName="UCComboCursoCurriculo"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas"
    TagPrefix="uc3" %>
<%@ Register Src="../../WebControls/Combos/UCComboTipoUAEscola.ascx" TagName="UCComboTipoUAEscola"
    TagPrefix="uc4" %>
<%@ Register src="../../WebControls/Mensagens/UCTotalRegistros.ascx" tagname="UCTotalRegistros" tagprefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc5" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoClassificacaoEscola.ascx" TagName="UCComboTipoClassificacaoEscola"
    TagPrefix="uc6" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsEscola" runat="server">
        <legend>Consulta de escolas</legend>
        <div id="_divPesquisa" runat="server">
            <uc4:UCComboTipoUAEscola ID="UCComboTipoUAEscola1" runat="server" />
            <uc3:UCFiltroEscolas ID="UCFiltroEscolas1" runat="server" />
            <asp:Label ID="LabelNome" runat="server" Text="Nome da escola" AssociatedControlID="_txtNome"></asp:Label>
            <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
            <asp:Label ID="LabelCodigo" runat="server" Text="Código da escola" AssociatedControlID="_txtCodigo"></asp:Label>
            <asp:TextBox ID="_txtCodigo" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
            <asp:Label ID="LabelTelefone" runat="server" Text="Telefone" AssociatedControlID="_txtTelefone"></asp:Label>
            <asp:TextBox ID="_txtTelefone" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
            <uc2:UCComboCursoCurriculo ID="UCComboCursoCurriculo" runat="server" />
            <uc6:UCComboTipoClassificacaoEscola ID="uccTipoClassificacaoEscola" runat="server" />
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click"
                CausesValidation="False" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click"
                CausesValidation="False" />
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc5:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_grvEscolas" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            BorderStyle="None" DataKeyNames="esc_id,uni_id" DataSourceID="odsEscola" EmptyDataText="A pesquisa não encontrou resultados."
            OnRowCommand="_grvEscolas_RowCommand" 
            OnRowDataBound="_grvEscolas_RowDataBound" 
            ondatabound="_grvEscolas_DataBound" AllowSorting="True" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Nome da escola" SortExpression="esc_nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" PostBackUrl="~/Academico/Escola/Cadastro.aspx"
                            CausesValidation="False" Text='<%# Bind("esc_nome") %>' CssClass="wrap400px"></asp:LinkButton>
                        <%--<asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("esc_nome") %>' CssClass="wrap400px"></asp:Label>--%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("esc_nome") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="esc_codigo" HeaderText="Código da escola" SortExpression="esc_codigo"/>
                <asp:BoundField DataField="tua_nome" HeaderText="Tipo de escola" SortExpression="tua_nome" />
                <asp:BoundField DataField="TIPO_MEIOCONTATO_TELEFONE" HeaderText="Telefone" SortExpression="TIPO_MEIOCONTATO_TELEFONE" />
                <asp:BoundField DataField="situacao" HeaderText="Situação" SortExpression="situacao">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Importação do fechamento">
                    <ItemTemplate>
                        <asp:ImageButton ID="_btnImportacao" runat="server" CommandName="ImportacaoFechamento" SkinID="btDetalhar"
                            CausesValidation="False" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc1:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_grvEscolas" />
    </fieldset>
    <asp:ObjectDataSource ID="odsEscola" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ESC_Escola"
        SelectMethod="GetSelectNaoPaginado" TypeName="MSTech.GestaoEscolar.BLL.ESC_EscolaBO"
        DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" UpdateMethod="Save">
        <SelectParameters>
            <asp:Parameter Name="esc_id" Type="Int32" />
            <asp:Parameter Name="esc_nome" Type="String" />
            <asp:Parameter Name="esc_codigo" Type="String" />
            <asp:Parameter Name="esc_situacao" Type="Byte" />
            <asp:Parameter DbType="Guid" Name="tua_id" />
            <asp:Parameter DbType="Guid" Name="ent_id" />
            <asp:Parameter DbType="Guid" Name="uad_idSuperior" />
            <asp:Parameter DbType="Guid" Name="gru_id" />
            <asp:Parameter DbType="Guid" Name="usu_id" />
            <asp:Parameter Name="TIPO_MEIOCONTATO_TELEFONE" Type="String" />
            <asp:Parameter Name="cur_id" Type="Int32" />
            <asp:Parameter Name="crr_id" Type="Int32" />
            <asp:Parameter Name="tce_id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
