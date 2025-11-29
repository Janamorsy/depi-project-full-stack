const Card = ({ 
  children, 
  title, 
  subtitle,
  headerAction,
  footer,
  className = '',
  bodyClassName = '',
  noPadding = false,
  ...props 
}) => {
  return (
    <div className={`card ${className}`} {...props}>
      {(title || headerAction) && (
        <div className="card-header d-flex justify-content-between align-items-center">
          <div>
            {title && <h5 className="mb-0">{title}</h5>}
            {subtitle && <small className="text-muted">{subtitle}</small>}
          </div>
          {headerAction && <div>{headerAction}</div>}
        </div>
      )}
      <div className={`card-body ${noPadding ? 'p-0' : ''} ${bodyClassName}`}>
        {children}
      </div>
      {footer && (
        <div className="card-footer">
          {footer}
        </div>
      )}
    </div>
  );
};

export default Card;
