<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Contato_UCGridContatoNomeTelefone"
    CodeBehind="UCGridContatoNomeTelefone.ascx.cs" %>
<%@ Register Src="../Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<%@ Register Src="../Combos/UCComboTipoResponsavelAluno.ascx" TagName="UCComboTipoResponsavelAluno"
    TagPrefix="uc2" %>
<asp:UpdatePanel ID="updGridContatoNomeTelefone" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updGridContatoNomeTelefone" />
        <asp:Label ID="_lblMessage" runat="server"></asp:Label>
        <asp:GridView ID="_grvContatoNomeTelefone" runat="server" AutoGenerateColumns="False"
            DataKeyNames="fmc_id" EmptyDataText="Não existem contatos cadastrados."
            OnRowDataBound="_grvContatoNomeTelefone_RowDataBound" OnRowCommand="_grvContatoNomeTelefone_RowCommand">
            <EmptyDataRowStyle Font-Bold="True" ForeColor="Red" />
            <Columns>
                <asp:TemplateField HeaderText="Nome do contato">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtContatoNome" runat="server" Text='<%# Bind("fmc_nome") %>' SkinID="text30C"
                            MaxLength="100"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo de contato">
                    <ItemTemplate>
                        <uc2:UCComboTipoResponsavelAluno ID="UCComboTipoResponsavelAluno1" runat="server" Titulo="Tipo de contato"
                            MostrarMessageSelecione="true" MostraLabel="false" CssClass="text30C" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Telefone">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtContatoTelefone" runat="server" Text='<%# Bind("fmc_telefone") %>'
                            SkinID="text30C" MaxLength="100"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ordem de aviso">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnSubir" runat="server" CausesValidation="false" OnClick="btnSubir_Click"
                            Height="16" Width="16" ToolTip="Subir" />
                        <asp:ImageButton ID="btnDescer" runat="server" CausesValidation="false" OnClick="btnDescer_Click"
                            Height="16" Width="16" ToolTip="Descer" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="center" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Adicionar">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnAdicionar" runat="server" CausesValidation="False" SkinID="btNovo"
                            ToolTip="Adicionar" OnClick="btnAdicionar_Click" Visible="False" />
                    </ItemTemplate>
                    <HeaderStyle Width="70px" CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Excluir">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnExcluir" SkinID="btExcluir" runat="server" ToolTip="Excluir contato"
                            CommandName="Excluir" />
                    </ItemTemplate>
                    <HeaderStyle Width="70px" CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsFichaMedicaContato" runat="server" DataObjectTypeName="MSTech.GestaoEscolar.Entities.ACA_FichaMedicaContato"
            MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords" SelectMethod="GetSelect"
            StartRowIndexParameterName="currentPage" TypeName="MSTech.GestaoEscolar.BLL.ACA_FichaMedicaContatoBO"
            DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" UpdateMethod="Save">
            <DeleteParameters>
                <asp:Parameter Name="entity" Type="Object" />
                <asp:Parameter Name="banco" Type="Object" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter Name="alu_id" Type="Int32" />
                <asp:Parameter DefaultValue="false" Name="paginado" Type="Boolean" />
                <asp:Parameter DefaultValue="1" Name="currentPage" Type="Int32" />
                <asp:Parameter DefaultValue="1" Name="pageSize" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
