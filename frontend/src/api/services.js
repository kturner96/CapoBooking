const API_BASE = import.meta.env.VITE_API_BASE_URL;

export async function getServices() {
  const response = await fetch(`${API_BASE}/services`);

  if (!response.ok) {
    throw new Error('Failed to load services.');
  }

  return response.json();
}

export async function createService(service) {
  const response = await fetch(`${API_BASE}/services`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(service),
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || 'Failed to create service.');
  }

  const contentType = response.headers.get('content-type') || '';

  if (contentType.includes('application/json')) {
    return response.json();
  }

  return null;
}

export async function deleteService(id) {
  const response = await fetch(`${API_BASE}/services/${id}`, {
    method: 'DELETE',
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || 'Failed to delete service.');
  }
}
