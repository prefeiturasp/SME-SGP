<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCParametroFormacaoTurmas.ascx.cs"
    Inherits="WebControls_ParametroFormacaoTurmas_UCParametroFormacaoTurmas" %>
<%@ Register Src="~/WebControls/Combos/UCComboCalendario.ascx" TagName="UCComboCalendario"
    TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboFormatoAvaliacao.ascx" TagName="UCComboFormatoAvaliacao"
    TagPrefix="uc2" %>
<fieldset>
    <table style="display: inline-block; float: left;">
        <tr>
            <td colspan="3">
                <uc1:UCComboCalendario ID="UCComboCalendario" runat="server" Obrigatorio="true" ValidationGroup="Parametro"
                    OnIndexChanged="UCComboCalendario_IndexChanged" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <uc2:UCComboFormatoAvaliacao ID="UCComboFormatoAvaliacao" runat="server" Obrigatorio="true"
                    ValidationGroup="Parametro" OnIndexChanged="UCComboFormatoAvaliacao_IndexChanged" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTipoDigitoCodigoTurma" runat="server" Text="Tipo do código da turma *"
                    AssociatedControlID="ddlTipoDigitoCodigoTurma"></asp:Label>
                <asp:DropDownList ID="ddlTipoDigitoCodigoTurma" AutoPostBack="true" runat="server" Width="170px" 
                    onselectedindexchanged="ddlTipoDigitoCodigoTurma_SelectedIndexChanged">
                    <asp:ListItem Value="-1" Text="-- Selecione um tipo --"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Numérico"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Alfabético"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Sem controle automático"></asp:ListItem>
                </asp:DropDownList>
                <asp:CompareValidator ID="cpvTipoDigitoCodigoTurma" runat="server" ControlToValidate="ddlTipoDigitoCodigoTurma"
                    Operator="GreaterThan" ValueToCompare="0" ErrorMessage="Tipo do código da turma é obrigatório."
                    Visible="false" ValidationGroup="Parametro" Display="Dynamic">*</asp:CompareValidator>
            </td>
            <td align="center">
                <div runat="server" id="DivPrefixoCodigoTurma">
                    <asp:Label ID="lblPrefixoCodigoTurma" runat="server" Text="Prefixo do código" AssociatedControlID="txtPrefixoCodigoTurma"></asp:Label>
                    <asp:TextBox ID="txtPrefixoCodigoTurma" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                </div>
            </td>
            <td align="center">
                <div id="DivQtdDigitoCodigoTurma" runat="server">
                    <asp:Label ID="lblQtdDigitoCodigoTurma" runat="server" Text="Qtd. de dígitos *" AssociatedControlID="txtQtdDigitoCodigoTurma"></asp:Label>
                    <asp:TextBox ID="txtQtdDigitoCodigoTurma" runat="server" SkinID="Numerico" Width="100px"
                        MaxLength="2"></asp:TextBox>
                    <asp:RangeValidator ID="rvQtdDigitoCodigoTurma" runat="server" ControlToValidate="txtQtdDigitoCodigoTurma"
                        ErrorMessage="Qtd. de dígitos deve estar entre 1 e 20." Visible="false" ValidationGroup="Parametro"
                        Display="Dynamic" MinimumValue="1" MaximumValue="20" Type="Integer">*</asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="rfvQtdDigitoCodigoTurma" runat="server" ControlToValidate="txtQtdDigitoCodigoTurma"
                        ErrorMessage="Qtd. de dígitos é obrigatório." Visible="false" ValidationGroup="Parametro"
                        Display="Dynamic">*</asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblControleCapacidade" runat="server" Text="Controle de capacidade *"
                    AssociatedControlID="ddlControleCapacidade"></asp:Label>
                <asp:DropDownList ID="ddlControleCapacidade" runat="server" Width="480px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCapacidade" runat="server" Text="Capacidade *" AssociatedControlID="txtCapacidade"></asp:Label>
                <asp:TextBox ID="txtCapacidade" runat="server" SkinID="Numerico" Width="100px" MaxLength="4"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCapacidade" runat="server" ControlToValidate="txtCapacidade"
                    ErrorMessage="Capacidade é obrigatório." ValidationGroup="Parametro" Display="Dynamic">*</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Label ID="lblQtdDeficiente" runat="server" AssociatedControlID="txtQtdDeficiente"></asp:Label>
                <asp:TextBox ID="txtQtdDeficiente" runat="server" SkinID="Numerico" Width="100px"
                    MaxLength="4"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvQtdDeficiente" runat="server" ControlToValidate="txtQtdDeficiente"
                    ErrorMessage="<%$ Resources:WebControls, ParametroFormacaoTurmas.UCParametroFormacaoTurmas.rfvQtdDeficiente.ErrorMessage %>" 
                    ValidationGroup="Parametro" Display="Dynamic">*</asp:RequiredFieldValidator>
            </td>
            <td style="padding-right: 15px;">
                <asp:Label ID="lblCapacidadeComDeficiente" runat="server" AssociatedControlID="txtCapacidadeComDeficiente"></asp:Label>
                <asp:TextBox ID="txtCapacidadeComDeficiente" runat="server" SkinID="Numerico" Width="100px"
                    MaxLength="4"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCapacidadeComDeficiente" runat="server" ControlToValidate="txtCapacidadeComDeficiente"
                    ErrorMessage="<%$ Resources:WebControls, ParametroFormacaoTurmas.UCParametroFormacaoTurmas.rfvCapacidadeComDeficiente.ErrorMessage %>" 
                    ValidationGroup="Parametro" Display="Dynamic">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div id="divCapacidades" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblTiposDeficienciaAlunoIncluidos" runat="server" 
                    Text="<%$ Resources:WebControls, ParametroFormacaoTurmas.UCParametroFormacaoTurmas.lblTiposDeficienciaAlunoIncluidos.Text %>"
                    AssociatedControlID="ddlTiposDeficienciaAlunoIncluidos"></asp:Label>
                <asp:DropDownList ID="ddlTiposDeficienciaAlunoIncluidos" runat="server" Width="480px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:CheckBox ID="chkDocenteEspecialista" runat="server" Text="Turma de docente especialista"
                    AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkDocenteEspecialista_CheckedChanged" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnIdCapac" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnCapacidades" runat="server"></asp:HiddenField>
    <fieldset style="display: inline-block; width: 280px; height: 160px; margin-left: 20px;">
        <legend>Turnos <span style="color: Red;">*</span></legend>
        <asp:Label ID="ltrMensagemVazio" runat="server"></asp:Label>
        <div id="divTurnos" runat="server" style="height: 120px; overflow: auto;">
            <asp:CheckBoxList ID="cblTurnos" runat="server" DataTextField="trn_descricao" DataValueField="trn_id">
            </asp:CheckBoxList>
        </div>
    </fieldset>
    <br />
    <fieldset id="fdsTipoDeficiencia" runat="server" style="display: none; width: 280px;
        height: 160px; margin-left: 20px;">
        <legend>

            <asp:Label runat="server" ID="lblLegend" 
                Text="<%$ Resources:WebControls, ParametroFormacaoTurmas.UCParametroFormacaoTurmas.lblLegend.Text %>"></asp:Label>

            <span style="color: Red;">*</span>
        </legend>
        <div id="divTipoDeficiencia" runat="server" style="height: 120px; overflow: auto;">
            <asp:CheckBoxList ID="cblTiposDeficiencia" runat="server" DataValueField="tde_id"
                DataTextField="tde_nome">
            </asp:CheckBoxList>
        </div>
    </fieldset>
</fieldset>
