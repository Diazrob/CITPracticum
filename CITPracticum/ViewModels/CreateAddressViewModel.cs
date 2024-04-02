using System.ComponentModel.DataAnnotations;

namespace CITPracticum.ViewModels
{
    public class CreateAddressViewModel
    {
        [Required(ErrorMessage = "Street information is required")]
        public string Street { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Province is required")]
        public string Prov { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Postal Code is required")]
        public string PostalCode { get; set; }
    }
}
