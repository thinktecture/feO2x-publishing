using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebApp.Contacts.GetContacts;

namespace WebApp.JsonAccess;

[JsonSerializable(typeof(List<ContactListDto>))]
public sealed partial class JsonContext : JsonSerializerContext;