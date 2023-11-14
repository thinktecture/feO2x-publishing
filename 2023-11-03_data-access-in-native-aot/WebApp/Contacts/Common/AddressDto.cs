using System;

namespace WebApp.Contacts.Common;

public readonly record struct AddressDto(Guid Id, string Street, string ZipCode, string City);