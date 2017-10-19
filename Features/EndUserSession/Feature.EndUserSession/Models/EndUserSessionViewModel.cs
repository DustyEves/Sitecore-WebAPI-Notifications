using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO = Feature.EndUserSession.DataTransferObjects;

namespace Feature.EndUserSession.Models.ViewModels
{
    public class EndUserSessionViewModel
    {
        public string ContactID { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }

      
        public string AuthenticationLvl { get; set; }

        public string ContactLvl { get; set; }

        public string ContactIdentifier { get; set; }

        public DTO.EndUserSessionObject ContextItem { get; set; }
    }
}