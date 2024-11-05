using System.Collections.Generic;

namespace TawassolProject.ViewModel
{
    public class PermissionsFormViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<CheckBoxViewModel> RoleCalims { get; set; }
    }
}