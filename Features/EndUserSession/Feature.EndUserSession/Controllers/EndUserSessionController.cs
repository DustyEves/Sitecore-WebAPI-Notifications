using DTO = Feature.EndUserSession.DataTransferObjects;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Sitecore.Data;
using Sitecore.Sites;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Sitecore.Data.Items;
using Sitecore.Analytics;
using Sitecore.Analytics.Model.Entities;
using Feature.EndUserSession.Models.ViewModels;
using Sitecore.Analytics.Model;

namespace Feature.EndUserSession.Controllers
{
    public class EndUserSessionController: GlassController
    {
        public ActionResult EndUserSession()
         {
            EndUserSessionViewModel vm = new EndUserSessionViewModel();
            vm.ContextItem = GetDataSourceItem<DTO.EndUserSessionObject>();
            
            if (Sitecore.Context.PageMode.IsNormal && Tracker.IsActive && Tracker.Current != null && Tracker.Current.Contact != null)
            {
                vm.ContactID = Tracker.Current.Contact.ContactId.ToString();
                var personalFacet = Tracker.Current.Contact.GetFacet<IContactPersonalInfo>("Personal");
                vm.ContactFirstName = personalFacet.FirstName;
                vm.ContactLastName = personalFacet.Surname;

                vm.AuthenticationLvl = Enum.GetName(typeof(AuthenticationLevel), Tracker.Current.Contact.Identifiers.AuthenticationLevel);
                vm.ContactLvl = Enum.GetName(typeof(ContactIdentificationLevel), Tracker.Current.Contact.Identifiers.IdentificationLevel);
                vm.ContactIdentifier = Tracker.Current.Contact.Identifiers.Identifier;

                Tracker.Current.EndTracking();
                Tracker.Current.EndVisit(true);
                Session.Abandon();                          
            }           
            
            return View("/Views/Features/EndUserSession/EndUSerSession.cshtml", vm );
        }
    }
}
