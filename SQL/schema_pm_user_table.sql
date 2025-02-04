create table public.pm_user (
  id uuid not null,
  name text null,
  phone_number text null,
  email text null,
  role text null,
  login text null,
  password text not null,
  created_at timestamp with time zone not null default now(),
  constraint pm_user_pkey primary key (id),
  constraint pm_user_email_key unique (email),
  constraint pm_user_id_key unique (id),
  constraint pm_user_login_key unique (login),
  constraint pm_user_phone_number_key unique (phone_number)
);