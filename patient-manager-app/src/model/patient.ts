export class Patient
{
  public constructor(
    public id: string,
    public name: string,
    public birthdate: string,
    public phoneNumber: string,
    public email: string,
    public createdAt: Date) {}
}