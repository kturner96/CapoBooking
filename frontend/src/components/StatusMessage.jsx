export default function StatusMessage({ type = 'info', message }) {
  if (!message) return null;

  return <div className={`message ${type}`}>{message}</div>;
}
