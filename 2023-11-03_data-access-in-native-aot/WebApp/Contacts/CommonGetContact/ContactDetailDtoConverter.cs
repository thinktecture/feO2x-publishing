using System;
using System.Collections.Generic;

namespace WebApp.Contacts.CommonGetContact;

public static class ContactDetailDtoConverter
{
    public static ContactDetailDto? ConvertToDto(this List<GetContactRecord> records)
    {
        if (records.Count == 0)
            return null;

        var firstItem = records[0];
        var addresses = Array.Empty<AddressDto>();
        if (firstItem is not { AddressId: null })
        {
            addresses = new AddressDto[records.Count];
            for (var i = 0; i < records.Count; i++)
            {
                var item = records[i];
                addresses[i] = new (item.AddressId!.Value, item.Street!, item.ZipCode!, item.City!);
            }
        }

        return new (firstItem.ContactId,
                    firstItem.FirstName,
                    firstItem.LastName,
                    firstItem.Email,
                    firstItem.PhoneNumber,
                    addresses);
    }
}