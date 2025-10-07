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
    this.dateTime = '';
  }
  
  static from(obj: any): Receipt {
    const r = Object.assign(new Receipt(), obj);
    r.vendor = obj.vendor ? Vendor.from(obj.vendor) : new Vendor();
    r.items = Array.isArray(obj.items) ? obj.items.map((i: any) => ReceiptItem.from(i)) : [new ReceiptItem()];
    r.dateTime = obj.dateTime ? new Date(obj.dateTime) : '';
    return r;
  }

  isEmpty(): boolean {
    return this.vendor.isEmpty() &&
      (!this.items || this.items.length === 0 || this.items.every(item => item.isEmpty())) &&
      (!this.receiptId || this.receiptId === '') &&
      (!this.dateTime || this.dateTime === '' || (this.dateTime instanceof Date && isNaN(this.dateTime.getTime())));
  }
}
