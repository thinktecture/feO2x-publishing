using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebApp.Contacts.Common;
using WebApp.Contacts.GetContacts;

namespace WebApp.JsonAccess;

[JsonSerializable(typeof(List<ContactListDto>))]
[JsonSerializable(typeof(ContactDetailDto))]
[JsonSerializable(typeof(IDictionary<string, string[]>))]
public sealed partial class JsonContext : JsonSerializerContext;