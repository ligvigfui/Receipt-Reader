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
}
