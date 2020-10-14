import { IProduct } from './products';

export interface IPagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IProduct[];
}

// 284-1 Pagination for be uncoupled to be used outside
export class Pagination implements IPagination {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: IProduct[];
}
