<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="Academico_RecursosHumanos_TipoVinculo_Cadastro" Title="Untitled Page"
    CodeBehind="Cadastro.aspx.cs" %>

<%@ PreviousPageType VirtualPath="~/Academico/RecursosHumanos/TipoVinculo/Busca.aspx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" />
    <fieldset id="fdsVinculo" runat="server">
        <legend>Cadastro de tipo de vínculo</legend>
        <asp:Label ID="lblCampoObrigatorio" runat="server" Text="Os campos marcados com asterisco (*) são obrigatórios."
            Font-Size="Small" ForeColor="Red"></asp:Label>
        <asp:Label ID="_lblNome" runat="server" Text="Nome do tipo de vínculo *" AssociatedControlID="_txtNome"></asp:Label>
        <asp:TextBox ID="_txtNome" runat="server" SkinID="text60C" MaxLength="100" Enabled="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvNome" runat="server" Display="Dynamic" ErrorMessage="Nome do tipo de vínculo é obrigatório."
            ControlToValidate="_txtNome">*</asp:RequiredFieldValidator>
        <asp:Label ID="_lblDescricao" runat="server" Text="Descrição do tipo de vínculo"
            AssociatedControlID="_txtDescricao"></asp:Label>
        <asp:TextBox ID="_txtDescricao" runat="server" TextMode="MultiLine" SkinID="limite4000" Enabled="false"></asp:TextBox>
        <asp:Label ID="_lblHrsSemanais" runat="server" Text="Horas semanais *" AssociatedControlID="_txtHrsSemanais"></asp:Label>
        <asp:TextBox ID="_txtHrsSemanais" runat="server" MaxLength="3" CssClass="numeric"
            SkinID="Numerico" Enabled="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvHrsSemanais" runat="server" Display="Dynamic"
            ErrorMessage="Horas semanais é obrigatório." ControlToValidate="_txtHrsSemanais">*</asp:RequiredFieldValidator>
        <asp:CompareValidator ID="_cpvhorasSemanais" runat="server" ErrorMessage="Horas semanais não pode ser maior do que 168."
            Display="Dynamic" ControlToValidate="_txtHrsSemanais" Operator="LessThanEqual"
            ValueToCompare="168" Type="Integer">*</asp:CompareValidator>
        <asp:CompareValidator ID="_cpvHorasSemanais2" runat="server" ErrorMessage="Horas semanais deve ser um número inteiro maior que 0 (zero)."
            Display="Dynamic" ControlToValidate="_txtHrsSemanais" Operator="GreaterThan"
            ValueToCompare="0" Type="Integer">*</asp:CompareValidator>
        <asp:Label ID="_lblMinAlmoco" runat="server" Text="Minutos de almoço *" AssociatedControlID="_txtMinAlmoco"></asp:Label>
        <asp:TextBox ID="_txtMinAlmoco" runat="server" MaxLength="3" CssClass="numeric" SkinID="Numerico" Enabled="false"></asp:TextBox>
        <asp:RequiredFieldValidator ID="_rfvMinAlmoco" runat="server" ErrorMessage="Minutos de almoço é obrigatório."
            ControlToValidate="_txtMinAlmoco" Display="Dynamic">*</asp:RequiredFieldValidator>
        <asp:CompareValidator ID="cpvMinAlmoco" runat="server" ErrorMessage="Minutos de almoço não pode ser maior do que 1440."
            Display="Dynamic" ControlToValidate="_txtMinAlmoco" Operator="LessThanEqual"
            ValueToCompare="1440" Type="Integer">*</asp:CompareValidator>
        <asp:CompareValidator ID="cpvMinAlmoco2" runat="server" ErrorMessage="Minutos de almoço deve ser um número inteiro maior que 0 (zero)."
            Display="Dynamic" ControlToValidate="_txtMinAlmoco" Operator="GreaterThan" ValueToCompare="0"
            Type="Integer">*</asp:CompareValidator>
        <asp:Label ID="_lblHrMinEntrada" runat="server" Text="Horário mínimo de entrada"
            AssociatedControlID="_txtHrMinEntrada"></asp:Label>
        <asp:TextBox ID="_txtHrMinEntrada" runat="server" SkinID="Hora" Enabled="false"></asp:TextBox>
        <asp:RegularExpressionValidator ID="_revHrMinEntrada" runat="server" ErrorMessage="Horário mínimo de entrada deve estar entre 00:00 e 23:59"
            ValidationExpression="(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]" ControlToValidate="_txtHrMinEntrada">*</asp:RegularExpressionValidator>
        <asp:Label ID="_lblHrMaxSaida" runat="server" Text="Horário máximo de saída" AssociatedControlID="_txtHrMaxSaida"></asp:Label>
        <asp:TextBox ID="_txtHrMaxSaida" runat="server" SkinID="Hora" Enabled="false"></asp:TextBox>
        <asp:RegularExpressionValidator ID="_rexHrMaxSaida" runat="server" ErrorMessage="Horário máximo de saída deve estar entre 00:00 e 23:59"
            ValidationExpression="(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]" ControlToValidate="_txtHrMaxSaida">*</asp:RegularExpressionValidator>
        <asp:CustomValidator ID="CvValidaIntervalo" runat="server" ErrorMessage="Horário mínimo de entrada deve ser menor que o horário máximo de saída."
            ControlToValidate="_txtHrMinEntrada" OnServerValidate="CvValidaIntervalo_ServerValidate"
            Display="Dynamic">*</asp:CustomValidator>
        <asp:Label ID="_lblCodigoIntegracao" runat="server" Text="Código de integração" AssociatedControlID="_txtCodigoIntegracao"></asp:Label>
        <asp:TextBox ID="_txtCodigoIntegracao" runat="server" SkinID="text20C" MaxLength="20" Enabled="false"></asp:TextBox>
        <asp:CheckBox ID="_ckbBloqueado" Text="Bloqueado" runat="server" Enabled="false" />
        <div class="right">
            <asp:Button ID="_btnCancelar" runat="server" Text="Voltar" OnClick="_btnCancelar_Click"
                CausesValidation="false" />
        </div>
    </fieldset>
</asp:Content>
