<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCertidaoCivil.ascx.cs"
    Inherits="GestaoEscolar.WebControls.CertidaoCivil.UCCertidaoCivil" %>
<asp:RadioButtonList ID="rblTipoCertidao" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
    OnSelectedIndexChanged="rblTipoCertidao_SelectedIndexChanged">
    <asp:ListItem Value="1" Selected="True">Certidão de nascimento</asp:ListItem>
    <asp:ListItem Value="2">Certidão de casamento</asp:ListItem>
</asp:RadioButtonList>
<fieldset>
    <legend><asp:Label ID="lblCertidao" runat="server" Text="Certidão de nascimento"></asp:Label></legend>
    <table>
        <tr>
            <td colspan="4">
                <asp:ImageButton ID="btnLimparCertidao" runat="server" Visible="true" SkinID="btLimpar"
                    ToolTip="Limpar certidão" CausesValidation="false" TabIndex="10" OnClick="btnLimparCertidao_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMatricula" runat="server" Text="Matrícula" AssociatedControlID="txtMatricula"></asp:Label>
                <asp:TextBox ID="txtMatricula" runat="server" SkinID="Numerico" MaxLength="32" Width="290"></asp:TextBox>
            </td>
            <td>
                 <asp:CheckBox ID="chbGemeos" runat="server" Text="Gêmeos" Visible="false" />
            </td>
        </tr>
        <tr>
            <td width="245px">
                <asp:Label ID="Label9" runat="server" Text="Número do termo" AssociatedControlID="tbNumTerm"></asp:Label>
                <asp:TextBox ID="tbNumTerm" runat="server" MaxLength="50" SkinID="text30C"></asp:TextBox>
            </td>
            <td width="185px">
                <asp:Label ID="Label10" runat="server" Text="Folha" AssociatedControlID="tbFolha"></asp:Label>
                <asp:TextBox ID="tbFolha" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
            </td>
            <td width="185px">
                <asp:Label ID="Label11" runat="server" Text="Livro" AssociatedControlID="tbLivro"></asp:Label>
                <asp:TextBox ID="tbLivro" runat="server" MaxLength="20" SkinID="text20C"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label12" runat="server" Text="Data emissão" AssociatedControlID="tbDtEmissao"></asp:Label>
                <asp:TextBox ID="tbDtEmissao" runat="server" SkinID="DataSemCalendario"></asp:TextBox>
                <asp:CustomValidator ID="cvDtEmissao" runat="server" ControlToValidate="tbDtEmissao"
                    Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
                <asp:CustomValidator ID="cpvDtEmissao" runat="server" Display="Dynamic" ValidationGroup="Aluno"
                    ControlToValidate="tbDtEmissao" ErrorMessage="A data de emissão da certidão não pode ser maior que a data atual."
                    OnServerValidate="ValidarDatas_ServerValidate">*</asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label13" runat="server" Text="Nome do Cartório" AssociatedControlID="tbNomeCart"></asp:Label>
                <asp:TextBox ID="tbNomeCart" runat="server" MaxLength="200" SkinID="text30C"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label14" runat="server" Text="Distrito" AssociatedControlID="tbDistritoCart"></asp:Label>
                <asp:TextBox ID="tbDistritoCart" runat="server" MaxLength="100" SkinID="text20C"></asp:TextBox>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label15" runat="server" Text="Cidade" AssociatedControlID="tbCidadeCart"></asp:Label>
                            <asp:TextBox ID="tbCidadeCart" runat="server" CssClass="tbCidadeCertidao_incremental"
                                Style="vertical-align: top; width: 160px;"></asp:TextBox>
                            <input id="_txtCid_id" runat="server" type="hidden" class="tbCid_idCertidao_incremental" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnCadastraCidade" runat="server" SkinID="btNovo" CausesValidation="false"
                                OnClick="btnCadastraCidade_Click" OnClientClick="$('#divCadastroCidade').dialog('open'); return true;"
                                Style="vertical-align: middle;" ToolTip="Cadastrar nova cidade" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Label ID="Label16" runat="server" Text="UF" AssociatedControlID="ddlUF"></asp:Label>
                <asp:DropDownList ID="ddlUF" runat="server" DataTextField="unf_nome" DataValueField="unf_id"
                    AppendDataBoundItems="True">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</fieldset>