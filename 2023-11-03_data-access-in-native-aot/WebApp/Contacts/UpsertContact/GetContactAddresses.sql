SELECT "Id", "ContactID", "Street", "ZipCode", "City"
FROM "Addresses"
WHERE "Id" = ANY($1) OR "ContactID" = $2;