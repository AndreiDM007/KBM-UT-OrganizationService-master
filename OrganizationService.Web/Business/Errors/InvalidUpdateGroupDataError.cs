using System;
using System.Collections.Generic;
using System.Text;
using Kebormed.Core.Business.Errors;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public class InvalidUpdateGroupDataError : BaseError
    {
        public override string Code => "invalid_update_group_data_error";

        public InvalidUpdateGroupDataError(string fieldName)
        {
            this.Field = fieldName;
        }        
    }
}
