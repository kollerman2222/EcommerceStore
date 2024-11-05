using System.ComponentModel.DataAnnotations;

namespace TawassolProject.ViewModel
{
    public class RoleFormViewModel
    {
        [Required, StringLength(256)]
        public string Name { get; set; }

        public string RoleId { get; set; }

    }
}