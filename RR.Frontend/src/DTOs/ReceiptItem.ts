import type { Measurement } from './Measurement'

export class ReceiptItem {
  originalRecognizedName?: string;
  name: string;
  quantity: number;
  measurement: Measurement;
  pricePerQuantity: number;
  price: number;

  constructor() {
    this.name = '';
    this.quantity = 0;
    this.measurement = 'pieces';
    this.pricePerQuantity = 0;
    this.price = 0;
  }

  static from(obj: any): ReceiptItem {
    return Object.assign(new ReceiptItem(), obj);
  }
  
  isEmpty(): boolean {
    return (!this.name || this.name.trim() === '') &&
      (!this.originalRecognizedName || this.originalRecognizedName.trim() === '') &&
      (!this.quantity || this.quantity === 0) &&
      (!this.pricePerQuantity || this.pricePerQuantity === 0) &&
      (!this.price || this.price === 0);
  }
}
