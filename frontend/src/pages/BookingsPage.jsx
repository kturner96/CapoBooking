import { useEffect, useState } from 'react';
import {
  createBooking,
  deleteBooking,
  getBookings,
  updateBookingStatus,
} from '../api/bookings';
import { getServices } from '../api/services';
import StatusMessage from '../components/StatusMessage';
import {
  buildBookingStartTime,
  formatDateTime,
  getTimeOptions,
} from '../utils/time';

const emptyForm = {
  serviceId: '',
  clientName: '',
  clientEmail: '',
  clientMobile: '',
  bookingDate: '',
  bookingTime: '',
};

export default function BookingsPage() {
  const [services, setServices] = useState([]);
  const [bookings, setBookings] = useState([]);
  const [loadingServices, setLoadingServices] = useState(true);
  const [loadingBookings, setLoadingBookings] = useState(true);
  const [form, setForm] = useState(emptyForm);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const timeOptions = getTimeOptions();

  async function loadServices() {
    try {
      setLoadingServices(true);
      const data = await getServices();
      setServices(Array.isArray(data) ? data : []);
    } finally {
      setLoadingServices(false);
    }
  }

  async function loadBookings() {
    try {
      setLoadingBookings(true);
      const data = await getBookings();
      setBookings(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err.message || 'Failed to load bookings.');
    } finally {
      setLoadingBookings(false);
    }
  }

  useEffect(() => {
    loadServices().catch((err) =>
      setError(err.message || 'Failed to load services.'),
    );
    loadBookings();
  }, []);

  function handleChange(event) {
    const { name, value } = event.target;
    setForm((current) => ({
      ...current,
      [name]: value,
    }));
  }

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      setError('');
      setSuccess('');

      await createBooking({
        serviceId: Number(form.serviceId),
        clientName: form.clientName.trim(),
        clientEmail: form.clientEmail.trim(),
        clientMobile: form.clientMobile.trim(),
        startTime: buildBookingStartTime(form.bookingDate, form.bookingTime),
      });

      setSuccess('Booking created successfully.');
      setForm(emptyForm);
      await loadBookings();
    } catch (err) {
      setError(err.message || 'Failed to create booking.');
    }
  }

  async function handleCancel(id) {
    try {
      setError('');
      setSuccess('');
      await updateBookingStatus(id, 'Cancelled');
      setSuccess('Booking cancelled successfully.');
      await loadBookings();
    } catch (err) {
      setError(err.message || 'Failed to cancel booking.');
    }
  }

  async function handleDelete(id) {
    try {
      setError('');
      setSuccess('');
      await deleteBooking(id);
      setSuccess('Booking deleted successfully.');
      await loadBookings();
    } catch (err) {
      setError(err.message || 'Failed to delete booking.');
    }
  }

  async function handleStatusChange(id, status) {
    try {
      setError('');
      setSuccess('');
      await updateBookingStatus(id, status);
      setSuccess(`Booking updated to ${status}.`);
      await loadBookings();
    } catch (err) {
      setError(err.message || 'Failed to update booking.');
    }
  }

  function getServiceName(serviceId) {
    const service = services.find((x) => x.serviceId === serviceId);
    return service?.name || `Service #${serviceId}`;
  }

  return (
    <section className='page-grid'>
      <div className='card'>
        <h2>Create booking</h2>

        <form onSubmit={handleSubmit} className='form'>
          <label>
            Service
            <select
              name='serviceId'
              value={form.serviceId}
              onChange={handleChange}
              required
              disabled={loadingServices}
            >
              <option value=''>Select a service</option>
              {services.map((service) => (
                <option key={service.serviceId} value={service.serviceId}>
                  {service.name}
                </option>
              ))}
            </select>
          </label>

          <label>
            Client name
            <input
              name='clientName'
              value={form.clientName}
              onChange={handleChange}
              maxLength={100}
              required
            />
          </label>

          <label>
            Client email
            <input
              name='clientEmail'
              type='email'
              value={form.clientEmail}
              onChange={handleChange}
              required
            />
          </label>

          <label>
            Client mobile
            <input
              name='clientMobile'
              type='tel'
              value={form.clientMobile}
              onChange={handleChange}
              required
            />
          </label>

          <label>
            Booking date
            <input
              name='bookingDate'
              type='date'
              value={form.bookingDate}
              onChange={handleChange}
              required
            />
          </label>

          <label>
            Booking time
            <select
              name='bookingTime'
              value={form.bookingTime}
              onChange={handleChange}
              required
            >
              <option value=''>Select a time</option>
              {timeOptions.map((time) => (
                <option key={time} value={time}>
                  {time}
                </option>
              ))}
            </select>
          </label>

          <button type='submit'>Create booking</button>
        </form>

        <StatusMessage type='error' message={error} />
        <StatusMessage type='success' message={success} />
      </div>

      <div className='card'>
        <h2>Booking list</h2>

        {loadingBookings ? (
          <p>Loading bookings...</p>
        ) : bookings.length === 0 ? (
          <p>No bookings found.</p>
        ) : (
          <div className='list'>
            {bookings.map((booking) => (
              <div key={booking.bookingId} className='list-item'>
                <div>
                  <strong>{getServiceName(booking.serviceId)}</strong>
                  <p>{booking.clientName}</p>
                  <p>{booking.clientEmail}</p>
                  <p>{booking.clientMobile}</p>
                  <small>{formatDateTime(booking.startTime)}</small>
                  <br />
                  <small>Status: {booking.status}</small>
                </div>

                <div className='button-group'>
                  <select
                    value=''
                    onChange={(e) =>
                      handleStatusChange(booking.bookingId, e.target.value)
                    }
                  >
                    <option value='' disabled>
                      Update status
                    </option>
                    <option value='Pending'>Pending</option>
                    <option value='Confirmed'>Confirmed</option>
                    <option value='Completed'>Completed</option>
                    <option value='Cancelled'>Cancelled</option>
                  </select>

                  <button
                    type='button'
                    className='danger'
                    onClick={() => handleDelete(booking.bookingId)}
                  >
                    Delete
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </section>
  );
}
