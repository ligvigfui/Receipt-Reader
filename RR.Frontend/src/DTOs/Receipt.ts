import type { Vendor } from './Vendor'
import type { ReceiptItem } from './ReceiptItem'

export interface Receipt {
  vendor: Vendor
  items: ReceiptItem[]
  total: number
  receiptId?: string
  dateTime?: string | Date
}
