const API_BASE = 'http://localhost:5063/api';

export async function getBookings() {
  const response = await fetch(`${API_BASE}/bookings`);

  if (!response.ok) {
    throw new Error('Failed to load bookings.');
  }

  return response.json();
}

export async function createBooking(booking) {
  const response = await fetch(`${API_BASE}/bookings`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(booking),
  });

  if (!response.ok) {
    const contentType = response.headers.get('content-type') || '';

    if (contentType.includes('application/json')) {
      const errorData = await response.json();

      if (typeof errorData === 'string') {
        throw new Error(errorData);
      }

      if (errorData?.title) {
        throw new Error(errorData.title);
      }

      if (errorData?.errors) {
        const firstError = Object.values(errorData.errors)[0];
        if (Array.isArray(firstError) && firstError.length > 0) {
          throw new Error(firstError[0]);
        }
      }
    }

    const text = await response.text();
    throw new Error(text || 'Failed to create booking.');
  }

  return response.json();
}

export async function updateBookingStatus(id, status) {
  const response = await fetch(`${API_BASE}/bookings/${id}/status`, {
    method: 'PATCH',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ status }),
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || 'Failed to update booking.');
  }

  return response.json();
}

export async function deleteBooking(id) {
  const response = await fetch(`${API_BASE}/bookings/${id}`, {
    method: 'DELETE',
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || 'Failed to delete booking.');
  }
}
