const API_BASE = (
    import.meta.env.VITE_API_URL as string | undefined
)?.replace(/\/+$/, '') ?? 'http://localhost:5102/api';

export async function apiGet<T>(path: string): Promise<T> {
    // Если /api уже указан в path, не добавляем его повторно.
    const normalizedPath = path.startsWith('/api/')
        ? path.slice(4)
        : path;

    const url = `${API_BASE}${normalizedPath.startsWith('/') ? '' : '/'}${normalizedPath}`;

    const res = await fetch(url, {
        headers: {
            Accept: 'application/json'
        }
    });

    if (!res.ok) {
        throw new Error(`API ${res.status}: ${res.statusText} (${url})`);
    }

    return res.json() as Promise<T>;
}

export function mediaUrl(path?: string | null): string {
    if (!path) return '';

    if (/^https?:\/\//i.test(path)) {
        return path;
    }

    // Файлы доступны через корень сайта, без /api.
    return path.startsWith('/') ? path : `/${path}`;
}

export { API_BASE };