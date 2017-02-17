<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCFrequenciaServico.ascx.cs"
    Inherits="GestaoEscolar.WebControls.FrequenciaServico.UCFrequenciaServico" %>
<asp:Label ID="lblFrequencia" runat="server" Text="Frequência *" AssociatedControlID="ddlFrequencia"></asp:Label>
<asp:DropDownList ID="ddlFrequencia" runat="server" SkinID="text60C" AutoPostBack="True"
    OnSelectedIndexChanged="ddlFrequencia_SelectedIndexChanged">
    <asp:ListItem Text="-- Selecione uma frequência --" Value="-1"></asp:ListItem>
    <asp:ListItem Text="Diário" Value="1"></asp:ListItem>
    <asp:ListItem Text="Diário com intervalo de minutos" Value="9"></asp:ListItem>
    <asp:ListItem Text="Diário com intervalo de segundos" Value="10"></asp:ListItem>
    <asp:ListItem Text="Semanal" Value="2"></asp:ListItem>
    <asp:ListItem Text="Mensal" Value="3"></asp:ListItem>
    <asp:ListItem Text="Segunda à Sexta" Value="4"></asp:ListItem>
    <asp:ListItem Text="Sábado e Domingo" Value="5"></asp:ListItem>
    <asp:ListItem Text="Várias vezes ao dia" Value="6"></asp:ListItem>
    <asp:ListItem Text="De hora em hora" Value="7"></asp:ListItem>
    <asp:ListItem Text="Personalizado" Value="8"></asp:ListItem>
</asp:DropDownList>
<asp:CompareValidator ID="cpvFrequencia" runat="server" ErrorMessage="Frequência é obrigatório."
    ControlToValidate="ddlFrequencia" Operator="GreaterThan" ValueToCompare="-1"
    Display="Dynamic" Type="Integer">*</asp:CompareValidator>
<div id="divCheckBoxDiasSemana" visible="false" runat="server">
    <asp:CheckBoxList runat="server" ID="cblDiasSemana">
        <asp:ListItem Text="Domingo" Value="1"></asp:ListItem>
        <asp:ListItem Text="Segunda-feira" Value="2"></asp:ListItem>
        <asp:ListItem Text="Terça-feira" Value="3"></asp:ListItem>
        <asp:ListItem Text="Quarta-feira" Value="4"></asp:ListItem>
        <asp:ListItem Text="Quinta-feira" Value="5"></asp:ListItem>
        <asp:ListItem Text="Sexta-feira" Value="6"></asp:ListItem>
        <asp:ListItem Text="Sábado" Value="7"></asp:ListItem>
    </asp:CheckBoxList>
    <asp:CustomValidator ID="cvCheckBoxDiasSemana" runat="server" ErrorMessage="É necessário escolher ao menos um dia da semana."
        Display="Dynamic" OnServerValidate="cvCheckBoxDiasSemana_ServerValidate">*</asp:CustomValidator>
</div>
<div id="divRadioDiasSemana" visible="false" runat="server">
    <asp:RadioButtonList runat="server" ID="rblDiasSemana">
        <asp:ListItem Text="Domingo" Value="1"></asp:ListItem>
        <asp:ListItem Text="Segunda-feira" Value="2" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Terça-feira" Value="3"></asp:ListItem>
        <asp:ListItem Text="Quarta-feira" Value="4"></asp:ListItem>
        <asp:ListItem Text="Quinta-feira" Value="5"></asp:ListItem>
        <asp:ListItem Text="Sexta-feira" Value="6"></asp:ListItem>
        <asp:ListItem Text="Sábado" Value="7"></asp:ListItem>
    </asp:RadioButtonList>
