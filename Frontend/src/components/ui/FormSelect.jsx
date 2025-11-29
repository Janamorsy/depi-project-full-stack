const FormSelect = ({
  label,
  name,
  value,
  onChange,
  options = [],
  placeholder = 'Select...',
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
      <select
        className={`form-select ${error ? 'is-invalid' : ''}`}
        name={name}
        value={value}
        onChange={handleChange}
        required={required}
        disabled={disabled}
        {...props}
      >
        {placeholder && <option value="">{placeholder}</option>}
        {options.map((opt, idx) => (
          <option key={opt.value || idx} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>
      {helpText && <small className="form-text text-muted">{helpText}</small>}
      {error && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};

export default FormSelect;
