<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Documentos_BoletimEscolarAluno_BoletimEscolarAluno" CodeBehind="BoletimEscolarAluno.aspx.cs" %>
<%@ Register Src="../../WebControls/AlunoBoletimEscolar/UCAlunoBoletimEscolar.ascx" TagName="UCAlunoBoletimEscolar"
    TagPrefix="uc1" %>  
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <asp:UpdatePanel ID="updMensagem" runat="server">
      <ContentTemplate>
         <asp:Label ID="_lblMessage" runat="server" EnableViewState="False"></asp:Label>
         <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
      </ContentTemplate>
   </asp:UpdatePanel>
     <uc1:ucalunoboletimescolar ID="UCAlunoBoletimEscolar" runat="server"  />
</asp:Content>
