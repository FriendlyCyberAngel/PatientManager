export class AppRequestCallBack<T, U>
{
  public complete?: (data: T) => void;
  public error?: (error: U) => void;
}