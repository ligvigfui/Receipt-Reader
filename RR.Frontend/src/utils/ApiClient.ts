export class ApiClient {
  private static baseUrl: string = import.meta.env.VITE_API_BASE_URL

  private static async getToken(): Promise<string | null> {
    return localStorage.getItem('token')
  }

  private static async fetchWithAuth(input: string, init: RequestInit = {}): Promise<Response> {
    const token = await this.getToken()
    const headers = new Headers(init.headers || {})
    if (token) {
      headers.set('Authorization', `Bearer ${token}`)
    }
    return fetch(input, { ...init, headers })
  }

  private static appendQueryParams(url: string, query: Record<string, any> | null): string {
    if (query && Object.keys(query).length > 0) {
      const qs = Object.entries(query)
        .filter(([, v]) => v !== undefined && v !== null)
        .map(([k, v]) => `${encodeURIComponent(k)}=${encodeURIComponent(v)}`)
        .join('&')
      return this.baseUrl + url + (url.includes('?') ? '&' : '?') + qs
    }
    return this.baseUrl + url
  }

  static async get<T>(
    url: string,
    query: Record<string, any> | null = null,
    init: RequestInit = {}
  ): Promise<T> {
    const fullUrl = this.appendQueryParams(url, query)
    const res = await this.fetchWithAuth(fullUrl, { ...init, method: 'GET' })
    return await res.json()
  }

  static async post<T>(
    url: string,
    query: Record<string, any> | null = null,
    body: any,
    init: RequestInit = {}
  ): Promise<T> {
    const fullUrl = this.appendQueryParams(url, query)
    const res = await this.fetchWithAuth(fullUrl, {
      ...init,
      method: 'POST',
      headers: { 'Content-Type': 'application/json', ...(init.headers || {}) },
      body: JSON.stringify(body)
    })
    return await res.json()
  }

  // Add put, delete, etc. as needed
}
