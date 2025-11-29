const VARIANT_CLASSES = {
  primary: 'bg-primary',
  secondary: 'bg-secondary',
  success: 'bg-success',
  danger: 'bg-danger',
  warning: 'bg-warning text-dark',
  info: 'bg-info'
};

const Badge = ({ children, variant = 'primary', className = '' }) => {
  return (
    <span className={`badge ${VARIANT_CLASSES[variant]} ${className}`}>
      {children}
    </span>
  );
};

export default Badge;
