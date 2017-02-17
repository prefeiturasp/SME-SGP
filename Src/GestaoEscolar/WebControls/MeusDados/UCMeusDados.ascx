<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_MeusDados_UCMeusDados" CodeBehind="UCMeusDados.ascx.cs" %>
<asp:Label ID="lblInformacao" runat="server"></asp:Label>
<asp:Label ID="lblMessage" runat="server" EnableViewState="False" AssociatedControlID="ValidationSummary1"></asp:Label>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="MeusDados" Style="display: none;" />
<fieldset>
    <legend>Meus dados</legend>
    <asp:Label ID="lblLogin" runat="server" Text="Login" AssociatedControlID="txtLogin"></asp:Label>
    <asp:TextBox ID="txtLogin" runat="server" SkinID="text30C" Enabled="false"></asp:TextBox>
    <asp:Label ID="lblEmail" runat="server" Text="E-mail" AssociatedControlID="txtEmail"></asp:Label>
    <asp:TextBox ID="txtEmail" runat="server" SkinID="text60C"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="E-mail é obrigatório."
        Display="Dynamic" ValidationGroup="MeusDados">*</asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
        ValidationGroup="MeusDados" Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
    <asp:CustomValidator ID="cvEmailExistente" runat="server" ControlToValidate="txtEmail"
        ValidationGroup="MeusDados" Display="Dynamic" ClientValidationFunction="cvEmailExistente_ClientValidate"
        ErrorMessage="Já existe um usuário cadastrado com este e-mail.">*</asp:CustomValidator>
    <div id="divSenha" class="senha" runat="server">
        <asp:Label ID="lblSenhaAtual" runat="server" Text="Senha atual *" AssociatedControlID="txtSenhaAtual"></asp:Label>
        <asp:TextBox ID="txtSenhaAtual" runat="server" TextMode="Password" SkinID="text20C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvSenhaAtual" ControlToValidate="txtSenhaAtual"
            runat="server" ErrorMessage="Senha atual é obrigatório." ValidationGroup="MeusDados">*</asp:RequiredFieldValidator>
        <asp:CustomValidator ID="cvSenhaAtual" ControlToValidate="txtSenhaAtual" Display="Dynamic"
            runat="server" ErrorMessage="Senha atual inválida." ValidationGroup="MeusDados" ClientValidationFunction="cvSenhaAtual_ClientValidate">*</asp:CustomValidator>
        <asp:Label ID="lblNovaSenha" runat="server" Text="Nova senha" AssociatedControlID="txtNovaSenha"></asp:Label>
        <asp:TextBox ID="txtNovaSenha" runat="server" TextMode="Password" SkinID="text20C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvNovaSenha" runat="server" ControlToValidate="txtNovaSenha"
            ErrorMessage="Nova senha é obrigatório." Display="Dynamic" ValidationGroup="MeusDados">*</asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="revNovaSenhaFormato" runat="server" ControlToValidate="txtNovaSenha"
            Display="Dynamic" ErrorMessage="A senha deve conter pelo menos uma combinação de letras e números ou letras 
            maiúsculas e minúsculas ou algum caracter especial (!, @, #, $, %, &amp;) somados a letras e/ou números."
            ValidationGroup="MeusDados">*</asp:RegularExpressionValidator>
        <asp:RegularExpressionValidator ID="revNovaSenhaTamanho" runat="server" ControlToValidate="txtNovaSenha"
            Display="Dynamic" ErrorMessage="A senha deve conter {0}." ValidationGroup="MeusDados">*</asp:RegularExpressionValidator>
        <asp:CompareValidator ID="cpvNovaSenha" runat="server" ErrorMessage="Senha atual e nova senha devem ser diferentes"
            Operator="NotEqual" ControlToCompare="txtNovaSenha" ControlToValidate="txtSenhaAtual"
            ValidationGroup="MeusDados">*</asp:CompareValidator>
        <asp:Label ID="lblMsnNovaSenha" runat="server" Text="({0}, utilizando letras e números)."></asp:Label>
        <asp:Label ID="lblConfNovaSenha" runat="server" Text="Confirmar nova senha" AssociatedControlID="txtConfNovaSenha"></asp:Label>
        <asp:TextBox ID="txtConfNovaSenha" runat="server" TextMode="Password" SkinID="text20C"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvConfNovaSenha" runat="server" ControlToValidate="txtConfNovaSenha"
            ErrorMessage="Confirmar nova senha é obrigatório." Display="Dynamic" ValidationGroup="MeusDados">*</asp:RequiredFieldValidator>
        <asp:CompareValidator ID="cpvConfNovaSenha" runat="server" ControlToCompare="txtNovaSenha"
            ControlToValidate="txtConfNovaSenha" ErrorMessage="Senha não confere." Display="Dynamic"
            ValidationGroup="MeusDados">*</asp:CompareValidator>
        <asp:CustomValidator ID="cvNovaSenhaHistorico" ControlToValidate="txtNovaSenha" Display="Dynamic"
            runat="server" ErrorMessage="Identificamos um erro! Por motivos de segurança, sua nova senha precisa ser diferente das últimas senhas utilizadas por você. Tente novamente!"
            ValidationGroup="MeusDados" ClientValidationFunction="cvNovaSenhaHistorico_ClientValidate">*</asp:CustomValidator>
    </div>
    <div class="right">
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            ValidationGroup="MeusDados" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="false"
            OnClick="btnCancelar_Click" />
    </div>
</fieldset>
<fieldset>
    <legend>Perfis de acesso</legend>
    <asp:GridView ID="dgvGrupos" runat="server" DataSourceID="odsGrupos" AllowPaging="True"
        AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="gru_nome" HeaderText="Grupo" />
            <asp:BoundField DataField="sis_nome" HeaderText="Sistema" />
            <asp:BoundField DataField="uad_nome" HeaderText="Unidade administrativa" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsGrupos" runat="server" DataObjectTypeName="MSTech.CoreSSO.Entities.SYS_Grupo"
        DeleteMethod="Delete" OldValuesParameterFormatString="original_{0}" SelectMethod="GetSelectBy_MeusDados"
        TypeName="MSTech.CoreSSO.BLL.SYS_GrupoBO" UpdateMethod="Save" EnablePaging="True"
        MaximumRowsParameterName="pageSize" SelectCountMethod="GetTotalRecords" StartRowIndexParameterName="currentPage"
        OnSelecting="odsGrupos_Selecting">
        <SelectParameters>
            <asp:Parameter DbType="Guid" Name="usu_id" />
            <asp:Parameter Name="paginado" Type="Boolean" />
        </SelectParameters>
    </asp:ObjectDataSource>
</fieldset>
