<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_Contato_UCContato"
    CodeBehind="UCContato.ascx.cs" %>
<%@ Register Src="../Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:UpdatePanel ID="updGridContatos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <uc1:UCLoader ID="UCLoader" runat="server" AssociatedUpdatePanelID="updGridContatos" />
        <asp:Label ID="lblMensagemErroContato" runat="server"></asp:Label>
        <asp:Repeater ID="rptTipoContato" runat="server" OnItemDataBound="rptTipoContato_ItemDataBound"
            OnItemCommand="rptTipoContato_ItemCommand">
            <HeaderTemplate>
                <table class="grid" cellspacing="0" style="border-style:none;border-collapse: collapse;">
                    <tr class="gridHeader">
                        <th>
                            <asp:Label ID="lblTipoContato" runat="server" Text='Tipo de contato'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="lblContato" runat="server" Text='Contato'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="lblLimpar" runat="server" Text=''></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="lblAdicionar" runat="server" Text=''></asp:Label>
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="gridRow">
                    <td>
                        <asp:HiddenField ID="lblBanco" runat="server" Value='<%# Bind("banco") %>' />
                        <asp:HiddenField ID="lbl_psc_id" runat="server" Value='<%# Bind("id") %>' />
                        <asp:HiddenField ID="lbl_tmc_id" runat="server" Value='<%# Bind("tmc_id") %>' />
                        <asp:HiddenField ID="lbl_tmc_validacao" runat="server" Value='<%# Bind("tmc_validacao") %>' />
                        <asp:Label ID="lbl_tmc_nome" runat="server" Text='<%# Bind("tmc_nome") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txt_psc_contato" runat="server" Text='<%# Bind("contato") %>' ValidationGroup='<%# _VS_ValidationGroup %>'></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revGenerico" runat="server" ControlToValidate="txt_psc_contato"
                            ValidationGroup='<%# _VS_ValidationGroup %>' Display="Dynamic" Visible="false">*</asp:RegularExpressionValidator>
                    </td>
                    <td width="70px" style="text-align: center;">
                        <asp:ImageButton ID="btnLimpar" runat="server" CausesValidation="False" SkinID="btLimpar"
                            ToolTip='<%# Eval("tmc_nome","Limpar {0}")%>' CommandName="Limpar" />
                    </td>
                    <td width="70px" style="text-align: center;">
                        <asp:ImageButton ID="btnAdicionar" runat="server" CausesValidation="False" SkinID="btNovo"
                            ToolTip='<%# Eval("tmc_nome","Adicionar {0}")%>' CommandName="Adicionar" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="gridAlternatingRow">
                    <td>
                        <asp:HiddenField ID="lblBanco" runat="server" Value='<%# Bind("banco") %>' />
                        <asp:HiddenField ID="lbl_psc_id" runat="server" Value='<%# Bind("id") %>' />
                        <asp:HiddenField ID="lbl_tmc_id" runat="server" Value='<%# Bind("tmc_id") %>' />
                        <asp:HiddenField ID="lbl_tmc_validacao" runat="server" Value='<%# Bind("tmc_validacao") %>' />
                        <asp:Label ID="lbl_tmc_nome" runat="server" Text='<%# Bind("tmc_nome") %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="txt_psc_contato" runat="server" Text='<%# Bind("contato") %>' ValidationGroup='<%# _VS_ValidationGroup %>'></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revGenerico" runat="server" ControlToValidate="txt_psc_contato"
                            ValidationGroup='<%# _VS_ValidationGroup %>' Display="Dynamic" Visible="false">*</asp:RegularExpressionValidator>
                    </td>
                    <td width="70px" style="text-align: center;">
                        <asp:ImageButton ID="btnLimpar" runat="server" CausesValidation="False" SkinID="btLimpar"
                            ToolTip='<%# Eval("tmc_nome","Limpar {0}")%>' CommandName="Limpar" />
                    </td>
                    <td width="70px" style="text-align: center;">
                        <asp:ImageButton ID="btnAdicionar" runat="server" CausesValidation="False" SkinID="btNovo"
                            ToolTip='<%# Eval("tmc_nome","Adicionar {0}")%>' CommandName="Adicionar" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
