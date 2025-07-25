export interface ApiResult<T> {
  success: boolean;
  errorCode: string;
  message: string;
  data: T | null;
}
