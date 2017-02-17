<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="BoletimEscolarDosAlunos.aspx.cs"
    Inherits="GestaoEscolar.Documentos.BoletimEscolar.BoletimEscolarDosAlunos" %>

<%@ Register Src="~/WebControls/BoletimCompletoAluno/UCBoletimAngular.ascx" TagName="UCBoletim" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="area-form">
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" /> 
            <uc:UCBoletim ID="ucBoletim" runat="server" />
        </div>
        <div style="text-align: right; margin: 5px;" class="area-botoes-bottom">
            <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" />
        </div>
    </fieldset>
</asp:Content>
