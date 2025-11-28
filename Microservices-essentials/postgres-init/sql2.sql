-- DROP TABLE IF EXISTS public."Users";

CREATE TABLE IF NOT EXISTS public."Users"
(
    "UserID" uuid NOT NULL,
    "PersonName" character varying COLLATE pg_catalog."default" NOT NULL,
    "Email" character varying COLLATE pg_catalog."default" NOT NULL,
    "Password" character varying COLLATE pg_catalog."default" NOT NULL,
    "Gender" character varying COLLATE pg_catalog."default",
    CONSTRAINT "Users_pkey" PRIMARY KEY ("UserID")
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Users"
    OWNER TO postgres;

-- Data
INSERT INTO public."Users" ("UserID", "PersonName", "Email", "Password", "Gender") VALUES
('31082212-903b-4448-a84f-270914957000', 'user1',     'user1@gmail.com',    'user1user1', 'Male'),
('8dc2a8af-15ee-4c3c-8968-d69ff45f420c', 'TESTUSER4', 'testuser4@gmail.com','testuser4',  'Male');
