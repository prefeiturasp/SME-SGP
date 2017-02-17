<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Configuracao_TipoAtividadeAvaliativa_Busca" CodeBehind="Busca.aspx.cs" %>

<%@ Register Src="~/WebControls/Combos/UCComboQtdePaginacao.ascx" TagName="UCComboQtdePaginacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <fieldset>
        <legend><asp:Literal ID="litTitulo" runat="server" Text="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.litTitulo.Text %>"></asp:Literal></legend>
        <div>
            <asp:Button ID="btnNovo" runat="server" Text="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.btnNovo.Text %>"
                OnClick="btnNovo_Click" />
        </div>
        <div>
            <uc1:UCComboQtdePaginacao ID="UCComboQtdePaginacao1" runat="server" OnIndexChanged="UCComboQtdePaginacao_IndexChanged" />
            <div id="divLegenda" runat="server" style="width: 280px;" visible="false">
                <b>Legenda:</b>
                <div style="border-style: solid; border-width: thin;">
                    <table id="tbLegenda" runat="server" style="border-collapse: separate !important; border-spacing: 2px !important;">
                        <tr runat="server" id="lnInativos">
                            <td style="border-style: solid; border-width: thin; width: 25px; height: 15px;">
                            </td>
                            <td>
                                <asp:Literal ID="litLegendaInativa" runat="server" Text="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.litLegendaInativa.Text %>"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
            <asp:GridView ID="grvTipoAtividadeAvaliativa" runat="server" AutoGenerateColumns="False"
                DataKeyNames="tav_id" DataSourceID="odsTipoAtividadeAvaliativa" AllowPaging="True"
                OnRowCommand="grvTipoAtividadeAvaliativa_RowCommand" EmptyDataText="Não existem tipos de atividades avaliativas cadastrados."
                OnRowDataBound="grvTipoAtividadeAvaliativa_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.grvTipoAtividadeAvaliativa.HeaderTextAtividade %>">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnAlterar" runat="server" Text='<%# Eval("tav_nome") %>' PostBackUrl="~/Configuracao/TipoAtividadeAvaliativa/Cadastro.aspx"
                                CommandName="Edit">
                            </asp:LinkButton>
                            <asp:Label ID="lblAlterar" runat="server" Text='<%# Eval("tav_nome") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="qat_nome" HeaderText="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.grvTipoAtividadeAvaliativa.HeaderTextQualificador %>" 
                        SortExpression="tav_nome">
                        <HeaderStyle CssClass="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.grvTipoAtividadeAvaliativa.HeaderTextAtivar %>" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnAtivarAtividade" runat="server" ToolTip="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.btnAtivarAtividade.ToolTip %>"
                                CommandName="AtivarAtividade" SkinID="btAtivar"></asp:ImageButton>
                            <asp:ImageButton ID="btnInativarAtividade" runat="server" ToolTip="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.btnInativarAtividade.ToolTip %>"
                                CommandName="InativarAtividade" SkinID="btDesativar"></asp:ImageButton>
                        </ItemTemplate>
                        <HeaderStyle CssClass="center" HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.grvTipoAtividadeAvaliativa.HeaderTextExcluir %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar" ToolTip="<%$ Resources:Configuracao, TipoAtividadeAvaliativa.Busca.btnExcluir.ToolTip %>" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsTipoAtividadeAvaliativa" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.CLS_TipoAtividadeAvaliativa"
                SelectMethod="SelecionaTipoAtividadeAvaliativaPaginado" TypeName="MSTech.GestaoEscolar.BLL.CLS_TipoAtividadeAvaliativaBO"
                EnablePaging="True" MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords"
                StartRowIndexParameterName="currentPage" OnSelecting="odsTipoAtividadeAvaliativa_Selecting">
            </asp:ObjectDataSource>
        </div>
    </fieldset>
</asp:Content>
