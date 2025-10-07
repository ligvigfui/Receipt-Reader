import { Address } from './Address';
import { VendorHQ } from './VendorHQ';

export class Vendor {
  name: string;
  address: Address;
  vendorHQ: VendorHQ;

  constructor() {
    this.name = '';
    this.address = new Address();
    this.vendorHQ = new VendorHQ();
  }

    static from(obj: any): Vendor {
    const v = Object.assign(new Vendor(), obj);
    v.address = obj.address ? Address.from(obj.address) : new Address();
    v.vendorHQ = obj.vendorHQ ? VendorHQ.from(obj.vendorHQ) : new VendorHQ();
    return v;
  }

  isEmpty(): boolean {
    return (!this.name || this.name.trim() === '') &&
      this.address.isEmpty() &&
      this.vendorHQ.isEmpty();
  }
}
