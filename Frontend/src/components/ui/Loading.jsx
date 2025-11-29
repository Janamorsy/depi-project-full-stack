const Loading = ({ size = 'md', text = 'Loading...', fullPage = false }) => {
  const sizeClasses = {
    sm: 'spinner-border-sm',
    md: '',
    lg: 'spinner-border spinner-lg'
  };

  const content = (
    <div className="text-center">
      <div className={`spinner-border ${sizeClasses[size]}`} role="status">
        <span className="visually-hidden">{text}</span>
      </div>
      {text && size !== 'sm' && <p className="mt-2 text-muted">{text}</p>}
    </div>
  );

  if (fullPage) {
    return (
      <div className="min-vh-100 d-flex align-items-center justify-content-center">
        {content}
      </div>
    );
  }

  return content;
};

export default Loading;
