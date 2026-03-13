export function buildBookingStartTime(date, time) {
  if (!date || !time) return '';
  return new Date(`${date}T${time}`).toISOString();
}

export function formatDateTime(value) {
  if (!value) return 'No date';

  const date = new Date(value);

  return date.toLocaleString('en-GB', {
    dateStyle: 'medium',
    timeStyle: 'short',
  });
}

export function getTimeOptions() {
  return Array.from({ length: 48 }, (_, index) => {
    const hours = String(Math.floor(index / 2)).padStart(2, '0');
    const minutes = index % 2 === 0 ? '00' : '30';
    return `${hours}:${minutes}`;
  });
}
