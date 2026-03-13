import { Navigate, Route, Routes } from 'react-router-dom';
import Layout from './components/Layout';
import BookingsPage from './pages/BookingsPage';
import ServicesPage from './pages/ServicesPage';

export default function App() {
  return (
    <Layout>
      <Routes>
        <Route path='/' element={<Navigate to='/bookings' replace />} />
        <Route path='/bookings' element={<BookingsPage />} />
        <Route path='/services' element={<ServicesPage />} />
      </Routes>
    </Layout>
  );
}
