import { useEffect, useState } from 'react';
import StatusMessage from '../components/StatusMessage';
import { createService, deleteService, getServices } from '../api/services';

const emptyForm = {
  name: '',
  description: '',
  durationMinutes: '',
};

export default function ServicesPage() {
  const [services, setServices] = useState([]);
  const [loading, setLoading] = useState(true);
  const [form, setForm] = useState(emptyForm);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  async function loadServices() {
    try {
      setLoading(true);
      setError('');
      const data = await getServices();
      setServices(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err.message || 'Failed to load services.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadServices();
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

      await createService({
        name: form.name.trim(),
        description: form.description.trim(),
        durationMinutes: Number(form.durationMinutes),
      });

      setSuccess('Service created successfully.');
      setForm(emptyForm);
      await loadServices();
    } catch (err) {
      setError(err.message || 'Failed to create service.');
    }
  }

  async function handleDelete(id) {
    try {
      setError('');
      setSuccess('');
      await deleteService(id);
      setSuccess('Service deleted successfully.');
      await loadServices();
    } catch (err) {
      setError(err.message || 'Failed to delete service.');
    }
  }

  return (
    <section className='page-grid'>
      <div className='card'>
        <h2>Create service</h2>

        <form onSubmit={handleSubmit} className='form'>
          <label>
            Service name
            <input
              name='name'
              value={form.name}
              onChange={handleChange}
              maxLength={100}
              required
            />
          </label>

          <label>
            Description
            <textarea
              name='description'
              value={form.description}
              onChange={handleChange}
              rows='4'
            />
          </label>

          <label>
            Duration (minutes)
            <input
              name='durationMinutes'
              type='number'
              min='1'
              value={form.durationMinutes}
              onChange={handleChange}
              required
            />
          </label>

          <button type='submit'>Create service</button>
        </form>

        <StatusMessage type='error' message={error} />
        <StatusMessage type='success' message={success} />
      </div>

      <div className='card'>
        <h2>Service list</h2>

        {loading ? (
          <p>Loading services...</p>
        ) : services.length === 0 ? (
          <p>No services found.</p>
        ) : (
          <div className='list'>
            {services.map((service) => (
              <div key={service.serviceId} className='list-item'>
                <div>
                  <strong>{service.name}</strong>
                  <p>{service.description || 'No description provided.'}</p>
                  <small>{service.durationMinutes} minutes</small>
                </div>

                <button
                  type='button'
                  className='danger'
                  onClick={() => handleDelete(service.serviceId)}
                >
                  Delete
                </button>
              </div>
            ))}
          </div>
        )}
      </div>
    </section>
  );
}
