<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebControls_BuscaAluno_UCCamposBuscaAluno"
    CodeBehind="UCCamposBuscaAluno.ascx.cs" %>
<div id="divNomeAluno" runat="server">
    <div id="_divEscolhaBusca" runat="server">
        <asp:Label ID="_lblEscolhaBusca" runat="server" Text="Tipo de busca por nome do aluno"
            AssociatedControlID="_rblEscolhaBusca"></asp:Label>
        <asp:RadioButtonList ID="_rblEscolhaBusca" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Text="Começa por" Value="2" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Contém" Value="1"></asp:ListItem>
            <asp:ListItem Text="Fonética" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <%--<div style="float: left;"> Retirado pois estava causando problema no layout de sp--%>
    <div>
        <asp:Label ID="_lblNome" runat="server" Text="Nome do aluno" AssociatedControlID="_txtNome"></asp:Label>
        <asp:TextBox ID="_txtNome" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
    </div>
</div>
<div id="divDtNascAluno" runat="server">
    <asp:Label ID="_lblDataNascimento" runat="server" Text="Data de nascimento" AssociatedControlID="_txtDataNascimento"></asp:Label>
    <asp:TextBox ID="_txtDataNascimento" runat="server" MaxLength="10" SkinID="DataSemCalendario"></asp:TextBox>
    <asp:CustomValidator ID="cvDataNascimento" runat="server" ControlToValidate="_txtDataNascimento"
        ValidationGroup="aluno" Display="Dynamic" ErrorMessage="" OnServerValidate="ValidarData_ServerValidate">* </asp:CustomValidator>
</div>
<div id="divNomeMaeAluno" runat="server">
    <asp:Label ID="_lblMae" runat="server" Text="Nome da mãe" AssociatedControlID="_txtMae"></asp:Label>
    <asp:TextBox ID="_txtMae" runat="server" MaxLength="200" SkinID="text60C"></asp:TextBox>
</div>
<div id="divMatriculaAluno" runat="server">
    <asp:Label ID="_lblMatricula" runat="server" AssociatedControlID="_txtMatricula"></asp:Label>
    <asp:TextBox ID="_txtMatricula" runat="server" MaxLength="50" SkinID="text20C"></asp:TextBox>
</div>
<div id="divMatriculaEstAluno" runat="server">
    <asp:Label ID="_lblMatrEst" runat="server" AssociatedControlID="_txtMatriculaEstadual"
        Text="Matrícula estadual" Visible="False"></asp:Label>
    <asp:TextBox ID="_txtMatriculaEstadual" runat="server" MaxLength="50" SkinID="text20C"
        Visible="False"></asp:TextBox>
</div>
