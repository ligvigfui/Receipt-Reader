import { Address } from './Address';

export class VendorHQ {
  name: string;
  address: Address;
  taxNumber: string;
  
  constructor() {
    this.name = '';
    this.address = new Address();
    this.taxNumber = '';
  }
  
  static from(obj: any): VendorHQ {
    const v = Object.assign(new VendorHQ(), obj);
    v.address = obj.address ? Address.from(obj.address) : new Address();
    return v;
  }
  
  isEmpty(): boolean {
    return (!this.name || this.name.trim() === '') &&
      this.address.isEmpty() &&
      (!this.taxNumber || this.taxNumber.trim() === '');
  }
}
