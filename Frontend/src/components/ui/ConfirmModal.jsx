import { useEffect, useRef } from 'react';
import Button from './Button';

const ConfirmModal = ({
  isOpen,
  onConfirm,
  onCancel,
  title = 'Confirm Action',
  message = 'Are you sure you want to proceed?',
  confirmText = 'Confirm',
  cancelText = 'Cancel',
  variant = 'danger', // 'danger', 'warning', 'primary'
  icon = null
}) => {
  const modalRef = useRef(null);
  const confirmBtnRef = useRef(null);

  useEffect(() => {
    if (isOpen) {
      // Focus the cancel button when modal opens (safer default)
      confirmBtnRef.current?.focus();
      // Prevent body scroll
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = '';
    }

    return () => {
      document.body.style.overflow = '';
    };
  }, [isOpen]);

  useEffect(() => {
    const handleEscape = (e) => {
      if (e.key === 'Escape' && isOpen) {
        onCancel();
      }
    };

    document.addEventListener('keydown', handleEscape);
    return () => document.removeEventListener('keydown', handleEscape);
  }, [isOpen, onCancel]);

  if (!isOpen) return null;

  const handleBackdropClick = (e) => {
    if (e.target === modalRef.current) {
      onCancel();
    }
  };

  const iconMap = {
    danger: 'bi-exclamation-triangle-fill',
    warning: 'bi-exclamation-circle-fill',
    primary: 'bi-question-circle-fill',
    success: 'bi-check-circle-fill'
  };

  const colorMap = {
    danger: '#dc3545',
    warning: '#ffc107',
    primary: '#0d6efd',
    success: '#198754'
  };

  const displayIcon = icon || iconMap[variant] || iconMap.primary;

  return (
    <div
      ref={modalRef}
      onClick={handleBackdropClick}
      style={{
        position: 'fixed',
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        backgroundColor: 'rgba(0, 0, 0, 0.5)',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        zIndex: 9999,
        padding: '1rem',
        animation: 'fadeIn 0.15s ease-out'
      }}
    >
      <div
        style={{
          backgroundColor: 'white',
          borderRadius: '12px',
          maxWidth: '400px',
          width: '100%',
          boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)',
          animation: 'slideIn 0.2s ease-out'
        }}
      >
        {/* Header */}
        <div style={{ padding: '1.5rem 1.5rem 0', textAlign: 'center' }}>
          <div
            style={{
              width: '56px',
              height: '56px',
              borderRadius: '50%',
              backgroundColor: `${colorMap[variant]}15`,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              margin: '0 auto 1rem'
            }}
          >
            <i
              className={`bi ${displayIcon}`}
              style={{ fontSize: '1.75rem', color: colorMap[variant] }}
            />
          </div>
          <h5 style={{ marginBottom: '0.5rem', fontWeight: '600', color: '#1a202c' }}>
            {title}
          </h5>
          <p style={{ color: '#6c757d', marginBottom: 0, fontSize: '0.9375rem' }}>
            {message}
          </p>
        </div>

        {/* Footer */}
        <div
          style={{
            padding: '1.5rem',
            display: 'flex',
            gap: '0.75rem',
            justifyContent: 'center'
          }}
        >
          <Button
            variant="secondary"
            onClick={onCancel}
            style={{ minWidth: '100px' }}
          >
            {cancelText}
          </Button>
          <Button
            ref={confirmBtnRef}
            variant={variant}
            onClick={onConfirm}
            style={{ minWidth: '100px' }}
          >
            {confirmText}
          </Button>
        </div>
      </div>

      <style>{`
        @keyframes fadeIn {
          from { opacity: 0; }
          to { opacity: 1; }
        }
        @keyframes slideIn {
          from { opacity: 0; transform: scale(0.95) translateY(-10px); }
          to { opacity: 1; transform: scale(1) translateY(0); }
        }
      `}</style>
    </div>
  );
};

export default ConfirmModal;
