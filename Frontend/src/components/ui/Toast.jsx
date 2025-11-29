import { useEffect } from 'react';

const TOAST_DURATION = 3000;

const Toast = ({ message, type = 'success', onClose }) => {
  useEffect(() => {
    const timer = setTimeout(onClose, TOAST_DURATION);
    return () => clearTimeout(timer);
  }, [onClose]);

  const getIcon = () => {
    switch (type) {
      case 'success': return 'bi-check-circle-fill';
      case 'error': return 'bi-x-circle-fill';
      case 'warning': return 'bi-exclamation-triangle-fill';
      case 'info': return 'bi-info-circle-fill';
      default: return 'bi-check-circle-fill';
    }
  };

  const getBackgroundClass = () => {
    switch (type) {
      case 'success': return 'bg-success';
      case 'error': return 'bg-danger';
      case 'warning': return 'bg-warning';
      case 'info': return 'bg-info';
      default: return 'bg-success';
    }
  };

  return (
    <div 
      className={`toast show align-items-center text-white ${getBackgroundClass()} border-0`}
      role="alert"
    >
      <div className="d-flex">
        <div className="toast-body d-flex align-items-center gap-2">
          <i className={`bi ${getIcon()}`}></i>
          {message}
        </div>
        <button 
          type="button" 
          className="btn-close btn-close-white me-2 m-auto" 
          onClick={onClose}
        ></button>
      </div>
    </div>
  );
};

export default Toast;
