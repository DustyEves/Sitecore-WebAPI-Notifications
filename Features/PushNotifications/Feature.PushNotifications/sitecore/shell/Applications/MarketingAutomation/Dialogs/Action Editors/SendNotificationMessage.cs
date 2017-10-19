
using Sitecore.Data;

using Sitecore.Web;
using Sitecore;
using System;
using Sitecore.Diagnostics;
using Sitecore.Security;
using Sitecore.Shell.Applications.MarketingAutomation.Controls;
using Sitecore.Shell.Applications.MarketingAutomation.Dialogs;
using Sitecore.Controls;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System.Text;

namespace Feature.PushNotifications.sitecore.shell
{

 
    
    public class SendNotificationMessage : EditorBase
    {
        
        protected DropDownList lstIcons;
        protected Literal litIconImages;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidIconImageSelected;
        protected Literal litBadgeImages;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hidBadgeSelected;

        protected TextBox txtTitle;
        protected TextBox txtBody;
        protected TextBox txtSubject;

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull((object)e, "e");
            base.OnLoad(e);
            if (this.Page.IsPostBack)
                return;
            InitValues();
        }
     

        private const string iconImageHtml = "<img class='js-{3} selImage {4}' alt='{0}', src='{1}' data-id='{2}' />";
        private void buildImageList(string listType, string query, Literal targetControl, string activeId)
        {
            MediaUrlOptions options = new MediaUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            Item[] images = MasterDB.SelectItems(query);
            StringBuilder bldr = new StringBuilder();
            foreach (var _image in images)
                bldr.AppendLine(string.Format(iconImageHtml,
                    _image.Name, //AltText
                     MediaManager.GetMediaUrl((MediaItem)_image, options), //Image URL
                     _image.ID.ToString(), //Image ID for javascript
                     listType,  //Type i.e. image or badge
                     _image.ID.ToString() == activeId ? "selected" : ""
                    ));

            targetControl.Text = bldr.ToString();
        }

        private Database MasterDB
        {
            get { return Sitecore.Configuration.Factory.GetDatabase("master"); }
        }

        protected override void OK_Click()
        {
           

            base.OK_Click();
        }
        protected override void ConfigureSaveParameters()
        {
            base.ConfigureSaveParameters();
            this.SetParameterValue("Title", txtTitle.Text);
            this.SetParameterValue("Body", txtBody.Text);
            this.SetParameterValue("Icon", hidIconImageSelected.Value);
            this.SetParameterValue("Badge", hidBadgeSelected.Value);
            this.SetParameterValue("Subject", txtSubject.Text);
        }

        private void InitValues()
        {
            buildImageList("icon", "/sitecore/media library/Push Notifications/Icons/*", litIconImages, GetParameterValueByKey("Icon"));
            buildImageList("badge", "/sitecore/media library/Push Notifications/Badges/*", litBadgeImages, GetParameterValueByKey("Badge"));

            txtTitle.Text = GetParameterValueByKey("Title");
            txtBody.Text = GetParameterValueByKey("Body");
            hidIconImageSelected.Value = GetParameterValueByKey("Icon");
            hidBadgeSelected.Value = GetParameterValueByKey("Badge");
            txtSubject.Text = GetParameterValueByKey("Subject");
        }
        protected void ClickMe_Click()
        {

        }
       
    }
}