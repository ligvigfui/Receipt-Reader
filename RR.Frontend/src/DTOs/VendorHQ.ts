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
}
