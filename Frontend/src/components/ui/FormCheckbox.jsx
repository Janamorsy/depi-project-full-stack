const FormCheckbox = ({
  label,
  name,
  checked,
  onChange,
  disabled = false,
  helpText,
  className = '',
  ...props
}) => {
  const handleChange = (e) => {
    if (onChange) {
      onChange(e);
    }
  };

  return (
    <div className={`mb-3 form-check ${className}`}>
      <input
        type="checkbox"
        className="form-check-input"
        id={name}
        name={name}
        checked={checked}
        onChange={handleChange}
        disabled={disabled}
        {...props}
      />
      <label className="form-check-label" htmlFor={name}>
        {label}
      </label>
      {helpText && <small className="form-text text-muted d-block">{helpText}</small>}
    </div>
  );
};

export default FormCheckbox;
