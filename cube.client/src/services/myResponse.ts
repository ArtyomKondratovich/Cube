
export class MyResponse<TResult> {
    value: TResult | null = null;
    responseResult: string = '';
    messages: string[] | null = null;
}