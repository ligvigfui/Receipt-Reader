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
}
