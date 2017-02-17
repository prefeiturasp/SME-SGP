<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCDadosAluno.ascx.cs" Inherits="GestaoEscolar.WebControls.HistoricoEscolar.UCDadosAluno" %>

<asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
<fieldset>
    <legend>
        <asp:Label ID="lblLegend" Text="<%$ Resources:UserControl, UCDadosAluno.lblLegend.Text %>" runat="server" EnableViewState="False"></asp:Label></legend>
    <table class="table-padding">
        <tr>
            <td>
                <asp:Label ID="lblNome" Text="<%$ Resources:UserControl, UCDadosAluno.lblNome.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" Enabled="false" MaxLength="200" SkinID="text60C"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblMatricula" Text="<%$ Resources:UserControl, UCDadosAluno.lblMatricula.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtMatricula"></asp:Label>
                <asp:TextBox ID="txtMatricula" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <fieldset>
        <legend>
            <asp:Label ID="lblLegendLocalNascimento" Text="<%$ Resources:UserControl, UCDadosAluno.lblLegendLocalNascimento.Text %>" runat="server" EnableViewState="False"></asp:Label></legend>
        <table class="table-padding">
            <tr>
                <td>
                    <asp:Label ID="lblCidade" Text="<%$ Resources:UserControl, UCDadosAluno.lblCidade.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtCidade"></asp:Label>
                    <asp:TextBox ID="txtCidade" runat="server" Enabled="false"></asp:TextBox>

                </td>
                <td>
                    <asp:Label ID="lblEstado" Text="<%$ Resources:UserControl, UCDadosAluno.lblEstado.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtEstado"></asp:Label>
                    <asp:TextBox ID="txtEstado" runat="server" Enabled="false"></asp:TextBox>

                </td>
                <td>
                    <asp:Label ID="lblNacionalidade" Text="<%$ Resources:UserControl, UCDadosAluno.lblNacionalidade.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtNacionalidade"></asp:Label>
                    <asp:TextBox ID="txtNacionalidade" runat="server" Enabled="false"></asp:TextBox>

                </td>
                <td>
                    <asp:Label ID="lblDataNascimento" Text="<%$ Resources:UserControl, UCDadosAluno.lblDataNascimento.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtDataNascimento"></asp:Label>
                    <asp:TextBox ID="txtDataNascimento" runat="server" Enabled="false"></asp:TextBox>

                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>
            <asp:Label ID="lblLegendIdentidade" Text="<%$ Resources:UserControl, UCDadosAluno.lblLegendIdentidade.Text %>" runat="server" EnableViewState="False"></asp:Label></legend>
        <table class="table-padding">
            <tr>
                <td>
                    <asp:Label ID="lblRG" Text="<%$ Resources:UserControl, UCDadosAluno.lblRG.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtRG"></asp:Label>
                    <asp:TextBox ID="txtRG" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblDataExpedicao" Text="<%$ Resources:UserControl, UCDadosAluno.lblDataExpedicao.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtDataExpedicao"></asp:Label>
                    <asp:TextBox ID="txtDataExpedicao" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblOrgao" Text="<%$ Resources:UserControl, UCDadosAluno.lblOrgao.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtOrgao"></asp:Label>
                    <asp:TextBox ID="txtOrgao" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblEstadoRG" Text="<%$ Resources:UserControl, UCDadosAluno.lblEstadoRG.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtEstadoRG"></asp:Label>
                    <asp:TextBox ID="txtEstadoRG" runat="server" Enabled="false"></asp:TextBox>
                </td>

            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>
            <asp:Label ID="lblLegendCertNascimento" Text="<%$ Resources:UserControl, UCDadosAluno.lblLegendCertNascimento.Text %>" runat="server" EnableViewState="False"></asp:Label></legend>
        <table class="table-padding">
            <tr>
                <td>
                    <asp:Label ID="lblNumeroCertidao" Text="<%$ Resources:UserControl, UCDadosAluno.lblNumeroCertidao.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtNumeroCertidao"></asp:Label>
                    <asp:TextBox ID="txtNumeroCertidao" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblFolha" Text="<%$ Resources:UserControl, UCDadosAluno.lblFolha.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtFolha"></asp:Label>
                    <asp:TextBox ID="txtFolha" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblLivro" Text="<%$ Resources:UserControl, UCDadosAluno.lblLivro.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtLivro"></asp:Label>
                    <asp:TextBox ID="txtLivro" runat="server" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="lblEstadoCN" Text="<%$ Resources:UserControl, UCDadosAluno.lblEstadoCN.Text %>" runat="server" EnableViewState="False" AssociatedControlID="txtEstadoCN"></asp:Label>
                    <asp:TextBox ID="txtEstadoCN" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
    </fieldset>
    <div class="right">       
        <asp:Button ID="btnVisualizarHistorico" runat="server" Text="Visualizar histórico" CausesValidation="false"
            OnClick="btnVisualizarHistorico_Click" />
         <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CausesValidation="false"
            OnClick="btnVoltar_Click" />
    </div>
</fieldset>
