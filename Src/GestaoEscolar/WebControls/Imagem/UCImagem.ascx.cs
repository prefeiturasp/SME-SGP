namespace GestaoEscolar.WebControls.Imagem
{
    using MSTech.GestaoEscolar.Web.WebProject;
    using System;
    using System.Web.UI;
    
    public partial class UCImagem : MotherUserControl
    {
        #region Propriedades


        /// <summary>
        /// Retorna ou seta o caminho da imagem.
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return img.ImageUrl;
            }

            set
            {
                img.ImageUrl = ResolveUrl(value);
            }
        }

        /// <summary>
        /// Retorna ou seta a propriedade AlternateText da imagem.
        /// </summary>
        public string AlternateText 
        {
            get
            {
                return img.AlternateText;
            }

            set
            {
                img.AlternateText = value;
            }
        }

        /// <summary>
        /// Retorna ou seta o ToolTipo da imagem.
        /// </summary>
        public string ToolTip
        {
            get
            {
                return img.ToolTip;
            }

            set
            {
                img.ToolTip = value;
            }
        }

        #endregion
                
    }
}