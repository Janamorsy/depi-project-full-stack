const FormGroup = ({ children, className = '' }) => {
  return (
    <div className={`mb-3 ${className}`}>
      {children}
    </div>
  );
};

export default FormGroup;
