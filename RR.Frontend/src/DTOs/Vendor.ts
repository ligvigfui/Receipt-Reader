import type { Address } from './Address'
import type { VendorHQ } from './VendorHQ'

export interface Vendor {
  name: string
  address?: Address
  taxNumber?: string
  vendorHQ?: VendorHQ
}
