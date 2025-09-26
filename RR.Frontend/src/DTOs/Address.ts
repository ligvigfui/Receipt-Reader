export class Address {
  country: string;
  region: string;
  postalCode: string;
  city: string;
  streetAddress: string;
  note: string;

  constructor() {
    this.country = '';
    this.region = '';
    this.postalCode = '';
    this.city = '';
    this.streetAddress = '';
    this.note = '';
  }
}
