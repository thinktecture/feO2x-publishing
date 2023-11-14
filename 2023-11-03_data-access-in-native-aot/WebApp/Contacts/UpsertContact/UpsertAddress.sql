INSERT INTO "Addresses" ("Id", "ContactID", "Street", "ZipCode", "City")
VALUES ($1, $2, $3, $4, $5)
ON CONFLICT ("Id") DO UPDATE
SET "Street" = excluded."Street",
    "ZipCode" = excluded."ZipCode",
    "City" = excluded."City";