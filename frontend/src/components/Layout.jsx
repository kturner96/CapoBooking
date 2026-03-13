import NavBar from './NavBar';

export default function Layout({ children }) {
  return (
    <div className='app-shell'>
      <div className='container'>
        <h1 className='app-title'>CAPO Booking Demo</h1>
        <p className='app-subtitle'>
          Simple frontend to demonstrate the booking API properly.
        </p>
        <NavBar />
        {children}
      </div>
    </div>
  );
}
