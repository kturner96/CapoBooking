import { NavLink } from 'react-router-dom';

export default function NavBar() {
  return (
    <nav className='nav'>
      <NavLink
        to='/bookings'
        className={({ isActive }) =>
          isActive ? 'nav-link active' : 'nav-link'
        }
      >
        Bookings
      </NavLink>

      <NavLink
        to='/services'
        className={({ isActive }) =>
          isActive ? 'nav-link active' : 'nav-link'
        }
      >
        Services
      </NavLink>
    </nav>
  );
}
