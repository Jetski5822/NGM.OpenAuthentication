using Orchard.Localization;
using Orchard.Workflows.Services;

namespace NGM.OpenAuthentication.Activities {
    public abstract class TaskBase : Task {
        protected TaskBase() {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
    }
}