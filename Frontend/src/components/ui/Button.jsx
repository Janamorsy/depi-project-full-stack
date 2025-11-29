const Button = ({ 
  children, 
  variant = 'primary', 
  size = 'md', 
  fullWidth = false, 
  loading = false, 
  disabled = false,
  type = 'button',
  onClick,
  className = '',
  ...props 
}) => {
  const sizeClasses = {
    sm: 'btn-sm',
    md: '',
    lg: 'btn-lg'
  };

  const variantClasses = {
    primary: 'btn-primary',
    secondary: 'btn-secondary',
    success: 'btn-success',
    danger: 'btn-danger',
    warning: 'btn-warning',
    info: 'btn-info',
    outline: 'btn-outline-primary',
    'outline-secondary': 'btn-outline-secondary',
    'outline-danger': 'btn-outline-danger',
    link: 'btn-link'
  };

  return (
    <button
      type={type}
      className={`btn ${variantClasses[variant]} ${sizeClasses[size]} ${fullWidth ? 'w-100' : ''} ${className}`}
      disabled={disabled || loading}
      onClick={onClick}
      {...props}
    >
      {loading ? (
        <>
          <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
          {children}
        </>
      ) : children}
    </button>
  );
};

export default Button;
