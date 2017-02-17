<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="WebControls_Contato_UCGridContato" Codebehind="UCGridContato.ascx.cs"  %>
<%@ Register Src="../Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>

        
        <asp:Label ID="_lblMessage" runat="server"></asp:Label>
        <asp:GridView ID="grvContato" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
            EmptyDataText="Não existem contatos cadastrados." OnDataBound="grvContato_DataBound" OnRowDataBound="grvContato_RowDataBound">
            <EmptyDataRowStyle Font-Bold="True" ForeColor="Red" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlTipoMeioContato" runat="server" AppendDataBoundItems="True"
                            DataSourceID="odsTipoMeioContato" DataTextField="tmc_nome" DataValueField="tmc_id"
                            SkinID="text30C">
                            <asp:ListItem Value="-1">-- Selecione o tipo --</asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="tbContato" SkinID="text60C" runat="server" Text='<%# Bind("contato") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnAdicionar" runat="server" CausesValidation="false"  SkinID="btNovo"
                            ToolTip="Adicionar" OnClick="btnAdicionar_Click" Visible="False" />
                    </ItemTemplate>
                    <HeaderStyle Width="70px" CssClass="center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsTipoMeioContato" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.SYS_TipoMeioContato"
            MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords" SelectMethod="GetSelect"
            StartRowIndexParameterName="currentPage" TypeName="MSTech.CoreSSO.BLL.SYS_TipoMeioContatoBO"
            DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" UpdateMethod="Save">
            <DeleteParameters>
                <asp:Parameter Name="entity" Type="Object" />
                <asp:Parameter Name="banco" Type="Object" />
            </DeleteParameters>
            <SelectParameters>
                <asp:Parameter DbType="Guid" DefaultValue="00000000-0000-0000-0000-000000000000"
                    Name="tmc_id" />
                <asp:Parameter Name="tmc_nome" Type="String" />
                <asp:Parameter Name="tmc_situacao" Type="Byte" />
                <asp:Parameter DefaultValue="false" Name="paginado" Type="Boolean" />
                <asp:Parameter DefaultValue="1" Name="currentPage" Type="Int32" />
                <asp:Parameter DefaultValue="1" Name="pageSize" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

