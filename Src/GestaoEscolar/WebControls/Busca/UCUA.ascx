<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCUA.ascx.cs" Inherits="MSTech.CoreSSO.UserControlLibrary.Buscas.UCUA" %>
<%@ Register Src="../Combos/UCComboEntidade.ascx" TagName="UCComboEntidade" TagPrefix="coresso" %>
<%@ Register Src="../Combos/UCComboTipoUnidadeAdministrativa.ascx" TagName="UCComboTipoUnidadeAdministrativa"
    TagPrefix="coresso" %>
<fieldset>
    <coresso:UCComboEntidade ID="UCComboEntidade1" runat="server" />
    <coresso:UCComboTipoUnidadeAdministrativa ID="UCComboTipoUnidadeAdministrativa1" runat="server" />
    <asp:Label ID="Label3" runat="server" Text="Nome" EnableViewState="False" AssociatedControlID="_txtNome"></asp:Label>
    <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
    <asp:Label ID="Label4" runat="server" Text="Código" EnableViewState="False" AssociatedControlID="_txtCodigo"></asp:Label>
    <asp:TextBox ID="_txtCodigo" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
    <div class="right">
        <asp:Button ID="_btnPesquisar" runat="server" OnClick="_btnPesquisar_Click" Text="Pesquisar"
            CausesValidation="False" />
        <asp:Button ID="_btnVoltar" runat="server" OnClientClick="$('#divBuscaUA').dialog('close');" Text="Voltar"
            CausesValidation="False" />
    </div>
</fieldset>
<fieldset id="fdsResultados" runat="server">
    <legend>Resultados</legend>
    <asp:GridView ID="_dgvUA" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="ent_id,uad_id,uad_nome" DataSourceID="odsUA" 
        EmptyDataText="A pesquisa não encontrou resultados." 
        onrowediting="_dgvUA_RowEditing">
        <Columns>
            <asp:BoundField DataField="ent_id" HeaderText="ent_id" ReadOnly="True" Visible="False" />
            <asp:BoundField DataField="uad_id" HeaderText="uad_id" ReadOnly="True" Visible="False" />
            <asp:TemplateField HeaderText="Nome">
                <ItemTemplate>
                    <asp:LinkButton ID="_lkbSelect" runat="server" CommandName="Edit" Text='<%# Bind("uad_nome") %>'
                        CausesValidation="False"></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="uad_codigo" HeaderText="Código" />
            <asp:BoundField DataField="tua_nome" HeaderText="Tipo de unidade administrativa" />
            <asp:BoundField DataField="ent_razaoSocial" HeaderText="Entidade" />
            <asp:BoundField DataField="uad_nomeSup" HeaderText="Unidade administrativa superior" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsUA" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.SYS_UnidadeAdministrativa"
        EnablePaging="True" MaximumRowsParameterName="pageSize" OnSelecting="odsUA_Selecting"
        SelectCountMethod="GetTotalRecords" SelectMethod="GetSelect" StartRowIndexParameterName="currentPage"
        TypeName="MSTech.CoreSSO.BLL.SYS_UnidadeAdministrativaBO">
    </asp:ObjectDataSource>
</fieldset>
