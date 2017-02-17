<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Configuracao_PeriodoCalendario_Busca" Codebehind="Busca.aspx.cs" %>

<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="_uppPeriodoCalendario" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <uc1:UCLoader ID="UCLoader2" runat="server" AssociatedUpdatePanelID="_uppPeriodoCalendario" />
            <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
            <fieldset>
                <legend>Listagem de tipos de período do calendário</legend>
                <div>
                    <asp:Button ID="_btnNovoTipoPeriodoCalendario" runat="server" Text="Incluir novo tipo de período do calendário"
                        OnClick="_btnNovoTipoPeriodoCalendario_Click" />
                </div>
                <asp:GridView ID="_dgvTipoPeriodoCalendario" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="tpc_id,tpc_ordem" DataSourceID="odsTipoPeriodoCalendario" HeaderStyle-HorizontalAlign="Center"
                    AllowPaging="False" EmptyDataText="Não existem tipos de período do calendário cadastrados."
                    OnRowCommand="_dgvTipoPeriodoCalendario_RowCommand" OnRowDataBound="_dgvTipoPeriodoCalendario_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Tipo de período do calendário">
                            <ItemTemplate>
                                <asp:LinkButton ID="_btnAlterar" runat="server" CommandName="Edit" Text='<%# Bind("tpc_nome") %>'
                                    PostBackUrl="~/Configuracao/TipoPeriodoCalendario/Cadastro.aspx"></asp:LinkButton>
                                <asp:Label ID="_lblAlterar" runat="server" Text='<%# Bind("tpc_nome") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Nome abreviado" DataField="tpc_nomeAbreviado" SortExpression="tpc_nomeAbreviado" />
                        <asp:TemplateField HeaderText="Ordem" SortExpression="tpc_ordem">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("tpc_ordem") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="_btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                    Height="16" Width="16" />
                                <asp:ImageButton ID="_btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                    Height="16" Width="16" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="tpc_foraPeriodoLetivo" HeaderText="Fora do período letivo"
                            SortExpression="tpc_foraPeriodoLetivo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="tpc_situacao" HeaderText="Bloqueado" SortExpression="tpc_situacao"
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle HorizontalAlign="Center" CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Excluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="_btnExcluir" SkinID="btExcluir" CommandName="Deletar" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsTipoPeriodoCalendario" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_TipoPeriodoCalendario"
                    SelectMethod="SelecionaTipoPeriodoCalendario" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoPeriodoCalendarioBO"
                    OnSelecting="odsTipoPeriodoCalendario_Selecting" SelectCountMethod="GetTotalRecords">
                    <SelectParameters>
                        <asp:Parameter DbType="Int32" Name="AppMinutosCacheLongo" />
                        <asp:Parameter DbType="Boolean" Name="removerRecesso" DefaultValue="False" />
                        <asp:Parameter DbType="Guid" Name="ent_id" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
