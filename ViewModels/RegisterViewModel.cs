using NGM.OpenAuthentication.Models;

namespace NGM.OpenAuthentication.ViewModels {
    public class RegisterViewModel {
        public RegisterViewModel() {}

        public RegisterViewModel(RegisterModel model) {
            Model = model;
        }

        public RegisterModel Model { get; set;}
    }
}