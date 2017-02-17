<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_AlunoResponsavel_UCAlunoResponsavel"
    CodeBehind="UCAlunoResponsavel.ascx.cs" %>
<%@ Register Src="../Combos/UCComboEstadoCivil.ascx" TagName="UCComboEstadoCivil"
    TagPrefix="uc2" %>
<%@ Register Src="../Combos/UCComboSexo.ascx" TagName="UCComboSexo" TagPrefix="uc3" %>
<%@ Register Src="../Combos/UCComboTipoEscolaridade.ascx" TagName="UCComboTipoEscolaridade"
    TagPrefix="uc6" %>
<%@ Register Src="../Busca/UCPessoasAluno.ascx" TagName="UCBuscaPessoasAluno" TagPrefix="uc7" %>
<style type="text/css">
    .style1
    {
        height: 63px;
    }
</style>
<asp:Label ID="lblMensagem" runat="server" EnableViewState="false"></asp:Label>
<table>
    <tr>
        <td colspan="2">
            <asp:Label ID="lblNome" runat="server" Text="Nome do responsável *" AssociatedControlID="txtNome">
            </asp:Label>
            <asp:TextBox ID="txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
            <asp:ImageButton ID="_btnPesquisarPessoa" runat="server" CausesValidation="False"
                SkinID="btPesquisar" OnClientClick="$('#divBuscaResponsavel').dialog('open');"
                OnClick="_btnPesquisarPessoa_Click" />
            <asp:RequiredFieldValidator ID="rfvNome" ControlToValidate="txtNome" ValidationGroup="AlunoResponsavel"
                runat="server" ErrorMessage="Nome é obrigatório.">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td width="285">
            <asp:Label ID="LabelDataNasc" runat="server" Text="Data de nascimento" AssociatedControlID="txtDataNasc"></asp:Label>
            <asp:TextBox ID="txtDataNasc" runat="server" MaxLength="10" SkinID="DataSemCalendario"></asp:TextBox>
            <asp:CompareValidator ID="_cpvDataNasc" runat="server" ControlToValidate="txtDataNasc"
                Display="Dynamic" ErrorMessage="Data de nascimento inválida." Operator="DataTypeCheck"
                Type="Date">*</asp:CompareValidator>
            <asp:CustomValidator ID="cvDataNascimento" runat="server" ControlToValidate="txtDataNasc"
                ValidationGroup="AlunoResponsavel" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">*</asp:CustomValidator>
        </td>
        <td id="tdNis" runat="server">
            <asp:Label ID="LabelNis" runat="server" Text="NIS" AssociatedControlID="txtNis"></asp:Label>
            <asp:TextBox ID="txtNis" runat="server" MaxLength="11" SkinID="numerico"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td width="285">
            <uc2:UCComboEstadoCivil ID="UCComboEstadoCivil1" runat="server" />
        </td>
        <td>
            <asp:Label ID="lblCPFResponsavel" runat="server" Text="CPF" AssociatedControlID="txtCPFResponsavel"></asp:Label>
            <asp:TextBox ID="txtCPFResponsavel" runat="server" SkinID="Numerico" MaxLength="11"
                Width="240px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td width="285" class="style1">
            <uc6:UCComboTipoEscolaridade ID="UCComboTipoEscolaridade1" runat="server" />
        </td>
        <td class="style1">
            <asp:Label ID="lblRGResponsavel" runat="server" Text="RG" AssociatedControlID="txtRGResponsavel"></asp:Label>
            <asp:TextBox ID="txtRGResponsavel" runat="server" SkinID="text30C" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="chkSituacaoFalecido" runat="server" Text="Falecido" />
            <asp:CheckBox ID="chkNaoConstaCertidaoNasc" runat="server" Text="Não consta na certidão de nascimento" />
            <asp:CheckBox ID="chkOmitidoFormaLei" runat="server" Text="Omitido na forma da lei" />
            <asp:CheckBox ID="chkApenasFiliacao" runat="server" Text="Apenas filiação, não responsável" />
            <asp:CheckBox ID="chkMoraComAluno" runat="server" Text="Mora com o aluno" />
        </td>
        <td>
            <uc3:UCComboSexo ID="UCComboSexo1" runat="server" />
            <asp:Label ID="LabelProfissao" runat="server" Text="Profissão" AssociatedControlID="txtProfissao"></asp:Label>
            <asp:TextBox ID="txtProfissao" runat="server" MaxLength="200" SkinID="text30C"></asp:TextBox>
        </td>
    </tr>
</table>
