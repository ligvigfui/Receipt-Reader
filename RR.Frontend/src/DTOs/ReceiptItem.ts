import type { Measurement } from './Measurement'

export interface ReceiptItem {
  originalRecognizedName?: string
  name: string
  quantity: number
  measurement: Measurement
  pricePerQuantity: number
  price: number
}
