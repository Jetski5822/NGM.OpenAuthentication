using Orchard;

namespace NGM.OpenAuthentication.Core {
    public class StateBag : IStateBag {
        private readonly IOrchardServices _orchardServices;

        public StateBag(IOrchardServices orchardServices) {
            _orchardServices = orchardServices;
        }

        public OpenAuthenticationParameters Parameters {
            get { return _orchardServices.WorkContext.GetState<OpenAuthenticationParameters>("OpenAuthenticationParameters"); }
            set { _orchardServices.WorkContext.SetState("OpenAuthenticationParameters", value); }
        }

        public void Clear() {
            Parameters = null;
        }
    }

    public interface IStateBag : IDependency
    {
        OpenAuthenticationParameters Parameters { get; set; }
        void Clear();
    }
}