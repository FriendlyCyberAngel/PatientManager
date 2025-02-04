export class Recommendation
{
  public constructor(
    public id: string,
    public patientId: string,
    public description: string,
    public completed: boolean,
    public createdAt: Date) {}
}