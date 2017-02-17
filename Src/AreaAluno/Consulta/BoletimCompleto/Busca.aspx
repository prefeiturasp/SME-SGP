<%@ Page Language="C#" MasterPageFile="~/MasterPageAluno.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Busca.aspx.cs" Inherits="AreaAluno.Consulta.BoletimCompleto.Busca" %>

<%@ Register Src="~/WebControls/AlunoBoletimEscolar/UCAlunoBoletimEscolar.ascx" TagName="UCAlunoBoletimEscolar" TagPrefix="uc1" %>
<%@ Register Src="~/WebControls/Combos/UCComboAnosLetivos.ascx" TagPrefix="uc1" TagName="UCComboAnosLetivos" %>
<%@ Register Src="~/WebControls/BoletimCompletoAluno/UCBoletimAngular.ascx" TagName="UCBoletim" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:UCComboAnosLetivos runat="server" ID="UCComboAnosLetivos" />

    <asp:Label ID="lblMensagem" runat="server" EnableViewState="False"></asp:Label>
    <uc:UCBoletim ID="ucBoletim" runat="server" />
</asp:Content>
