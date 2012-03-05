using System.Collections.Generic;
using NGM.OpenAuthentication.Core;
using NGM.OpenAuthentication.Providers.Facebook.Services;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Models;

namespace NGM.OpenAuthentication.Providers.Facebook.Permissions {
    [OrchardFeature("Facebook")]
    public class FacebookScopePermissions : IScopePermissionProvider
    {
        public virtual Feature Feature { get; set; }

        public AccessControlProvider Provider {
            get { return new FacebookAccessControlProvider(); }
        }

        public IEnumerable<ScopePermission> GetPermissions()
        {
            return new[] {
                             new ScopePermission { Resource = "Data", Scope = "user_about_me"},
                             new ScopePermission { Resource = "Data", Scope = "user_activities"},
                             new ScopePermission { Resource = "Data", Scope = "user_birthday"},
                             new ScopePermission { Resource = "Data", Scope = "user_education_history"},
                             new ScopePermission { Resource = "Data", Scope = "user_events"},
                             new ScopePermission { Resource = "Data", Scope = "user_groups"},
                             new ScopePermission { Resource = "Data", Scope = "user_hometown"},
                             new ScopePermission { Resource = "Data", Scope = "user_interests"},
                             new ScopePermission { Resource = "Data", Scope = "user_likes"},
                             new ScopePermission { Resource = "Data", Scope = "user_location"},
                             new ScopePermission { Resource = "Data", Scope = "user_notes"},
                             new ScopePermission { Resource = "Data", Scope = "user_online_presence"},
                             new ScopePermission { Resource = "Data", Scope = "user_photo_video_tags"},
                             new ScopePermission { Resource = "Data", Scope = "user_relationships"},
                             new ScopePermission { Resource = "Data", Scope = "user_relationship_details"},
                             new ScopePermission { Resource = "Data", Scope = "user_religion_politics"},
                             new ScopePermission { Resource = "Data", Scope = "user_website"},
                             new ScopePermission { Resource = "Data", Scope = "user_work_history"},
                             new ScopePermission { Resource = "Data", Scope = "email", IsEnabled = true},
                             new ScopePermission { Resource = "Data", Scope = "read_friendlists"},
                             new ScopePermission { Resource = "Data", Scope = "read_insights"},
                             new ScopePermission { Resource = "Data", Scope = "read_mailbox"},
                             new ScopePermission { Resource = "Data", Scope = "read_requests"},
                             new ScopePermission { Resource = "Data", Scope = "read_stream", IsEnabled = true},
                             new ScopePermission { Resource = "Data", Scope = "xmpp_login"},
                             new ScopePermission { Resource = "Data", Scope = "ads_management"},
                             new ScopePermission { Resource = "Data", Scope = "user_checkins"},
                             new ScopePermission { Resource = "Data", Scope = "friends_about_me"},
                             new ScopePermission { Resource = "Data", Scope = "friends_activities"},
                             new ScopePermission { Resource = "Data", Scope = "friends_birthday"},
                             new ScopePermission { Resource = "Data", Scope = "friends_education_history"},
                             new ScopePermission { Resource = "Data", Scope = "friends_events"},
                             new ScopePermission { Resource = "Data", Scope = "friends_groups"},
                             new ScopePermission { Resource = "Data", Scope = "friends_hometown"},
                             new ScopePermission { Resource = "Data", Scope = "friends_interests"},
                             new ScopePermission { Resource = "Data", Scope = "friends_likes"},
                             new ScopePermission { Resource = "Data", Scope = "friends_location"},
                             new ScopePermission { Resource = "Data", Scope = "friends_notes"},
                             new ScopePermission { Resource = "Data", Scope = "friends_online_presence"},
                             new ScopePermission { Resource = "Data", Scope = "friends_photo_video_tags"},
                             new ScopePermission { Resource = "Data", Scope = "friends_photos"},
                             new ScopePermission { Resource = "Data", Scope = "friends_relationships"},
                             new ScopePermission { Resource = "Data", Scope = "friends_relationship_details"},
                             new ScopePermission { Resource = "Data", Scope = "friends_religion_politics"},
                             new ScopePermission { Resource = "Data", Scope = "friends_status"},
                             new ScopePermission { Resource = "Data", Scope = "friends_videos"},
                             new ScopePermission { Resource = "Data", Scope = "friends_website"},
                             new ScopePermission { Resource = "Data", Scope = "friends_work_history"},
                             new ScopePermission { Resource = "Data", Scope = "manage_friendlists"},
                             new ScopePermission { Resource = "Data", Scope = "friends_checkins"},
                             new ScopePermission { Resource = "Publishing", Scope = "publish_stream", IsEnabled = true},
                             new ScopePermission { Resource = "Publishing", Scope = "create_event"},
                             new ScopePermission { Resource = "Publishing", Scope = "rsvp_event"},
                             new ScopePermission { Resource = "Publishing", Scope = "sms"},
                             new ScopePermission { Resource = "Publishing", Scope = "offline_access", IsEnabled = true},
                             new ScopePermission { Resource = "Publishing", Scope = "publish_checkins"},
                             new ScopePermission { Resource = "Page", Scope = "manage_pages"},
                         };
        }
    }
}