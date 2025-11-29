const FormTextarea = ({
  label,
  name,
  value,
  onChange,
  placeholder,
  rows = 3,
  required = false,
  disabled = false,
  helpText,
  error,
  className = '',
  ...props
}) => {
  const handleChange = (e) => {
    if (onChange) {
      onChange(e);
    }
  };

  return (
    <div className={`mb-3 ${className}`}>
      {label && (
        <label className="form-label">
          {label}
          {required && <span className="text-danger"> *</span>}
        </label>
      )}
      <textarea
        className={`form-control ${error ? 'is-invalid' : ''}`}
        name={name}
        value={value}
        onChange={handleChange}
        placeholder={placeholder}
        rows={rows}
        required={required}
        disabled={disabled}
        {...props}
      />
      {helpText && <small className="form-text text-muted">{helpText}</small>}
      {error && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};

export default FormTextarea;
