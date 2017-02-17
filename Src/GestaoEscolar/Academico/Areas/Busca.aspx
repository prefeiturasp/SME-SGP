<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="GestaoEscolar.Academico.Areas.Busca" %>

<%@ Register Src="~/WebControls/Mensagens/UCTotalRegistros.ascx" TagName="UCTotalRegistros" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMensagem" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlBusca" runat="server" GroupingText="<%$ Resources:Academico, Areas.Busca.pnlBusca.GroupingText %>">
        <asp:Button ID="btnNovaArea" runat="server" Text="<%$ Resources:Academico, Areas.Busca.btnNovaArea.Text %>" OnClick="btnNovaArea_Click" CausesValidation="false" />
        <asp:UpdatePanel ID="updAreas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="grvAreas" runat="server" AutoGenerateColumns="false" DataSourceID="odsAreas" DataKeyNames="tad_id,tad_ordem"
                    OnDataBound="grvAreas_DataBound" OnRowDataBound="grvAreas_RowDataBound" OnRowCommand="grvAreas_RowCommand" 
                    EmptyDataText="<%$ Resources:Academico, Areas.Busca.grvAreas.EmptyDataText %>" SkinID="GridResponsive">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Areas.Busca.grvAreas.HeaderTextNome %>">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnAlterar" runat="server" CommandName="Edit" CausesValidation="False" Text='<%# Bind("tad_nome") %>'
                                    PostBackUrl="~/Academico/Areas/Cadastro.aspx" ToolTip="<%$ Resources:Academico, Areas.Busca.grvAreas.btnAlterar.ToolTip %>">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="<%$ Resources:Academico, Areas.Busca.grvAreas.HeaderTextCadastroEscola %>" DataField="tad_cadastroEscola">
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                            <ItemStyle CssClass="center" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Areas.Busca.grvAreas.HeaderTextOrdem %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnSubir" runat="server" CausesValidation="false" CommandName="Subir"
                                    Height="16" Width="16"
                                    ToolTip="<%$ Resources:Academico, Areas.Busca.grvAreas.btnSubir.ToolTip %>" />
                                <asp:ImageButton ID="btnDescer" runat="server" CausesValidation="false" CommandName="Descer"
                                    Height="16" Width="16"
                                    ToolTip="<%$ Resources:Academico, Areas.Busca.grvAreas.btnDescer.ToolTip %>" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Areas.Busca.grvAreas.HeaderTextLinksDocumentos %>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnLinkAreas" SkinID="btDetalhar" runat="server" PostBackUrl="~/Academico/Areas/Documentos.aspx"
                                    CommandName="Edit"
                                    ToolTip="<%$ Resources:Academico, Areas.Busca.grvAreas.btnLinkAreas.ToolTip %>" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Academico, Areas.Busca.grvAreas.HeaderTextExcluir%>">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" CommandName="Deletar"
                                    CausesValidation="False"
                                    ToolTip="<%$ Resources:Academico, Areas.Busca.grvAreas.btnExcluir.ToolTip %>" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <uc1:UCTotalRegistros ID="UCTotalRegistros" runat="server" AssociatedGridViewID="grvAreas" />
                <asp:ObjectDataSource ID="odsAreas" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="SelecionarAtivosPermissao" TypeName="MSTech.GestaoEscolar.BLL.ACA_TipoAreaDocumentoBO"
                    OnSelecting="odsAreas_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="admin" Type="Boolean" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <div id="divConfirmaExclusao" title="Confirmação" class="hide">
        <%-- conteudo do pop up de salvar --%>
        <asp:UpdatePanel ID="updMensagemExclusao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label runat="server" ID="lblPopUpExclusao" EnableViewState="false"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <hr />
        <div class="btnsDireita">
            <asp:Button ID="btnSim" runat="server" Text="<%$ Resources:Academico, Areas.Busca.btnSim.Text %>" CausesValidation="False"
                ToolTip="<%$ Resources:Academico, Areas.Busca.btnSim.ToolTip %>" OnClick="btnSim_Click" />
            <asp:Button ID="btnNao" runat="server" Text="<%$ Resources:Academico, Areas.Busca.btnNao.Text %>" CausesValidation="False"
                ToolTip="<%$ Resources:Academico, Areas.Busca.btnNao.ToolTip %>" OnClientClick="$('#divConfirmaExclusao').dialog('close');return false;" />
        </div>
    </div>
</asp:Content>
