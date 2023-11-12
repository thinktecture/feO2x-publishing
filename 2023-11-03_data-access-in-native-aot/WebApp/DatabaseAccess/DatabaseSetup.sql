CREATE TABLE IF NOT EXISTS "Contacts" (
    "Id" UUID CONSTRAINT "PK_Contacts" PRIMARY KEY,
    "FirstName" VARCHAR(255) NOT NULL,
    "LastName" VARCHAR(255) NOT NULL,
    "Email" VARCHAR(255),
    "PhoneNumber" VARCHAR(20)
);

CREATE TABLE IF NOT EXISTS "Addresses" (
    "Id" UUID CONSTRAINT "PK_Addresses" PRIMARY KEY,
    "ContactID" UUID NOT NULL,
    "Street" VARCHAR(255) NOT NULL,
    "ZipCode" VARCHAR(20) NOT NULL,
    "City" VARCHAR(255) NOT NULL,
    CONSTRAINT "FK_Addresses_Contacts" FOREIGN KEY ("ContactID") REFERENCES "Contacts"("Id")
);
INSERT INTO "Contacts" ("Id", "FirstName", "LastName", "Email", "PhoneNumber")
VALUES
    ('D10DF224-7E72-4CB0-94B2-81725D818A1C', 'Alice', 'Smith', 'alice.smith@live.com', '555-1234'),
    ('054AB8AC-369F-410C-9F66-140D1F240613', 'Bob', 'Johnson', 'bob.johnson@gmail.com', '555-2345'),
    ('CCC51159-2AC7-435B-B7D2-4CC25791622D', 'Carol', 'Williams', 'carol.williams@yahoo.com', '555-3456')
ON CONFLICT ("Id") DO NOTHING;

INSERT INTO "Addresses" ("Id", "ContactID", "Street", "ZipCode", "City")
VALUES
    ('E3F45628-00E6-4CB7-99C9-A45DDDC49615', 'D10DF224-7E72-4CB0-94B2-81725D818A1C', '123 Maple Street', '90210', 'Springfield'),
    ('BF718594-C61B-4FF4-AD16-D2347046CEDE', 'D10DF224-7E72-4CB0-94B2-81725D818A1C', '456 Oak Avenue', '12345', 'Shelbyville'),
    ('2D570181-2186-4FDE-B1AD-25DDF04D5AE3', '054AB8AC-369F-410C-9F66-140D1F240613', '789 Pine Road', '67890', 'Evergreen')
ON CONFLICT ("Id") DO NOTHING;
