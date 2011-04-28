using System;
using System.Collections.Generic;
using Orchard;

namespace NGM.OpenAuthentication.Core
{
    public interface IScopePermissionProvider : IDependency
    {
        IEnumerable<ScopePermission> GetPermissions();
    }

    public class FacebookScopePermissions : IScopePermissionProvider
    {

        public IEnumerable<ScopePermission> GetPermissions()
        {
            return new[] {
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_about_me"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_activities"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_birthday"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_education_history"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_events"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_groups"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_hometown"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_interests"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_likes"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_location"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_notes"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_online_presence"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_photo_video_tags"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_relationships"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_relationship_details"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_religion_politics"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_website"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_work_history"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "email"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "read_friendlists"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "read_insights"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "read_mailbox"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "read_requests"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "read_stream"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "xmpp_login"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "ads_management"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "user_checkins"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_about_me"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_activities"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_birthday"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_education_history"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_events"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_groups"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_hometown"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_interests"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_likes"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_location"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_notes"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_online_presence"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_photo_video_tags"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_photos"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_relationships"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_relationship_details"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_religion_politics"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_status"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_videos"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_website"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_work_history"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "manage_friendlists"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Data", Scope = "friends_checkins"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Publishing", Scope = "publish_stream"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Publishing", Scope = "create_event"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Publishing", Scope = "rsvp_event"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Publishing", Scope = "sms"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Publishing", Scope = "offline_access"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Publishing", Scope = "publish_checkins"},
                new ScopePermission { Provider = Provider.Facebook, Resource = "Page", Scope = "manage_pages"},
            };
        }
    }

    public class MicrosoftConnectScopePermissions : IScopePermissionProvider {
        public IEnumerable<ScopePermission> GetPermissions() {
            return new[] {
                new ScopePermission {Provider = Provider.LiveId, Resource = "Activities", Scope = "WL_Activities.View"},
                new ScopePermission {Provider = Provider.LiveId, Resource = "Activities", Scope = "WL_Activities.Update"},
                new ScopePermission {Provider = Provider.LiveId, Resource = "Contacts", Scope = "WL_Contacts.View"},
                new ScopePermission {Provider = Provider.LiveId, Resource = "Photos", Scope = "WL_Photos.View"},
                new ScopePermission {Provider = Provider.LiveId, Resource = "Photos", Scope = "WL_Profiles.View"},
                new ScopePermission {Provider = Provider.LiveId, Resource = "Real-Time Shared Experiances", Scope = "Messenger.SignIn"},
            };
        }
    }
}