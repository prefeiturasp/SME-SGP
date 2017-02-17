<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_Turno_Cadastro" EnableViewState="true" CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/Turno/Busca.aspx" %>
<%@ Register Src="~/WebControls/Combos/UCComboTipoTurno.ascx" TagName="_UCComboTipoTurno"
    TagPrefix="uc1" %>
<%@ Register Src="../../WebControls/FiltroEscolas/UCFiltroEscolas.ascx" TagName="UCFiltroEscolas"
    TagPrefix="uc2" %>
<%@ Register Src="../../WebControls/Mensagens/UCCamposObrigatorios.ascx" TagName="UCCamposObrigatorios"
    TagPrefix="uc3" %>
<%@ Register Src="../../WebControls/Mensagens/UCLoader.ascx" TagName="UCLoader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="_ValidationTurno" />
    <fieldset>
        <legend>Cadastro de turnos</legend>
        <uc3:UCCamposObrigatorios ID="UCCamposObrigatorios1" runat="server" />
        <uc1:_UCComboTipoTurno ID="_UCComboTipoTurno" runat="server" PermiteEditar="false" />
        <asp:Label ID="_lblDescricao" runat="server" Text="Descrição do turno *" AssociatedControlID="_txtDescricao"></asp:Label>
        <asp:TextBox ID="_txtDescricao" runat="server" MaxLength="200" SkinID="text60C" Enabled="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvDescricao" ControlToValidate="_txtDescricao"
            runat="server" ErrorMessage="Descrição do turno é obrigatório." ValidationGroup="_ValidationTurno">*</asp:RequiredFieldValidator>
        <asp:UpdatePanel ID="updcontroleTempo" UpdateMode="Always" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblcontroleTempo" runat="server" Text="Controle de horas/aulas" AssociatedControlID="ddlcontroleTempo"></asp:Label>
                <asp:DropDownList ID="ddlcontroleTempo" runat="server" SkinID="text30C" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlcontroleTempo_SelectedIndexChanged" Enabled="false">
                    <asp:ListItem Text="Tempos de aula" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Horas" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblHoraInicio" runat="server" Text="Hora do início do turno *" AssociatedControlID="txtHoraInicio"></asp:Label>
                <asp:TextBox ID="txtHoraInicio" runat="server" SkinID="Hora" Enabled="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvHoraInicio" runat="server" ControlToValidate="txtHoraInicio"
                    Display="Dynamic" ErrorMessage="Hora do início do turno é obrigatória." ValidationGroup="_ValidationTurno">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revHoraInicio" runat="server" ControlToValidate="txtHoraInicio"
                    ValidationExpression="^([0-1][0-9]|[2][0-3])(:([0-5][0-9])){1,2}$" ErrorMessage="Hora do início do turno deve estar entre 00:00 e 23:59 no formato HH:mm."
                    ValidationGroup="_ValidationTurno">*</asp:RegularExpressionValidator>
                <asp:Label ID="lblHoraFim" runat="server" Text="Hora do fim do turno  *" AssociatedControlID="txtHoraFim"></asp:Label>
                <asp:TextBox ID="txtHoraFim" runat="server" SkinID="Hora" Enabled="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvHoraFim" runat="server" ControlToValidate="txtHoraFim"
                    Display="Dynamic" ErrorMessage="Hora do fim do turno é obrigatória." ValidationGroup="_ValidationTurno">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revHoraFim" runat="server" ControlToValidate="txtHoraFim"
                    ValidationExpression="^([0-1][0-9]|[2][0-3])(:([0-5][0-9])){1,2}$" ErrorMessage="Hora do fim do turno deve estar entre 00:00 e 23:59 no formato HH:mm."
                    ValidationGroup="_ValidationTurno">*</asp:RegularExpressionValidator>
                <asp:CompareValidator ID="cpvHorario" runat="server" ErrorMessage="Hora do fim do turno deve ser maior que hora do início do turno."
                    ControlToCompare="txtHoraInicio" ControlToValidate="txtHoraFim" Operator="GreaterThan"
                    Type="String" ValidationGroup="_ValidationTurno">*</asp:CompareValidator>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:CheckBox ID="_ckbBloqueado" Text="Bloqueado" runat="server" Enabled="false" />
    </fieldset>
    <fieldset id="fdsDiasSemana" runat="server">
        <legend>
            <asp:Label ID="lblDiasSemana" runat="server" Text="Dias da semana *" />
        </legend>
        <asp:CheckBoxList ID="chkDiasSemana" runat="server" RepeatDirection="Horizontal"
            CellPadding="20" CellSpacing="20" Enabled="false">
            <asp:ListItem Value="1">Domingo</asp:ListItem>
            <asp:ListItem Value="2">Segunda-feira</asp:ListItem>
            <asp:ListItem Value="3">Terça-feira</asp:ListItem>
            <asp:ListItem Value="4">Quarta-feira</asp:ListItem>
            <asp:ListItem Value="5">Quinta-feira</asp:ListItem>
            <asp:ListItem Value="6">Sexta-feira</asp:ListItem>
            <asp:ListItem Value="7">Sábado</asp:ListItem>
        </asp:CheckBoxList>
    </fieldset>
    <fieldset>
        <legend>Cadastro de horários</legend>
        <asp:UpdatePanel ID="updGridHorario" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:UCLoader ID="UCLoader1" runat="server" AssociatedUpdatePanelID="updGridHorario" />
                <asp:Label ID="_lblMessageHorario" runat="server" EnableViewState="false"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Horario" />
                <table class="grid" cellspacing="0" style="width: 100%">
                    <tr class="gridHeader">
                        <th>
                            <asp:Label ID="_lblHoraInicial" runat="server" Text="Hora inicial *"></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="_lblHoraFinal" runat="server" Text="Hora final *"></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="_lblTipoHorario" runat="server" Text="Tipo de horário *"></asp:Label>
                        </th>
                        <th></th>
                    </tr>
                    <asp:Repeater ID="_rptHorarios" runat="server" OnItemDataBound="_rptTipoContato_ItemDataBound">
                        <ItemTemplate>
                            <tr class="gridRow">
                                <td class="hide">
                                    <asp:Label ID="banco" runat="server" Text='<%# Bind("banco") %>'></asp:Label>
                                </td>
                                <td class="hide">
                                    <asp:Label ID="trh_id" runat="server" Text='<%# Bind("trh_id") %>'></asp:Label>
                                </td>
                                <td class="hide">
                                    <asp:Label ID="trh_tipo" runat="server" Text='<%# Bind("trh_tipo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="_txtHoraInicial" runat="server" SkinID="Hora" Text='<%# Bind("trh_horaInicio") %>'
                                        ValidationGroup="Horario" Enabled="false"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="_rfvHoraInicial" runat="server" ControlToValidate="_txtHoraInicial"
                                        Display="Dynamic" ErrorMessage="Hora inicial é obrigatório." ValidationGroup="Horario">*</asp:RequiredFieldValidator>
                                    <asp:Label ID="_lblErroHoraInicial" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="_txtHoraFinal" runat="server" SkinID="Hora" Text='<%# Bind("trh_horaFim") %>'
                                        ValidationGroup="Horario" Enabled="false"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="_rfvHoraFinal" runat="server" ControlToValidate="_txtHoraFinal"
                                        Display="Dynamic" ErrorMessage="Hora final é obrigatório." ValidationGroup="Horario">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cpvHorario" runat="server" ErrorMessage="Hora final deve ser maior que hora inicial."
                                        ControlToCompare="_txtHoraInicial" ControlToValidate="_txtHoraFinal" Operator="GreaterThan"
                                        Type="String" ValidationGroup="Horario">*</asp:CompareValidator>
                                    <asp:Label ID="_lblErroHoraFinal" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="_ddlTipoHorario" runat="server" Enabled="false">
                                        <asp:ListItem Value="0" Text="-- Selecione um tipo de horário --" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Aula normal"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Aula fora do período do turno"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Intervalo entre aulas"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Intervalo entre períodos"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="_cvTipoHorario" runat="server" ErrorMessage="Tipo de horário é obrigatório."
                                        ControlToValidate="_ddlTipoHorario" Operator="GreaterThan" ValueToCompare="0"
                                        Display="Dynamic" Visible="True" ValidationGroup="Horario">*</asp:CompareValidator>
                                    <asp:Label ID="_lblErroTipoHorario" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                                </td>
                                <td></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="gridAlternatingRow">
                                <td class="hide">
                                    <asp:Label ID="banco" runat="server" Text='<%# Bind("banco") %>'></asp:Label>
                                </td>
                                <td class="hide">
                                    <asp:Label ID="trh_id" runat="server" Text='<%# Bind("trh_id") %>'></asp:Label>
                                </td>
                                <td class="hide">
                                    <asp:Label ID="trh_tipo" runat="server" Text='<%# Bind("trh_tipo") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="_txtHoraInicial" runat="server" SkinID="Hora" Text='<%# Bind("trh_horaInicio") %>'
                                        ValidationGroup="Horario" Enabled="false"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="_rfvHoraInicial" runat="server" ControlToValidate="_txtHoraInicial"
                                        Display="Dynamic" ErrorMessage="Hora inicial é obrigatório." ValidationGroup="Horario">*</asp:RequiredFieldValidator>
                                    <asp:Label ID="_lblErroHoraInicial" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="_txtHoraFinal" runat="server" SkinID="Hora" Text='<%# Bind("trh_horaFim") %>'
                                        ValidationGroup="Horario" Enabled="false"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="_rfvHoraFinal" runat="server" ControlToValidate="_txtHoraFinal"
                                        Display="Dynamic" ErrorMessage="Hora final é obrigatório." ValidationGroup="Horario">*</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cpvHorario" runat="server" ErrorMessage="Hora final deve ser maior que hora inicial."
                                        ControlToCompare="_txtHoraInicial" ControlToValidate="_txtHoraFinal" Operator="GreaterThan"
                                        Type="String" ValidationGroup="Horario">*</asp:CompareValidator>
                                    <asp:Label ID="_lblErroHoraFinal" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="_ddlTipoHorario" runat="server" Enabled="false">
                                        <asp:ListItem Value="0" Text="-- Selecione um tipo de horário --" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Aula normal"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Aula fora do período do turno"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Intervalo entre aulas"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Intervalo entre períodos"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="_cvTipoHorario" runat="server" ErrorMessage="Tipo de horário é obrigatório."
                                        ControlToValidate="_ddlTipoHorario" Operator="GreaterThan" ValueToCompare="0"
                                        Display="Dynamic" Visible="True" ValidationGroup="Horario">*</asp:CompareValidator>
                                    <asp:Label ID="_lblErroTipoHorario" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                                </td>
                                <td></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
    <fieldset>
        <div class="right">
            <asp:Button ID="_btnCancelar" runat="server" Text="Voltar" OnClick="_btnCancelar_Click"
                CausesValidation="false" />
        </div>
    </fieldset>
</asp:Content>
