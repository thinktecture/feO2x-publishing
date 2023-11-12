SELECT "Id", "FirstName", "LastName", "Email", "PhoneNumber"
FROM "Contacts"
OFFSET $1
LIMIT $2