import { Vendor } from './Vendor';
import { ReceiptItem } from './ReceiptItem';

export class Receipt {
  vendor: Vendor;
  items: ReceiptItem[];
  total: number;
  receiptId?: string;
  dateTime: string | Date;

  constructor() {
    this.vendor = new Vendor();
    this.items = [new ReceiptItem()];
    this.total = 0;
    this.dateTime = new Date();
  }
}
