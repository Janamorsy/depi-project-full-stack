import { Link } from 'react-router-dom';
import { Card } from '../ui';

const AuthLayout = ({ 
  title, 
  subtitle, 
  children, 
  footer,
  maxWidth = 'md' 
}) => {
  const widthClasses = {
    sm: 'col-md-4',
    md: 'col-md-5',
    lg: 'col-md-6',
    xl: 'col-md-8'
  };

  return (
    <div className="min-vh-100 d-flex align-items-center bg-light py-5">
      <div className="container">
        <div className="row justify-content-center">
          <div className={widthClasses[maxWidth]}>
            <div className="text-center mb-4">
              <Link to="/" className="text-decoration-none">
                <h2 className="text-primary d-flex align-items-center justify-content-center gap-2">
                  <i className="fas fa-heartbeat"></i>
                  <span>NileCare</span>
                </h2>
              </Link>
            </div>
            <Card className="shadow">
              <div className="p-4">
                <div className="text-center mb-4">
                  <h2>{title}</h2>
                  {subtitle && <p className="text-muted">{subtitle}</p>}
                </div>
                {children}
                {footer && (
                  <div className="text-center mt-4">
                    {footer}
                  </div>
                )}
              </div>
            </Card>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AuthLayout;
