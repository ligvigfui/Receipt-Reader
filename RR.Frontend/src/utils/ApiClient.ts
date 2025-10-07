export class ApiClient {
  private static baseUrl: string = import.meta.env.VITE_API_BASE_URL

  private static async fetchWithAuth(url: string, init: RequestInit = {}, query?: Record<string, any>): Promise<any> {
    let fullUrl = this.baseUrl + url
    if (query && Object.keys(query).length > 0) {
      const qs = Object.entries(query)
        .filter(([, v]) => v !== undefined && v !== null)
        .map(([k, v]) => `${encodeURIComponent(k)}=${encodeURIComponent(v)}`)
        .join('&')
      fullUrl = this.baseUrl + url + (url.includes('?') ? '&' : '?') + qs
    }
    const token = localStorage.getItem('token')
    const headers = new Headers(init.headers || {})
    if (token) {
      headers.set('Authorization', `Bearer ${token}`)
    }
    try {
      const result = await fetch(fullUrl, { ...init, headers })
      const json = await result.json()
      return {
        ...json,
        isOk: () => result.ok,
      }
    }
    catch (error) {
      return {
        isOk: () => false,
        type: 'FetchError',
        errors: ['Server is unreachable.'],
      }
    }
  }

  // Overload signatures
  static async get<S>(
    url: string,
    query?: Record<string, any>,
    init?: RequestInit
  ): Promise<S & { isOk: () => boolean } & GlobalErrorResponse>
  static async get<S, E>(
    url: string,
    query?: Record<string, any>,
    init?: RequestInit
  ): Promise<S & { isOk: () => boolean } & E>

  // Implementation
  static async get<S, E = {}>(
    url: string,
    query?: Record<string, any>,
    init: RequestInit = {}
  ): Promise<(S & { isOk: () => boolean } & GlobalErrorResponse) | (S & { isOk: () => boolean } & E)> {
    const data = await this.fetchWithAuth(url, { ...init, method: 'GET' }, query)
    return {
      ...data,
    } as S & { isOk: () => boolean } & E
  }

  // Overload signatures
  static async post<S>(
    url: string,
    body: any,
    query?: Record<string, any>,
    init?: RequestInit,
  ): Promise<S & { isOk: () => boolean } & GlobalErrorResponse>
  static async post<S, E>(
    url: string,
    body: any,
    query?: Record<string, any>,
    init?: RequestInit
  ): Promise<S & { isOk: () => boolean } & E>

  // Implementation
  static async post<S, E = {}>(
    url: string,
    body: any,
    query?: Record<string, any>,
    init: RequestInit = {}
  ): Promise<(S & { isOk: () => boolean } & GlobalErrorResponse) | (S & { isOk: () => boolean } & E)> {
    const data = await this.fetchWithAuth(url, {
      ...init,
      method: 'POST',
      headers: { 'Content-Type': 'application/json', ...(init.headers || {}) },
      body: JSON.stringify(body)
    },
    query)
    return {
      ...data,
    } as S & { isOk: () => boolean } & E
  }

  // Add put, delete, etc. as needed
}