</div>
<div id="divDiaMes" visible="false" runat="server">
    <asp:Label ID="lblDiaMes" runat="server" Text="Dia do mês *" AssociatedControlID="ddlDiaMes"></asp:Label>
    <asp:DropDownList ID="ddlDiaMes" runat="server" Width="50">
        <asp:ListItem Text="1" Value="1"></asp:ListItem>
        <asp:ListItem Text="2" Value="2"></asp:ListItem>
        <asp:ListItem Text="3" Value="3"></asp:ListItem>
        <asp:ListItem Text="4" Value="4"></asp:ListItem>
        <asp:ListItem Text="5" Value="5"></asp:ListItem>
        <asp:ListItem Text="6" Value="6"></asp:ListItem>
        <asp:ListItem Text="7" Value="7"></asp:ListItem>
        <asp:ListItem Text="8" Value="8"></asp:ListItem>
        <asp:ListItem Text="9" Value="9"></asp:ListItem>
        <asp:ListItem Text="10" Value="10"></asp:ListItem>
        <asp:ListItem Text="11" Value="11"></asp:ListItem>
        <asp:ListItem Text="12" Value="12"></asp:ListItem>
        <asp:ListItem Text="13" Value="13"></asp:ListItem>
        <asp:ListItem Text="14" Value="14"></asp:ListItem>
        <asp:ListItem Text="15" Value="15"></asp:ListItem>
        <asp:ListItem Text="16" Value="16"></asp:ListItem>
        <asp:ListItem Text="17" Value="17"></asp:ListItem>
        <asp:ListItem Text="18" Value="18"></asp:ListItem>
        <asp:ListItem Text="19" Value="19"></asp:ListItem>
        <asp:ListItem Text="20" Value="20"></asp:ListItem>
        <asp:ListItem Text="21" Value="21"></asp:ListItem>
        <asp:ListItem Text="22" Value="22"></asp:ListItem>
        <asp:ListItem Text="23" Value="23"></asp:ListItem>
        <asp:ListItem Text="24" Value="24"></asp:ListItem>
        <asp:ListItem Text="25" Value="25"></asp:ListItem>
        <asp:ListItem Text="26" Value="26"></asp:ListItem>
        <asp:ListItem Text="27" Value="27"></asp:ListItem>
        <asp:ListItem Text="28" Value="28"></asp:ListItem>
        <asp:ListItem Text="29" Value="29"></asp:ListItem>
        <asp:ListItem Text="30" Value="30"></asp:ListItem>
    </asp:DropDownList>
</div>
<div id="divHora" runat="server">
    <asp:Label ID="lblHorario" runat="server" Text="Horário *" AssociatedControlID="txtHora"></asp:Label>
    <asp:TextBox ID="txtHora" Width="40" runat="server" SkinID="Hora"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvHora" runat="server" ControlToValidate="txtHora"
        Display="Dynamic" ErrorMessage="Horário é obrigatório.">*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="revHora" runat="server" ControlToValidate="txtHora"
        ValidationExpression="^([0-1][0-9]|[2][0-3])(:([0-5][0-9])){1,2}$" ErrorMessage="Horário deve estar entre 00:00 e 23:59 no formato HH:mm.">*</asp:RegularExpressionValidator>
