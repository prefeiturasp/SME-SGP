using MSTech.GestaoEscolar.Web.WebProject;

public partial class WebControls_Mensagens_UCLoader : MotherUserControl
{
    #region Propriedades

    /// <summary>
    /// ID do updatePanel associado ao Progress.
    /// </summary>
    public string AssociatedUpdatePanelID
    {
        set
        {
            upgProgress.AssociatedUpdatePanelID = value;
        }
    }

    #endregion
}
