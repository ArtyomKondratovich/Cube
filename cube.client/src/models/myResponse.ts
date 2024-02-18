
export interface MyResponse<TResult> {
    value: TResult | null;
    responseResult: string;
    messages: string[] | null;
}