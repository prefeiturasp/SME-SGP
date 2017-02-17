<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCLoader.ascx.cs" Inherits="AreaAluno.WebControls.Mensagens.UCLoader" %>

<asp:UpdateProgress ID="upgProgress" runat="server" DisplayAfter="0" >
    <ProgressTemplate>
        <div class="loader">
            <asp:Image ID="Image1" SkinID="imgLoader" runat="server" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
