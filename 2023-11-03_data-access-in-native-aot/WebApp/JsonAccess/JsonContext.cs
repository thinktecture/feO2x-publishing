using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebApp.Contacts.GetContact;
using WebApp.Contacts.GetContacts;

namespace WebApp.JsonAccess;

[JsonSerializable(typeof(List<ContactListDto>))]
[JsonSerializable(typeof(ContactDetailDto))]
public sealed partial class JsonContext : JsonSerializerContext;