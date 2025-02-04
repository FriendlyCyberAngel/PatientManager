create table public.pm_recommendation (
  id uuid not null default gen_random_uuid (),
  patient_id uuid null default gen_random_uuid (),
  description text null default ''::text,
  completed boolean not null default false,
  created_at timestamp with time zone not null default now(),
  constraint pm_recommendation_pkey primary key (id),
  constraint pm_recommendation_patient_id_fkey foreign KEY (patient_id) references pm_patient (id) on update CASCADE on delete CASCADE
);