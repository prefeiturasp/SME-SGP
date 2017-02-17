<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Manutencao.aspx.cs" 
    Inherits="AreaAluno.Manutencao" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset style="text-align: center;"><legend></legend>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Image ID="imgErro" runat="server" ImageAlign="Middle" Width="35" Height="35" />
                &nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblMensagem" Font-Size="Large" runat="server" Text="Ocorreu um erro inesperado. Por favor, tente novamente. Se o problema persistir, avise a equipe de suporte e apoio da ferramenta."></asp:Label>
                <br />
                <asp:Label ID="lblDataErro" runat="server" Font-Size="Large"></asp:Label>
                <br />
                <br />
                <br />
                <a href="Index.aspx" title="Clique aqui para voltar para a página inicial do sistema.">Voltar para página inicial</a>
                <br />
                <br />
                <a href="Logout.ashx" title="Clique aqui para voltar para a página de login do sistema.">Voltar para página de login</a>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            </fieldset>
        </div>
    </form>
</body>
</html>
