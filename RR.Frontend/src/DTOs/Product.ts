export class Product {
  id?: number;
  userId?: number;
  groupId?: number;
  isPublic: boolean;
  name: string;
  description?: string;
  quantity?: number;
  measurement?: string;
  imageUrl?: string;

  constructor() {
    this.isPublic = true;
    this.name = '';
    this.description = '';
    this.quantity = undefined;
    this.measurement = '';
    this.imageUrl = '';
  }

  static from(obj: any): Product {
    const p = Object.assign(new Product(), obj);
    return p;
  }

  isEmpty(): boolean {
    return (!this.name || this.name.trim() === '') &&
      (!this.description || this.description.trim() === '') &&
      (!this.quantity || this.quantity === 0) &&
      (!this.measurement || this.measurement === '') &&
      (!this.imageUrl || this.imageUrl.trim() === '');
  }
}