</div>
<div id="divVariasHoras" runat="server">
    <br>
    <fieldset style="width: 200px;">
        <legend>Horários</legend>
        <asp:Repeater ID="rptHoras" runat="server" OnItemDataBound="rptHoras_ItemDataBound">
            <ItemTemplate>
                <div>
                    <div style="display:inline-block;">
                        <asp:Label ID="lblHora" runat="server" Text="Hora *" AssociatedControlID="ddlHora"></asp:Label>
                        <asp:DropDownList ID="ddlHora" runat="server" Enabled="true">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="cpvHora" runat="server" ControlToValidate="ddlHora" ValueToCompare="-1"
                            Operator="GreaterThan" Type="Integer" ErrorMessage="Hora é obrigatório." Display="Dynamic"
                            ValidationGroup='<%#VS_ValidationGroup %>' Enabled='<%#VS_ObrigatorioHorario %>'>*</asp:CompareValidator>

                    </div>
                    <div style="display:inline-block;">
                        <asp:Label ID="lblMinuto" runat="server" Text="Minuto *" AssociatedControlID="ddlMinuto"></asp:Label>
                        <asp:DropDownList ID="ddlMinuto" runat="server" Enabled="true" OnSelectedIndexChanged="ddlMinuto_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="cpvMinuto" runat="server" ValueToCompare="-1" Operator="GreaterThan"
                            Type="Integer" ErrorMessage="Minuto é obrigatório." Display="Dynamic" ControlToValidate="ddlMinuto"
                            ValidationGroup='<%#VS_ValidationGroup %>' Enabled='<%#VS_ObrigatorioHorario %>'>*</asp:CompareValidator>
                    </div>
                    
                    <div style="display:inline-block;">
                        <asp:Button ID="btnExcluirHora" runat="server" Text="Excluir" CausesValidation="false"
                            OnClick="btnExcluirHora_Click" />
                    </div>
                    <br />
                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%#Bind("id") %>'></asp:Label>
                    <asp:Label ID="lblHoraDado" runat="server" Visible="false" Text='<%#Bind("hora") %>'></asp:Label>
                    <asp:Label ID="lblMinutoDado" runat="server" Visible="false" Text='<%#Bind("minuto") %>'></asp:Label>
                </div>
                <br />
            </ItemTemplate>
        </asp:Repeater>
        <div class="right">
                <asp:Button ID="btnNovaHora" runat="server" Text="Adicionar horário" OnClick="btnNovaHora_Click"
        CausesValidation="False" />
        </div>
    </fieldset>

</div>
<div id="divHoraEmHora" runat="server">
    <asp:Label ID="lblMinutoHora" runat="server" Text="Minuto *" AssociatedControlID="txtMinutoHora"></asp:Label>
    <asp:TextBox ID="txtMinutoHora" Width="40" runat="server" SkinID="Numerico" MaxLength="2"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvMinutoHora" runat="server" ErrorMessage="Minuto é obrigatório."
        Display="Dynamic" ControlToValidate="txtMinutoHora" ValidationGroup="Servico">*</asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rgMinutoHora" runat="server" ErrorMessage="Minuto deve estar entre 0 e 59."
        Display="Dynamic" ControlToValidate="txtMinutoHora" MaximumValue="59" MinimumValue="0"
        ValidationGroup="Servico">*</asp:RangeValidator>
</div>
<div id="divIntervaloMinutos" runat="server">
    <asp:Label ID="lblIntervaloMinutos" runat="server" Text="Minutos de intervalo *" AssociatedControlID="txtMinutoIntervalo"></asp:Label>
    <asp:TextBox ID="txtMinutoIntervalo" Width="40" runat="server" SkinID="Numerico" MaxLength="2"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvMinutoIntervalo" runat="server" ErrorMessage="Minutos é obrigatório."
        Display="Dynamic" ControlToValidate="txtMinutoIntervalo" ValidationGroup="Servico">*</asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rvrfvMinutoIntervalo" runat="server" ErrorMessage="Minuto deve estar entre 0 e 59."
        Display="Dynamic" ControlToValidate="txtMinutoIntervalo" MaximumValue="59" MinimumValue="0"
        ValidationGroup="Servico">*</asp:RangeValidator>
</div>
<div id="divIntervaloSegundos" runat="server">
    <asp:Label ID="lblIntervaloSegundos" runat="server" Text="Segundos de intervalo *" AssociatedControlID="txtSegundosIntervalo"></asp:Label>
    <asp:TextBox ID="txtSegundosIntervalo" Width="40" runat="server" SkinID="Numerico" MaxLength="2"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvSegundoIntervalo" runat="server" ErrorMessage="Segundos é obrigatório."
        Display="Dynamic" ControlToValidate="txtSegundosIntervalo" ValidationGroup="Servico">*</asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rvrfvSegundosIntervalo" runat="server" ErrorMessage="Segundo deve estar entre 0 e 59."
        Display="Dynamic" ControlToValidate="txtSegundosIntervalo" MaximumValue="59" MinimumValue="0"
        ValidationGroup="Servico">*</asp:RangeValidator>
</div>
