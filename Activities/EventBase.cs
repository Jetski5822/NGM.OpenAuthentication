using Orchard.Localization;
using Orchard.Workflows.Services;

namespace NGM.OpenAuthentication.Activities {
    public abstract class EventBase : Event {
        protected EventBase() {
            T = NullLocalizer.Instance;
        }

        public override bool CanStartWorkflow {
            get { return true; }
        }

        public Localizer T { get; set; }
    }
}