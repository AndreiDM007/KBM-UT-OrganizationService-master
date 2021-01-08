using System;
using System.Collections.Generic;
using System.Text;

namespace Kebormed.Core.OrganizationService.Web.Business.Services.Group.Models
{
    public class DeleteGroupCommand
    {
        public int OrganizationId { get; set; }
        public int GroupId { get; set; }
    }
}
