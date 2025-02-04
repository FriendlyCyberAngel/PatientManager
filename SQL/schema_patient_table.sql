create table public.pm_patient (
  id uuid not null default gen_random_uuid (),
  name text null default ''::text,
  birthdate date null,
  phone_number text null default ''::text,
  email text null default ''::text,
  created_at timestamp without time zone null default now(),
  constraint pm_patient_pkey primary key (id),
  constraint pm_patient_email_key unique (email),
  constraint pm_patient_phone_number_key unique (phone_number)
);