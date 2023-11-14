INSERT INTO "Contacts" ("Id", "FirstName", "LastName", "Email", "PhoneNumber")
VALUES ($1, $2, $3, $4, $5)
ON CONFLICT ("Id") DO UPDATE
SET "FirstName" = excluded."FirstName",
    "LastName" = excluded."LastName",
    "Email" = excluded."Email",
    "PhoneNumber" = excluded."PhoneNumber";