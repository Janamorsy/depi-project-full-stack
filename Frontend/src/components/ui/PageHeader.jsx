import Avatar from './Avatar';

const PageHeader = ({ 
  title, 
  subtitle,
  avatar,
  avatarAlt,
  action,
  className = '' 
}) => {
  return (
    <div className={`d-flex align-items-center justify-content-between mb-4 ${className}`}>
      <div className="d-flex align-items-center">
        {avatar && (
          <Avatar src={avatar} alt={avatarAlt || title} size="lg" className="me-3" />
        )}
        <div>
          <h1 className="mb-0">{title}</h1>
          {subtitle && <p className="text-muted mb-0">{subtitle}</p>}
        </div>
      </div>
      {action && <div>{action}</div>}
    </div>
  );
};

export default PageHeader;
