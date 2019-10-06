using System.ComponentModel.DataAnnotations;

namespace MessengerService2.Models
{
    [MetadataType(typeof(MessagesMetadata))]
    public partial class Messages
    {
    }

   [MetadataType(typeof(UsersMetadata))]
    public partial class Users
    {
    }

    [MetadataType(typeof(UserRolesMetadata))]
    public partial class UserRoles
    {
    }
}