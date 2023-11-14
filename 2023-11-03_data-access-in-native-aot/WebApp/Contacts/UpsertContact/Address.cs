using System;

namespace WebApp.Contacts.UpsertContact;

public readonly record struct Address(Guid Id, Guid ContactId, string Street, string ZipCode, string City);