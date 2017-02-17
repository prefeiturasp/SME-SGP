<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_RecursosHumanos_Cargo_Busca" Title="Untitled Page" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="../../../WebControls/Combos/UCComboTipoVinculo.ascx" TagName="UCComboTipoVinculo"
    TagPrefix="uc1" %>
<%@ Register Src="../../../WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros"
    TagPrefix="uc2" %>
<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset id="fdsCargo" runat="server">
        <legend>Consulta de cargos</legend>
        <div id="_divPesquisa" runat="server">
            <uc1:UCComboTipoVinculo ID="_UCComboTipoVinculo" runat="server" />
            <asp:Label ID="_lblNome" runat="server" Text="Nome do cargo" AssociatedControlID="_txtNome"></asp:Label>
            <asp:TextBox ID="_txtNome" runat="server" SkinID="text60C"></asp:TextBox>
            <asp:Label ID="_lblCodigo" runat="server" Text="Código do cargo" AssociatedControlID="_txtCodigo"></asp:Label>
            <asp:TextBox ID="_txtCodigo" runat="server" SkinID="text15C"></asp:TextBox>
        </div>
        <div class="right">
            <asp:Button ID="_btnPesquisar" runat="server" Text="Pesquisar" OnClick="_btnPesquisar_Click" />
            <asp:Button ID="_btnLimparPesquisa" runat="server" Text="Limpar pesquisa" OnClick="_btnLimparPesquisa_Click" />
            <asp:Button ID="_btnNovo" runat="server" Text="Incluir novo cargo" OnClick="_btnNovo_Click" />
        </div>
    </fieldset>
    <fieldset id="fdsResultados" runat="server">
        <legend>Resultados</legend>
        <uc3:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
        <asp:GridView ID="_dgvCargo" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            DataKeyNames="crg_id" DataSourceID="_odsCargo" CellPadding="0" Font-Bold="False"
            EmptyDataText="A pesquisa não encontrou resultados." HeaderStyle-HorizontalAlign="Center"
            OnRowCommand="_dgvCargo_RowCommand" OnRowDataBound="_dgvCargo_RowDataBound" OnDataBound="_dgvCargo_DataBound" AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderText="Nome do cargo" SortExpression="crg_nome">
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("crg_nome") %>'
                            PostBackUrl="~/Academico/RecursosHumanos/Cargo/Cadastro.aspx" CssClass="wrap400px"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="crg_codigo" HeaderText="Código do cargo" SortExpression="crg_codigo" />
                <asp:BoundField DataField="tvi_nome" HeaderText="Tipo de vínculo" SortExpression="tvi_nome"/>
                <asp:BoundField DataField="crg_cargoDocente" HeaderText="Docente" SortExpression="crg_cargoDocente"/>
                <asp:BoundField DataField="crg_situacao" HeaderText="Bloqueado" SortExpression="crg_situacao"
                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" CommandName="Deletar" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <uc2:UCTotalRegistros ID="UCTotalRegistros1" runat="server" AssociatedGridViewID="_dgvCargo" />
    </fieldset>
    <asp:ObjectDataSource ID="_odsCargo" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.RHU_Cargo"
       SelectMethod="SelecionaCargoPaginado"  TypeName="MSTech.GestaoEscolar.BLL.RHU_CargoBO"
      ></asp:ObjectDataSource>
</asp:Content>
