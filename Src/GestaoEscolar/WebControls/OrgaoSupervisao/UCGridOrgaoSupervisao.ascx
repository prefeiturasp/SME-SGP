<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_OrgaoSupervisao_UCGridOrgaoSupervisao" Codebehind="UCGridOrgaoSupervisao.ascx.cs" %>
<%@ Register Src="../Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:UpdatePanel ID="updGridOrgaoSupervisao" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updGridOrgaoSupervisao" />
        <asp:Button ID="_btnNovo" runat="server" Text="Adicionar órgão de supervisão" CausesValidation="False"
            OnClick="_btnNovo_Click" />
        <asp:GridView ID="grvOrgaoSupervisao" runat="server" AutoGenerateColumns="False"
            EmptyDataText="Não existem orgãos de supervisão cadastrados." OnRowCommand="grvOrgaoSupervisao_RowCommand"
            OnRowDataBound="grvOrgaoSupervisao_RowDataBound">
            <EmptyDataRowStyle Font-Bold="True" ForeColor="Red" />
            <Columns>
                <asp:BoundField DataField="esc_id" HeaderText="esc_id">
                    <HeaderStyle CssClass="hide" />
                    <ItemStyle CssClass="hide" />
                </asp:BoundField>
                <asp:BoundField DataField="eos_id" HeaderText="eos_id">
                    <HeaderStyle CssClass="hide" />
                    <ItemStyle CssClass="hide" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Descrição">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("eos_nome") %>' CssClass="wrap400px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="_btnAlterar" runat="server" CausesValidation="False" CommandName="Alterar"
                            Text='<%# Bind("eos_nome") %>' CssClass="wrap400px"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="orgao" HeaderText="Órgão" />
                <asp:TemplateField HeaderText="Excluir">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="_btnExcluir" runat="server" CausesValidation="False" CommandName="Excluir"
                            SkinID="btExcluir" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
