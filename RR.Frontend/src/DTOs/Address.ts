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

  static from(obj: any): Address {
    return Object.assign(new Address(), obj);
  }
  
  isEmpty(): boolean {
    return (!this.country || this.country.trim() === '') &&
      (!this.region || this.region.trim() === '') &&
      (!this.postalCode || this.postalCode.trim() === '') &&
      (!this.city || this.city.trim() === '') &&
      (!this.streetAddress || this.streetAddress.trim() === '') &&
      (!this.note || this.note.trim() === '');
  }
}
