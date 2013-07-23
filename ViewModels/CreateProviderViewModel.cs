using System.ComponentModel.DataAnnotations;

namespace NGM.OpenAuthentication.ViewModels {
    public class CreateProviderViewModel {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string ProviderName { get; set; }
        public string ProviderIdentifier { get; set; }
        public string ProviderIdKey { get; set; }
        public string ProviderSecret { get; set; }
    }
}